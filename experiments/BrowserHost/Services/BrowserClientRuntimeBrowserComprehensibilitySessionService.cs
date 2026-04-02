namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserComprehensibilitySession
{
    ValueTask<BrowserClientRuntimeBrowserComprehensibilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserComprehensibilitySessionService : IBrowserClientRuntimeBrowserComprehensibilitySession
{
    private readonly IBrowserClientRuntimeBrowserScannabilityReadyState _runtimeBrowserScannabilityReadyState;

    public BrowserClientRuntimeBrowserComprehensibilitySessionService(IBrowserClientRuntimeBrowserScannabilityReadyState runtimeBrowserScannabilityReadyState)
    {
        _runtimeBrowserScannabilityReadyState = runtimeBrowserScannabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserComprehensibilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserScannabilityReadyStateResult scannabilityReadyState = await _runtimeBrowserScannabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserComprehensibilitySessionResult result = new()
        {
            ProfileId = scannabilityReadyState.ProfileId,
            SessionId = scannabilityReadyState.SessionId,
            SessionPath = scannabilityReadyState.SessionPath,
            BrowserScannabilityReadyStateVersion = scannabilityReadyState.BrowserScannabilityReadyStateVersion,
            BrowserScannabilitySessionVersion = scannabilityReadyState.BrowserScannabilitySessionVersion,
            LaunchMode = scannabilityReadyState.LaunchMode,
            AssetRootPath = scannabilityReadyState.AssetRootPath,
            ProfilesRootPath = scannabilityReadyState.ProfilesRootPath,
            CacheRootPath = scannabilityReadyState.CacheRootPath,
            ConfigRootPath = scannabilityReadyState.ConfigRootPath,
            SettingsFilePath = scannabilityReadyState.SettingsFilePath,
            StartupProfilePath = scannabilityReadyState.StartupProfilePath,
            RequiredAssets = scannabilityReadyState.RequiredAssets,
            ReadyAssetCount = scannabilityReadyState.ReadyAssetCount,
            CompletedSteps = scannabilityReadyState.CompletedSteps,
            TotalSteps = scannabilityReadyState.TotalSteps,
            Exists = scannabilityReadyState.Exists,
            ReadSucceeded = scannabilityReadyState.ReadSucceeded
        };

        if (!scannabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser comprehensibility session blocked for profile '{scannabilityReadyState.ProfileId}'.";
            result.Error = scannabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserComprehensibilitySessionVersion = "runtime-browser-comprehensibility-session-v1";
        result.BrowserComprehensibilityStages =
        [
            "open-browser-comprehensibility-session",
            "bind-browser-scannability-ready-state",
            "publish-browser-comprehensibility-ready"
        ];
        result.BrowserComprehensibilitySummary = $"Runtime browser comprehensibility session prepared {result.BrowserComprehensibilityStages.Length} comprehensibility stage(s) for profile '{scannabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser comprehensibility session ready for profile '{scannabilityReadyState.ProfileId}' with {result.BrowserComprehensibilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserComprehensibilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserComprehensibilitySessionVersion { get; set; } = string.Empty;
    public string BrowserScannabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserScannabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserComprehensibilityStages { get; set; } = Array.Empty<string>();
    public string BrowserComprehensibilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

