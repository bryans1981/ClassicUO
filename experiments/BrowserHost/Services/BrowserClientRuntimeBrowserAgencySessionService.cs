namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAgencySession
{
    ValueTask<BrowserClientRuntimeBrowserAgencySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAgencySessionService : IBrowserClientRuntimeBrowserAgencySession
{
    private readonly IBrowserClientRuntimeBrowserEmpowermentReadyState _runtimeBrowserEmpowermentReadyState;

    public BrowserClientRuntimeBrowserAgencySessionService(IBrowserClientRuntimeBrowserEmpowermentReadyState runtimeBrowserEmpowermentReadyState)
    {
        _runtimeBrowserEmpowermentReadyState = runtimeBrowserEmpowermentReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAgencySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserEmpowermentReadyStateResult empowermentReadyState = await _runtimeBrowserEmpowermentReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAgencySessionResult result = new()
        {
            ProfileId = empowermentReadyState.ProfileId,
            SessionId = empowermentReadyState.SessionId,
            SessionPath = empowermentReadyState.SessionPath,
            BrowserEmpowermentReadyStateVersion = empowermentReadyState.BrowserEmpowermentReadyStateVersion,
            BrowserEmpowermentSessionVersion = empowermentReadyState.BrowserEmpowermentSessionVersion,
            LaunchMode = empowermentReadyState.LaunchMode,
            AssetRootPath = empowermentReadyState.AssetRootPath,
            ProfilesRootPath = empowermentReadyState.ProfilesRootPath,
            CacheRootPath = empowermentReadyState.CacheRootPath,
            ConfigRootPath = empowermentReadyState.ConfigRootPath,
            SettingsFilePath = empowermentReadyState.SettingsFilePath,
            StartupProfilePath = empowermentReadyState.StartupProfilePath,
            RequiredAssets = empowermentReadyState.RequiredAssets,
            ReadyAssetCount = empowermentReadyState.ReadyAssetCount,
            CompletedSteps = empowermentReadyState.CompletedSteps,
            TotalSteps = empowermentReadyState.TotalSteps,
            Exists = empowermentReadyState.Exists,
            ReadSucceeded = empowermentReadyState.ReadSucceeded
        };

        if (!empowermentReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser agency session blocked for profile '{empowermentReadyState.ProfileId}'.";
            result.Error = empowermentReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAgencySessionVersion = "runtime-browser-agency-session-v1";
        result.BrowserAgencyStages =
        [
            "open-browser-agency-session",
            "bind-browser-empowerment-ready-state",
            "publish-browser-agency-ready"
        ];
        result.BrowserAgencySummary = $"Runtime browser agency session prepared {result.BrowserAgencyStages.Length} agency stage(s) for profile '{empowermentReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser agency session ready for profile '{empowermentReadyState.ProfileId}' with {result.BrowserAgencyStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAgencySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAgencySessionVersion { get; set; } = string.Empty;
    public string BrowserEmpowermentReadyStateVersion { get; set; } = string.Empty;
    public string BrowserEmpowermentSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAgencyStages { get; set; } = Array.Empty<string>();
    public string BrowserAgencySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
