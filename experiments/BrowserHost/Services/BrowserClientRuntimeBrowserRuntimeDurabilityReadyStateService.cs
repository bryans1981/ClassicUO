namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRuntimeDurabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserRuntimeDurabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRuntimeDurabilityReadyStateService : IBrowserClientRuntimeBrowserRuntimeDurabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserRuntimeDurabilitySession _runtimeBrowserRuntimeDurabilitySession;

    public BrowserClientRuntimeBrowserRuntimeDurabilityReadyStateService(IBrowserClientRuntimeBrowserRuntimeDurabilitySession runtimeBrowserRuntimeDurabilitySession)
    {
        _runtimeBrowserRuntimeDurabilitySession = runtimeBrowserRuntimeDurabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRuntimeDurabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRuntimeDurabilitySessionResult runtimedurabilitySession = await _runtimeBrowserRuntimeDurabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserRuntimeDurabilityReadyStateResult result = new()
        {
            ProfileId = runtimedurabilitySession.ProfileId,
            SessionId = runtimedurabilitySession.SessionId,
            SessionPath = runtimedurabilitySession.SessionPath,
            BrowserRuntimeDurabilitySessionVersion = runtimedurabilitySession.BrowserRuntimeDurabilitySessionVersion,
            BrowserLiveResilienceReadyStateVersion = runtimedurabilitySession.BrowserLiveResilienceReadyStateVersion,
            BrowserLiveResilienceSessionVersion = runtimedurabilitySession.BrowserLiveResilienceSessionVersion,
            LaunchMode = runtimedurabilitySession.LaunchMode,
            AssetRootPath = runtimedurabilitySession.AssetRootPath,
            ProfilesRootPath = runtimedurabilitySession.ProfilesRootPath,
            CacheRootPath = runtimedurabilitySession.CacheRootPath,
            ConfigRootPath = runtimedurabilitySession.ConfigRootPath,
            SettingsFilePath = runtimedurabilitySession.SettingsFilePath,
            StartupProfilePath = runtimedurabilitySession.StartupProfilePath,
            RequiredAssets = runtimedurabilitySession.RequiredAssets,
            ReadyAssetCount = runtimedurabilitySession.ReadyAssetCount,
            CompletedSteps = runtimedurabilitySession.CompletedSteps,
            TotalSteps = runtimedurabilitySession.TotalSteps,
            Exists = runtimedurabilitySession.Exists,
            ReadSucceeded = runtimedurabilitySession.ReadSucceeded
        };

        if (!runtimedurabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser runtimedurability ready state blocked for profile '{runtimedurabilitySession.ProfileId}'.";
            result.Error = runtimedurabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRuntimeDurabilityReadyStateVersion = "runtime-browser-runtimedurability-ready-state-v1";
        result.BrowserRuntimeDurabilityReadyChecks =
        [
            "browser-liveresilience-ready-state-ready",
            "browser-runtimedurability-session-ready",
            "browser-runtimedurability-ready"
        ];
        result.BrowserRuntimeDurabilityReadySummary = $"Runtime browser runtimedurability ready state passed {result.BrowserRuntimeDurabilityReadyChecks.Length} runtimedurability readiness check(s) for profile '{runtimedurabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser runtimedurability ready state ready for profile '{runtimedurabilitySession.ProfileId}' with {result.BrowserRuntimeDurabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRuntimeDurabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserRuntimeDurabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeDurabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserLiveResilienceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLiveResilienceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRuntimeDurabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserRuntimeDurabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
