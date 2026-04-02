namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserApproachabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserApproachabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserApproachabilityReadyStateService : IBrowserClientRuntimeBrowserApproachabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserApproachabilitySession _runtimeBrowserApproachabilitySession;

    public BrowserClientRuntimeBrowserApproachabilityReadyStateService(IBrowserClientRuntimeBrowserApproachabilitySession runtimeBrowserApproachabilitySession)
    {
        _runtimeBrowserApproachabilitySession = runtimeBrowserApproachabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserApproachabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserApproachabilitySessionResult approachabilitySession = await _runtimeBrowserApproachabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserApproachabilityReadyStateResult result = new()
        {
            ProfileId = approachabilitySession.ProfileId,
            SessionId = approachabilitySession.SessionId,
            SessionPath = approachabilitySession.SessionPath,
            BrowserApproachabilitySessionVersion = approachabilitySession.BrowserApproachabilitySessionVersion,
            BrowserLearnabilityReadyStateVersion = approachabilitySession.BrowserLearnabilityReadyStateVersion,
            BrowserLearnabilitySessionVersion = approachabilitySession.BrowserLearnabilitySessionVersion,
            LaunchMode = approachabilitySession.LaunchMode,
            AssetRootPath = approachabilitySession.AssetRootPath,
            ProfilesRootPath = approachabilitySession.ProfilesRootPath,
            CacheRootPath = approachabilitySession.CacheRootPath,
            ConfigRootPath = approachabilitySession.ConfigRootPath,
            SettingsFilePath = approachabilitySession.SettingsFilePath,
            StartupProfilePath = approachabilitySession.StartupProfilePath,
            RequiredAssets = approachabilitySession.RequiredAssets,
            ReadyAssetCount = approachabilitySession.ReadyAssetCount,
            CompletedSteps = approachabilitySession.CompletedSteps,
            TotalSteps = approachabilitySession.TotalSteps,
            Exists = approachabilitySession.Exists,
            ReadSucceeded = approachabilitySession.ReadSucceeded
        };

        if (!approachabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser approachability ready state blocked for profile '{approachabilitySession.ProfileId}'.";
            result.Error = approachabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserApproachabilityReadyStateVersion = "runtime-browser-approachability-ready-state-v1";
        result.BrowserApproachabilityReadyChecks =
        [
            "browser-learnability-ready-state-ready",
            "browser-approachability-session-ready",
            "browser-approachability-ready"
        ];
        result.BrowserApproachabilityReadySummary = $"Runtime browser approachability ready state passed {result.BrowserApproachabilityReadyChecks.Length} approachability readiness check(s) for profile '{approachabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser approachability ready state ready for profile '{approachabilitySession.ProfileId}' with {result.BrowserApproachabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserApproachabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserApproachabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserApproachabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserLearnabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLearnabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserApproachabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserApproachabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
