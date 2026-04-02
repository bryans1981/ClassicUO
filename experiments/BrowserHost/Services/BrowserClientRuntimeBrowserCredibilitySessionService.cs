namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCredibilitySession
{
    ValueTask<BrowserClientRuntimeBrowserCredibilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCredibilitySessionService : IBrowserClientRuntimeBrowserCredibilitySession
{
    private readonly IBrowserClientRuntimeBrowserValueReadyState _runtimeBrowserValueReadyState;

    public BrowserClientRuntimeBrowserCredibilitySessionService(IBrowserClientRuntimeBrowserValueReadyState runtimeBrowserValueReadyState)
    {
        _runtimeBrowserValueReadyState = runtimeBrowserValueReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCredibilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserValueReadyStateResult valueReadyState = await _runtimeBrowserValueReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserCredibilitySessionResult result = new()
        {
            ProfileId = valueReadyState.ProfileId,
            SessionId = valueReadyState.SessionId,
            SessionPath = valueReadyState.SessionPath,
            BrowserValueReadyStateVersion = valueReadyState.BrowserValueReadyStateVersion,
            BrowserValueSessionVersion = valueReadyState.BrowserValueSessionVersion,
            LaunchMode = valueReadyState.LaunchMode,
            AssetRootPath = valueReadyState.AssetRootPath,
            ProfilesRootPath = valueReadyState.ProfilesRootPath,
            CacheRootPath = valueReadyState.CacheRootPath,
            ConfigRootPath = valueReadyState.ConfigRootPath,
            SettingsFilePath = valueReadyState.SettingsFilePath,
            StartupProfilePath = valueReadyState.StartupProfilePath,
            RequiredAssets = valueReadyState.RequiredAssets,
            ReadyAssetCount = valueReadyState.ReadyAssetCount,
            CompletedSteps = valueReadyState.CompletedSteps,
            TotalSteps = valueReadyState.TotalSteps,
            Exists = valueReadyState.Exists,
            ReadSucceeded = valueReadyState.ReadSucceeded
        };

        if (!valueReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser credibility session blocked for profile '{valueReadyState.ProfileId}'.";
            result.Error = valueReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCredibilitySessionVersion = "runtime-browser-credibility-session-v1";
        result.BrowserCredibilityStages =
        [
            "open-browser-credibility-session",
            "bind-browser-value-ready-state",
            "publish-browser-credibility-ready"
        ];
        result.BrowserCredibilitySummary = $"Runtime browser credibility session prepared {result.BrowserCredibilityStages.Length} credibility stage(s) for profile '{valueReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser credibility session ready for profile '{valueReadyState.ProfileId}' with {result.BrowserCredibilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCredibilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserCredibilitySessionVersion { get; set; } = string.Empty;
    public string BrowserValueReadyStateVersion { get; set; } = string.Empty;
    public string BrowserValueSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCredibilityStages { get; set; } = Array.Empty<string>();
    public string BrowserCredibilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
