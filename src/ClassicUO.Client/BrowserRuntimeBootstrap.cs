// SPDX-License-Identifier: BSD-2-Clause

using ClassicUO.Configuration;
using ClassicUO.Utility;
using ClassicUO.Utility.Platforms;
using ClassicUO.Utility.Logging;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace ClassicUO
{
    internal static class BrowserRuntimeBootstrap
    {
        private static uint? _browserLocalIpOverride;

        public static BrowserRuntimeBootstrapState CaptureState()
        {
            return new BrowserRuntimeBootstrapState
            {
                IsBrowser = PlatformHelper.IsBrowser,
                StorageConfigured = BrowserFileSystemBootstrap.IsConfigured,
                AssetsRootPath = BrowserVirtualPaths.AssetsRoot,
                ProfilesRootPath = BrowserVirtualPaths.ProfilesRoot,
                CacheRootPath = BrowserVirtualPaths.CacheRoot,
                ConfigRootPath = BrowserVirtualPaths.ConfigRoot,
                UltimaOnlineDirectory = Settings.GlobalSettings?.UltimaOnlineDirectory ?? string.Empty,
                ClientVersion = Settings.GlobalSettings?.ClientVersion ?? string.Empty
            };
        }

        public static void ApplyBrowserStartupDefaults()
        {
            if (!PlatformHelper.IsBrowser)
            {
                return;
            }

            Settings.GlobalSettings.UltimaOnlineDirectory = BrowserVirtualPaths.AssetsRoot;
            Settings.GlobalSettings.ProfilesPath = BrowserVirtualPaths.ProfilesRoot;
            Environment.SetEnvironmentVariable("FNA_PLATFORM_BACKEND", "SDL2");
            Environment.SetEnvironmentVariable("FNA3D_FORCE_DRIVER", "OpenGL");

            if (string.IsNullOrWhiteSpace(Settings.GlobalSettings.IP))
            {
                Settings.GlobalSettings.IP = "ws://127.0.0.1:2594";
            }
            else if (!Settings.GlobalSettings.IP.StartsWith("ws://", StringComparison.OrdinalIgnoreCase)
                && !Settings.GlobalSettings.IP.StartsWith("wss://", StringComparison.OrdinalIgnoreCase))
            {
                Settings.GlobalSettings.IP = $"ws://{Settings.GlobalSettings.IP}";
            }

            Settings.GlobalSettings.Port = 2594;

            Settings.CustomSettingsFilepath = null;
            Settings.GlobalSettings.Plugins = Array.Empty<string>();
            Settings.GlobalSettings.UseVerdata = false;
            Settings.GlobalSettings.SaveAccount = false;
            Settings.GlobalSettings.AutoLogin = false;
            Settings.GlobalSettings.Reconnect = false;
            Settings.GlobalSettings.ReconnectTime = 0;
            Settings.GlobalSettings.LoginMusic = false;
            Settings.GlobalSettings.LoginMusicVolume = 0;
            Settings.GlobalSettings.Username = string.Empty;
            Settings.GlobalSettings.Password = string.Empty;
            Settings.GlobalSettings.LastServerNum = 0;
            Settings.GlobalSettings.LastServerName = string.Empty;
            Settings.GlobalSettings.OverrideFile = string.Empty;
            Settings.GlobalSettings.ScreenScale = 1f;
            if (string.IsNullOrWhiteSpace(Settings.GlobalSettings.Language))
            {
                Settings.GlobalSettings.Language = "ENU";
            }
            Settings.GlobalSettings.ForceDriver = 0;
            Settings.GlobalSettings.MapsLayouts = string.Empty;
            Settings.GlobalSettings.WindowPosition = new Point(0, 0);
            Settings.GlobalSettings.WindowSize = new Point(1280, 720);
            Settings.GlobalSettings.IgnoreRelayIp = true;
            CUOEnviroment.NoServerPing = true;

            EnsureBrowserClientVersionFromAssets();
        }

        public static void ApplyBrowserProfileDefaults(Profile profile)
        {
            if (!PlatformHelper.IsBrowser || profile == null)
            {
                return;
            }

            profile.WindowClientBounds = new Point(1280, 720);
            profile.GameWindowPosition = new Point(0, 0);
            profile.GameWindowSize = new Point(1280, 720);
            profile.GameWindowFullSize = false;
            profile.WindowBorderless = false;
            profile.ReduceFPSWhenInactive = false;
        }

        public static void ApplyBrowserSettingsSaveDefaults(Settings settings)
        {
            if (!PlatformHelper.IsBrowser || settings == null)
            {
                return;
            }

            settings.Username = string.Empty;
            settings.Password = string.Empty;
            settings.SaveAccount = false;
            settings.AutoLogin = false;
            settings.Reconnect = false;
            settings.ReconnectTime = 0;
            settings.LoginMusic = false;
            settings.LoginMusicVolume = 0;
            settings.LastServerNum = 0;
            settings.LastServerName = string.Empty;
            settings.OverrideFile = string.Empty;
            settings.ScreenScale = 1f;
            settings.IgnoreRelayIp = true;
            settings.RunMouseInASeparateThread = false;
            settings.FixedTimeStep = false;
            settings.FPS = 60;
            settings.IsWindowMaximized = false;
            settings.WindowPosition = new Point(0, 0);
            settings.WindowSize = new Point(1280, 720);
            settings.ProfilesPath = string.Empty;
        }

        public static void EnsureBrowserStorageBootstrap()
        {
            if (!PlatformHelper.IsBrowser)
            {
                return;
            }

            if (!BrowserFileSystemBootstrap.IsConfigured)
            {
                BrowserFileSystemBootstrap.ConfigureProvider(CreateDefaultBrowserStorageProvider());
                Log.Warn("Browser storage provider was not configured by the host. Using temporary in-memory browser storage.");
            }
        }

        public static void AttachBrowserStorageProvider(IBrowserBinaryAssetSource source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            BrowserFileSystemBootstrap.ConfigureProvider(CreateDefaultBrowserStorageProvider(source));
        }

        public static bool ShouldAllowWindowResizing()
        {
            return !PlatformHelper.IsBrowser;
        }

        public static bool ShouldPersistDesktopWindowState()
        {
            return !PlatformHelper.IsBrowser;
        }

        public static bool ShouldUseDesktopIdleSleep()
        {
            return !PlatformHelper.IsBrowser;
        }

        public static string GetCrashLogRootPath()
        {
            return PlatformHelper.IsBrowser
                ? BrowserVirtualPaths.CacheFile("logs")
                : Path.Combine(CUOEnviroment.ExecutablePath, "Logs");
        }

        public static string GetScreenshotRootPath()
        {
            return PlatformHelper.IsBrowser
                ? BrowserVirtualPaths.CacheFile("client")
                : Path.Combine(CUOEnviroment.ExecutablePath, "Data", "Client");
        }

        public static string GetClientDataRootPath()
        {
            return PlatformHelper.IsBrowser
                ? BrowserVirtualPaths.CacheFile("client")
                : Path.Combine(CUOEnviroment.ExecutablePath, "Data", "Client");
        }

        public static string GetSettingsFilePath()
        {
            return PlatformHelper.IsBrowser
                ? BrowserVirtualPaths.ConfigFile("settings.json")
                : Path.Combine(CUOEnviroment.ExecutablePath, "settings.json");
        }

        public static string GetProfilesRootPath()
        {
            return PlatformHelper.IsBrowser
                ? BrowserVirtualPaths.ProfilesRoot
                : Path.Combine(CUOEnviroment.ExecutablePath, "Data", "Profiles");
        }

        public static string GetUltimaOnlineDirectoryRoot()
        {
            return PlatformHelper.IsBrowser
                ? BrowserVirtualPaths.AssetsRoot
                : CUOEnviroment.ExecutablePath;
        }

        public static uint GetBrowserLocalIpDefault()
        {
            return _browserLocalIpOverride ?? 0x100007f;
        }

        public static bool ShouldUseBrowserLocalIpDefault()
        {
            return PlatformHelper.IsBrowser;
        }

        public static bool ShouldIgnoreDesktopStartupArgumentOverrides()
        {
            return PlatformHelper.IsBrowser;
        }

        public static bool ShouldIgnoreCustomSettingsPath()
        {
            return PlatformHelper.IsBrowser;
        }

        public static bool ShouldSkipDesktopPluginLoading()
        {
            return PlatformHelper.IsBrowser;
        }

        public static bool ShouldInitializeDesktopDllMap()
        {
            return !PlatformHelper.IsBrowser;
        }

        public static bool ShouldPersistDesktopProfileState()
        {
            return !PlatformHelper.IsBrowser;
        }

        public static bool ShouldEnableUltimaLive()
        {
            return !PlatformHelper.IsBrowser;
        }

        public static void ConfigureBrowserStorageProvider(IBrowserStorageProvider provider)
        {
            BrowserFileSystemBootstrap.ConfigureProvider(provider);
        }

        private static void EnsureBrowserClientVersionFromAssets()
        {
            if (!PlatformHelper.IsBrowser || Settings.GlobalSettings == null)
            {
                return;
            }

            var clientExePath = Path.Combine(Settings.GlobalSettings.UltimaOnlineDirectory, "client.exe");

            if (ClientVersionHelper.TryParseFromFile(clientExePath, out var clientVersionText) &&
                ClientVersionHelper.IsClientVersionValid(clientVersionText, out _))
            {
                if (!string.Equals(Settings.GlobalSettings.ClientVersion, clientVersionText, StringComparison.OrdinalIgnoreCase))
                {
                    Settings.GlobalSettings.ClientVersion = clientVersionText;
                }

                BrowserRuntimeStatusReporter.Report("browser-client-version", clientVersionText);
                return;
            }

            BrowserRuntimeStatusReporter.Report("browser-client-version", "unresolved");
            Log.Warn($"Browser startup: could not resolve client version from browser assets at {clientExePath}");
        }

        public static void ApplyBrowserLoginOverride()
        {
            Console.WriteLine("BROWSER_STATUS browser-login-override-enter|");

            if (!BrowserFileSystemBootstrap.IsConfigured)
            {
                Console.WriteLine("BROWSER_STATUS browser-login-override-storage-missing|");
                return;
            }

            string[] candidatePaths =
            {
                BrowserVirtualPaths.AssetFile("browser-login.json"),
                BrowserVirtualPaths.ConfigFile("browser-login.json")
            };

            string loginPath = null;
            foreach (string candidatePath in candidatePaths)
            {
                Console.WriteLine($"BROWSER_STATUS browser-login-override-check|{candidatePath}");

                if (FileSystemHelper.FileExists(candidatePath))
                {
                    loginPath = candidatePath;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(loginPath))
            {
                Console.WriteLine("BROWSER_STATUS browser-login-override-missing|");
                return;
            }

            try
            {
                string json = FileSystemHelper.ReadAllText(loginPath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    Console.WriteLine($"BROWSER_STATUS browser-login-override-empty|{loginPath}");
                    return;
                }

                string username = string.Empty;
                string password = string.Empty;

                using (JsonDocument document = JsonDocument.Parse(json))
                {
                    JsonElement root = document.RootElement;

                    if (root.TryGetProperty("username", out JsonElement usernameElement))
                    {
                        username = usernameElement.GetString() ?? string.Empty;
                    }

                    if (root.TryGetProperty("password", out JsonElement passwordElement))
                    {
                        password = passwordElement.GetString() ?? string.Empty;
                    }

                    if (root.TryGetProperty("localIp", out JsonElement localIpElement))
                    {
                        string localIp = localIpElement.GetString() ?? string.Empty;

                        if (IPAddress.TryParse(localIp, out IPAddress parsedLocalIp))
                        {
                            byte[] addressBytes = parsedLocalIp.MapToIPv4().GetAddressBytes();
                            _browserLocalIpOverride = (uint) (addressBytes[0]
                                | (addressBytes[1] << 8)
                                | (addressBytes[2] << 16)
                                | (addressBytes[3] << 24));
                            BrowserRuntimeStatusReporter.Report("browser-local-ip", localIp);
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.WriteLine($"BROWSER_STATUS browser-login-override-invalid|{loginPath}");
                    return;
                }

                Settings.GlobalSettings.Username = username;
                Settings.GlobalSettings.Password = Crypter.Encrypt(password);
                Settings.GlobalSettings.SaveAccount = false;
                Settings.GlobalSettings.AutoLogin = true;
                CUOEnviroment.SkipLoginScreen = true;
                Console.WriteLine($"BROWSER_STATUS browser-login-override-applied|{username}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BROWSER_STATUS browser-login-override-failed|{ex.Message}");
                Log.Warn($"Failed to apply browser login override: {ex.Message}");
            }
        }

        private static IBrowserStorageProvider CreateDefaultBrowserStorageProvider(IBrowserBinaryAssetSource assetsSource = null)
        {
            assetsSource ??= new BrowserMountedBinaryAssetSource(BrowserVirtualPaths.AssetsRoot);

            return BrowserFileSystemBootstrap.CreateRootedProvider(
                BrowserFileSystemBootstrap.CreateReadOnlyAssetProvider(assetsSource),
                new InMemoryBrowserStorageProvider(),
                new InMemoryBrowserStorageProvider(),
                new InMemoryBrowserStorageProvider()
            );
        }
    }

    internal sealed class BrowserRuntimeBootstrapState
    {
        public bool IsBrowser { get; set; }
        public bool StorageConfigured { get; set; }
        public string AssetsRootPath { get; set; } = string.Empty;
        public string ProfilesRootPath { get; set; } = string.Empty;
        public string CacheRootPath { get; set; } = string.Empty;
        public string ConfigRootPath { get; set; } = string.Empty;
        public string UltimaOnlineDirectory { get; set; } = string.Empty;
        public string ClientVersion { get; set; } = string.Empty;
    }

    internal sealed class BrowserLoginBootstrapOptions
    {
        [System.Text.Json.Serialization.JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("localIp")]
        public string LocalIp { get; set; } = string.Empty;
    }

}
