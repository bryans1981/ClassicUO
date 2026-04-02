namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAccessibilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAccessibilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAccessibilityReadyStateService : IBrowserClientRuntimeBrowserAccessibilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserAccessibilitySession _runtimeBrowserAccessibilitySession;

    public BrowserClientRuntimeBrowserAccessibilityReadyStateService(IBrowserClientRuntimeBrowserAccessibilitySession runtimeBrowserAccessibilitySession)
    {
        _runtimeBrowserAccessibilitySession = runtimeBrowserAccessibilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAccessibilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAccessibilitySessionResult accessibilitySession = await _runtimeBrowserAccessibilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAccessibilityReadyStateResult result = new()
        {
            ProfileId = accessibilitySession.ProfileId,
            SessionId = accessibilitySession.SessionId,
            SessionPath = accessibilitySession.SessionPath,
            BrowserAccessibilitySessionVersion = accessibilitySession.BrowserAccessibilitySessionVersion,
            BrowserUsabilityReadyStateVersion = accessibilitySession.BrowserUsabilityReadyStateVersion,
            BrowserUsabilitySessionVersion = accessibilitySession.BrowserUsabilitySessionVersion,
            LaunchMode = accessibilitySession.LaunchMode,
            AssetRootPath = accessibilitySession.AssetRootPath,
            ProfilesRootPath = accessibilitySession.ProfilesRootPath,
            CacheRootPath = accessibilitySession.CacheRootPath,
            ConfigRootPath = accessibilitySession.ConfigRootPath,
            SettingsFilePath = accessibilitySession.SettingsFilePath,
            StartupProfilePath = accessibilitySession.StartupProfilePath,
            RequiredAssets = accessibilitySession.RequiredAssets,
            ReadyAssetCount = accessibilitySession.ReadyAssetCount,
            CompletedSteps = accessibilitySession.CompletedSteps,
            TotalSteps = accessibilitySession.TotalSteps,
            Exists = accessibilitySession.Exists,
            ReadSucceeded = accessibilitySession.ReadSucceeded
        };

        if (!accessibilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser accessibility ready state blocked for profile '{accessibilitySession.ProfileId}'.";
            result.Error = accessibilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAccessibilityReadyStateVersion = "runtime-browser-accessibility-ready-state-v1";
        result.BrowserAccessibilityReadyChecks =
        [
            "browser-usability-ready-state-ready",
            "browser-accessibility-session-ready",
            "browser-accessibility-ready"
        ];
        result.BrowserAccessibilityReadySummary = $"Runtime browser accessibility ready state passed {result.BrowserAccessibilityReadyChecks.Length} accessibility readiness check(s) for profile '{accessibilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser accessibility ready state ready for profile '{accessibilitySession.ProfileId}' with {result.BrowserAccessibilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAccessibilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAccessibilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAccessibilitySessionVersion { get; set; } = string.Empty;
    public string BrowserUsabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserUsabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAccessibilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAccessibilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
