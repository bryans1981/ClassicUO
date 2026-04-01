using System.Text.Json;

namespace BrowserHost.Services;

public interface IBrowserClientStartupArtifactReader
{
    ValueTask<BrowserClientStartupArtifactRead> ReadStartupArtifactAsync(string profileId = "default");
}

public sealed class BrowserClientStartupArtifactReaderService : IBrowserClientStartupArtifactReader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly BrowserStorageService _storageService;

    public BrowserClientStartupArtifactReaderService(BrowserStorageService storageService)
    {
        _storageService = storageService;
    }

    public async ValueTask<BrowserClientStartupArtifactRead> ReadStartupArtifactAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        string normalizedProfileId = NormalizeProfileId(profileId);
        string artifactPath = $"/cache/startup/{normalizedProfileId}/launch-plan.json";

        BrowserFileTextResult readResult = await _storageService.ReadTextAsync(artifactPath);
        BrowserClientStartupArtifactRead result = new()
        {
            ProfileId = normalizedProfileId,
            ArtifactPath = artifactPath,
            Exists = readResult.Exists
        };

        if (!readResult.Exists)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"No startup artifact found at {artifactPath}.";
            result.Error = readResult.Error;
            return result;
        }

        try
        {
            BrowserClientStartupRun? startupRun = JsonSerializer.Deserialize<BrowserClientStartupRun>(readResult.Text, JsonOptions);

            if (startupRun is null)
            {
                result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
                result.Summary = $"Startup artifact at {artifactPath} could not be deserialized.";
                result.Error = "Startup artifact deserialized to null.";
                return result;
            }

            result.ReadSucceeded = true;
            result.StartupRun = startupRun;
            result.IsReady = startupRun.IsReady;
            result.Length = readResult.Text.Length;
            result.CompletedSteps = startupRun.CompletedSteps;
            result.TotalSteps = startupRun.TotalSteps;
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = startupRun.IsReady
                ? $"Startup artifact loaded for profile '{normalizedProfileId}'."
                : $"Startup artifact loaded but not ready for profile '{normalizedProfileId}'.";
            return result;
        }
        catch (Exception ex)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Startup artifact read failed for profile '{normalizedProfileId}'.";
            result.Error = ex.Message;
            return result;
        }
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

public sealed class BrowserClientStartupArtifactRead
{
    public string ProfileId { get; set; } = "default";
    public string ArtifactPath { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public bool ReadSucceeded { get; set; }
    public bool IsReady { get; set; }
    public int Length { get; set; }
    public int CompletedSteps { get; set; }
    public int TotalSteps { get; set; }
    public BrowserClientStartupRun StartupRun { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
