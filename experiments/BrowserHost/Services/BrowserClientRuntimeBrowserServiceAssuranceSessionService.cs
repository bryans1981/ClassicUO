namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserServiceAssuranceSession
{
    ValueTask<BrowserClientRuntimeBrowserServiceAssuranceSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserServiceAssuranceSessionService : IBrowserClientRuntimeBrowserServiceAssuranceSession
{
    private readonly IBrowserClientRuntimeBrowserRuntimeOperabilityReadyState _runtimeBrowserRuntimeOperabilityReadyState;

    public BrowserClientRuntimeBrowserServiceAssuranceSessionService(IBrowserClientRuntimeBrowserRuntimeOperabilityReadyState runtimeBrowserRuntimeOperabilityReadyState)
    {
        _runtimeBrowserRuntimeOperabilityReadyState = runtimeBrowserRuntimeOperabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserServiceAssuranceSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRuntimeOperabilityReadyStateResult runtimeoperabilityReadyState = await _runtimeBrowserRuntimeOperabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserServiceAssuranceSessionResult result = new()
        {
            ProfileId = runtimeoperabilityReadyState.ProfileId,
            SessionId = runtimeoperabilityReadyState.SessionId,
            SessionPath = runtimeoperabilityReadyState.SessionPath,
            BrowserRuntimeOperabilityReadyStateVersion = runtimeoperabilityReadyState.BrowserRuntimeOperabilityReadyStateVersion,
            BrowserRuntimeOperabilitySessionVersion = runtimeoperabilityReadyState.BrowserRuntimeOperabilitySessionVersion,
            LaunchMode = runtimeoperabilityReadyState.LaunchMode,
            AssetRootPath = runtimeoperabilityReadyState.AssetRootPath,
            ProfilesRootPath = runtimeoperabilityReadyState.ProfilesRootPath,
            CacheRootPath = runtimeoperabilityReadyState.CacheRootPath,
            ConfigRootPath = runtimeoperabilityReadyState.ConfigRootPath,
            SettingsFilePath = runtimeoperabilityReadyState.SettingsFilePath,
            StartupProfilePath = runtimeoperabilityReadyState.StartupProfilePath,
            RequiredAssets = runtimeoperabilityReadyState.RequiredAssets,
            ReadyAssetCount = runtimeoperabilityReadyState.ReadyAssetCount,
            CompletedSteps = runtimeoperabilityReadyState.CompletedSteps,
            TotalSteps = runtimeoperabilityReadyState.TotalSteps,
            Exists = runtimeoperabilityReadyState.Exists,
            ReadSucceeded = runtimeoperabilityReadyState.ReadSucceeded
        };

        if (!runtimeoperabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser serviceassurance session blocked for profile '{runtimeoperabilityReadyState.ProfileId}'.";
            result.Error = runtimeoperabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserServiceAssuranceSessionVersion = "runtime-browser-serviceassurance-session-v1";
        result.BrowserServiceAssuranceStages =
        [
            "open-browser-serviceassurance-session",
            "bind-browser-runtimeoperability-ready-state",
            "publish-browser-serviceassurance-ready"
        ];
        result.BrowserServiceAssuranceSummary = $"Runtime browser serviceassurance session prepared {result.BrowserServiceAssuranceStages.Length} serviceassurance stage(s) for profile '{runtimeoperabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser serviceassurance session ready for profile '{runtimeoperabilityReadyState.ProfileId}' with {result.BrowserServiceAssuranceStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserServiceAssuranceSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserServiceAssuranceSessionVersion { get; set; } = string.Empty;
    public string BrowserRuntimeOperabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeOperabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserServiceAssuranceStages { get; set; } = Array.Empty<string>();
    public string BrowserServiceAssuranceSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
