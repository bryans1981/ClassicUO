namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAssurabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserAssurabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAssurabilitySessionService : IBrowserClientRuntimeBrowserAssurabilitySession
{
    private readonly IBrowserClientRuntimeBrowserDependabilityReadyState _runtimeBrowserDependabilityReadyState;

    public BrowserClientRuntimeBrowserAssurabilitySessionService(IBrowserClientRuntimeBrowserDependabilityReadyState runtimeBrowserDependabilityReadyState)
    {
        _runtimeBrowserDependabilityReadyState = runtimeBrowserDependabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAssurabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDependabilityReadyStateResult dependabilityReadyState = await _runtimeBrowserDependabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAssurabilitySessionResult result = new()
        {
            ProfileId = dependabilityReadyState.ProfileId,
            SessionId = dependabilityReadyState.SessionId,
            SessionPath = dependabilityReadyState.SessionPath,
            BrowserDependabilityReadyStateVersion = dependabilityReadyState.BrowserDependabilityReadyStateVersion,
            BrowserDependabilitySessionVersion = dependabilityReadyState.BrowserDependabilitySessionVersion,
            LaunchMode = dependabilityReadyState.LaunchMode,
            AssetRootPath = dependabilityReadyState.AssetRootPath,
            ProfilesRootPath = dependabilityReadyState.ProfilesRootPath,
            CacheRootPath = dependabilityReadyState.CacheRootPath,
            ConfigRootPath = dependabilityReadyState.ConfigRootPath,
            SettingsFilePath = dependabilityReadyState.SettingsFilePath,
            StartupProfilePath = dependabilityReadyState.StartupProfilePath,
            RequiredAssets = dependabilityReadyState.RequiredAssets,
            ReadyAssetCount = dependabilityReadyState.ReadyAssetCount,
            CompletedSteps = dependabilityReadyState.CompletedSteps,
            TotalSteps = dependabilityReadyState.TotalSteps,
            Exists = dependabilityReadyState.Exists,
            ReadSucceeded = dependabilityReadyState.ReadSucceeded
        };

        if (!dependabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser assurability session blocked for profile '{dependabilityReadyState.ProfileId}'.";
            result.Error = dependabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAssurabilitySessionVersion = "runtime-browser-assurability-session-v1";
        result.BrowserAssurabilityStages =
        [
            "open-browser-assurability-session",
            "bind-browser-dependability-ready-state",
            "publish-browser-assurability-ready"
        ];
        result.BrowserAssurabilitySummary = $"Runtime browser assurability session prepared {result.BrowserAssurabilityStages.Length} assurability stage(s) for profile '{dependabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser assurability session ready for profile '{dependabilityReadyState.ProfileId}' with {result.BrowserAssurabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAssurabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAssurabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserDependabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDependabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAssurabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserAssurabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
