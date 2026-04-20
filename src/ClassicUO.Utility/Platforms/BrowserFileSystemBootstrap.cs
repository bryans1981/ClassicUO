// SPDX-License-Identifier: BSD-2-Clause

using System;

namespace ClassicUO.Utility.Platforms
{
    public static class BrowserFileSystemBootstrap
    {
        public static bool IsConfigured => BrowserFileSystem.IsProviderConfigured;

        public static void ConfigureProvider(IBrowserStorageProvider provider)
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            BrowserFileSystem.SetProvider(provider);
        }
    }
}
