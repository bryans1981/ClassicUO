namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFollowThroughSession
{
    ValueTask<BrowserClientRuntimeBrowserFollowThroughSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFollowThroughSessionService : IBrowserClientRuntimeBrowserFollowThroughSession
{
    private readonly IBrowserClientRuntimeBrowserDeterminationReadyState _runtimeBrowserDeterminationReadyState;

    public BrowserClientRuntimeBrowserFollowThroughSessionService(IBrowserClientRuntimeBrowserDeterminationReadyState runtimeBrowserDeterminationReadyState)
    {
        _runtimeBrowserDeterminationReadyState = runtimeBrowserDeterminationReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFollowThroughSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDeterminationReadyStateResult determinationReadyState = await _runtimeBrowserDeterminationReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserFollowThroughSessionResult result = new()
        {
            ProfileId = determinationReadyState.ProfileId,
            SessionId = determinationReadyState.SessionId,
            SessionPath = determinationReadyState.SessionPath,
            BrowserDeterminationReadyStateVersion = determinationReadyState.BrowserDeterminationReadyStateVersion,
            BrowserDeterminationSessionVersion = determinationReadyState.BrowserDeterminationSessionVersion,
            LaunchMode = determinationReadyState.LaunchMode,
            AssetRootPath = determinationReadyState.AssetRootPath,
            ProfilesRootPath = determinationReadyState.ProfilesRootPath,
            CacheRootPath = determinationReadyState.CacheRootPath,
            ConfigRootPath = determinationReadyState.ConfigRootPath,
            SettingsFilePath = determinationReadyState.SettingsFilePath,
            StartupProfilePath = determinationReadyState.StartupProfilePath,
            RequiredAssets = determinationReadyState.RequiredAssets,
            ReadyAssetCount = determinationReadyState.ReadyAssetCount,
            CompletedSteps = determinationReadyState.CompletedSteps,
            TotalSteps = determinationReadyState.TotalSteps,
            Exists = determinationReadyState.Exists,
            ReadSucceeded = determinationReadyState.ReadSucceeded
        };

        if (!determinationReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser followthrough session blocked for profile '{determinationReadyState.ProfileId}'.";
            result.Error = determinationReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFollowThroughSessionVersion = "runtime-browser-followthrough-session-v1";
        result.BrowserFollowThroughStages =
        [
            "open-browser-followthrough-session",
            "bind-browser-determination-ready-state",
            "publish-browser-followthrough-ready"
        ];
        result.BrowserFollowThroughSummary = $"Runtime browser followthrough session prepared {result.BrowserFollowThroughStages.Length} followthrough stage(s) for profile '{determinationReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser followthrough session ready for profile '{determinationReadyState.ProfileId}' with {result.BrowserFollowThroughStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFollowThroughSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserFollowThroughSessionVersion { get; set; } = string.Empty;
    public string BrowserDeterminationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDeterminationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFollowThroughStages { get; set; } = Array.Empty<string>();
    public string BrowserFollowThroughSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
