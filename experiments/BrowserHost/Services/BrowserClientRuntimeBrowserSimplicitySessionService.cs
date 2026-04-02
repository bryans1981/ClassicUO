namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSimplicitySession
{
    ValueTask<BrowserClientRuntimeBrowserSimplicitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSimplicitySessionService : IBrowserClientRuntimeBrowserSimplicitySession
{
    private readonly IBrowserClientRuntimeBrowserLegibilityReadyState _runtimeBrowserLegibilityReadyState;

    public BrowserClientRuntimeBrowserSimplicitySessionService(IBrowserClientRuntimeBrowserLegibilityReadyState runtimeBrowserLegibilityReadyState)
    {
        _runtimeBrowserLegibilityReadyState = runtimeBrowserLegibilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSimplicitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserLegibilityReadyStateResult legibilityReadyState = await _runtimeBrowserLegibilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSimplicitySessionResult result = new()
        {
            ProfileId = legibilityReadyState.ProfileId,
            SessionId = legibilityReadyState.SessionId,
            SessionPath = legibilityReadyState.SessionPath,
            BrowserLegibilityReadyStateVersion = legibilityReadyState.BrowserLegibilityReadyStateVersion,
            BrowserLegibilitySessionVersion = legibilityReadyState.BrowserLegibilitySessionVersion,
            LaunchMode = legibilityReadyState.LaunchMode,
            AssetRootPath = legibilityReadyState.AssetRootPath,
            ProfilesRootPath = legibilityReadyState.ProfilesRootPath,
            CacheRootPath = legibilityReadyState.CacheRootPath,
            ConfigRootPath = legibilityReadyState.ConfigRootPath,
            SettingsFilePath = legibilityReadyState.SettingsFilePath,
            StartupProfilePath = legibilityReadyState.StartupProfilePath,
            RequiredAssets = legibilityReadyState.RequiredAssets,
            ReadyAssetCount = legibilityReadyState.ReadyAssetCount,
            CompletedSteps = legibilityReadyState.CompletedSteps,
            TotalSteps = legibilityReadyState.TotalSteps,
            Exists = legibilityReadyState.Exists,
            ReadSucceeded = legibilityReadyState.ReadSucceeded
        };

        if (!legibilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser simplicity session blocked for profile '{legibilityReadyState.ProfileId}'.";
            result.Error = legibilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSimplicitySessionVersion = "runtime-browser-simplicity-session-v1";
        result.BrowserSimplicityStages =
        [
            "open-browser-simplicity-session",
            "bind-browser-legibility-ready-state",
            "publish-browser-simplicity-ready"
        ];
        result.BrowserSimplicitySummary = $"Runtime browser simplicity session prepared {result.BrowserSimplicityStages.Length} simplicity stage(s) for profile '{legibilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser simplicity session ready for profile '{legibilityReadyState.ProfileId}' with {result.BrowserSimplicityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSimplicitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSimplicitySessionVersion { get; set; } = string.Empty;
    public string BrowserLegibilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserLegibilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSimplicityStages { get; set; } = Array.Empty<string>();
    public string BrowserSimplicitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

