// SPDX-License-Identifier: BSD-2-Clause

using System.IO;

namespace ClassicUO.Utility.Platforms
{
    public interface IClassicUOFileSystem
    {
        bool FileExists(string path);
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        string[] GetFiles(string path);
        string[] GetFiles(string path, string searchPattern);
        Stream OpenRead(string path);
        Stream OpenReadWrite(string path);
        Stream OpenWrite(string path);
        Stream OpenAppend(string path);
        long GetFileLength(string path);
        void CopyFile(string sourcePath, string destinationPath, bool overwrite);
        void DeleteFile(string path);
        string ReadAllText(string path);
        void WriteAllText(string path, string contents);
    }
}
