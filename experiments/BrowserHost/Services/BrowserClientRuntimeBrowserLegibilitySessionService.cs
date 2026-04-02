namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLegibilitySession
{
    ValueTask<BrowserClientRuntimeBrowserLegibilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLegibilitySessionService : IBrowserClientRuntimeBrowserLegibilitySession
{
    private readonly IBrowserClientRuntimeBrowserReadabilityReadyState _runtimeBrowserReadabilityReadyState;

    public BrowserClientRuntimeBrowserLegibilitySessionService(IBrowserClientRuntimeBrowserReadabilityReadyState runtimeBrowserReadabilityReadyState)
    {
        _runtimeBrowserReadabilityReadyState = runtimeBrowserReadabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLegibilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserReadabilityReadyStateResult readabilityReadyState = await _runtimeBrowserReadabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLegibilitySessionResult result = new()
        {
            ProfileId = readabilityReadyState.ProfileId,
            SessionId = readabilityReadyState.SessionId,
            SessionPath = readabilityReadyState.SessionPath,
            BrowserReadabilityReadyStateVersion = readabilityReadyState.BrowserReadabilityReadyStateVersion,
            BrowserReadabilitySessionVersion = readabilityReadyState.BrowserReadabilitySessionVersion,
            LaunchMode = readabilityReadyState.LaunchMode,
            AssetRootPath = readabilityReadyState.AssetRootPath,
            ProfilesRootPath = readabilityReadyState.ProfilesRootPath,
            CacheRootPath = readabilityReadyState.CacheRootPath,
            ConfigRootPath = readabilityReadyState.ConfigRootPath,
            SettingsFilePath = readabilityReadyState.SettingsFilePath,
            StartupProfilePath = readabilityReadyState.StartupProfilePath,
            RequiredAssets = readabilityReadyState.RequiredAssets,
            ReadyAssetCount = readabilityReadyState.ReadyAssetCount,
            CompletedSteps = readabilityReadyState.CompletedSteps,
            TotalSteps = readabilityReadyState.TotalSteps,
            Exists = readabilityReadyState.Exists,
            ReadSucceeded = readabilityReadyState.ReadSucceeded
        };

        if (!readabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser legibility session blocked for profile '{readabilityReadyState.ProfileId}'.";
            result.Error = readabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLegibilitySessionVersion = "runtime-browser-legibility-session-v1";
        result.BrowserLegibilityStages =
        [
            "open-browser-legibility-session",
            "bind-browser-readability-ready-state",
            "publish-browser-legibility-ready"
        ];
        result.BrowserLegibilitySummary = $"Runtime browser legibility session prepared {result.BrowserLegibilityStages.Length} legibility stage(s) for profile '{readabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser legibility session ready for profile '{readabilityReadyState.ProfileId}' with {result.BrowserLegibilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLegibilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserLegibilitySessionVersion { get; set; } = string.Empty;
    public string BrowserReadabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserReadabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLegibilityStages { get; set; } = Array.Empty<string>();
    public string BrowserLegibilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

