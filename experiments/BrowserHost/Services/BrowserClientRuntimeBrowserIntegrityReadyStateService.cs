namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserIntegrityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserIntegrityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserIntegrityReadyStateService : IBrowserClientRuntimeBrowserIntegrityReadyState
{
    private readonly IBrowserClientRuntimeBrowserIntegritySession _runtimeBrowserIntegritySession;

    public BrowserClientRuntimeBrowserIntegrityReadyStateService(IBrowserClientRuntimeBrowserIntegritySession runtimeBrowserIntegritySession)
    {
        _runtimeBrowserIntegritySession = runtimeBrowserIntegritySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserIntegrityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserIntegritySessionResult integritySession = await _runtimeBrowserIntegritySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserIntegrityReadyStateResult result = new()
        {
            ProfileId = integritySession.ProfileId,
            SessionId = integritySession.SessionId,
            SessionPath = integritySession.SessionPath,
            BrowserIntegritySessionVersion = integritySession.BrowserIntegritySessionVersion,
            BrowserRiskReadyStateVersion = integritySession.BrowserRiskReadyStateVersion,
            BrowserRiskSessionVersion = integritySession.BrowserRiskSessionVersion,
            LaunchMode = integritySession.LaunchMode,
            AssetRootPath = integritySession.AssetRootPath,
            ProfilesRootPath = integritySession.ProfilesRootPath,
            CacheRootPath = integritySession.CacheRootPath,
            ConfigRootPath = integritySession.ConfigRootPath,
            SettingsFilePath = integritySession.SettingsFilePath,
            StartupProfilePath = integritySession.StartupProfilePath,
            RequiredAssets = integritySession.RequiredAssets,
            ReadyAssetCount = integritySession.ReadyAssetCount,
            CompletedSteps = integritySession.CompletedSteps,
            TotalSteps = integritySession.TotalSteps,
            Exists = integritySession.Exists,
            ReadSucceeded = integritySession.ReadSucceeded,
            BrowserIntegritySession = integritySession
        };

        if (!integritySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser integrity ready state blocked for profile '{integritySession.ProfileId}'.";
            result.Error = integritySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserIntegrityReadyStateVersion = "runtime-browser-integrity-ready-state-v1";
        result.BrowserIntegrityReadyChecks =
        [
            "browser-risk-ready-state-ready",
            "browser-integrity-session-ready",
            "browser-integrity-ready"
        ];
        result.BrowserIntegrityReadySummary = $"Runtime browser integrity ready state passed {result.BrowserIntegrityReadyChecks.Length} integrity readiness check(s) for profile '{integritySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser integrity ready state ready for profile '{integritySession.ProfileId}' with {result.BrowserIntegrityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserIntegrityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserIntegrityReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserIntegrityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserIntegrityReadySummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserIntegritySessionResult BrowserIntegritySession { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
