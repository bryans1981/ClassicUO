namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserScannabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserScannabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserScannabilitySessionService : IBrowserClientRuntimeBrowserScannabilitySession
{
    private readonly IBrowserClientRuntimeBrowserUnderstandabilityReadyState _runtimeBrowserUnderstandabilityReadyState;

    public BrowserClientRuntimeBrowserScannabilitySessionService(IBrowserClientRuntimeBrowserUnderstandabilityReadyState runtimeBrowserUnderstandabilityReadyState)
    {
        _runtimeBrowserUnderstandabilityReadyState = runtimeBrowserUnderstandabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserScannabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserUnderstandabilityReadyStateResult understandabilityReadyState = await _runtimeBrowserUnderstandabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserScannabilitySessionResult result = new()
        {
            ProfileId = understandabilityReadyState.ProfileId,
            SessionId = understandabilityReadyState.SessionId,
            SessionPath = understandabilityReadyState.SessionPath,
            BrowserUnderstandabilityReadyStateVersion = understandabilityReadyState.BrowserUnderstandabilityReadyStateVersion,
            BrowserUnderstandabilitySessionVersion = understandabilityReadyState.BrowserUnderstandabilitySessionVersion,
            LaunchMode = understandabilityReadyState.LaunchMode,
            AssetRootPath = understandabilityReadyState.AssetRootPath,
            ProfilesRootPath = understandabilityReadyState.ProfilesRootPath,
            CacheRootPath = understandabilityReadyState.CacheRootPath,
            ConfigRootPath = understandabilityReadyState.ConfigRootPath,
            SettingsFilePath = understandabilityReadyState.SettingsFilePath,
            StartupProfilePath = understandabilityReadyState.StartupProfilePath,
            RequiredAssets = understandabilityReadyState.RequiredAssets,
            ReadyAssetCount = understandabilityReadyState.ReadyAssetCount,
            CompletedSteps = understandabilityReadyState.CompletedSteps,
            TotalSteps = understandabilityReadyState.TotalSteps,
            Exists = understandabilityReadyState.Exists,
            ReadSucceeded = understandabilityReadyState.ReadSucceeded
        };

        if (!understandabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser scannability session blocked for profile '{understandabilityReadyState.ProfileId}'.";
            result.Error = understandabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserScannabilitySessionVersion = "runtime-browser-scannability-session-v1";
        result.BrowserScannabilityStages =
        [
            "open-browser-scannability-session",
            "bind-browser-understandability-ready-state",
            "publish-browser-scannability-ready"
        ];
        result.BrowserScannabilitySummary = $"Runtime browser scannability session prepared {result.BrowserScannabilityStages.Length} scannability stage(s) for profile '{understandabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser scannability session ready for profile '{understandabilityReadyState.ProfileId}' with {result.BrowserScannabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserScannabilitySessionResult
{
    public bool IsReady { get; set; }
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
    public string[] BrowserScannabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserScannabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

