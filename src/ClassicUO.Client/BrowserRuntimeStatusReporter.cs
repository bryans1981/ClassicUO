// SPDX-License-Identifier: BSD-2-Clause

using ClassicUO.Utility.Platforms;
using System;

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

            Console.WriteLine($"BROWSER_STATUS {stage}|{detail ?? string.Empty}");
        }
    }
}
