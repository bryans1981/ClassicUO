using System.Buffers.Binary;
using System.Text;

namespace BrowserHost.Services;

public sealed class BrowserAssetSourceService
{
    private readonly BrowserStorageService _storageService;

    public BrowserAssetSourceService(BrowserStorageService storageService)
    {
        _storageService = storageService;
    }

    public ValueTask<bool> FileExistsAsync(string path)
    {
        return _storageService.FileExistsAsync(path);
    }

    public async ValueTask<int> GetFileLengthAsync(string path)
    {
        BrowserFileBytesResult result = await _storageService.ReadBytesBase64Async(path);
        return result.Exists ? result.Length : 0;
    }

    public async ValueTask<byte[]> ReadAllBytesAsync(string path)
    {
        BrowserFileBytesResult result = await _storageService.ReadBytesBase64Async(path);

        if (!result.Exists)
        {
            throw new FileNotFoundException(path);
        }

        return string.IsNullOrEmpty(result.Base64) ? Array.Empty<byte>() : Convert.FromBase64String(result.Base64);
    }
}

public sealed class TileDataProbeService
{
    private readonly BrowserAssetSourceService _assetSourceService;

    public TileDataProbeService(BrowserAssetSourceService assetSourceService)
    {
        _assetSourceService = assetSourceService;
    }

    public async ValueTask<BrowserTileDataProbeResult> ProbeAsync(string path)
    {
        BrowserTileDataProbeResult result = new()
        {
            Path = path
        };

        var started = DateTime.UtcNow;

        try
        {
            result.Exists = await _assetSourceService.FileExistsAsync(path);

            if (!result.Exists)
            {
                return result;
            }

            byte[] bytes = await _assetSourceService.ReadAllBytesAsync(path);
            result.Length = bytes.Length;

            TileDataFirstLandParse oldFormat = ParseFirstLand(bytes, isOldFormat: true);
            TileDataFirstLandParse newFormat = ParseFirstLand(bytes, isOldFormat: false);
            TileDataFirstLandParse selected = SelectBest(oldFormat, newFormat);

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
        ReadOnlySpan<byte> span = bytes;
        const int headerLength = 4;
        int flagsLength = isOldFormat ? 4 : 8;
        const int textureIdLength = 2;
        const int nameLength = 20;
        int minimumLength = headerLength + flagsLength + textureIdLength + nameLength;

        if (span.Length < minimumLength)
        {
            throw new InvalidDataException("tiledata.mul is smaller than the first land-tile record.");
        }

        uint header = BinaryPrimitives.ReadUInt32LittleEndian(span[..4]);
        int offset = 4;
        string flags;

        if (isOldFormat)
        {
            flags = BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(offset, 4)).ToString();
            offset += 4;
        }
        else
        {
            flags = BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(offset, 8)).ToString();
            offset += 8;
        }

        ushort textureId = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(offset, 2));
        offset += 2;
        string name = Encoding.UTF8.GetString(span.Slice(offset, 20)).TrimEnd('\0');

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
