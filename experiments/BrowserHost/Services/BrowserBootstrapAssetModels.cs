namespace BrowserHost.Services;

public sealed class BrowserBootstrapAssetRequest
{
    public string TileDataPath { get; set; } = "/uo/tiledata.mul";
    public string ClilocPath { get; set; } = "/uo/cliloc.enu";
    public string HuesPath { get; set; } = "/uo/hues.mul";
}

public sealed class BrowserBootstrapAssetSnapshot
{
    public BrowserTileDataReadResult TileData { get; set; } = new();
    public BrowserClilocReadResult Cliloc { get; set; } = new();
    public BrowserHuesReadResult Hues { get; set; } = new();
    public int LoadedCount { get; set; }
    public int UsedCacheCount { get; set; }
    public double TotalMs { get; set; }
}
