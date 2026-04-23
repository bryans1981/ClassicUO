// SPDX-License-Identifier: BSD-2-Clause

using ClassicUO.Utility.Platforms;
using System;
using System.IO;
using System.Text;

namespace ClassicUO.Utility
{
    public static class FileSystemHelper
    {
        public static IClassicUOFileSystem Current { get; } = BrowserFileSystemBootstrap.ShouldUseBrowserFileSystem()
            ? new BrowserFileSystem()
            : new DesktopFileSystem();

        public static bool FileExists(string path) => Current.FileExists(path);

        public static bool DirectoryExists(string path) => Current.DirectoryExists(path);

        public static void CreateDirectory(string path) => Current.CreateDirectory(path);

        public static string[] GetFiles(string path) => Current.GetFiles(path);

        public static string[] GetFiles(string path, string searchPattern) => Current.GetFiles(path, searchPattern);

        public static Stream OpenRead(string path) => Current.OpenRead(path);

        public static Stream OpenReadWrite(string path) => Current.OpenReadWrite(path);

        public static Stream OpenWrite(string path) => Current.OpenWrite(path);

        public static Stream OpenAppend(string path) => Current.OpenAppend(path);

        public static long GetFileLength(string path) => Current.GetFileLength(path);

        public static void CopyFile(string sourcePath, string destinationPath, bool overwrite = true) => Current.CopyFile(sourcePath, destinationPath, overwrite);

        public static void DeleteFile(string path) => Current.DeleteFile(path);

        public static string ReadAllText(string path) => Current.ReadAllText(path);

        public static void WriteAllText(string path, string contents) => Current.WriteAllText(path, contents);

        public static string CreateFolderIfNotExists(string path, params string[] parts)
        {
            if (!DirectoryExists(path))
            {
                CreateDirectory(path);
            }

            char[] invalid = Path.GetInvalidFileNameChars();

            for (int i = 0; i < parts.Length; i++)
            {
                for (int j = 0; j < invalid.Length; j++)
                {
                    parts[i] = parts[i].Replace(invalid[j].ToString(), "");
                }
            }

            StringBuilder sb = new StringBuilder();

            foreach (string part in parts)
            {
                sb.Append(Path.Combine(path, part));

                string r = sb.ToString();

                if (!DirectoryExists(r))
                {
                    CreateDirectory(r);
                }

                path = r;
                sb.Clear();
            }

            return path;
        }

        public static void EnsureFileExists(string path)
        {
            if (!FileExists(path))
            {
                throw new FileNotFoundException(path);
            }
        }

        public static void CopyAllTo(this DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);

                diSourceSubDir.CopyAllTo(nextTargetSubDir);
            }
        }
    }
}
