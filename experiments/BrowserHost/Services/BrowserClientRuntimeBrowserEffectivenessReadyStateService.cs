namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserEffectivenessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserEffectivenessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserEffectivenessReadyStateService : IBrowserClientRuntimeBrowserEffectivenessReadyState
{
    private readonly IBrowserClientRuntimeBrowserEffectivenessSession _runtimeBrowserEffectivenessSession;

    public BrowserClientRuntimeBrowserEffectivenessReadyStateService(IBrowserClientRuntimeBrowserEffectivenessSession runtimeBrowserEffectivenessSession)
    {
        _runtimeBrowserEffectivenessSession = runtimeBrowserEffectivenessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserEffectivenessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserEffectivenessSessionResult effectivenessSession = await _runtimeBrowserEffectivenessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserEffectivenessReadyStateResult result = new()
        {
            ProfileId = effectivenessSession.ProfileId,
            SessionId = effectivenessSession.SessionId,
            SessionPath = effectivenessSession.SessionPath,
            BrowserEffectivenessSessionVersion = effectivenessSession.BrowserEffectivenessSessionVersion,
            BrowserEfficiencyReadyStateVersion = effectivenessSession.BrowserEfficiencyReadyStateVersion,
            BrowserEfficiencySessionVersion = effectivenessSession.BrowserEfficiencySessionVersion,
            LaunchMode = effectivenessSession.LaunchMode,
            AssetRootPath = effectivenessSession.AssetRootPath,
            ProfilesRootPath = effectivenessSession.ProfilesRootPath,
            CacheRootPath = effectivenessSession.CacheRootPath,
            ConfigRootPath = effectivenessSession.ConfigRootPath,
            SettingsFilePath = effectivenessSession.SettingsFilePath,
            StartupProfilePath = effectivenessSession.StartupProfilePath,
            RequiredAssets = effectivenessSession.RequiredAssets,
            ReadyAssetCount = effectivenessSession.ReadyAssetCount,
            CompletedSteps = effectivenessSession.CompletedSteps,
            TotalSteps = effectivenessSession.TotalSteps,
            Exists = effectivenessSession.Exists,
            ReadSucceeded = effectivenessSession.ReadSucceeded
        };

        if (!effectivenessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser effectiveness ready state blocked for profile '{effectivenessSession.ProfileId}'.";
            result.Error = effectivenessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserEffectivenessReadyStateVersion = "runtime-browser-effectiveness-ready-state-v1";
        result.BrowserEffectivenessReadyChecks =
        [
            "browser-efficiency-ready-state-ready",
            "browser-effectiveness-session-ready",
            "browser-effectiveness-ready"
        ];
        result.BrowserEffectivenessReadySummary = $"Runtime browser effectiveness ready state passed {result.BrowserEffectivenessReadyChecks.Length} effectiveness readiness check(s) for profile '{effectivenessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser effectiveness ready state ready for profile '{effectivenessSession.ProfileId}' with {result.BrowserEffectivenessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserEffectivenessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserEffectivenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserEffectivenessSessionVersion { get; set; } = string.Empty;
    public string BrowserEfficiencyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserEfficiencySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserEffectivenessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserEffectivenessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
