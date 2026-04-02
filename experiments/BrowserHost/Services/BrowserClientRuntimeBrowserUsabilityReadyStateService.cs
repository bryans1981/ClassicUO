namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserUsabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserUsabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserUsabilityReadyStateService : IBrowserClientRuntimeBrowserUsabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserUsabilitySession _runtimeBrowserUsabilitySession;

    public BrowserClientRuntimeBrowserUsabilityReadyStateService(IBrowserClientRuntimeBrowserUsabilitySession runtimeBrowserUsabilitySession)
    {
        _runtimeBrowserUsabilitySession = runtimeBrowserUsabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserUsabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserUsabilitySessionResult usabilitySession = await _runtimeBrowserUsabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserUsabilityReadyStateResult result = new()
        {
            ProfileId = usabilitySession.ProfileId,
            SessionId = usabilitySession.SessionId,
            SessionPath = usabilitySession.SessionPath,
            BrowserUsabilitySessionVersion = usabilitySession.BrowserUsabilitySessionVersion,
            BrowserSupportabilityReadyStateVersion = usabilitySession.BrowserSupportabilityReadyStateVersion,
            BrowserSupportabilitySessionVersion = usabilitySession.BrowserSupportabilitySessionVersion,
            LaunchMode = usabilitySession.LaunchMode,
            AssetRootPath = usabilitySession.AssetRootPath,
            ProfilesRootPath = usabilitySession.ProfilesRootPath,
            CacheRootPath = usabilitySession.CacheRootPath,
            ConfigRootPath = usabilitySession.ConfigRootPath,
            SettingsFilePath = usabilitySession.SettingsFilePath,
            StartupProfilePath = usabilitySession.StartupProfilePath,
            RequiredAssets = usabilitySession.RequiredAssets,
            ReadyAssetCount = usabilitySession.ReadyAssetCount,
            CompletedSteps = usabilitySession.CompletedSteps,
            TotalSteps = usabilitySession.TotalSteps,
            Exists = usabilitySession.Exists,
            ReadSucceeded = usabilitySession.ReadSucceeded
        };

        if (!usabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser usability ready state blocked for profile '{usabilitySession.ProfileId}'.";
            result.Error = usabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserUsabilityReadyStateVersion = "runtime-browser-usability-ready-state-v1";
        result.BrowserUsabilityReadyChecks =
        [
            "browser-supportability-ready-state-ready",
            "browser-usability-session-ready",
            "browser-usability-ready"
        ];
        result.BrowserUsabilityReadySummary = $"Runtime browser usability ready state passed {result.BrowserUsabilityReadyChecks.Length} usability readiness check(s) for profile '{usabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser usability ready state ready for profile '{usabilitySession.ProfileId}' with {result.BrowserUsabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserUsabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserUsabilityReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserUsabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserUsabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
