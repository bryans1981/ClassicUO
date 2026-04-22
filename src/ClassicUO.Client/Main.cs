// SPDX-License-Identifier: BSD-2-Clause

using ClassicUO.Configuration;
using ClassicUO.Game;
using ClassicUO.Game.Managers;
using ClassicUO.IO;
using ClassicUO.Network;
using ClassicUO.Resources;
using ClassicUO.Utility;
using ClassicUO.Utility.Logging;
using ClassicUO.Utility.Platforms;
using SDL3;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace ClassicUO
{
    internal static class Bootstrap
    {
        [UnmanagedCallersOnly(EntryPoint = "Initialize", CallConvs = new Type[] { typeof(CallConvCdecl) })]
        static unsafe void Initialize(IntPtr* argv, int argc, HostBindings* hostSetup)
        {
            var args = new string[argc];
            for (int i = 0; i < argc; i++)
            {
                args[i] = Marshal.PtrToStringAnsi(argv[i]);
            }

            var host = new UnmanagedAssistantHost(hostSetup);
            Boot(host, args);
        }


        [STAThread]
        public static void Main(string[] args) => Boot(null, args);


        public static void Boot(UnmanagedAssistantHost pluginHost, string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            Log.Start(LogTypes.All);

            if (!PlatformHelper.IsBrowser)
            {
                DllMap.Init();
            }

            CUOEnviroment.GameThread = Thread.CurrentThread;
            CUOEnviroment.GameThread.Name = "CUO_MAIN_THREAD";
#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("######################## [START LOG] ########################");

#if DEV_BUILD
                sb.AppendLine($"ClassicUO [DEV_BUILD] - {CUOEnviroment.Version} - {DateTime.Now}");
#else
                sb.AppendLine($"ClassicUO [STANDARD_BUILD] - {CUOEnviroment.Version} - {DateTime.Now}");
#endif

                sb.AppendLine
                    ($"OS: {Environment.OSVersion.Platform} {(Environment.Is64BitOperatingSystem ? "x64" : "x86")}");

                sb.AppendLine($"Thread: {Thread.CurrentThread.Name}");
                sb.AppendLine();

                if (Settings.GlobalSettings != null)
                {
                    sb.AppendLine($"Shard: {Settings.GlobalSettings.IP}");
                    sb.AppendLine($"ClientVersion: {Settings.GlobalSettings.ClientVersion}");
                    sb.AppendLine();
                }

                sb.AppendFormat("Exception:\n{0}\n", e.ExceptionObject);
                sb.AppendLine("######################## [END LOG] ########################");
                sb.AppendLine();
                sb.AppendLine();

                Log.Panic(e.ExceptionObject.ToString());
                string path = PlatformHelper.IsBrowser
                    ? BrowserVirtualPaths.CacheFile("logs")
                    : Path.Combine(CUOEnviroment.ExecutablePath, "Logs");

                if (!FileSystemHelper.DirectoryExists(path))
                    FileSystemHelper.CreateDirectory(path);

                using (LogFile crashfile = new LogFile(path, "crash.txt"))
                {
                    crashfile.WriteAsync(sb.ToString()).RunSynchronously();
                }
            };
#endif
            ReadSettingsFromArgs(args);
            BrowserRuntimeBootstrap.ApplyBrowserStartupDefaults();
            BrowserRuntimeBootstrap.EnsureBrowserStorageBootstrap();
            BrowserRuntimeBootstrapState browserBootstrapState = BrowserRuntimeBootstrap.CaptureState();

            if (!PlatformHelper.IsBrowser)
            {
                if (CUOEnviroment.IsHighDPI)
                {
                    Environment.SetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI", "1");
                }

                // NOTE: this is a workaroud to fix d3d11 on windows 11 + scale windows
                Environment.SetEnvironmentVariable("FNA3D_D3D11_FORCE_BITBLT", "1");
                Environment.SetEnvironmentVariable("FNA3D_BACKBUFFER_SCALE_NEAREST", "1");
                Environment.SetEnvironmentVariable("FNA3D_OPENGL_FORCE_COMPATIBILITY_PROFILE", "1");
                Environment.SetEnvironmentVariable(SDL.SDL_HINT_MOUSE_FOCUS_CLICKTHROUGH, "1");
                Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + Path.Combine(CUOEnviroment.ExecutablePath, "Data", "Plugins"));
            }

            string globalSettingsPath = Settings.GetSettingsFilepath();

            if (!FileSystemHelper.DirectoryExists(Path.GetDirectoryName(globalSettingsPath)) || !FileSystemHelper.FileExists(globalSettingsPath))
            {
                // settings specified in path does not exists, make new one
                {
                    // TODO:
                    Settings.GlobalSettings.Save();
                }
            }

            Settings.GlobalSettings = ConfigurationResolver.Load(globalSettingsPath, SettingsJsonContext.RealDefault.Settings);

            ReadSettingsFromArgs(args);

            // still invalid, cannot load settings
            if (Settings.GlobalSettings == null)
            {
                Settings.GlobalSettings = new Settings();
                Settings.GlobalSettings.Save();
            }

            BrowserRuntimeBootstrap.ApplyBrowserStartupDefaults();

            if (string.IsNullOrWhiteSpace(Settings.GlobalSettings.Language))
            {
                Log.Trace("language is not set. Trying to get the OS language.");
                try
                {
                    Settings.GlobalSettings.Language = CultureInfo.InstalledUICulture.ThreeLetterWindowsLanguageName;

                    if (string.IsNullOrWhiteSpace(Settings.GlobalSettings.Language))
                    {
                        Log.Warn("cannot read the OS language. Rolled back to ENU");

                        Settings.GlobalSettings.Language = "ENU";
                    }

                    Log.Trace($"language set: '{Settings.GlobalSettings.Language}'");
                }
                catch
                {
                    Log.Warn("cannot read the OS language. Rolled back to ENU");

                    Settings.GlobalSettings.Language = "ENU";
                }
            }

            if (string.IsNullOrWhiteSpace(Settings.GlobalSettings.UltimaOnlineDirectory))
            {
                Settings.GlobalSettings.UltimaOnlineDirectory = CUOEnviroment.ExecutablePath;
            }

            const uint INVALID_UO_DIRECTORY = 0x100;
            const uint INVALID_UO_VERSION = 0x200;

            uint flags = 0;

            if (!PlatformHelper.IsBrowser)
            {
                if (!FileSystemHelper.DirectoryExists(Settings.GlobalSettings.UltimaOnlineDirectory) || !FileSystemHelper.FileExists(Path.Combine(Settings.GlobalSettings.UltimaOnlineDirectory, "tiledata.mul")))
                {
                    flags |= INVALID_UO_DIRECTORY;
                }

                string clientVersionText = Settings.GlobalSettings.ClientVersion;

                if (!ClientVersionHelper.IsClientVersionValid(Settings.GlobalSettings.ClientVersion, out ClientVersion clientVersion))
                {
                    Log.Warn($"Client version [{clientVersionText}] is invalid, let's try to read the client.exe");

                    if (
                        !ClientVersionHelper.TryParseFromFile(Path.Combine(Settings.GlobalSettings.UltimaOnlineDirectory, "client.exe"), out clientVersionText) ||
                        !ClientVersionHelper.IsClientVersionValid(clientVersionText, out clientVersion)
                    )
                    {
                        Log.Error("Invalid client version: " + clientVersionText);

                        flags |= INVALID_UO_VERSION;
                    }
                    else
                    {
                        Log.Trace($"Found a valid client.exe [{clientVersionText} - {clientVersion}]");

                        // update the wrong/missing client version in settings.json
                        Settings.GlobalSettings.ClientVersion = clientVersionText;
                    }
                }
            }

            if (flags != 0)
            {
                if ((flags & INVALID_UO_DIRECTORY) != 0)
                {
                    Client.ShowErrorMessage(ResGeneral.YourUODirectoryIsInvalid);
                }
                else if ((flags & INVALID_UO_VERSION) != 0)
                {
                    Client.ShowErrorMessage(ResGeneral.YourUOClientVersionIsInvalid);
                }

                if (!PlatformHelper.IsBrowser)
                {
                    PlatformHelper.LaunchBrowser(ResGeneral.ClassicUOLink);
                }
                else
                {
                    Log.Warn("Browser startup validation failed; skipping external browser launch.");
                }
            }
            else
            {
                Log.Trace($"Browser bootstrap: storageConfigured={browserBootstrapState.StorageConfigured}, assets={browserBootstrapState.AssetsRootPath}, profiles={browserBootstrapState.ProfilesRootPath}, cache={browserBootstrapState.CacheRootPath}, config={browserBootstrapState.ConfigRootPath}");

                switch (Settings.GlobalSettings.ForceDriver)
                {
                    case 1: // OpenGL
                        Environment.SetEnvironmentVariable("FNA3D_FORCE_DRIVER", "OpenGL");

                        break;

                    case 2: // Vulkan
                        Environment.SetEnvironmentVariable("FNA3D_FORCE_DRIVER", "Vulkan");

                        break;
                }

                Client.Run(pluginHost, browserBootstrapState);
            }

            Log.Trace("Closing...");
        }

        private static void ReadSettingsFromArgs(string[] args)
        {
            for (int i = 0; i <= args.Length - 1; i++)
            {
                string cmd = args[i].ToLower();

                // NOTE: Command-line option name should start with "-" character
                if (cmd.Length == 0 || cmd[0] != '-')
                {
                    continue;
                }

                cmd = cmd.Remove(0, 1);
                string value = string.Empty;

                if (i < args.Length - 1)
                {
                    if (!string.IsNullOrWhiteSpace(args[i + 1]) && !args[i + 1].StartsWith("-"))
                    {
                        value = args[++i];
                    }
                }

                Log.Trace($"ARG: {cmd}, VALUE: {value}");

                switch (cmd)
                {
                    // Here we have it! Using `-settings` option we can now set the filepath that will be used
                    // to load and save ClassicUO main settings instead of default `./settings.json`
                    // NOTE: All individual settings like `username`, `password`, etc passed in command-line options
                    // will override and overwrite those in the settings file because they have higher priority
                    case "settings":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring custom settings path in browser mode.");
                            break;
                        }

                        Settings.CustomSettingsFilepath = value;

                        break;

                    case "highdpi":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring high-DPI override in browser mode.");
                            break;
                        }

                        CUOEnviroment.IsHighDPI = true;

                        break;

                    case "username":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring username override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.Username = value;

                        break;

                    case "password":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring password override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.Password = Crypter.Encrypt(value);

                        break;

                    case "password_enc": // Non-standard setting, similar to `password` but for already encrypted password
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring encrypted password override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.Password = value;

                        break;

                    case "ip":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring IP override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.IP = value;

                        break;

                    case "port":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring port override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.Port = ushort.Parse(value);

                        break;

                    case "ignore_relay_ip":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring relay-IP override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.IgnoreRelayIp = bool.Parse(value);

                        break;

                    case "filesoverride":
                    case "uofilesoverride":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring files override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.OverrideFile = value;

                        break;

                    case "ultimaonlinedirectory":
                    case "uopath":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring Ultima Online directory override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.UltimaOnlineDirectory = value;

                        break;

                    case "profilespath":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring profiles path override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.ProfilesPath = value;

                        break;

                    case "clientversion":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring client version override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.ClientVersion = value;

                        break;

                    case "lastcharactername":
                    case "lastcharname":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring last character override in browser mode.");
                            break;
                        }

                        LastCharacterManager.OverrideLastCharacter(value);

                        break;

                    case "lastservernum":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring last server number override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.LastServerNum = ushort.Parse(value);

                        break;

                    case "last_server_name":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring last server name override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.LastServerName = value;
                        break;

                    case "fps":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring FPS override in browser mode.");
                            break;
                        }

                        int v = int.Parse(value);

                        if (v < Constants.MIN_FPS)
                        {
                            v = Constants.MIN_FPS;
                        }
                        else if (v > Constants.MAX_FPS)
                        {
                            v = Constants.MAX_FPS;
                        }

                        Settings.GlobalSettings.FPS = v;

                        break;

                    case "screen_scale":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring screen scale override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.ScreenScale = float.Parse(value);

                        break;

                    case "debug":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring debug override in browser mode.");
                            break;
                        }

                        CUOEnviroment.Debug = true;

                        break;

                    case "profiler":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring profiler override in browser mode.");
                            break;
                        }

                        Profiler.Enabled = bool.Parse(value);

                        break;

                    case "saveaccount":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring save-account override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.SaveAccount = bool.Parse(value);

                        break;

                    case "autologin":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring auto-login override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.AutoLogin = bool.Parse(value);

                        break;

                    case "reconnect":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring reconnect override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.Reconnect = bool.Parse(value);

                        break;

                    case "reconnect_time":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring reconnect time override in browser mode.");
                            break;
                        }

                        if (!int.TryParse(value, out int reconnectTime) || reconnectTime < 1000)
                        {
                            reconnectTime = 1000;
                        }

                        Settings.GlobalSettings.ReconnectTime = reconnectTime;

                        break;

                    case "login_music":
                    case "music":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring login music override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.LoginMusic = bool.Parse(value);

                        break;

                    case "login_music_volume":
                    case "music_volume":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring login music volume override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.LoginMusicVolume = int.Parse(value);

                        break;

                    case "fixed_time_step":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring fixed time step override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.FixedTimeStep = bool.Parse(value);

                        break;

                    case "run_mouse_in_separate_thread":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring separate mouse thread override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.RunMouseInASeparateThread = bool.Parse(value);

                        break;

                    case "skiploginscreen":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring skip login screen override in browser mode.");
                            break;
                        }

                        CUOEnviroment.SkipLoginScreen = true;

                        break;

                    case "plugins":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring plugin list in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.Plugins = string.IsNullOrEmpty(value) ? new string[0] : value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        break;

                    case "use_verdata":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring verdata override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.UseVerdata = bool.Parse(value);

                        break;

                    case "maps_layouts":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring maps layouts override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.MapsLayouts = value;

                        break;

                    case "encryption":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring encryption override in browser mode.");
                            break;
                        }

                        Settings.GlobalSettings.Encryption = byte.Parse(value);

                        break;

                    case "force_driver":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring graphics driver selection in browser mode.");
                            break;
                        }

                        if (byte.TryParse(value, out byte res))
                        {
                            switch (res)
                            {
                                case 1: // OpenGL
                                    Settings.GlobalSettings.ForceDriver = 1;

                                    break;

                                case 2: // Vulkan
                                    Settings.GlobalSettings.ForceDriver = 2;

                                    break;

                                default: // use default
                                    Settings.GlobalSettings.ForceDriver = 0;

                                    break;
                            }
                        }
                        else
                        {
                            Settings.GlobalSettings.ForceDriver = 0;
                        }

                        break;

                    case "packetlog":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring packet log in browser mode.");
                            break;
                        }

                        PacketLogger.Default.Enabled = true;
                        PacketLogger.Default.CreateFile();

                        break;

                    case "language":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring language override in browser mode.");
                            break;
                        }

                        switch (value?.ToUpperInvariant())
                        {
                            case "RUS": Settings.GlobalSettings.Language = "RUS"; break;
                            case "FRA": Settings.GlobalSettings.Language = "FRA"; break;
                            case "DEU": Settings.GlobalSettings.Language = "DEU"; break;
                            case "ESP": Settings.GlobalSettings.Language = "ESP"; break;
                            case "JPN": Settings.GlobalSettings.Language = "JPN"; break;
                            case "KOR": Settings.GlobalSettings.Language = "KOR"; break;
                            case "PTB": Settings.GlobalSettings.Language = "PTB"; break;
                            case "ITA": Settings.GlobalSettings.Language = "ITA"; break;
                            case "CHT": Settings.GlobalSettings.Language = "CHT"; break;
                            default:

                                Settings.GlobalSettings.Language = "ENU";
                                break;

                        }

                        break;

                    case "no_server_ping":
                        if (PlatformHelper.IsBrowser)
                        {
                            Log.Trace("Ignoring no-server-ping override in browser mode.");
                            break;
                        }

                        CUOEnviroment.NoServerPing = true;

                        break;
                }
            }
        }

    }
}




