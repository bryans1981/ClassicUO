namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLearnabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserLearnabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLearnabilitySessionService : IBrowserClientRuntimeBrowserLearnabilitySession
{
    private readonly IBrowserClientRuntimeBrowserDiscoverabilityReadyState _runtimeBrowserDiscoverabilityReadyState;

    public BrowserClientRuntimeBrowserLearnabilitySessionService(IBrowserClientRuntimeBrowserDiscoverabilityReadyState runtimeBrowserDiscoverabilityReadyState)
    {
        _runtimeBrowserDiscoverabilityReadyState = runtimeBrowserDiscoverabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLearnabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDiscoverabilityReadyStateResult discoverabilityReadyState = await _runtimeBrowserDiscoverabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLearnabilitySessionResult result = new()
        {
            ProfileId = discoverabilityReadyState.ProfileId,
            SessionId = discoverabilityReadyState.SessionId,
            SessionPath = discoverabilityReadyState.SessionPath,
            BrowserDiscoverabilityReadyStateVersion = discoverabilityReadyState.BrowserDiscoverabilityReadyStateVersion,
            BrowserDiscoverabilitySessionVersion = discoverabilityReadyState.BrowserDiscoverabilitySessionVersion,
            LaunchMode = discoverabilityReadyState.LaunchMode,
            AssetRootPath = discoverabilityReadyState.AssetRootPath,
            ProfilesRootPath = discoverabilityReadyState.ProfilesRootPath,
            CacheRootPath = discoverabilityReadyState.CacheRootPath,
            ConfigRootPath = discoverabilityReadyState.ConfigRootPath,
            SettingsFilePath = discoverabilityReadyState.SettingsFilePath,
            StartupProfilePath = discoverabilityReadyState.StartupProfilePath,
            RequiredAssets = discoverabilityReadyState.RequiredAssets,
            ReadyAssetCount = discoverabilityReadyState.ReadyAssetCount,
            CompletedSteps = discoverabilityReadyState.CompletedSteps,
            TotalSteps = discoverabilityReadyState.TotalSteps,
            Exists = discoverabilityReadyState.Exists,
            ReadSucceeded = discoverabilityReadyState.ReadSucceeded
        };

        if (!discoverabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser learnability session blocked for profile '{discoverabilityReadyState.ProfileId}'.";
            result.Error = discoverabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLearnabilitySessionVersion = "runtime-browser-learnability-session-v1";
        result.BrowserLearnabilityStages =
        [
            "open-browser-learnability-session",
            "bind-browser-discoverability-ready-state",
            "publish-browser-learnability-ready"
        ];
        result.BrowserLearnabilitySummary = $"Runtime browser learnability session prepared {result.BrowserLearnabilityStages.Length} learnability stage(s) for profile '{discoverabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser learnability session ready for profile '{discoverabilityReadyState.ProfileId}' with {result.BrowserLearnabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLearnabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserLearnabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserDiscoverabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDiscoverabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLearnabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserLearnabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
