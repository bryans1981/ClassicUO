namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserReliabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserReliabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserReliabilityReadyStateService : IBrowserClientRuntimeBrowserReliabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserReliabilitySession _runtimeBrowserReliabilitySession;

    public BrowserClientRuntimeBrowserReliabilityReadyStateService(IBrowserClientRuntimeBrowserReliabilitySession runtimeBrowserReliabilitySession)
    {
        _runtimeBrowserReliabilitySession = runtimeBrowserReliabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserReliabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserReliabilitySessionResult reliabilitySession = await _runtimeBrowserReliabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserReliabilityReadyStateResult result = new()
        {
            ProfileId = reliabilitySession.ProfileId,
            SessionId = reliabilitySession.SessionId,
            SessionPath = reliabilitySession.SessionPath,
            BrowserReliabilitySessionVersion = reliabilitySession.BrowserReliabilitySessionVersion,
            BrowserCredibilityReadyStateVersion = reliabilitySession.BrowserCredibilityReadyStateVersion,
            BrowserCredibilitySessionVersion = reliabilitySession.BrowserCredibilitySessionVersion,
            LaunchMode = reliabilitySession.LaunchMode,
            AssetRootPath = reliabilitySession.AssetRootPath,
            ProfilesRootPath = reliabilitySession.ProfilesRootPath,
            CacheRootPath = reliabilitySession.CacheRootPath,
            ConfigRootPath = reliabilitySession.ConfigRootPath,
            SettingsFilePath = reliabilitySession.SettingsFilePath,
            StartupProfilePath = reliabilitySession.StartupProfilePath,
            RequiredAssets = reliabilitySession.RequiredAssets,
            ReadyAssetCount = reliabilitySession.ReadyAssetCount,
            CompletedSteps = reliabilitySession.CompletedSteps,
            TotalSteps = reliabilitySession.TotalSteps,
            Exists = reliabilitySession.Exists,
            ReadSucceeded = reliabilitySession.ReadSucceeded
        };

        if (!reliabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser reliability ready state blocked for profile '{reliabilitySession.ProfileId}'.";
            result.Error = reliabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserReliabilityReadyStateVersion = "runtime-browser-reliability-ready-state-v1";
        result.BrowserReliabilityReadyChecks =
        [
            "browser-credibility-ready-state-ready",
            "browser-reliability-session-ready",
            "browser-reliability-ready"
        ];
        result.BrowserReliabilityReadySummary = $"Runtime browser reliability ready state passed {result.BrowserReliabilityReadyChecks.Length} reliability readiness check(s) for profile '{reliabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser reliability ready state ready for profile '{reliabilitySession.ProfileId}' with {result.BrowserReliabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserReliabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserReliabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserReliabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserCredibilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCredibilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserReliabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserReliabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
