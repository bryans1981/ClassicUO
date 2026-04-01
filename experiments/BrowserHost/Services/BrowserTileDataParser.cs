using System.Text;

namespace BrowserHost.Services;

internal static class BrowserTileDataParser
{
    public static BrowserTileDataReadResult ParseFirstLand(string path, byte[] bytes)
    {
        TileDataFirstLandParse selected = SelectBest(ParseFirstLand(bytes, isOldFormat: true), ParseFirstLand(bytes, isOldFormat: false));

        return new BrowserTileDataReadResult
        {
            Path = path,
            Exists = true,
            Length = bytes.Length,
            IsOldFormat = selected.IsOldFormat,
            Header = selected.Header,
            FirstLandFlags = selected.Flags,
            FirstLandTextureId = selected.TextureId,
            FirstLandName = selected.Name
        };
    }

    private static TileDataFirstLandParse SelectBest(TileDataFirstLandParse oldFormat, TileDataFirstLandParse newFormat)
    {
        bool oldLooksValid = LooksLikeTileName(oldFormat.Name);
        bool newLooksValid = LooksLikeTileName(newFormat.Name);

        if (oldLooksValid && !newLooksValid)
        {
            return oldFormat;
        }

        if (!oldLooksValid && newLooksValid)
        {
            return newFormat;
        }

        return oldFormat;
    }

    private static TileDataFirstLandParse ParseFirstLand(byte[] bytes, bool isOldFormat)
    {
        const int headerLength = 4;
        int flagsLength = isOldFormat ? 4 : 8;
        const int textureIdLength = 2;
        const int nameLength = 20;
        int minimumLength = headerLength + flagsLength + textureIdLength + nameLength;

        if (bytes.Length < minimumLength)
        {
            throw new InvalidDataException("tiledata.mul is smaller than the first land-tile record.");
        }

        using BinaryReader reader = new BinaryReader(new MemoryStream(bytes, writable: false), Encoding.UTF8, leaveOpen: false);
        uint header = reader.ReadUInt32();
        string flags = isOldFormat ? reader.ReadUInt32().ToString() : reader.ReadUInt64().ToString();
        ushort textureId = reader.ReadUInt16();
        byte[] nameBytes = reader.ReadBytes(20);

        if (nameBytes.Length != 20)
        {
            throw new EndOfStreamException("tiledata.mul ended before the first land-tile name was fully read.");
        }

        string name = Encoding.UTF8.GetString(nameBytes).TrimEnd('\0');

        return new TileDataFirstLandParse(isOldFormat, header, flags, textureId, name);
    }

    private static bool LooksLikeTileName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        foreach (char c in value.Trim())
        {
            if (char.IsLetterOrDigit(c))
            {
                continue;
            }

            switch (c)
            {
                case ' ':
                case '!':
                case '\'':
                case '(':
                case ')':
                case '.':
                case ',':
                case '_':
                case '-':
                    continue;
                default:
                    return false;
            }
        }

        return true;
    }

    private readonly record struct TileDataFirstLandParse(bool IsOldFormat, uint Header, string Flags, ushort TextureId, string Name);
}
