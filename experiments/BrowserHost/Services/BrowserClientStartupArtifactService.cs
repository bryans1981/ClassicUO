using System.Text.Json;
using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientStartupArtifacts
{
    ValueTask<BrowserClientStartupArtifact> CreateStartupArtifactAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientStartupArtifactService : IBrowserClientStartupArtifacts
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly IBrowserClientStartupIntegration _startupIntegration;
    private readonly BrowserStorageService _storageService;

    public BrowserClientStartupArtifactService(
        IBrowserClientStartupIntegration startupIntegration,
        BrowserStorageService storageService
    )
    {
        _startupIntegration = startupIntegration;
        _storageService = storageService;
    }

    public async ValueTask<BrowserClientStartupArtifact> CreateStartupArtifactAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientStartupRun startupRun = await _startupIntegration.PrepareStartupRunAsync(request, profileId);
        string artifactPath = BrowserVirtualPaths.CacheFile($"startup/{startupRun.ProfileId}/launch-plan.json");

        string json = JsonSerializer.Serialize(startupRun, JsonOptions);
        BrowserFileWriteResult writeResult = await _storageService.WriteTextAsync(artifactPath, json);

        return new BrowserClientStartupArtifact
        {
            IsReady = startupRun.IsReady && writeResult.Succeeded,
            ArtifactPath = artifactPath,
            ProfileId = startupRun.ProfileId,
            WriteSucceeded = writeResult.Succeeded,
            Length = writeResult.Length,
            StartupRun = startupRun,
            TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds,
            Summary = writeResult.Succeeded
                ? $"Startup artifact written to {artifactPath}."
                : $"Startup artifact write failed for {artifactPath}.",
            Error = writeResult.Error
        };
    }
}

public sealed class BrowserClientStartupArtifact
{
    public bool IsReady { get; set; }
    public string ArtifactPath { get; set; } = string.Empty;
    public string ProfileId { get; set; } = "default";
    public bool WriteSucceeded { get; set; }
    public int Length { get; set; }
    public BrowserClientStartupRun StartupRun { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
