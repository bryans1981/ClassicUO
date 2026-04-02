namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserReliabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserReliabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserReliabilitySessionService : IBrowserClientRuntimeBrowserReliabilitySession
{
    private readonly IBrowserClientRuntimeBrowserCredibilityReadyState _runtimeBrowserCredibilityReadyState;

    public BrowserClientRuntimeBrowserReliabilitySessionService(IBrowserClientRuntimeBrowserCredibilityReadyState runtimeBrowserCredibilityReadyState)
    {
        _runtimeBrowserCredibilityReadyState = runtimeBrowserCredibilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserReliabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCredibilityReadyStateResult credibilityReadyState = await _runtimeBrowserCredibilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserReliabilitySessionResult result = new()
        {
            ProfileId = credibilityReadyState.ProfileId,
            SessionId = credibilityReadyState.SessionId,
            SessionPath = credibilityReadyState.SessionPath,
            BrowserCredibilityReadyStateVersion = credibilityReadyState.BrowserCredibilityReadyStateVersion,
            BrowserCredibilitySessionVersion = credibilityReadyState.BrowserCredibilitySessionVersion,
            LaunchMode = credibilityReadyState.LaunchMode,
            AssetRootPath = credibilityReadyState.AssetRootPath,
            ProfilesRootPath = credibilityReadyState.ProfilesRootPath,
            CacheRootPath = credibilityReadyState.CacheRootPath,
            ConfigRootPath = credibilityReadyState.ConfigRootPath,
            SettingsFilePath = credibilityReadyState.SettingsFilePath,
            StartupProfilePath = credibilityReadyState.StartupProfilePath,
            RequiredAssets = credibilityReadyState.RequiredAssets,
            ReadyAssetCount = credibilityReadyState.ReadyAssetCount,
            CompletedSteps = credibilityReadyState.CompletedSteps,
            TotalSteps = credibilityReadyState.TotalSteps,
            Exists = credibilityReadyState.Exists,
            ReadSucceeded = credibilityReadyState.ReadSucceeded
        };

        if (!credibilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser reliability session blocked for profile '{credibilityReadyState.ProfileId}'.";
            result.Error = credibilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserReliabilitySessionVersion = "runtime-browser-reliability-session-v1";
        result.BrowserReliabilityStages =
        [
            "open-browser-reliability-session",
            "bind-browser-credibility-ready-state",
            "publish-browser-reliability-ready"
        ];
        result.BrowserReliabilitySummary = $"Runtime browser reliability session prepared {result.BrowserReliabilityStages.Length} reliability stage(s) for profile '{credibilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser reliability session ready for profile '{credibilityReadyState.ProfileId}' with {result.BrowserReliabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserReliabilitySessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserReliabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserReliabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
