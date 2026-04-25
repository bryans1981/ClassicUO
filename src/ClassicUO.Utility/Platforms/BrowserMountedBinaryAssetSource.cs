// SPDX-License-Identifier: BSD-2-Clause

using System;
using System.IO;
using System.Linq;
using ClassicUO.Utility;

namespace ClassicUO.Utility.Platforms
{
    public sealed class BrowserMountedBinaryAssetSource : IBrowserBinaryAssetSource
    {
        private readonly string _rootPath;

        public BrowserMountedBinaryAssetSource(string rootPath)
        {
            if (string.IsNullOrWhiteSpace(rootPath))
            {
                throw new ArgumentException("Root path cannot be empty.", nameof(rootPath));
            }

            _rootPath = BrowserFileSystem.NormalizePath(rootPath);
        }

        public bool FileExists(string path)
        {
            return File.Exists(GetAbsolutePath(path));
        }

        public string[] GetFiles(string path)
        {
            string absolutePath = GetAbsolutePath(path);

            if (!Directory.Exists(absolutePath))
            {
                return Array.Empty<string>();
            }

            SearchOption searchOption = string.Equals(absolutePath, _rootPath, StringComparison.OrdinalIgnoreCase)
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;

            return Directory.GetFiles(absolutePath, "*", searchOption)
                            .OrderBy(filePath => filePath, StringComparer.OrdinalIgnoreCase)
                            .Select(ToBrowserPath)
                            .ToArray();
        }

        public string[] GetFiles(string path, string searchPattern)
        {
            return GetFiles(path)
                   .Where(filePath => MatchesSearchPattern(filePath, searchPattern))
                   .ToArray();
        }

        public Stream OpenReadStream(string path)
        {
            return File.OpenRead(GetAbsolutePath(path));
        }

        public bool TryReadFile(string path, out ReadOnlyMemory<byte> bytes)
        {
            string absolutePath = GetAbsolutePath(path);

            if (!File.Exists(absolutePath))
            {
                bytes = default;
                return false;
            }

            bytes = File.ReadAllBytes(absolutePath);
            return true;
        }

        private string GetAbsolutePath(string path)
        {
            string normalizedPath = BrowserFileSystem.NormalizePath(path);

            if (BrowserVirtualPaths.IsUnderRoot(normalizedPath, _rootPath))
            {
                return normalizedPath;
            }

            return BrowserVirtualPaths.Combine(_rootPath, normalizedPath.TrimStart('/'));
        }

        private static string ToBrowserPath(string path)
        {
            return BrowserFileSystem.NormalizePath(path);
        }

        private static bool MatchesSearchPattern(string path, string searchPattern)
        {
            string fileName = Path.GetFileName(path);

            if (string.IsNullOrWhiteSpace(searchPattern) || searchPattern == "*")
            {
                return true;
            }

            if (searchPattern.StartsWith("*.", StringComparison.Ordinal))
            {
                string extension = searchPattern[1..];
                return fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase);
            }

            return string.Equals(fileName, searchPattern, StringComparison.OrdinalIgnoreCase);
        }
    }
}
