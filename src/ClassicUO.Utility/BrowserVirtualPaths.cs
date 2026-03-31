// SPDX-License-Identifier: BSD-2-Clause

using System;
using System.Collections.Generic;

namespace ClassicUO.Utility
{
    public enum BrowserVirtualRoot
    {
        Unknown = 0,
        Assets,
        Profiles,
        Cache,
        Config
    }

    public readonly struct BrowserVirtualPathInfo
    {
        public BrowserVirtualPathInfo(string fullPath, BrowserVirtualRoot root, string relativePath)
        {
            FullPath = fullPath ?? BrowserVirtualPaths.Root;
            Root = root;
            RelativePath = relativePath ?? string.Empty;
        }

        public string FullPath { get; }
        public BrowserVirtualRoot Root { get; }
        public string RelativePath { get; }
        public bool IsKnownRoot => Root != BrowserVirtualRoot.Unknown;
    }

    public static class BrowserVirtualPaths
    {
        public const string Root = "/";
        public const string AssetsRoot = "/uo";
        public const string ProfilesRoot = "/profiles";
        public const string CacheRoot = "/cache";
        public const string ConfigRoot = "/config";

        public static string Normalize(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return Root;
            }

            string normalized = path.Replace('\\', '/').Trim();
            string[] segments = normalized.Split('/', StringSplitOptions.RemoveEmptyEntries);
            List<string> stack = new List<string>(segments.Length);

            foreach (string segment in segments)
            {
                if (segment == ".")
                {
                    continue;
                }

                if (segment == "..")
                {
                    if (stack.Count > 0)
                    {
                        stack.RemoveAt(stack.Count - 1);
                    }

                    continue;
                }

                stack.Add(segment);
            }

            return stack.Count == 0 ? Root : Root + string.Join('/', stack);
        }

        public static string Combine(params string[] parts)
        {
            if (parts is null || parts.Length == 0)
            {
                return Root;
            }

            List<string> segments = new List<string>(parts.Length);

            foreach (string part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    continue;
                }

                string normalizedPart = part.Replace('\\', '/').Trim('/');

                if (normalizedPart.Length == 0)
                {
                    continue;
                }

                segments.Add(normalizedPart);
            }

            return segments.Count == 0 ? Root : Normalize(Root + string.Join('/', segments));
        }

        public static string AssetFile(string relativePath) => Combine(AssetsRoot, relativePath);

        public static string ProfileFile(string profileId, string relativePath) => Combine(ProfilesRoot, profileId, relativePath);

        public static string CacheFile(string relativePath) => Combine(CacheRoot, relativePath);

        public static string ConfigFile(string relativePath) => Combine(ConfigRoot, relativePath);

        public static BrowserVirtualPathInfo Classify(string path)
        {
            string normalizedPath = Normalize(path);

            if (TryGetRelativePath(normalizedPath, AssetsRoot, out string assetsRelativePath))
            {
                return new BrowserVirtualPathInfo(normalizedPath, BrowserVirtualRoot.Assets, assetsRelativePath);
            }

            if (TryGetRelativePath(normalizedPath, ProfilesRoot, out string profilesRelativePath))
            {
                return new BrowserVirtualPathInfo(normalizedPath, BrowserVirtualRoot.Profiles, profilesRelativePath);
            }

            if (TryGetRelativePath(normalizedPath, CacheRoot, out string cacheRelativePath))
            {
                return new BrowserVirtualPathInfo(normalizedPath, BrowserVirtualRoot.Cache, cacheRelativePath);
            }

            if (TryGetRelativePath(normalizedPath, ConfigRoot, out string configRelativePath))
            {
                return new BrowserVirtualPathInfo(normalizedPath, BrowserVirtualRoot.Config, configRelativePath);
            }

            return new BrowserVirtualPathInfo(normalizedPath, BrowserVirtualRoot.Unknown, normalizedPath.TrimStart('/'));
        }

        public static bool IsUnderRoot(string path, string root)
        {
            string normalizedPath = Normalize(path);
            string normalizedRoot = Normalize(root);

            return TryGetRelativePath(normalizedPath, normalizedRoot, out _);
        }

        private static bool TryGetRelativePath(string path, string root, out string relativePath)
        {
            string normalizedPath = Normalize(path);
            string normalizedRoot = Normalize(root);

            if (string.Equals(normalizedPath, normalizedRoot, StringComparison.OrdinalIgnoreCase))
            {
                relativePath = string.Empty;
                return true;
            }

            if (normalizedPath.StartsWith(normalizedRoot + "/", StringComparison.OrdinalIgnoreCase))
            {
                relativePath = normalizedPath[(normalizedRoot.Length + 1)..];
                return true;
            }

            relativePath = string.Empty;
            return false;
        }
    }
}
