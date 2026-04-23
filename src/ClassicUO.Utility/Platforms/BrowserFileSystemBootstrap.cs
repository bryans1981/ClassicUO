// SPDX-License-Identifier: BSD-2-Clause

using System;
using ClassicUO.Utility.Platforms;

namespace ClassicUO.Utility.Platforms
{
    public static class BrowserFileSystemBootstrap
    {
        public static bool ShouldUseBrowserFileSystem()
        {
            return PlatformHelper.IsBrowser;
        }

        public static bool IsConfigured => BrowserFileSystem.IsProviderConfigured;

        public static IBrowserStorageProvider CreateReadOnlyAssetProvider(IBrowserBinaryAssetSource source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new BrowserBinaryAssetStorageProvider(source);
        }

        public static IBrowserStorageProvider CreateRootedProvider
        (
            IBrowserStorageProvider assetsProvider,
            IBrowserStorageProvider profilesProvider,
            IBrowserStorageProvider cacheProvider,
            IBrowserStorageProvider configProvider
        )
        {
            return new RootedBrowserStorageProvider(assetsProvider, profilesProvider, cacheProvider, configProvider);
        }

        public static void ConfigureProvider(IBrowserStorageProvider provider)
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            BrowserFileSystem.SetProvider(provider);
        }

        public static void ConfigureReadOnlyAssetProvider(IBrowserBinaryAssetSource source)
        {
            ConfigureProvider(CreateReadOnlyAssetProvider(source));
        }
    }
}
