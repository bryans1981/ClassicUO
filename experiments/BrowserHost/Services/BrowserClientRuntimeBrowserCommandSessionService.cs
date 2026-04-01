namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCommandSession
{
    ValueTask<BrowserClientRuntimeBrowserCommandSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCommandSessionService : IBrowserClientRuntimeBrowserCommandSession
{
    private readonly IBrowserClientRuntimeBrowserPointerReadyState _runtimeBrowserPointerReadyState;

    public BrowserClientRuntimeBrowserCommandSessionService(IBrowserClientRuntimeBrowserPointerReadyState runtimeBrowserPointerReadyState)
    {
        _runtimeBrowserPointerReadyState = runtimeBrowserPointerReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCommandSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPointerReadyStateResult pointerReadyState = await _runtimeBrowserPointerReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserCommandSessionResult result = new()
        {
            ProfileId = pointerReadyState.ProfileId,
            SessionId = pointerReadyState.SessionId,
            SessionPath = pointerReadyState.SessionPath,
            BrowserPointerReadyStateVersion = pointerReadyState.BrowserPointerReadyStateVersion,
            BrowserPointerSessionVersion = pointerReadyState.BrowserPointerSessionVersion,
            LaunchMode = pointerReadyState.LaunchMode,
            AssetRootPath = pointerReadyState.AssetRootPath,
            ProfilesRootPath = pointerReadyState.ProfilesRootPath,
            CacheRootPath = pointerReadyState.CacheRootPath,
            ConfigRootPath = pointerReadyState.ConfigRootPath,
            SettingsFilePath = pointerReadyState.SettingsFilePath,
            StartupProfilePath = pointerReadyState.StartupProfilePath,
            RequiredAssets = pointerReadyState.RequiredAssets,
            ReadyAssetCount = pointerReadyState.ReadyAssetCount,
            CompletedSteps = pointerReadyState.CompletedSteps,
            TotalSteps = pointerReadyState.TotalSteps,
            Exists = pointerReadyState.Exists,
            ReadSucceeded = pointerReadyState.ReadSucceeded,
            BrowserPointerReadyState = pointerReadyState
        };

        if (!pointerReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser command session blocked for profile '{pointerReadyState.ProfileId}'.";
            result.Error = pointerReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCommandSessionVersion = "runtime-browser-command-session-v1";
        result.BrowserCommandStages =
        [
            "open-browser-command-session",
            "bind-browser-pointer-ready-state",
            "publish-browser-command-ready"
        ];
        result.BrowserCommandSummary = $"Runtime browser command session prepared {result.BrowserCommandStages.Length} command stage(s) for profile '{pointerReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser command session ready for profile '{pointerReadyState.ProfileId}' with {result.BrowserCommandStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCommandSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserCommandSessionVersion { get; set; } = string.Empty;
    public string BrowserPointerReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPointerSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCommandStages { get; set; } = Array.Empty<string>();
    public string BrowserCommandSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserPointerReadyStateResult BrowserPointerReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
