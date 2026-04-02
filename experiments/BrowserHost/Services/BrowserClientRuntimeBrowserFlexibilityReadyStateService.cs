namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFlexibilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserFlexibilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFlexibilityReadyStateService : IBrowserClientRuntimeBrowserFlexibilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserFlexibilitySession _runtimeBrowserFlexibilitySession;

    public BrowserClientRuntimeBrowserFlexibilityReadyStateService(IBrowserClientRuntimeBrowserFlexibilitySession runtimeBrowserFlexibilitySession)
    {
        _runtimeBrowserFlexibilitySession = runtimeBrowserFlexibilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFlexibilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserFlexibilitySessionResult flexibilitySession = await _runtimeBrowserFlexibilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserFlexibilityReadyStateResult result = new()
        {
            ProfileId = flexibilitySession.ProfileId,
            SessionId = flexibilitySession.SessionId,
            SessionPath = flexibilitySession.SessionPath,
            BrowserFlexibilitySessionVersion = flexibilitySession.BrowserFlexibilitySessionVersion,
            BrowserCustomizabilityReadyStateVersion = flexibilitySession.BrowserCustomizabilityReadyStateVersion,
            BrowserCustomizabilitySessionVersion = flexibilitySession.BrowserCustomizabilitySessionVersion,
            LaunchMode = flexibilitySession.LaunchMode,
            AssetRootPath = flexibilitySession.AssetRootPath,
            ProfilesRootPath = flexibilitySession.ProfilesRootPath,
            CacheRootPath = flexibilitySession.CacheRootPath,
            ConfigRootPath = flexibilitySession.ConfigRootPath,
            SettingsFilePath = flexibilitySession.SettingsFilePath,
            StartupProfilePath = flexibilitySession.StartupProfilePath,
            RequiredAssets = flexibilitySession.RequiredAssets,
            ReadyAssetCount = flexibilitySession.ReadyAssetCount,
            CompletedSteps = flexibilitySession.CompletedSteps,
            TotalSteps = flexibilitySession.TotalSteps,
            Exists = flexibilitySession.Exists,
            ReadSucceeded = flexibilitySession.ReadSucceeded
        };

        if (!flexibilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser flexibility ready state blocked for profile '{flexibilitySession.ProfileId}'.";
            result.Error = flexibilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFlexibilityReadyStateVersion = "runtime-browser-flexibility-ready-state-v1";
        result.BrowserFlexibilityReadyChecks =
        [
            "browser-customizability-ready-state-ready",
            "browser-flexibility-session-ready",
            "browser-flexibility-ready"
        ];
        result.BrowserFlexibilityReadySummary = $"Runtime browser flexibility ready state passed {result.BrowserFlexibilityReadyChecks.Length} flexibility readiness check(s) for profile '{flexibilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser flexibility ready state ready for profile '{flexibilitySession.ProfileId}' with {result.BrowserFlexibilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFlexibilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserFlexibilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserFlexibilitySessionVersion { get; set; } = string.Empty;
    public string BrowserCustomizabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCustomizabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFlexibilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserFlexibilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
