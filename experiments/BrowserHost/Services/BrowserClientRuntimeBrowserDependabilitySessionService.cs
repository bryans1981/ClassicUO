namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDependabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserDependabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDependabilitySessionService : IBrowserClientRuntimeBrowserDependabilitySession
{
    private readonly IBrowserClientRuntimeBrowserCredenceReadyState _runtimeBrowserCredenceReadyState;

    public BrowserClientRuntimeBrowserDependabilitySessionService(IBrowserClientRuntimeBrowserCredenceReadyState runtimeBrowserCredenceReadyState)
    {
        _runtimeBrowserCredenceReadyState = runtimeBrowserCredenceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDependabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCredenceReadyStateResult credenceReadyState = await _runtimeBrowserCredenceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserDependabilitySessionResult result = new()
        {
            ProfileId = credenceReadyState.ProfileId,
            SessionId = credenceReadyState.SessionId,
            SessionPath = credenceReadyState.SessionPath,
            BrowserCredenceReadyStateVersion = credenceReadyState.BrowserCredenceReadyStateVersion,
            BrowserCredenceSessionVersion = credenceReadyState.BrowserCredenceSessionVersion,
            LaunchMode = credenceReadyState.LaunchMode,
            AssetRootPath = credenceReadyState.AssetRootPath,
            ProfilesRootPath = credenceReadyState.ProfilesRootPath,
            CacheRootPath = credenceReadyState.CacheRootPath,
            ConfigRootPath = credenceReadyState.ConfigRootPath,
            SettingsFilePath = credenceReadyState.SettingsFilePath,
            StartupProfilePath = credenceReadyState.StartupProfilePath,
            RequiredAssets = credenceReadyState.RequiredAssets,
            ReadyAssetCount = credenceReadyState.ReadyAssetCount,
            CompletedSteps = credenceReadyState.CompletedSteps,
            TotalSteps = credenceReadyState.TotalSteps,
            Exists = credenceReadyState.Exists,
            ReadSucceeded = credenceReadyState.ReadSucceeded
        };

        if (!credenceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser dependability session blocked for profile '{credenceReadyState.ProfileId}'.";
            result.Error = credenceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDependabilitySessionVersion = "runtime-browser-dependability-session-v1";
        result.BrowserDependabilityStages =
        [
            "open-browser-dependability-session",
            "bind-browser-credence-ready-state",
            "publish-browser-dependability-ready"
        ];
        result.BrowserDependabilitySummary = $"Runtime browser dependability session prepared {result.BrowserDependabilityStages.Length} dependability stage(s) for profile '{credenceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser dependability session ready for profile '{credenceReadyState.ProfileId}' with {result.BrowserDependabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDependabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserDependabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserCredenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCredenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDependabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserDependabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
