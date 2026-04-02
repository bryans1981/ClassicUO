namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAdaptabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAdaptabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAdaptabilityReadyStateService : IBrowserClientRuntimeBrowserAdaptabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserAdaptabilitySession _runtimeBrowserAdaptabilitySession;

    public BrowserClientRuntimeBrowserAdaptabilityReadyStateService(IBrowserClientRuntimeBrowserAdaptabilitySession runtimeBrowserAdaptabilitySession)
    {
        _runtimeBrowserAdaptabilitySession = runtimeBrowserAdaptabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAdaptabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAdaptabilitySessionResult adaptabilitySession = await _runtimeBrowserAdaptabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAdaptabilityReadyStateResult result = new()
        {
            ProfileId = adaptabilitySession.ProfileId,
            SessionId = adaptabilitySession.SessionId,
            SessionPath = adaptabilitySession.SessionPath,
            BrowserAdaptabilitySessionVersion = adaptabilitySession.BrowserAdaptabilitySessionVersion,
            BrowserInclusivityReadyStateVersion = adaptabilitySession.BrowserInclusivityReadyStateVersion,
            BrowserInclusivitySessionVersion = adaptabilitySession.BrowserInclusivitySessionVersion,
            LaunchMode = adaptabilitySession.LaunchMode,
            AssetRootPath = adaptabilitySession.AssetRootPath,
            ProfilesRootPath = adaptabilitySession.ProfilesRootPath,
            CacheRootPath = adaptabilitySession.CacheRootPath,
            ConfigRootPath = adaptabilitySession.ConfigRootPath,
            SettingsFilePath = adaptabilitySession.SettingsFilePath,
            StartupProfilePath = adaptabilitySession.StartupProfilePath,
            RequiredAssets = adaptabilitySession.RequiredAssets,
            ReadyAssetCount = adaptabilitySession.ReadyAssetCount,
            CompletedSteps = adaptabilitySession.CompletedSteps,
            TotalSteps = adaptabilitySession.TotalSteps,
            Exists = adaptabilitySession.Exists,
            ReadSucceeded = adaptabilitySession.ReadSucceeded
        };

        if (!adaptabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser adaptability ready state blocked for profile '{adaptabilitySession.ProfileId}'.";
            result.Error = adaptabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAdaptabilityReadyStateVersion = "runtime-browser-adaptability-ready-state-v1";
        result.BrowserAdaptabilityReadyChecks =
        [
            "browser-inclusivity-ready-state-ready",
            "browser-adaptability-session-ready",
            "browser-adaptability-ready"
        ];
        result.BrowserAdaptabilityReadySummary = $"Runtime browser adaptability ready state passed {result.BrowserAdaptabilityReadyChecks.Length} adaptability readiness check(s) for profile '{adaptabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser adaptability ready state ready for profile '{adaptabilitySession.ProfileId}' with {result.BrowserAdaptabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAdaptabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAdaptabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAdaptabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserInclusivityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInclusivitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAdaptabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAdaptabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
