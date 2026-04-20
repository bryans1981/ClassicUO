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

        public static void ConfigureBrowserStorageProvider(IBrowserStorageProvider provider)
        {
            BrowserFileSystemBootstrap.ConfigureProvider(provider);
        }
    }
}
