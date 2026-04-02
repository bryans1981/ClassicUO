namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserNavigabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserNavigabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserNavigabilityReadyStateService : IBrowserClientRuntimeBrowserNavigabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserNavigabilitySession _runtimeBrowserNavigabilitySession;

    public BrowserClientRuntimeBrowserNavigabilityReadyStateService(IBrowserClientRuntimeBrowserNavigabilitySession runtimeBrowserNavigabilitySession)
    {
        _runtimeBrowserNavigabilitySession = runtimeBrowserNavigabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserNavigabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserNavigabilitySessionResult navigabilitySession = await _runtimeBrowserNavigabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserNavigabilityReadyStateResult result = new()
        {
            ProfileId = navigabilitySession.ProfileId,
            SessionId = navigabilitySession.SessionId,
            SessionPath = navigabilitySession.SessionPath,
            BrowserNavigabilitySessionVersion = navigabilitySession.BrowserNavigabilitySessionVersion,
            BrowserApproachabilityReadyStateVersion = navigabilitySession.BrowserApproachabilityReadyStateVersion,
            BrowserApproachabilitySessionVersion = navigabilitySession.BrowserApproachabilitySessionVersion,
            LaunchMode = navigabilitySession.LaunchMode,
            AssetRootPath = navigabilitySession.AssetRootPath,
            ProfilesRootPath = navigabilitySession.ProfilesRootPath,
            CacheRootPath = navigabilitySession.CacheRootPath,
            ConfigRootPath = navigabilitySession.ConfigRootPath,
            SettingsFilePath = navigabilitySession.SettingsFilePath,
            StartupProfilePath = navigabilitySession.StartupProfilePath,
            RequiredAssets = navigabilitySession.RequiredAssets,
            ReadyAssetCount = navigabilitySession.ReadyAssetCount,
            CompletedSteps = navigabilitySession.CompletedSteps,
            TotalSteps = navigabilitySession.TotalSteps,
            Exists = navigabilitySession.Exists,
            ReadSucceeded = navigabilitySession.ReadSucceeded
        };

        if (!navigabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser navigability ready state blocked for profile '{navigabilitySession.ProfileId}'.";
            result.Error = navigabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserNavigabilityReadyStateVersion = "runtime-browser-navigability-ready-state-v1";
        result.BrowserNavigabilityReadyChecks =
        [
            "browser-approachability-ready-state-ready",
            "browser-navigability-session-ready",
            "browser-navigability-ready"
        ];
        result.BrowserNavigabilityReadySummary = $"Runtime browser navigability ready state passed {result.BrowserNavigabilityReadyChecks.Length} navigability readiness check(s) for profile '{navigabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser navigability ready state ready for profile '{navigabilitySession.ProfileId}' with {result.BrowserNavigabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserNavigabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserNavigabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserNavigabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserApproachabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserApproachabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserNavigabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserNavigabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

