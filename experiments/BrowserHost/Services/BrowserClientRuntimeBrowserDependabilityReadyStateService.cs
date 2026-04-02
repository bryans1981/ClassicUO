namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDependabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserDependabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDependabilityReadyStateService : IBrowserClientRuntimeBrowserDependabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserDependabilitySession _runtimeBrowserDependabilitySession;

    public BrowserClientRuntimeBrowserDependabilityReadyStateService(IBrowserClientRuntimeBrowserDependabilitySession runtimeBrowserDependabilitySession)
    {
        _runtimeBrowserDependabilitySession = runtimeBrowserDependabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDependabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDependabilitySessionResult dependabilitySession = await _runtimeBrowserDependabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserDependabilityReadyStateResult result = new()
        {
            ProfileId = dependabilitySession.ProfileId,
            SessionId = dependabilitySession.SessionId,
            SessionPath = dependabilitySession.SessionPath,
            BrowserDependabilitySessionVersion = dependabilitySession.BrowserDependabilitySessionVersion,
            BrowserCredenceReadyStateVersion = dependabilitySession.BrowserCredenceReadyStateVersion,
            BrowserCredenceSessionVersion = dependabilitySession.BrowserCredenceSessionVersion,
            LaunchMode = dependabilitySession.LaunchMode,
            AssetRootPath = dependabilitySession.AssetRootPath,
            ProfilesRootPath = dependabilitySession.ProfilesRootPath,
            CacheRootPath = dependabilitySession.CacheRootPath,
            ConfigRootPath = dependabilitySession.ConfigRootPath,
            SettingsFilePath = dependabilitySession.SettingsFilePath,
            StartupProfilePath = dependabilitySession.StartupProfilePath,
            RequiredAssets = dependabilitySession.RequiredAssets,
            ReadyAssetCount = dependabilitySession.ReadyAssetCount,
            CompletedSteps = dependabilitySession.CompletedSteps,
            TotalSteps = dependabilitySession.TotalSteps,
            Exists = dependabilitySession.Exists,
            ReadSucceeded = dependabilitySession.ReadSucceeded
        };

        if (!dependabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser dependability ready state blocked for profile '{dependabilitySession.ProfileId}'.";
            result.Error = dependabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDependabilityReadyStateVersion = "runtime-browser-dependability-ready-state-v1";
        result.BrowserDependabilityReadyChecks =
        [
            "browser-credence-ready-state-ready",
            "browser-dependability-session-ready",
            "browser-dependability-ready"
        ];
        result.BrowserDependabilityReadySummary = $"Runtime browser dependability ready state passed {result.BrowserDependabilityReadyChecks.Length} dependability readiness check(s) for profile '{dependabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser dependability ready state ready for profile '{dependabilitySession.ProfileId}' with {result.BrowserDependabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDependabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserDependabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDependabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserCredenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCredenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDependabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserDependabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
