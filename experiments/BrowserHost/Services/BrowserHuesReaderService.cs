using System.Text;

namespace BrowserHost.Services;

public sealed class BrowserHuesReaderService
{
    private readonly BrowserAssetLoaderHarnessService _loaderHarness;

    public BrowserHuesReaderService(BrowserAssetLoaderHarnessService loaderHarness)
    {
        _loaderHarness = loaderHarness;
    }

    public async ValueTask<BrowserHuesReadResult> ReadAsync(string path)
    {
        BrowserHuesReadResult result = new()
        {
            Path = path
        };

        var started = DateTime.UtcNow;

        try
        {
            BrowserAssetLoadResult<HuesFirstEntryParse> loadResult = await _loaderHarness.LoadParsedAsync(
                path,
                static bytes => bytes,
                static bytes => ParseHues(bytes),
                $"hues-raw:{path}",
                $"hues-first-entry:{path}"
            );

            result.Exists = loadResult.Exists;
            result.Length = loadResult.Length;
            result.UsedParsedCache = loadResult.UsedParsedCache;

            if (!loadResult.Exists || !loadResult.HasValue)
            {
                result.TotalMs = (DateTime.UtcNow - started).TotalMilliseconds;
                return result;
            }

            HuesFirstEntryParse parsed = loadResult.Value;
            result.FirstGroupHeader = parsed.FirstGroupHeader;
            result.FirstPaletteColor16 = parsed.FirstPaletteColor16;
            result.FirstHueName = parsed.FirstHueName;
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

    private static HuesFirstEntryParse ParseHues(byte[] bytes)
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
        return new HuesFirstEntryParse(header, firstColor, name);
    }

    private readonly record struct HuesFirstEntryParse(uint FirstGroupHeader, ushort FirstPaletteColor16, string FirstHueName);
}
