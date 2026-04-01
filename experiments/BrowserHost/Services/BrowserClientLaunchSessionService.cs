using System.Text.Json;
using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientLaunchSession
{
    ValueTask<BrowserClientLaunchSession> CreateLaunchSessionAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientLaunchSessionService : IBrowserClientLaunchSession
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly IBrowserClientStartupArtifacts _startupArtifacts;
    private readonly IBrowserClientStartupArtifactReader _startupArtifactReader;
    private readonly BrowserStorageService _storageService;

    public BrowserClientLaunchSessionService(
        IBrowserClientStartupArtifacts startupArtifacts,
        IBrowserClientStartupArtifactReader startupArtifactReader,
        BrowserStorageService storageService
    )
    {
        _startupArtifacts = startupArtifacts;
        _startupArtifactReader = startupArtifactReader;
        _storageService = storageService;
    }

    public async ValueTask<BrowserClientLaunchSession> CreateLaunchSessionAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        string normalizedProfileId = NormalizeProfileId(profileId);

        BrowserClientStartupArtifact artifact = await _startupArtifacts.CreateStartupArtifactAsync(request, normalizedProfileId);
        BrowserClientStartupArtifactRead artifactRead = await _startupArtifactReader.ReadStartupArtifactAsync(normalizedProfileId);

        BrowserClientLaunchSession session = new()
        {
            ProfileId = normalizedProfileId,
            ArtifactPath = artifact.ArtifactPath,
            ArtifactWriteSucceeded = artifact.WriteSucceeded,
            ArtifactReadSucceeded = artifactRead.ReadSucceeded,
            UsedExistingArtifact = false
        };

        if (!artifact.WriteSucceeded || !artifactRead.ReadSucceeded || !artifactRead.IsReady)
        {
            session.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            session.Summary = $"Launch session blocked for profile '{normalizedProfileId}'.";
            session.Error = string.IsNullOrWhiteSpace(artifactRead.Error) ? artifact.Error : artifactRead.Error;
            return session;
        }

        BrowserClientLaunchPlan launchPlan = artifactRead.StartupRun.LaunchPlan;
        string sessionId = $"session-{DateTimeOffset.UtcNow:yyyyMMddHHmmssfff}";
        string sessionPath = BrowserVirtualPaths.CacheFile($"startup/{normalizedProfileId}/launch-session.json");

        session.IsReady = true;
        session.SessionId = sessionId;
        session.SessionPath = sessionPath;
        session.AssetRootPath = launchPlan.AssetRootPath;
        session.ProfilesRootPath = launchPlan.ProfilesRootPath;
        session.CacheRootPath = launchPlan.CacheRootPath;
        session.ConfigRootPath = launchPlan.ConfigRootPath;
        session.SettingsFilePath = launchPlan.SettingsFilePath;
        session.StartupProfilePath = launchPlan.StartupProfilePath;
        session.ReadyAssetCount = launchPlan.ReadyAssetCount;
        session.CompletedSteps = artifactRead.CompletedSteps;
        session.TotalSteps = artifactRead.TotalSteps;
        session.RequiredAssets = launchPlan.Assets
            .Where(static x => x.IsRequired)
            .Select(static x => x.Path)
            .ToArray();

        string initialJson = JsonSerializer.Serialize(session, JsonOptions);
        BrowserFileWriteResult writeResult = await _storageService.WriteTextAsync(sessionPath, initialJson);

        if (!writeResult.Succeeded)
        {
            session.WriteSucceeded = false;
            session.IsReady = false;
            session.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            session.Summary = $"Launch session write failed for profile '{normalizedProfileId}'.";
            session.Error = writeResult.Error;
            return session;
        }

        session.WriteSucceeded = true;
        session.IsReady = true;
        session.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        session.Summary = $"Launch session '{sessionId}' created for profile '{normalizedProfileId}'.";
        session.Error = string.Empty;

        string finalJson = JsonSerializer.Serialize(session, JsonOptions);
        BrowserFileWriteResult finalWriteResult = await _storageService.WriteTextAsync(sessionPath, finalJson);

        if (!finalWriteResult.Succeeded)
        {
            session.WriteSucceeded = false;
            session.IsReady = false;
            session.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            session.Summary = $"Launch session rewrite failed for profile '{normalizedProfileId}'.";
            session.Error = finalWriteResult.Error;
        }

        return session;
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

public sealed class BrowserClientLaunchSession
{
    public bool IsReady { get; set; }
    public string ProfileId { get; set; } = "default";
    public string SessionId { get; set; } = string.Empty;
    public string SessionPath { get; set; } = string.Empty;
    public string ArtifactPath { get; set; } = string.Empty;
    public bool ArtifactWriteSucceeded { get; set; }
    public bool ArtifactReadSucceeded { get; set; }
    public bool UsedExistingArtifact { get; set; }
    public bool WriteSucceeded { get; set; }
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public int ReadyAssetCount { get; set; }
    public int CompletedSteps { get; set; }
    public int TotalSteps { get; set; }
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
