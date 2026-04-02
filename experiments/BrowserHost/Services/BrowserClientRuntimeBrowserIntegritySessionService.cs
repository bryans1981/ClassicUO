namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserIntegritySession
{
    ValueTask<BrowserClientRuntimeBrowserIntegritySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserIntegritySessionService : IBrowserClientRuntimeBrowserIntegritySession
{
    private readonly IBrowserClientRuntimeBrowserRiskReadyState _runtimeBrowserRiskReadyState;

    public BrowserClientRuntimeBrowserIntegritySessionService(IBrowserClientRuntimeBrowserRiskReadyState runtimeBrowserRiskReadyState)
    {
        _runtimeBrowserRiskReadyState = runtimeBrowserRiskReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserIntegritySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserRiskReadyStateResult riskReadyState = await _runtimeBrowserRiskReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserIntegritySessionResult result = new()
        {
            ProfileId = riskReadyState.ProfileId,
            SessionId = riskReadyState.SessionId,
            SessionPath = riskReadyState.SessionPath,
            BrowserRiskReadyStateVersion = riskReadyState.BrowserRiskReadyStateVersion,
            BrowserRiskSessionVersion = riskReadyState.BrowserRiskSessionVersion,
            LaunchMode = riskReadyState.LaunchMode,
            AssetRootPath = riskReadyState.AssetRootPath,
            ProfilesRootPath = riskReadyState.ProfilesRootPath,
            CacheRootPath = riskReadyState.CacheRootPath,
            ConfigRootPath = riskReadyState.ConfigRootPath,
            SettingsFilePath = riskReadyState.SettingsFilePath,
            StartupProfilePath = riskReadyState.StartupProfilePath,
            RequiredAssets = riskReadyState.RequiredAssets,
            ReadyAssetCount = riskReadyState.ReadyAssetCount,
            CompletedSteps = riskReadyState.CompletedSteps,
            TotalSteps = riskReadyState.TotalSteps,
            Exists = riskReadyState.Exists,
            ReadSucceeded = riskReadyState.ReadSucceeded,
            BrowserRiskReadyState = riskReadyState
        };

        if (!riskReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser integrity session blocked for profile '{riskReadyState.ProfileId}'.";
            result.Error = riskReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserIntegritySessionVersion = "runtime-browser-integrity-session-v1";
        result.BrowserIntegrityStages =
        [
            "open-browser-integrity-session",
            "bind-browser-risk-ready-state",
            "publish-browser-integrity-ready"
        ];
        result.BrowserIntegritySummary = $"Runtime browser integrity session prepared {result.BrowserIntegrityStages.Length} integrity stage(s) for profile '{riskReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser integrity session ready for profile '{riskReadyState.ProfileId}' with {result.BrowserIntegrityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserIntegritySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserIntegritySessionVersion { get; set; } = string.Empty;
    public string BrowserRiskReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRiskSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserIntegrityStages { get; set; } = Array.Empty<string>();
    public string BrowserIntegritySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserRiskReadyStateResult BrowserRiskReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
