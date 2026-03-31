// SPDX-License-Identifier: BSD-2-Clause

using System.IO;

namespace ClassicUO.Utility.Platforms
{
    internal sealed class DesktopFileSystem : IClassicUOFileSystem
    {
        public bool FileExists(string path) => File.Exists(path);

        public bool DirectoryExists(string path) => Directory.Exists(path);

        public void CreateDirectory(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public string[] GetFiles(string path) => Directory.GetFiles(path);

        public string[] GetFiles(string path, string searchPattern) => Directory.GetFiles(path, searchPattern);

        public Stream OpenRead(string path) => File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        public Stream OpenReadWrite(string path) => File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

        public Stream OpenWrite(string path)
        {
            string directory = Path.GetDirectoryName(path);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read);
        }

        public Stream OpenAppend(string path)
        {
            string directory = Path.GetDirectoryName(path);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return File.Open(path, FileMode.Append, FileAccess.Write, FileShare.Read);
        }

        public long GetFileLength(string path) => new FileInfo(path).Length;

        public void CopyFile(string sourcePath, string destinationPath, bool overwrite) => File.Copy(sourcePath, destinationPath, overwrite);

        public void DeleteFile(string path) => File.Delete(path);

        public string ReadAllText(string path) => File.ReadAllText(path);

        public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);
    }
}
