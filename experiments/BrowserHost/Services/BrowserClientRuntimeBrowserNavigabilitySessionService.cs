namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserNavigabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserNavigabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserNavigabilitySessionService : IBrowserClientRuntimeBrowserNavigabilitySession
{
    private readonly IBrowserClientRuntimeBrowserApproachabilityReadyState _runtimeBrowserApproachabilityReadyState;

    public BrowserClientRuntimeBrowserNavigabilitySessionService(IBrowserClientRuntimeBrowserApproachabilityReadyState runtimeBrowserApproachabilityReadyState)
    {
        _runtimeBrowserApproachabilityReadyState = runtimeBrowserApproachabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserNavigabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserApproachabilityReadyStateResult approachabilityReadyState = await _runtimeBrowserApproachabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserNavigabilitySessionResult result = new()
        {
            ProfileId = approachabilityReadyState.ProfileId,
            SessionId = approachabilityReadyState.SessionId,
            SessionPath = approachabilityReadyState.SessionPath,
            BrowserApproachabilityReadyStateVersion = approachabilityReadyState.BrowserApproachabilityReadyStateVersion,
            BrowserApproachabilitySessionVersion = approachabilityReadyState.BrowserApproachabilitySessionVersion,
            LaunchMode = approachabilityReadyState.LaunchMode,
            AssetRootPath = approachabilityReadyState.AssetRootPath,
            ProfilesRootPath = approachabilityReadyState.ProfilesRootPath,
            CacheRootPath = approachabilityReadyState.CacheRootPath,
            ConfigRootPath = approachabilityReadyState.ConfigRootPath,
            SettingsFilePath = approachabilityReadyState.SettingsFilePath,
            StartupProfilePath = approachabilityReadyState.StartupProfilePath,
            RequiredAssets = approachabilityReadyState.RequiredAssets,
            ReadyAssetCount = approachabilityReadyState.ReadyAssetCount,
            CompletedSteps = approachabilityReadyState.CompletedSteps,
            TotalSteps = approachabilityReadyState.TotalSteps,
            Exists = approachabilityReadyState.Exists,
            ReadSucceeded = approachabilityReadyState.ReadSucceeded
        };

        if (!approachabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser navigability session blocked for profile '{approachabilityReadyState.ProfileId}'.";
            result.Error = approachabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserNavigabilitySessionVersion = "runtime-browser-navigability-session-v1";
        result.BrowserNavigabilityStages =
        [
            "open-browser-navigability-session",
            "bind-browser-approachability-ready-state",
            "publish-browser-navigability-ready"
        ];
        result.BrowserNavigabilitySummary = $"Runtime browser navigability session prepared {result.BrowserNavigabilityStages.Length} navigability stage(s) for profile '{approachabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser navigability session ready for profile '{approachabilityReadyState.ProfileId}' with {result.BrowserNavigabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserNavigabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserNavigabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserApproachabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserApproachabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserNavigabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserNavigabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

