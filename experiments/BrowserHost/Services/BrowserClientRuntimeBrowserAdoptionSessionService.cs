namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAdoptionSession
{
    ValueTask<BrowserClientRuntimeBrowserAdoptionSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAdoptionSessionService : IBrowserClientRuntimeBrowserAdoptionSession
{
    private readonly IBrowserClientRuntimeBrowserAcceptanceReadyState _runtimeBrowserAcceptanceReadyState;

    public BrowserClientRuntimeBrowserAdoptionSessionService(IBrowserClientRuntimeBrowserAcceptanceReadyState runtimeBrowserAcceptanceReadyState)
    {
        _runtimeBrowserAcceptanceReadyState = runtimeBrowserAcceptanceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAdoptionSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAcceptanceReadyStateResult acceptanceReadyState = await _runtimeBrowserAcceptanceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAdoptionSessionResult result = new()
        {
            ProfileId = acceptanceReadyState.ProfileId,
            SessionId = acceptanceReadyState.SessionId,
            SessionPath = acceptanceReadyState.SessionPath,
            BrowserAcceptanceReadyStateVersion = acceptanceReadyState.BrowserAcceptanceReadyStateVersion,
            BrowserAcceptanceSessionVersion = acceptanceReadyState.BrowserAcceptanceSessionVersion,
            LaunchMode = acceptanceReadyState.LaunchMode,
            AssetRootPath = acceptanceReadyState.AssetRootPath,
            ProfilesRootPath = acceptanceReadyState.ProfilesRootPath,
            CacheRootPath = acceptanceReadyState.CacheRootPath,
            ConfigRootPath = acceptanceReadyState.ConfigRootPath,
            SettingsFilePath = acceptanceReadyState.SettingsFilePath,
            StartupProfilePath = acceptanceReadyState.StartupProfilePath,
            RequiredAssets = acceptanceReadyState.RequiredAssets,
            ReadyAssetCount = acceptanceReadyState.ReadyAssetCount,
            CompletedSteps = acceptanceReadyState.CompletedSteps,
            TotalSteps = acceptanceReadyState.TotalSteps,
            Exists = acceptanceReadyState.Exists,
            ReadSucceeded = acceptanceReadyState.ReadSucceeded
        };

        if (!acceptanceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser adoption session blocked for profile '{acceptanceReadyState.ProfileId}'.";
            result.Error = acceptanceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAdoptionSessionVersion = "runtime-browser-adoption-session-v1";
        result.BrowserAdoptionStages =
        [
            "open-browser-adoption-session",
            "bind-browser-acceptance-ready-state",
            "publish-browser-adoption-ready"
        ];
        result.BrowserAdoptionSummary = $"Runtime browser adoption session prepared {result.BrowserAdoptionStages.Length} adoption stage(s) for profile '{acceptanceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser adoption session ready for profile '{acceptanceReadyState.ProfileId}' with {result.BrowserAdoptionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAdoptionSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAdoptionSessionVersion { get; set; } = string.Empty;
    public string BrowserAcceptanceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAcceptanceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAdoptionStages { get; set; } = Array.Empty<string>();
    public string BrowserAdoptionSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
