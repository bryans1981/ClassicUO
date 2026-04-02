namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFluencyReadyState
{
    ValueTask<BrowserClientRuntimeBrowserFluencyReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFluencyReadyStateService : IBrowserClientRuntimeBrowserFluencyReadyState
{
    private readonly IBrowserClientRuntimeBrowserFluencySession _runtimeBrowserFluencySession;

    public BrowserClientRuntimeBrowserFluencyReadyStateService(IBrowserClientRuntimeBrowserFluencySession runtimeBrowserFluencySession)
    {
        _runtimeBrowserFluencySession = runtimeBrowserFluencySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFluencyReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFluencySessionResult fluencySession = await _runtimeBrowserFluencySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserFluencyReadyStateResult result = new()
        {
            ProfileId = fluencySession.ProfileId,
            SessionId = fluencySession.SessionId,
            SessionPath = fluencySession.SessionPath,
            BrowserFluencySessionVersion = fluencySession.BrowserFluencySessionVersion,
            BrowserFamiliarityReadyStateVersion = fluencySession.BrowserFamiliarityReadyStateVersion,
            BrowserFamiliaritySessionVersion = fluencySession.BrowserFamiliaritySessionVersion,
            LaunchMode = fluencySession.LaunchMode,
            AssetRootPath = fluencySession.AssetRootPath,
            ProfilesRootPath = fluencySession.ProfilesRootPath,
            CacheRootPath = fluencySession.CacheRootPath,
            ConfigRootPath = fluencySession.ConfigRootPath,
            SettingsFilePath = fluencySession.SettingsFilePath,
            StartupProfilePath = fluencySession.StartupProfilePath,
            RequiredAssets = fluencySession.RequiredAssets,
            ReadyAssetCount = fluencySession.ReadyAssetCount,
            CompletedSteps = fluencySession.CompletedSteps,
            TotalSteps = fluencySession.TotalSteps,
            Exists = fluencySession.Exists,
            ReadSucceeded = fluencySession.ReadSucceeded
        };

        if (!fluencySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser fluency ready state blocked for profile '{fluencySession.ProfileId}'.";
            result.Error = fluencySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFluencyReadyStateVersion = "runtime-browser-fluency-ready-state-v1";
        result.BrowserFluencyReadyChecks =
        [
            "browser-familiarity-ready-state-ready",
            "browser-fluency-session-ready",
            "browser-fluency-ready"
        ];
        result.BrowserFluencyReadySummary = $"Runtime browser fluency ready state passed {result.BrowserFluencyReadyChecks.Length} fluency readiness check(s) for profile '{fluencySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser fluency ready state ready for profile '{fluencySession.ProfileId}' with {result.BrowserFluencyReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFluencyReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserFluencyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFluencySessionVersion { get; set; } = string.Empty;
    public string BrowserFamiliarityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFamiliaritySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFluencyReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserFluencyReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

