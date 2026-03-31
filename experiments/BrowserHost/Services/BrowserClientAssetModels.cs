namespace BrowserHost.Services;

public sealed class BrowserClientTileDataDetails
{
    public bool Exists { get; set; }
    public int Length { get; set; }
    public bool IsOldFormat { get; set; }
    public uint Header { get; set; }
    public string FirstLandFlags { get; set; } = string.Empty;
    public int FirstLandTextureId { get; set; }
    public string FirstLandName { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserClientClilocDetails
{
    public bool Exists { get; set; }
    public int Length { get; set; }
    public bool UsedProcessedCache { get; set; }
    public int Header1 { get; set; }
    public short Header2 { get; set; }
    public int FirstEntryNumber { get; set; }
    public byte FirstEntryFlag { get; set; }
    public short FirstEntryTextLength { get; set; }
    public string FirstEntryText { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Error { get; set; } = string.Empty;
}

public sealed class BrowserClientHuesDetails
{
    public bool Exists { get; set; }
    public int Length { get; set; }
    public bool UsedParsedCache { get; set; }
    public uint FirstGroupHeader { get; set; }
    public int FirstPaletteColor16 { get; set; }
    public string FirstHueName { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Error { get; set; } = string.Empty;
}
