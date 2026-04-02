namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAgencyReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAgencyReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAgencyReadyStateService : IBrowserClientRuntimeBrowserAgencyReadyState
{
    private readonly IBrowserClientRuntimeBrowserAgencySession _runtimeBrowserAgencySession;

    public BrowserClientRuntimeBrowserAgencyReadyStateService(IBrowserClientRuntimeBrowserAgencySession runtimeBrowserAgencySession)
    {
        _runtimeBrowserAgencySession = runtimeBrowserAgencySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAgencyReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAgencySessionResult agencySession = await _runtimeBrowserAgencySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAgencyReadyStateResult result = new()
        {
            ProfileId = agencySession.ProfileId,
            SessionId = agencySession.SessionId,
            SessionPath = agencySession.SessionPath,
            BrowserAgencySessionVersion = agencySession.BrowserAgencySessionVersion,
            BrowserEmpowermentReadyStateVersion = agencySession.BrowserEmpowermentReadyStateVersion,
            BrowserEmpowermentSessionVersion = agencySession.BrowserEmpowermentSessionVersion,
            LaunchMode = agencySession.LaunchMode,
            AssetRootPath = agencySession.AssetRootPath,
            ProfilesRootPath = agencySession.ProfilesRootPath,
            CacheRootPath = agencySession.CacheRootPath,
            ConfigRootPath = agencySession.ConfigRootPath,
            SettingsFilePath = agencySession.SettingsFilePath,
            StartupProfilePath = agencySession.StartupProfilePath,
            RequiredAssets = agencySession.RequiredAssets,
            ReadyAssetCount = agencySession.ReadyAssetCount,
            CompletedSteps = agencySession.CompletedSteps,
            TotalSteps = agencySession.TotalSteps,
            Exists = agencySession.Exists,
            ReadSucceeded = agencySession.ReadSucceeded
        };

        if (!agencySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser agency ready state blocked for profile '{agencySession.ProfileId}'.";
            result.Error = agencySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAgencyReadyStateVersion = "runtime-browser-agency-ready-state-v1";
        result.BrowserAgencyReadyChecks =
        [
            "browser-empowerment-ready-state-ready",
            "browser-agency-session-ready",
            "browser-agency-ready"
        ];
        result.BrowserAgencyReadySummary = $"Runtime browser agency ready state passed {result.BrowserAgencyReadyChecks.Length} agency readiness check(s) for profile '{agencySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser agency ready state ready for profile '{agencySession.ProfileId}' with {result.BrowserAgencyReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAgencyReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAgencyReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserAgencyReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAgencyReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
