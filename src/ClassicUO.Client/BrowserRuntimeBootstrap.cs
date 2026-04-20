// SPDX-License-Identifier: BSD-2-Clause

using ClassicUO.Configuration;
using ClassicUO.Utility;
using ClassicUO.Utility.Platforms;
using ClassicUO.Utility.Logging;
using System;

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

        public static BrowserRuntimePolicy GetRuntimePolicy()
        {
            return PlatformHelper.IsBrowser
                ? BrowserRuntimePolicy.BrowserDefault
                : BrowserRuntimePolicy.DesktopDefault;
        }

        public static void ApplyBrowserStartupDefaults()
        {
            if (!PlatformHelper.IsBrowser)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(Settings.GlobalSettings.UltimaOnlineDirectory))
            {
                Settings.GlobalSettings.UltimaOnlineDirectory = BrowserVirtualPaths.AssetsRoot;
            }

            if (string.IsNullOrWhiteSpace(Settings.GlobalSettings.ClientVersion))
            {
                Settings.GlobalSettings.ClientVersion = ClientVersionHelper.ToVersionString(ClientVersion.CV_7010400);
            }
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

    internal sealed class BrowserRuntimePolicy
    {
        public static BrowserRuntimePolicy BrowserDefault { get; } = new BrowserRuntimePolicy
        {
            UseSeparateMouseThread = false,
            FixedTimeStep = false,
            TargetFps = 60
        };

        public static BrowserRuntimePolicy DesktopDefault { get; } = new BrowserRuntimePolicy
        {
            UseSeparateMouseThread = Settings.GlobalSettings?.RunMouseInASeparateThread ?? true,
            FixedTimeStep = Settings.GlobalSettings?.FixedTimeStep ?? false,
            TargetFps = Settings.GlobalSettings?.FPS > 0 ? Settings.GlobalSettings.FPS : 60
        };

        public bool UseSeparateMouseThread { get; set; }
        public bool FixedTimeStep { get; set; }
        public int TargetFps { get; set; }
    }
}
