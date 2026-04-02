namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserScannabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserScannabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserScannabilityReadyStateService : IBrowserClientRuntimeBrowserScannabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserScannabilitySession _runtimeBrowserScannabilitySession;

    public BrowserClientRuntimeBrowserScannabilityReadyStateService(IBrowserClientRuntimeBrowserScannabilitySession runtimeBrowserScannabilitySession)
    {
        _runtimeBrowserScannabilitySession = runtimeBrowserScannabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserScannabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserScannabilitySessionResult scannabilitySession = await _runtimeBrowserScannabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserScannabilityReadyStateResult result = new()
        {
            ProfileId = scannabilitySession.ProfileId,
            SessionId = scannabilitySession.SessionId,
            SessionPath = scannabilitySession.SessionPath,
            BrowserScannabilitySessionVersion = scannabilitySession.BrowserScannabilitySessionVersion,
            BrowserUnderstandabilityReadyStateVersion = scannabilitySession.BrowserUnderstandabilityReadyStateVersion,
            BrowserUnderstandabilitySessionVersion = scannabilitySession.BrowserUnderstandabilitySessionVersion,
            LaunchMode = scannabilitySession.LaunchMode,
            AssetRootPath = scannabilitySession.AssetRootPath,
            ProfilesRootPath = scannabilitySession.ProfilesRootPath,
            CacheRootPath = scannabilitySession.CacheRootPath,
            ConfigRootPath = scannabilitySession.ConfigRootPath,
            SettingsFilePath = scannabilitySession.SettingsFilePath,
            StartupProfilePath = scannabilitySession.StartupProfilePath,
            RequiredAssets = scannabilitySession.RequiredAssets,
            ReadyAssetCount = scannabilitySession.ReadyAssetCount,
            CompletedSteps = scannabilitySession.CompletedSteps,
            TotalSteps = scannabilitySession.TotalSteps,
            Exists = scannabilitySession.Exists,
            ReadSucceeded = scannabilitySession.ReadSucceeded
        };

        if (!scannabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser scannability ready state blocked for profile '{scannabilitySession.ProfileId}'.";
            result.Error = scannabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserScannabilityReadyStateVersion = "runtime-browser-scannability-ready-state-v1";
        result.BrowserScannabilityReadyChecks =
        [
            "browser-understandability-ready-state-ready",
            "browser-scannability-session-ready",
            "browser-scannability-ready"
        ];
        result.BrowserScannabilityReadySummary = $"Runtime browser scannability ready state passed {result.BrowserScannabilityReadyChecks.Length} scannability readiness check(s) for profile '{scannabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser scannability ready state ready for profile '{scannabilitySession.ProfileId}' with {result.BrowserScannabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserScannabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserScannabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserScannabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserUnderstandabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserUnderstandabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserScannabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserScannabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

