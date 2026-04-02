namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserConvictionSession
{
    ValueTask<BrowserClientRuntimeBrowserConvictionSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserConvictionSessionService : IBrowserClientRuntimeBrowserConvictionSession
{
    private readonly IBrowserClientRuntimeBrowserAssurabilityReadyState _runtimeBrowserAssurabilityReadyState;

    public BrowserClientRuntimeBrowserConvictionSessionService(IBrowserClientRuntimeBrowserAssurabilityReadyState runtimeBrowserAssurabilityReadyState)
    {
        _runtimeBrowserAssurabilityReadyState = runtimeBrowserAssurabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserConvictionSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAssurabilityReadyStateResult assurabilityReadyState = await _runtimeBrowserAssurabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserConvictionSessionResult result = new()
        {
            ProfileId = assurabilityReadyState.ProfileId,
            SessionId = assurabilityReadyState.SessionId,
            SessionPath = assurabilityReadyState.SessionPath,
            BrowserAssurabilityReadyStateVersion = assurabilityReadyState.BrowserAssurabilityReadyStateVersion,
            BrowserAssurabilitySessionVersion = assurabilityReadyState.BrowserAssurabilitySessionVersion,
            LaunchMode = assurabilityReadyState.LaunchMode,
            AssetRootPath = assurabilityReadyState.AssetRootPath,
            ProfilesRootPath = assurabilityReadyState.ProfilesRootPath,
            CacheRootPath = assurabilityReadyState.CacheRootPath,
            ConfigRootPath = assurabilityReadyState.ConfigRootPath,
            SettingsFilePath = assurabilityReadyState.SettingsFilePath,
            StartupProfilePath = assurabilityReadyState.StartupProfilePath,
            RequiredAssets = assurabilityReadyState.RequiredAssets,
            ReadyAssetCount = assurabilityReadyState.ReadyAssetCount,
            CompletedSteps = assurabilityReadyState.CompletedSteps,
            TotalSteps = assurabilityReadyState.TotalSteps,
            Exists = assurabilityReadyState.Exists,
            ReadSucceeded = assurabilityReadyState.ReadSucceeded
        };

        if (!assurabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser conviction session blocked for profile '{assurabilityReadyState.ProfileId}'.";
            result.Error = assurabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserConvictionSessionVersion = "runtime-browser-conviction-session-v1";
        result.BrowserConvictionStages =
        [
            "open-browser-conviction-session",
            "bind-browser-assurability-ready-state",
            "publish-browser-conviction-ready"
        ];
        result.BrowserConvictionSummary = $"Runtime browser conviction session prepared {result.BrowserConvictionStages.Length} conviction stage(s) for profile '{assurabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser conviction session ready for profile '{assurabilityReadyState.ProfileId}' with {result.BrowserConvictionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserConvictionSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserConvictionSessionVersion { get; set; } = string.Empty;
    public string BrowserAssurabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAssurabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserConvictionStages { get; set; } = Array.Empty<string>();
    public string BrowserConvictionSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
