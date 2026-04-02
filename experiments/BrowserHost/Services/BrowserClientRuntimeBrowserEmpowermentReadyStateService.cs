namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserEmpowermentReadyState
{
    ValueTask<BrowserClientRuntimeBrowserEmpowermentReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserEmpowermentReadyStateService : IBrowserClientRuntimeBrowserEmpowermentReadyState
{
    private readonly IBrowserClientRuntimeBrowserEmpowermentSession _runtimeBrowserEmpowermentSession;

    public BrowserClientRuntimeBrowserEmpowermentReadyStateService(IBrowserClientRuntimeBrowserEmpowermentSession runtimeBrowserEmpowermentSession)
    {
        _runtimeBrowserEmpowermentSession = runtimeBrowserEmpowermentSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserEmpowermentReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserEmpowermentSessionResult empowermentSession = await _runtimeBrowserEmpowermentSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserEmpowermentReadyStateResult result = new()
        {
            ProfileId = empowermentSession.ProfileId,
            SessionId = empowermentSession.SessionId,
            SessionPath = empowermentSession.SessionPath,
            BrowserEmpowermentSessionVersion = empowermentSession.BrowserEmpowermentSessionVersion,
            BrowserEnablementReadyStateVersion = empowermentSession.BrowserEnablementReadyStateVersion,
            BrowserEnablementSessionVersion = empowermentSession.BrowserEnablementSessionVersion,
            LaunchMode = empowermentSession.LaunchMode,
            AssetRootPath = empowermentSession.AssetRootPath,
            ProfilesRootPath = empowermentSession.ProfilesRootPath,
            CacheRootPath = empowermentSession.CacheRootPath,
            ConfigRootPath = empowermentSession.ConfigRootPath,
            SettingsFilePath = empowermentSession.SettingsFilePath,
            StartupProfilePath = empowermentSession.StartupProfilePath,
            RequiredAssets = empowermentSession.RequiredAssets,
            ReadyAssetCount = empowermentSession.ReadyAssetCount,
            CompletedSteps = empowermentSession.CompletedSteps,
            TotalSteps = empowermentSession.TotalSteps,
            Exists = empowermentSession.Exists,
            ReadSucceeded = empowermentSession.ReadSucceeded
        };

        if (!empowermentSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser empowerment ready state blocked for profile '{empowermentSession.ProfileId}'.";
            result.Error = empowermentSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserEmpowermentReadyStateVersion = "runtime-browser-empowerment-ready-state-v1";
        result.BrowserEmpowermentReadyChecks =
        [
            "browser-enablement-ready-state-ready",
            "browser-empowerment-session-ready",
            "browser-empowerment-ready"
        ];
        result.BrowserEmpowermentReadySummary = $"Runtime browser empowerment ready state passed {result.BrowserEmpowermentReadyChecks.Length} empowerment readiness check(s) for profile '{empowermentSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser empowerment ready state ready for profile '{empowermentSession.ProfileId}' with {result.BrowserEmpowermentReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserEmpowermentReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserEmpowermentReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserEmpowermentReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserEmpowermentReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
