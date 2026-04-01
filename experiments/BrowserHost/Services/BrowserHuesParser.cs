using System.Text;

namespace BrowserHost.Services;

internal static class BrowserHuesParser
{
    public static BrowserHuesReadResult ParseFirstGroup(string path, byte[] bytes)
    {
        const int colorTableEntries = 32;
        const int colorTableSize = colorTableEntries * 2;
        const int blockFooterSize = 2 + 2 + 20;
        const int huesBlockSize = colorTableSize + blockFooterSize;
        const int huesGroupSize = 4 + (8 * huesBlockSize);

        if (bytes.Length < huesGroupSize)
        {
            throw new InvalidDataException("hues.mul is smaller than the first hues group.");
        }

        using BinaryReader reader = new BinaryReader(new MemoryStream(bytes, writable: false), Encoding.UTF8, leaveOpen: false);
        uint header = reader.ReadUInt32();
        ushort firstColor = reader.ReadUInt16();
        reader.BaseStream.Position = 4 + colorTableSize + 2 + 2;
        byte[] nameBytes = reader.ReadBytes(20);

        if (nameBytes.Length != 20)
        {
            throw new EndOfStreamException("hues.mul ended before the first hue name was fully read.");
        }

        string name = Encoding.UTF8.GetString(nameBytes).TrimEnd('\0');

        return new BrowserHuesReadResult
        {
            Path = path,
            Exists = true,
            Length = bytes.Length,
            FirstGroupHeader = header,
            FirstPaletteColor16 = firstColor,
            FirstHueName = name
        };
    }
}
