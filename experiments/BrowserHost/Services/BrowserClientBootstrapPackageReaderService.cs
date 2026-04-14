using System.Text.Json;
using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientBootstrapPackageReader
{
    ValueTask<BrowserClientBootstrapPackageReadResult> ReadAsync(string profileId = "default");
}

public sealed class BrowserClientBootstrapPackageReaderService : IBrowserClientBootstrapPackageReader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly BrowserStorageService _storageService;

    public BrowserClientBootstrapPackageReaderService(BrowserStorageService storageService)
    {
        _storageService = storageService;
    }

    public async ValueTask<BrowserClientBootstrapPackageReadResult> ReadAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        string normalizedProfileId = NormalizeProfileId(profileId);
        string artifactPath = BrowserVirtualPaths.CacheFile($"startup/{normalizedProfileId}/browser-bootstrap-package.json");
        BrowserFileTextResult readResult = await _storageService.ReadTextAsync(artifactPath);

        BrowserClientBootstrapPackageReadResult result = new()
        {
            ProfileId = normalizedProfileId,
            ArtifactPath = artifactPath,
            Exists = readResult.Exists,
            ReadSucceeded = readResult.Exists && !string.IsNullOrWhiteSpace(readResult.Text),
            Length = readResult.Text.Length
        };

        if (!result.ReadSucceeded)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = result.Exists
                ? $"Browser bootstrap package read failed for profile '{normalizedProfileId}'."
                : $"Browser bootstrap package is missing for profile '{normalizedProfileId}'.";
            result.Error = readResult.Error;
            return result;
        }

        BrowserClientBootstrapPackage? package = JsonSerializer.Deserialize<BrowserClientBootstrapPackage>(readResult.Text, JsonOptions);
        result.IsReady = package is not null && string.Equals(package.PackageVersion, "browser-bootstrap-package-v1", StringComparison.Ordinal);
        result.Package = package;
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser bootstrap package read for profile '{normalizedProfileId}'."
            : $"Browser bootstrap package loaded but not validated for profile '{normalizedProfileId}'.";
        return result;
    }

    private static string NormalizeProfileId(string profileId)
    {
        if (string.IsNullOrWhiteSpace(profileId))
        {
            return "default";
        }

        char[] chars = profileId.Trim().Select(static c => char.IsLetterOrDigit(c) || c is '-' or '_' ? c : '-').ToArray();
        string normalized = new string(chars).Trim('-');
        return string.IsNullOrWhiteSpace(normalized) ? "default" : normalized;
    }
}

public sealed class BrowserClientBootstrapPackageReadResult
{
    public bool IsReady { get; set; }
    public string ProfileId { get; set; } = "default";
    public string ArtifactPath { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public bool ReadSucceeded { get; set; }
    public long Length { get; set; }
    public double TotalMs { get; set; }
    public BrowserClientBootstrapPackage? Package { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
