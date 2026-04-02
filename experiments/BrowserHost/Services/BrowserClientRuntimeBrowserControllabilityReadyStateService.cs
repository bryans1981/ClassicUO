namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserControllabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserControllabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserControllabilityReadyStateService : IBrowserClientRuntimeBrowserControllabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserControllabilitySession _runtimeBrowserControllabilitySession;

    public BrowserClientRuntimeBrowserControllabilityReadyStateService(IBrowserClientRuntimeBrowserControllabilitySession runtimeBrowserControllabilitySession)
    {
        _runtimeBrowserControllabilitySession = runtimeBrowserControllabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserControllabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserControllabilitySessionResult controllabilitySession = await _runtimeBrowserControllabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserControllabilityReadyStateResult result = new()
        {
            ProfileId = controllabilitySession.ProfileId,
            SessionId = controllabilitySession.SessionId,
            SessionPath = controllabilitySession.SessionPath,
            BrowserControllabilitySessionVersion = controllabilitySession.BrowserControllabilitySessionVersion,
            BrowserEffectivenessReadyStateVersion = controllabilitySession.BrowserEffectivenessReadyStateVersion,
            BrowserEffectivenessSessionVersion = controllabilitySession.BrowserEffectivenessSessionVersion,
            LaunchMode = controllabilitySession.LaunchMode,
            AssetRootPath = controllabilitySession.AssetRootPath,
            ProfilesRootPath = controllabilitySession.ProfilesRootPath,
            CacheRootPath = controllabilitySession.CacheRootPath,
            ConfigRootPath = controllabilitySession.ConfigRootPath,
            SettingsFilePath = controllabilitySession.SettingsFilePath,
            StartupProfilePath = controllabilitySession.StartupProfilePath,
            RequiredAssets = controllabilitySession.RequiredAssets,
            ReadyAssetCount = controllabilitySession.ReadyAssetCount,
            CompletedSteps = controllabilitySession.CompletedSteps,
            TotalSteps = controllabilitySession.TotalSteps,
            Exists = controllabilitySession.Exists,
            ReadSucceeded = controllabilitySession.ReadSucceeded
        };

        if (!controllabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser controllability ready state blocked for profile '{controllabilitySession.ProfileId}'.";
            result.Error = controllabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserControllabilityReadyStateVersion = "runtime-browser-controllability-ready-state-v1";
        result.BrowserControllabilityReadyChecks =
        [
            "browser-effectiveness-ready-state-ready",
            "browser-controllability-session-ready",
            "browser-controllability-ready"
        ];
        result.BrowserControllabilityReadySummary = $"Runtime browser controllability ready state passed {result.BrowserControllabilityReadyChecks.Length} controllability readiness check(s) for profile '{controllabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser controllability ready state ready for profile '{controllabilitySession.ProfileId}' with {result.BrowserControllabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserControllabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserControllabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserControllabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserEffectivenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserEffectivenessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserControllabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserControllabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
