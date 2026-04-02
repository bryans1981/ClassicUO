namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSustainabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserSustainabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSustainabilitySessionService : IBrowserClientRuntimeBrowserSustainabilitySession
{
    private readonly IBrowserClientRuntimeBrowserDurabilityReadyState _runtimeBrowserDurabilityReadyState;

    public BrowserClientRuntimeBrowserSustainabilitySessionService(IBrowserClientRuntimeBrowserDurabilityReadyState runtimeBrowserDurabilityReadyState)
    {
        _runtimeBrowserDurabilityReadyState = runtimeBrowserDurabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSustainabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDurabilityReadyStateResult durabilityReadyState = await _runtimeBrowserDurabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSustainabilitySessionResult result = new()
        {
            ProfileId = durabilityReadyState.ProfileId,
            SessionId = durabilityReadyState.SessionId,
            SessionPath = durabilityReadyState.SessionPath,
            BrowserDurabilityReadyStateVersion = durabilityReadyState.BrowserDurabilityReadyStateVersion,
            BrowserDurabilitySessionVersion = durabilityReadyState.BrowserDurabilitySessionVersion,
            LaunchMode = durabilityReadyState.LaunchMode,
            AssetRootPath = durabilityReadyState.AssetRootPath,
            ProfilesRootPath = durabilityReadyState.ProfilesRootPath,
            CacheRootPath = durabilityReadyState.CacheRootPath,
            ConfigRootPath = durabilityReadyState.ConfigRootPath,
            SettingsFilePath = durabilityReadyState.SettingsFilePath,
            StartupProfilePath = durabilityReadyState.StartupProfilePath,
            RequiredAssets = durabilityReadyState.RequiredAssets,
            ReadyAssetCount = durabilityReadyState.ReadyAssetCount,
            CompletedSteps = durabilityReadyState.CompletedSteps,
            TotalSteps = durabilityReadyState.TotalSteps,
            Exists = durabilityReadyState.Exists,
            ReadSucceeded = durabilityReadyState.ReadSucceeded
        };

        if (!durabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser sustainability session blocked for profile '{durabilityReadyState.ProfileId}'.";
            result.Error = durabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSustainabilitySessionVersion = "runtime-browser-sustainability-session-v1";
        result.BrowserSustainabilityStages =
        [
            "open-browser-sustainability-session",
            "bind-browser-durability-ready-state",
            "publish-browser-sustainability-ready"
        ];
        result.BrowserSustainabilitySummary = $"Runtime browser sustainability session prepared {result.BrowserSustainabilityStages.Length} sustainability stage(s) for profile '{durabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sustainability session ready for profile '{durabilityReadyState.ProfileId}' with {result.BrowserSustainabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSustainabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSustainabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserDurabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDurabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSustainabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserSustainabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
