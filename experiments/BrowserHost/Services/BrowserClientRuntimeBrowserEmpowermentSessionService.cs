namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserEmpowermentSession
{
    ValueTask<BrowserClientRuntimeBrowserEmpowermentSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserEmpowermentSessionService : IBrowserClientRuntimeBrowserEmpowermentSession
{
    private readonly IBrowserClientRuntimeBrowserEnablementReadyState _runtimeBrowserEnablementReadyState;

    public BrowserClientRuntimeBrowserEmpowermentSessionService(IBrowserClientRuntimeBrowserEnablementReadyState runtimeBrowserEnablementReadyState)
    {
        _runtimeBrowserEnablementReadyState = runtimeBrowserEnablementReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserEmpowermentSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserEnablementReadyStateResult enablementReadyState = await _runtimeBrowserEnablementReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserEmpowermentSessionResult result = new()
        {
            ProfileId = enablementReadyState.ProfileId,
            SessionId = enablementReadyState.SessionId,
            SessionPath = enablementReadyState.SessionPath,
            BrowserEnablementReadyStateVersion = enablementReadyState.BrowserEnablementReadyStateVersion,
            BrowserEnablementSessionVersion = enablementReadyState.BrowserEnablementSessionVersion,
            LaunchMode = enablementReadyState.LaunchMode,
            AssetRootPath = enablementReadyState.AssetRootPath,
            ProfilesRootPath = enablementReadyState.ProfilesRootPath,
            CacheRootPath = enablementReadyState.CacheRootPath,
            ConfigRootPath = enablementReadyState.ConfigRootPath,
            SettingsFilePath = enablementReadyState.SettingsFilePath,
            StartupProfilePath = enablementReadyState.StartupProfilePath,
            RequiredAssets = enablementReadyState.RequiredAssets,
            ReadyAssetCount = enablementReadyState.ReadyAssetCount,
            CompletedSteps = enablementReadyState.CompletedSteps,
            TotalSteps = enablementReadyState.TotalSteps,
            Exists = enablementReadyState.Exists,
            ReadSucceeded = enablementReadyState.ReadSucceeded
        };

        if (!enablementReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser empowerment session blocked for profile '{enablementReadyState.ProfileId}'.";
            result.Error = enablementReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserEmpowermentSessionVersion = "runtime-browser-empowerment-session-v1";
        result.BrowserEmpowermentStages =
        [
            "open-browser-empowerment-session",
            "bind-browser-enablement-ready-state",
            "publish-browser-empowerment-ready"
        ];
        result.BrowserEmpowermentSummary = $"Runtime browser empowerment session prepared {result.BrowserEmpowermentStages.Length} empowerment stage(s) for profile '{enablementReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser empowerment session ready for profile '{enablementReadyState.ProfileId}' with {result.BrowserEmpowermentStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserEmpowermentSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserEmpowermentSessionVersion { get; set; } = string.Empty;
    public string BrowserEnablementReadyStateVersion { get; set; } = string.Empty;
    public string BrowserEnablementSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserEmpowermentStages { get; set; } = Array.Empty<string>();
    public string BrowserEmpowermentSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
