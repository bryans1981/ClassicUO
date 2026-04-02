namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserRiskSession
{
    ValueTask<BrowserClientRuntimeBrowserRiskSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserRiskSessionService : IBrowserClientRuntimeBrowserRiskSession
{
    private readonly IBrowserClientRuntimeBrowserAssuranceReadyState _runtimeBrowserAssuranceReadyState;

    public BrowserClientRuntimeBrowserRiskSessionService(IBrowserClientRuntimeBrowserAssuranceReadyState runtimeBrowserAssuranceReadyState)
    {
        _runtimeBrowserAssuranceReadyState = runtimeBrowserAssuranceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserRiskSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAssuranceReadyStateResult assuranceReadyState = await _runtimeBrowserAssuranceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserRiskSessionResult result = new()
        {
            ProfileId = assuranceReadyState.ProfileId,
            SessionId = assuranceReadyState.SessionId,
            SessionPath = assuranceReadyState.SessionPath,
            BrowserAssuranceReadyStateVersion = assuranceReadyState.BrowserAssuranceReadyStateVersion,
            BrowserAssuranceSessionVersion = assuranceReadyState.BrowserAssuranceSessionVersion,
            LaunchMode = assuranceReadyState.LaunchMode,
            AssetRootPath = assuranceReadyState.AssetRootPath,
            ProfilesRootPath = assuranceReadyState.ProfilesRootPath,
            CacheRootPath = assuranceReadyState.CacheRootPath,
            ConfigRootPath = assuranceReadyState.ConfigRootPath,
            SettingsFilePath = assuranceReadyState.SettingsFilePath,
            StartupProfilePath = assuranceReadyState.StartupProfilePath,
            RequiredAssets = assuranceReadyState.RequiredAssets,
            ReadyAssetCount = assuranceReadyState.ReadyAssetCount,
            CompletedSteps = assuranceReadyState.CompletedSteps,
            TotalSteps = assuranceReadyState.TotalSteps,
            Exists = assuranceReadyState.Exists,
            ReadSucceeded = assuranceReadyState.ReadSucceeded,
            BrowserAssuranceReadyState = assuranceReadyState
        };

        if (!assuranceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser risk session blocked for profile '{assuranceReadyState.ProfileId}'.";
            result.Error = assuranceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserRiskSessionVersion = "runtime-browser-risk-session-v1";
        result.BrowserRiskStages =
        [
            "open-browser-risk-session",
            "bind-browser-assurance-ready-state",
            "publish-browser-risk-ready"
        ];
        result.BrowserRiskSummary = $"Runtime browser risk session prepared {result.BrowserRiskStages.Length} risk stage(s) for profile '{assuranceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser risk session ready for profile '{assuranceReadyState.ProfileId}' with {result.BrowserRiskStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserRiskSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserRiskSessionVersion { get; set; } = string.Empty;
    public string BrowserAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAssuranceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserRiskStages { get; set; } = Array.Empty<string>();
    public string BrowserRiskSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserAssuranceReadyStateResult BrowserAssuranceReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
