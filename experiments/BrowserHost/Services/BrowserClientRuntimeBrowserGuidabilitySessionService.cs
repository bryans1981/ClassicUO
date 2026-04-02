namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserGuidabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserGuidabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserGuidabilitySessionService : IBrowserClientRuntimeBrowserGuidabilitySession
{
    private readonly IBrowserClientRuntimeBrowserNavigabilityReadyState _runtimeBrowserNavigabilityReadyState;

    public BrowserClientRuntimeBrowserGuidabilitySessionService(IBrowserClientRuntimeBrowserNavigabilityReadyState runtimeBrowserNavigabilityReadyState)
    {
        _runtimeBrowserNavigabilityReadyState = runtimeBrowserNavigabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserGuidabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserNavigabilityReadyStateResult navigabilityReadyState = await _runtimeBrowserNavigabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserGuidabilitySessionResult result = new()
        {
            ProfileId = navigabilityReadyState.ProfileId,
            SessionId = navigabilityReadyState.SessionId,
            SessionPath = navigabilityReadyState.SessionPath,
            BrowserNavigabilityReadyStateVersion = navigabilityReadyState.BrowserNavigabilityReadyStateVersion,
            BrowserNavigabilitySessionVersion = navigabilityReadyState.BrowserNavigabilitySessionVersion,
            LaunchMode = navigabilityReadyState.LaunchMode,
            AssetRootPath = navigabilityReadyState.AssetRootPath,
            ProfilesRootPath = navigabilityReadyState.ProfilesRootPath,
            CacheRootPath = navigabilityReadyState.CacheRootPath,
            ConfigRootPath = navigabilityReadyState.ConfigRootPath,
            SettingsFilePath = navigabilityReadyState.SettingsFilePath,
            StartupProfilePath = navigabilityReadyState.StartupProfilePath,
            RequiredAssets = navigabilityReadyState.RequiredAssets,
            ReadyAssetCount = navigabilityReadyState.ReadyAssetCount,
            CompletedSteps = navigabilityReadyState.CompletedSteps,
            TotalSteps = navigabilityReadyState.TotalSteps,
            Exists = navigabilityReadyState.Exists,
            ReadSucceeded = navigabilityReadyState.ReadSucceeded
        };

        if (!navigabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser guidability session blocked for profile '{navigabilityReadyState.ProfileId}'.";
            result.Error = navigabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserGuidabilitySessionVersion = "runtime-browser-guidability-session-v1";
        result.BrowserGuidabilityStages =
        [
            "open-browser-guidability-session",
            "bind-browser-navigability-ready-state",
            "publish-browser-guidability-ready"
        ];
        result.BrowserGuidabilitySummary = $"Runtime browser guidability session prepared {result.BrowserGuidabilityStages.Length} guidability stage(s) for profile '{navigabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser guidability session ready for profile '{navigabilityReadyState.ProfileId}' with {result.BrowserGuidabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserGuidabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserGuidabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserNavigabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserNavigabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserGuidabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserGuidabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

