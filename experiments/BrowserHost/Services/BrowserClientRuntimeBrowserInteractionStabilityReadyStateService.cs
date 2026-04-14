namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserInteractionStabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserInteractionStabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserInteractionStabilityReadyStateService : IBrowserClientRuntimeBrowserInteractionStabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserInteractionStabilitySession _runtimeBrowserInteractionStabilitySession;

    public BrowserClientRuntimeBrowserInteractionStabilityReadyStateService(IBrowserClientRuntimeBrowserInteractionStabilitySession runtimeBrowserInteractionStabilitySession)
    {
        _runtimeBrowserInteractionStabilitySession = runtimeBrowserInteractionStabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserInteractionStabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserInteractionStabilitySessionResult interactionstabilitySession = await _runtimeBrowserInteractionStabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserInteractionStabilityReadyStateResult result = new()
        {
            ProfileId = interactionstabilitySession.ProfileId,
            SessionId = interactionstabilitySession.SessionId,
            SessionPath = interactionstabilitySession.SessionPath,
            BrowserInteractionStabilitySessionVersion = interactionstabilitySession.BrowserInteractionStabilitySessionVersion,
            BrowserStateStabilityReadyStateVersion = interactionstabilitySession.BrowserStateStabilityReadyStateVersion,
            BrowserStateStabilitySessionVersion = interactionstabilitySession.BrowserStateStabilitySessionVersion,
            LaunchMode = interactionstabilitySession.LaunchMode,
            AssetRootPath = interactionstabilitySession.AssetRootPath,
            ProfilesRootPath = interactionstabilitySession.ProfilesRootPath,
            CacheRootPath = interactionstabilitySession.CacheRootPath,
            ConfigRootPath = interactionstabilitySession.ConfigRootPath,
            SettingsFilePath = interactionstabilitySession.SettingsFilePath,
            StartupProfilePath = interactionstabilitySession.StartupProfilePath,
            RequiredAssets = interactionstabilitySession.RequiredAssets,
            ReadyAssetCount = interactionstabilitySession.ReadyAssetCount,
            CompletedSteps = interactionstabilitySession.CompletedSteps,
            TotalSteps = interactionstabilitySession.TotalSteps,
            Exists = interactionstabilitySession.Exists,
            ReadSucceeded = interactionstabilitySession.ReadSucceeded
        };

        if (!interactionstabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser interactionstability ready state blocked for profile '{interactionstabilitySession.ProfileId}'.";
            result.Error = interactionstabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserInteractionStabilityReadyStateVersion = "runtime-browser-interactionstability-ready-state-v1";
        result.BrowserInteractionStabilityReadyChecks =
        [
            "browser-statestability-ready-state-ready",
            "browser-interactionstability-session-ready",
            "browser-interactionstability-ready"
        ];
        result.BrowserInteractionStabilityReadySummary = $"Runtime browser interactionstability ready state passed {result.BrowserInteractionStabilityReadyChecks.Length} interactionstability readiness check(s) for profile '{interactionstabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser interactionstability ready state ready for profile '{interactionstabilitySession.ProfileId}' with {result.BrowserInteractionStabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserInteractionStabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserInteractionStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInteractionStabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserStateStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserStateStabilitySessionVersion { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ProfileId { get; set; } = "default";
    public string SessionId { get; set; } = string.Empty;
    public string SessionPath { get; set; } = string.Empty;
    public bool Exists { get; set; }
    public bool ReadSucceeded { get; set; }
    public string AssetRootPath { get; set; } = string.Empty;
    public string ProfilesRootPath { get; set; } = string.Empty;
    public string CacheRootPath { get; set; } = string.Empty;
    public string ConfigRootPath { get; set; } = string.Empty;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public int ReadyAssetCount { get; set; }
    public int CompletedSteps { get; set; }
    public int TotalSteps { get; set; }
    public string[] BrowserInteractionStabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserInteractionStabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
