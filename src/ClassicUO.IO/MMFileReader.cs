using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using ClassicUO.Utility.Platforms;

namespace ClassicUO.IO
{
    public class MMFileReader : FileReader
    {
        private readonly MemoryMappedViewAccessor _accessor;
        private readonly MemoryMappedFile _mmf;
        private readonly BinaryReader _file;

        public MMFileReader(Stream stream, string filePath = null) : base(stream, filePath)
        {
            if (Length <= 0)
                return;

            if (stream is FileStream fileStream && !PlatformHelper.IsBrowser)
            {
                _mmf = MemoryMappedFile.CreateFromFile
                (
                    fileStream,
                    null,
                    0,
                    MemoryMappedFileAccess.Read,
                    HandleInheritability.None,
                    false
                );

                _accessor = _mmf.CreateViewAccessor(0, Length, MemoryMappedFileAccess.Read);

                try
                {
                    unsafe
                    {
                        byte* ptr = null;
                        _accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
                        _file = new BinaryReader(new UnmanagedMemoryStream(ptr, Length));
                    }
                }
                catch (Exception ex)
                {
                    _accessor.SafeMemoryMappedViewHandle.ReleasePointer();

                    throw new InvalidOperationException("Failed to acquire memory-mapped file pointer.", ex);
                }
            }
            else
            {
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }

                _file = new BinaryReader(stream);
            }
        }

        public override BinaryReader Reader => _file;

        public override void Dispose()
        {
            _accessor?.SafeMemoryMappedViewHandle.ReleasePointer();
            _accessor?.Dispose();
            _mmf?.Dispose();

            base.Dispose();
        }
    }
}
