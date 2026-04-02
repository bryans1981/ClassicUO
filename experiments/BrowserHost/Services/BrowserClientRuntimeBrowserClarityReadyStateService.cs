namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserClarityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserClarityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserClarityReadyStateService : IBrowserClientRuntimeBrowserClarityReadyState
{
    private readonly IBrowserClientRuntimeBrowserClaritySession _runtimeBrowserClaritySession;

    public BrowserClientRuntimeBrowserClarityReadyStateService(IBrowserClientRuntimeBrowserClaritySession runtimeBrowserClaritySession)
    {
        _runtimeBrowserClaritySession = runtimeBrowserClaritySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserClarityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserClaritySessionResult claritySession = await _runtimeBrowserClaritySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserClarityReadyStateResult result = new()
        {
            ProfileId = claritySession.ProfileId,
            SessionId = claritySession.SessionId,
            SessionPath = claritySession.SessionPath,
            BrowserClaritySessionVersion = claritySession.BrowserClaritySessionVersion,
            BrowserGuidabilityReadyStateVersion = claritySession.BrowserGuidabilityReadyStateVersion,
            BrowserGuidabilitySessionVersion = claritySession.BrowserGuidabilitySessionVersion,
            LaunchMode = claritySession.LaunchMode,
            AssetRootPath = claritySession.AssetRootPath,
            ProfilesRootPath = claritySession.ProfilesRootPath,
            CacheRootPath = claritySession.CacheRootPath,
            ConfigRootPath = claritySession.ConfigRootPath,
            SettingsFilePath = claritySession.SettingsFilePath,
            StartupProfilePath = claritySession.StartupProfilePath,
            RequiredAssets = claritySession.RequiredAssets,
            ReadyAssetCount = claritySession.ReadyAssetCount,
            CompletedSteps = claritySession.CompletedSteps,
            TotalSteps = claritySession.TotalSteps,
            Exists = claritySession.Exists,
            ReadSucceeded = claritySession.ReadSucceeded
        };

        if (!claritySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser clarity ready state blocked for profile '{claritySession.ProfileId}'.";
            result.Error = claritySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserClarityReadyStateVersion = "runtime-browser-clarity-ready-state-v1";
        result.BrowserClarityReadyChecks =
        [
            "browser-guidability-ready-state-ready",
            "browser-clarity-session-ready",
            "browser-clarity-ready"
        ];
        result.BrowserClarityReadySummary = $"Runtime browser clarity ready state passed {result.BrowserClarityReadyChecks.Length} clarity readiness check(s) for profile '{claritySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser clarity ready state ready for profile '{claritySession.ProfileId}' with {result.BrowserClarityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserClarityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserClarityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserClaritySessionVersion { get; set; } = string.Empty;
    public string BrowserGuidabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserGuidabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserClarityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserClarityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

