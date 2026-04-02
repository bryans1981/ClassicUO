namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserApproachabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserApproachabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserApproachabilitySessionService : IBrowserClientRuntimeBrowserApproachabilitySession
{
    private readonly IBrowserClientRuntimeBrowserLearnabilityReadyState _runtimeBrowserLearnabilityReadyState;

    public BrowserClientRuntimeBrowserApproachabilitySessionService(IBrowserClientRuntimeBrowserLearnabilityReadyState runtimeBrowserLearnabilityReadyState)
    {
        _runtimeBrowserLearnabilityReadyState = runtimeBrowserLearnabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserApproachabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLearnabilityReadyStateResult learnabilityReadyState = await _runtimeBrowserLearnabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserApproachabilitySessionResult result = new()
        {
            ProfileId = learnabilityReadyState.ProfileId,
            SessionId = learnabilityReadyState.SessionId,
            SessionPath = learnabilityReadyState.SessionPath,
            BrowserLearnabilityReadyStateVersion = learnabilityReadyState.BrowserLearnabilityReadyStateVersion,
            BrowserLearnabilitySessionVersion = learnabilityReadyState.BrowserLearnabilitySessionVersion,
            LaunchMode = learnabilityReadyState.LaunchMode,
            AssetRootPath = learnabilityReadyState.AssetRootPath,
            ProfilesRootPath = learnabilityReadyState.ProfilesRootPath,
            CacheRootPath = learnabilityReadyState.CacheRootPath,
            ConfigRootPath = learnabilityReadyState.ConfigRootPath,
            SettingsFilePath = learnabilityReadyState.SettingsFilePath,
            StartupProfilePath = learnabilityReadyState.StartupProfilePath,
            RequiredAssets = learnabilityReadyState.RequiredAssets,
            ReadyAssetCount = learnabilityReadyState.ReadyAssetCount,
            CompletedSteps = learnabilityReadyState.CompletedSteps,
            TotalSteps = learnabilityReadyState.TotalSteps,
            Exists = learnabilityReadyState.Exists,
            ReadSucceeded = learnabilityReadyState.ReadSucceeded
        };

        if (!learnabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser approachability session blocked for profile '{learnabilityReadyState.ProfileId}'.";
            result.Error = learnabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserApproachabilitySessionVersion = "runtime-browser-approachability-session-v1";
        result.BrowserApproachabilityStages =
        [
            "open-browser-approachability-session",
            "bind-browser-learnability-ready-state",
            "publish-browser-approachability-ready"
        ];
        result.BrowserApproachabilitySummary = $"Runtime browser approachability session prepared {result.BrowserApproachabilityStages.Length} approachability stage(s) for profile '{learnabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser approachability session ready for profile '{learnabilityReadyState.ProfileId}' with {result.BrowserApproachabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserApproachabilitySessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserApproachabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserApproachabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
