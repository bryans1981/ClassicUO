namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSessionContinuityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSessionContinuityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSessionContinuityReadyStateService : IBrowserClientRuntimeBrowserSessionContinuityReadyState
{
    private readonly IBrowserClientRuntimeBrowserSessionContinuitySession _runtimeBrowserSessionContinuitySession;

    public BrowserClientRuntimeBrowserSessionContinuityReadyStateService(IBrowserClientRuntimeBrowserSessionContinuitySession runtimeBrowserSessionContinuitySession)
    {
        _runtimeBrowserSessionContinuitySession = runtimeBrowserSessionContinuitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSessionContinuityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSessionContinuitySessionResult sessioncontinuitySession = await _runtimeBrowserSessionContinuitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSessionContinuityReadyStateResult result = new()
        {
            ProfileId = sessioncontinuitySession.ProfileId,
            SessionId = sessioncontinuitySession.SessionId,
            SessionPath = sessioncontinuitySession.SessionPath,
            BrowserSessionContinuitySessionVersion = sessioncontinuitySession.BrowserSessionContinuitySessionVersion,
            BrowserRuntimeContinuityReadyStateVersion = sessioncontinuitySession.BrowserRuntimeContinuityReadyStateVersion,
            BrowserRuntimeContinuitySessionVersion = sessioncontinuitySession.BrowserRuntimeContinuitySessionVersion,
            LaunchMode = sessioncontinuitySession.LaunchMode,
            AssetRootPath = sessioncontinuitySession.AssetRootPath,
            ProfilesRootPath = sessioncontinuitySession.ProfilesRootPath,
            CacheRootPath = sessioncontinuitySession.CacheRootPath,
            ConfigRootPath = sessioncontinuitySession.ConfigRootPath,
            SettingsFilePath = sessioncontinuitySession.SettingsFilePath,
            StartupProfilePath = sessioncontinuitySession.StartupProfilePath,
            RequiredAssets = sessioncontinuitySession.RequiredAssets,
            ReadyAssetCount = sessioncontinuitySession.ReadyAssetCount,
            CompletedSteps = sessioncontinuitySession.CompletedSteps,
            TotalSteps = sessioncontinuitySession.TotalSteps,
            Exists = sessioncontinuitySession.Exists,
            ReadSucceeded = sessioncontinuitySession.ReadSucceeded
        };

        if (!sessioncontinuitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser sessioncontinuity ready state blocked for profile '{sessioncontinuitySession.ProfileId}'.";
            result.Error = sessioncontinuitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSessionContinuityReadyStateVersion = "runtime-browser-sessioncontinuity-ready-state-v1";
        result.BrowserSessionContinuityReadyChecks =
        [
            "browser-runtimecontinuity-ready-state-ready",
            "browser-sessioncontinuity-session-ready",
            "browser-sessioncontinuity-ready"
        ];
        result.BrowserSessionContinuityReadySummary = $"Runtime browser sessioncontinuity ready state passed {result.BrowserSessionContinuityReadyChecks.Length} sessioncontinuity readiness check(s) for profile '{sessioncontinuitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sessioncontinuity ready state ready for profile '{sessioncontinuitySession.ProfileId}' with {result.BrowserSessionContinuityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSessionContinuityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSessionContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSessionContinuitySessionVersion { get; set; } = string.Empty;
    public string BrowserRuntimeContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserRuntimeContinuitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSessionContinuityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSessionContinuityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
