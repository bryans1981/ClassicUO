// SPDX-License-Identifier: BSD-2-Clause

using ClassicUO.Utility.Platforms;
using System;
#if BROWSER_WASM
using System.Runtime.InteropServices.JavaScript;
#endif

namespace ClassicUO
{
    internal static partial class BrowserRuntimeStatusReporter
    {
        public static void Report(string stage, string detail = "")
        {
            if (!PlatformHelper.IsBrowser)
            {
                return;
            }

            try
            {
                Console.WriteLine($"BROWSER_STATUS {stage}|{detail ?? string.Empty}");
                PostStatus(stage ?? string.Empty, detail ?? string.Empty);
            }
            catch
            {
                // Best effort only. The browser shell uses this for diagnostics.
            }
        }

#if BROWSER_WASM
        [JSImport("classicuoPostStatus", "globalThis")]
        private static partial void PostStatus(string stage, string detail);
#else
        private static void PostStatus(string stage, string detail)
        {
        }
#endif
    }
}
