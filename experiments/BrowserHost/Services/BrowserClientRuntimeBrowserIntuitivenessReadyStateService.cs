namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserIntuitivenessReadyState
{
    ValueTask<BrowserClientRuntimeBrowserIntuitivenessReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserIntuitivenessReadyStateService : IBrowserClientRuntimeBrowserIntuitivenessReadyState
{
    private readonly IBrowserClientRuntimeBrowserIntuitivenessSession _runtimeBrowserIntuitivenessSession;

    public BrowserClientRuntimeBrowserIntuitivenessReadyStateService(IBrowserClientRuntimeBrowserIntuitivenessSession runtimeBrowserIntuitivenessSession)
    {
        _runtimeBrowserIntuitivenessSession = runtimeBrowserIntuitivenessSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserIntuitivenessReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserIntuitivenessSessionResult intuitivenessSession = await _runtimeBrowserIntuitivenessSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserIntuitivenessReadyStateResult result = new()
        {
            ProfileId = intuitivenessSession.ProfileId,
            SessionId = intuitivenessSession.SessionId,
            SessionPath = intuitivenessSession.SessionPath,
            BrowserIntuitivenessSessionVersion = intuitivenessSession.BrowserIntuitivenessSessionVersion,
            BrowserClarityReadyStateVersion = intuitivenessSession.BrowserClarityReadyStateVersion,
            BrowserClaritySessionVersion = intuitivenessSession.BrowserClaritySessionVersion,
            LaunchMode = intuitivenessSession.LaunchMode,
            AssetRootPath = intuitivenessSession.AssetRootPath,
            ProfilesRootPath = intuitivenessSession.ProfilesRootPath,
            CacheRootPath = intuitivenessSession.CacheRootPath,
            ConfigRootPath = intuitivenessSession.ConfigRootPath,
            SettingsFilePath = intuitivenessSession.SettingsFilePath,
            StartupProfilePath = intuitivenessSession.StartupProfilePath,
            RequiredAssets = intuitivenessSession.RequiredAssets,
            ReadyAssetCount = intuitivenessSession.ReadyAssetCount,
            CompletedSteps = intuitivenessSession.CompletedSteps,
            TotalSteps = intuitivenessSession.TotalSteps,
            Exists = intuitivenessSession.Exists,
            ReadSucceeded = intuitivenessSession.ReadSucceeded
        };

        if (!intuitivenessSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser intuitiveness ready state blocked for profile '{intuitivenessSession.ProfileId}'.";
            result.Error = intuitivenessSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserIntuitivenessReadyStateVersion = "runtime-browser-intuitiveness-ready-state-v1";
        result.BrowserIntuitivenessReadyChecks =
        [
            "browser-clarity-ready-state-ready",
            "browser-intuitiveness-session-ready",
            "browser-intuitiveness-ready"
        ];
        result.BrowserIntuitivenessReadySummary = $"Runtime browser intuitiveness ready state passed {result.BrowserIntuitivenessReadyChecks.Length} intuitiveness readiness check(s) for profile '{intuitivenessSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser intuitiveness ready state ready for profile '{intuitivenessSession.ProfileId}' with {result.BrowserIntuitivenessReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserIntuitivenessReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserIntuitivenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserIntuitivenessSessionVersion { get; set; } = string.Empty;
    public string BrowserClarityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserClaritySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserIntuitivenessReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserIntuitivenessReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

