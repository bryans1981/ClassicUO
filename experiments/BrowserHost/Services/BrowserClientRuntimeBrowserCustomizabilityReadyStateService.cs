namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCustomizabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserCustomizabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCustomizabilityReadyStateService : IBrowserClientRuntimeBrowserCustomizabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserCustomizabilitySession _runtimeBrowserCustomizabilitySession;

    public BrowserClientRuntimeBrowserCustomizabilityReadyStateService(IBrowserClientRuntimeBrowserCustomizabilitySession runtimeBrowserCustomizabilitySession)
    {
        _runtimeBrowserCustomizabilitySession = runtimeBrowserCustomizabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCustomizabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCustomizabilitySessionResult customizabilitySession = await _runtimeBrowserCustomizabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserCustomizabilityReadyStateResult result = new()
        {
            ProfileId = customizabilitySession.ProfileId,
            SessionId = customizabilitySession.SessionId,
            SessionPath = customizabilitySession.SessionPath,
            BrowserCustomizabilitySessionVersion = customizabilitySession.BrowserCustomizabilitySessionVersion,
            BrowserControllabilityReadyStateVersion = customizabilitySession.BrowserControllabilityReadyStateVersion,
            BrowserControllabilitySessionVersion = customizabilitySession.BrowserControllabilitySessionVersion,
            LaunchMode = customizabilitySession.LaunchMode,
            AssetRootPath = customizabilitySession.AssetRootPath,
            ProfilesRootPath = customizabilitySession.ProfilesRootPath,
            CacheRootPath = customizabilitySession.CacheRootPath,
            ConfigRootPath = customizabilitySession.ConfigRootPath,
            SettingsFilePath = customizabilitySession.SettingsFilePath,
            StartupProfilePath = customizabilitySession.StartupProfilePath,
            RequiredAssets = customizabilitySession.RequiredAssets,
            ReadyAssetCount = customizabilitySession.ReadyAssetCount,
            CompletedSteps = customizabilitySession.CompletedSteps,
            TotalSteps = customizabilitySession.TotalSteps,
            Exists = customizabilitySession.Exists,
            ReadSucceeded = customizabilitySession.ReadSucceeded
        };

        if (!customizabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser customizability ready state blocked for profile '{customizabilitySession.ProfileId}'.";
            result.Error = customizabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCustomizabilityReadyStateVersion = "runtime-browser-customizability-ready-state-v1";
        result.BrowserCustomizabilityReadyChecks =
        [
            "browser-controllability-ready-state-ready",
            "browser-customizability-session-ready",
            "browser-customizability-ready"
        ];
        result.BrowserCustomizabilityReadySummary = $"Runtime browser customizability ready state passed {result.BrowserCustomizabilityReadyChecks.Length} customizability readiness check(s) for profile '{customizabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser customizability ready state ready for profile '{customizabilitySession.ProfileId}' with {result.BrowserCustomizabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCustomizabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserCustomizabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCustomizabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserControllabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserControllabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCustomizabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserCustomizabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
