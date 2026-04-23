// SPDX-License-Identifier: BSD-2-Clause

using ClassicUO.Configuration;
using ClassicUO.Utility;
using ClassicUO.Utility.Platforms;
using ClassicUO.Utility.Logging;
using Microsoft.Xna.Framework;
using System;
using System.IO;

namespace ClassicUO
{
    internal static class BrowserRuntimeBootstrap
    {
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

            if (
                string.IsNullOrWhiteSpace(Settings.GlobalSettings.ClientVersion)
                || !ClientVersionHelper.IsClientVersionValid(Settings.GlobalSettings.ClientVersion, out _)
            )
            {
                Settings.GlobalSettings.ClientVersion = ClientVersionHelper.ToVersionString(ClientVersion.CV_7010400);
            }

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
            Settings.GlobalSettings.Encryption = 0;
            if (string.IsNullOrWhiteSpace(Settings.GlobalSettings.Language))
            {
                Settings.GlobalSettings.Language = "ENU";
            }
            Settings.GlobalSettings.ForceDriver = 0;
            Settings.GlobalSettings.MapsLayouts = string.Empty;
            Settings.GlobalSettings.WindowPosition = new Point(0, 0);
            Settings.GlobalSettings.WindowSize = new Point(1280, 720);
            Settings.GlobalSettings.IgnoreRelayIp = true;
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
            settings.Encryption = 0;
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
                Log.Warn("Browser storage provider is not configured yet. Browser startup will remain limited until the host attaches one.");
            }
        }

        public static void AttachBrowserStorageProvider(IBrowserBinaryAssetSource source)
        {
            BrowserFileSystemBootstrap.ConfigureReadOnlyAssetProvider(source);
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
            return 0x100007f;
        }

        public static bool ShouldIgnoreDesktopStartupArgumentOverrides()
        {
            return PlatformHelper.IsBrowser;
        }

        public static void ConfigureBrowserStorageProvider(IBrowserStorageProvider provider)
        {
            BrowserFileSystemBootstrap.ConfigureProvider(provider);
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

}
