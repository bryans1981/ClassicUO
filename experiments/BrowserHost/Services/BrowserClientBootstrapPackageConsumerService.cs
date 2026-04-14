using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientBootstrapPackageConsumer
{
    ValueTask<BrowserClientBootstrapPackageConsumerResult> ConsumeAsync(string profileId = "default");
}

public sealed class BrowserClientBootstrapPackageConsumerService : IBrowserClientBootstrapPackageConsumer
{
    private readonly IBrowserClientBootstrapPackageReader _bootstrapPackageReader;

    public BrowserClientBootstrapPackageConsumerService(IBrowserClientBootstrapPackageReader bootstrapPackageReader)
    {
        _bootstrapPackageReader = bootstrapPackageReader;
    }

    public async ValueTask<BrowserClientBootstrapPackageConsumerResult> ConsumeAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientBootstrapPackageReadResult packageRead = await _bootstrapPackageReader.ReadAsync(profileId);

        BrowserClientStartupAsset[] startupAssets = packageRead.Package is null
            ? Array.Empty<BrowserClientStartupAsset>()
            : packageRead.Package.RequiredAssets.Select(
                assetPath => new BrowserClientStartupAsset
                {
                    Id = GetAssetId(assetPath),
                    Path = assetPath,
                    Exists = true,
                    ReadSucceeded = true,
                    LoadedOnDemand = false,
                    UsedParsedCache = false,
                    Length = 0,
                    IsRequired = true,
                    Summary = assetPath
                }
            ).ToArray();

        bool isReady = packageRead.IsReady && startupAssets.Length == 3;

        return new BrowserClientBootstrapPackageConsumerResult
        {
            IsReady = isReady,
            ProfileId = packageRead.ProfileId,
            PackageVersion = packageRead.Package?.PackageVersion ?? string.Empty,
            ArtifactPath = packageRead.ArtifactPath,
            ReadSucceeded = packageRead.ReadSucceeded,
            RequiredAssetCount = startupAssets.Length,
            Assets = startupAssets,
            TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds,
            Summary = isReady
                ? $"Browser bootstrap package consumed for profile '{packageRead.ProfileId}'."
                : $"Browser bootstrap package consumption blocked for profile '{packageRead.ProfileId}'.",
            Error = packageRead.Error
        };
    }

    private static string GetAssetId(string assetPath)
    {
        string fileName = Path.GetFileNameWithoutExtension(assetPath);
        return string.IsNullOrWhiteSpace(fileName) ? assetPath : fileName;
    }
}

public sealed class BrowserClientBootstrapPackageConsumerResult
{
    public bool IsReady { get; set; }
    public string ProfileId { get; set; } = "default";
    public string PackageVersion { get; set; } = string.Empty;
    public string ArtifactPath { get; set; } = string.Empty;
    public bool ReadSucceeded { get; set; }
    public int RequiredAssetCount { get; set; }
    public BrowserClientStartupAsset[] Assets { get; set; } = Array.Empty<BrowserClientStartupAsset>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
