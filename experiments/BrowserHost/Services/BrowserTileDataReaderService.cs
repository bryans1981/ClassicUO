using System.Text;

namespace BrowserHost.Services;

public sealed class BrowserTileDataReaderService
{
    private readonly BrowserAssetLoaderHarnessService _loaderHarness;

    public BrowserTileDataReaderService(BrowserAssetLoaderHarnessService loaderHarness)
    {
        _loaderHarness = loaderHarness;
    }

    public async ValueTask<BrowserTileDataReadResult> ReadAsync(string path)
    {
        BrowserTileDataReadResult result = new()
        {
            Path = path
        };

        var started = DateTime.UtcNow;

        try
        {
            BrowserAssetLoadResult<TileDataFirstLandParse> loadResult = await _loaderHarness.LoadParsedAsync(
                path,
                static bytes => bytes,
                static bytes => SelectBest(ParseFirstLand(bytes, isOldFormat: true), ParseFirstLand(bytes, isOldFormat: false)),
                $"tiledata-raw:{path}",
                $"tiledata-first-land:{path}"
            );

            result.Exists = loadResult.Exists;
            result.Length = loadResult.Length;

            if (!loadResult.Exists || !loadResult.HasValue)
            {
                result.TotalMs = (DateTime.UtcNow - started).TotalMilliseconds;
                return result;
            }

            TileDataFirstLandParse selected = loadResult.Value;

            result.IsOldFormat = selected.IsOldFormat;
            result.Header = selected.Header;
            result.FirstLandFlags = selected.Flags;
            result.FirstLandTextureId = selected.TextureId;
            result.FirstLandName = selected.Name;
            result.TotalMs = (DateTime.UtcNow - started).TotalMilliseconds;

            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex.Message;
            result.TotalMs = (DateTime.UtcNow - started).TotalMilliseconds;
            return result;
        }
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
