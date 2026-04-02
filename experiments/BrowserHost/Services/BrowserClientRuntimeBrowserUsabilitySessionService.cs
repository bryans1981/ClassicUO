namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserUsabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserUsabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserUsabilitySessionService : IBrowserClientRuntimeBrowserUsabilitySession
{
    private readonly IBrowserClientRuntimeBrowserSupportabilityReadyState _runtimeBrowserSupportabilityReadyState;

    public BrowserClientRuntimeBrowserUsabilitySessionService(IBrowserClientRuntimeBrowserSupportabilityReadyState runtimeBrowserSupportabilityReadyState)
    {
        _runtimeBrowserSupportabilityReadyState = runtimeBrowserSupportabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserUsabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSupportabilityReadyStateResult supportabilityReadyState = await _runtimeBrowserSupportabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserUsabilitySessionResult result = new()
        {
            ProfileId = supportabilityReadyState.ProfileId,
            SessionId = supportabilityReadyState.SessionId,
            SessionPath = supportabilityReadyState.SessionPath,
            BrowserSupportabilityReadyStateVersion = supportabilityReadyState.BrowserSupportabilityReadyStateVersion,
            BrowserSupportabilitySessionVersion = supportabilityReadyState.BrowserSupportabilitySessionVersion,
            LaunchMode = supportabilityReadyState.LaunchMode,
            AssetRootPath = supportabilityReadyState.AssetRootPath,
            ProfilesRootPath = supportabilityReadyState.ProfilesRootPath,
            CacheRootPath = supportabilityReadyState.CacheRootPath,
            ConfigRootPath = supportabilityReadyState.ConfigRootPath,
            SettingsFilePath = supportabilityReadyState.SettingsFilePath,
            StartupProfilePath = supportabilityReadyState.StartupProfilePath,
            RequiredAssets = supportabilityReadyState.RequiredAssets,
            ReadyAssetCount = supportabilityReadyState.ReadyAssetCount,
            CompletedSteps = supportabilityReadyState.CompletedSteps,
            TotalSteps = supportabilityReadyState.TotalSteps,
            Exists = supportabilityReadyState.Exists,
            ReadSucceeded = supportabilityReadyState.ReadSucceeded
        };

        if (!supportabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser usability session blocked for profile '{supportabilityReadyState.ProfileId}'.";
            result.Error = supportabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserUsabilitySessionVersion = "runtime-browser-usability-session-v1";
        result.BrowserUsabilityStages =
        [
            "open-browser-usability-session",
            "bind-browser-supportability-ready-state",
            "publish-browser-usability-ready"
        ];
        result.BrowserUsabilitySummary = $"Runtime browser usability session prepared {result.BrowserUsabilityStages.Length} usability stage(s) for profile '{supportabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser usability session ready for profile '{supportabilityReadyState.ProfileId}' with {result.BrowserUsabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserUsabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserUsabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSupportabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSupportabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserUsabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserUsabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
