namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDeterminationSession
{
    ValueTask<BrowserClientRuntimeBrowserDeterminationSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDeterminationSessionService : IBrowserClientRuntimeBrowserDeterminationSession
{
    private readonly IBrowserClientRuntimeBrowserResolveReadyState _runtimeBrowserResolveReadyState;

    public BrowserClientRuntimeBrowserDeterminationSessionService(IBrowserClientRuntimeBrowserResolveReadyState runtimeBrowserResolveReadyState)
    {
        _runtimeBrowserResolveReadyState = runtimeBrowserResolveReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDeterminationSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserResolveReadyStateResult resolveReadyState = await _runtimeBrowserResolveReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserDeterminationSessionResult result = new()
        {
            ProfileId = resolveReadyState.ProfileId,
            SessionId = resolveReadyState.SessionId,
            SessionPath = resolveReadyState.SessionPath,
            BrowserResolveReadyStateVersion = resolveReadyState.BrowserResolveReadyStateVersion,
            BrowserResolveSessionVersion = resolveReadyState.BrowserResolveSessionVersion,
            LaunchMode = resolveReadyState.LaunchMode,
            AssetRootPath = resolveReadyState.AssetRootPath,
            ProfilesRootPath = resolveReadyState.ProfilesRootPath,
            CacheRootPath = resolveReadyState.CacheRootPath,
            ConfigRootPath = resolveReadyState.ConfigRootPath,
            SettingsFilePath = resolveReadyState.SettingsFilePath,
            StartupProfilePath = resolveReadyState.StartupProfilePath,
            RequiredAssets = resolveReadyState.RequiredAssets,
            ReadyAssetCount = resolveReadyState.ReadyAssetCount,
            CompletedSteps = resolveReadyState.CompletedSteps,
            TotalSteps = resolveReadyState.TotalSteps,
            Exists = resolveReadyState.Exists,
            ReadSucceeded = resolveReadyState.ReadSucceeded
        };

        if (!resolveReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser determination session blocked for profile '{resolveReadyState.ProfileId}'.";
            result.Error = resolveReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDeterminationSessionVersion = "runtime-browser-determination-session-v1";
        result.BrowserDeterminationStages =
        [
            "open-browser-determination-session",
            "bind-browser-resolve-ready-state",
            "publish-browser-determination-ready"
        ];
        result.BrowserDeterminationSummary = $"Runtime browser determination session prepared {result.BrowserDeterminationStages.Length} determination stage(s) for profile '{resolveReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser determination session ready for profile '{resolveReadyState.ProfileId}' with {result.BrowserDeterminationStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDeterminationSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserDeterminationSessionVersion { get; set; } = string.Empty;
    public string BrowserResolveReadyStateVersion { get; set; } = string.Empty;
    public string BrowserResolveSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDeterminationStages { get; set; } = Array.Empty<string>();
    public string BrowserDeterminationSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
