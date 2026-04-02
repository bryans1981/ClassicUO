namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserInclusivitySession
{
    ValueTask<BrowserClientRuntimeBrowserInclusivitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserInclusivitySessionService : IBrowserClientRuntimeBrowserInclusivitySession
{
    private readonly IBrowserClientRuntimeBrowserAccessibilityReadyState _runtimeBrowserAccessibilityReadyState;

    public BrowserClientRuntimeBrowserInclusivitySessionService(IBrowserClientRuntimeBrowserAccessibilityReadyState runtimeBrowserAccessibilityReadyState)
    {
        _runtimeBrowserAccessibilityReadyState = runtimeBrowserAccessibilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserInclusivitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAccessibilityReadyStateResult accessibilityReadyState = await _runtimeBrowserAccessibilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserInclusivitySessionResult result = new()
        {
            ProfileId = accessibilityReadyState.ProfileId,
            SessionId = accessibilityReadyState.SessionId,
            SessionPath = accessibilityReadyState.SessionPath,
            BrowserAccessibilityReadyStateVersion = accessibilityReadyState.BrowserAccessibilityReadyStateVersion,
            BrowserAccessibilitySessionVersion = accessibilityReadyState.BrowserAccessibilitySessionVersion,
            LaunchMode = accessibilityReadyState.LaunchMode,
            AssetRootPath = accessibilityReadyState.AssetRootPath,
            ProfilesRootPath = accessibilityReadyState.ProfilesRootPath,
            CacheRootPath = accessibilityReadyState.CacheRootPath,
            ConfigRootPath = accessibilityReadyState.ConfigRootPath,
            SettingsFilePath = accessibilityReadyState.SettingsFilePath,
            StartupProfilePath = accessibilityReadyState.StartupProfilePath,
            RequiredAssets = accessibilityReadyState.RequiredAssets,
            ReadyAssetCount = accessibilityReadyState.ReadyAssetCount,
            CompletedSteps = accessibilityReadyState.CompletedSteps,
            TotalSteps = accessibilityReadyState.TotalSteps,
            Exists = accessibilityReadyState.Exists,
            ReadSucceeded = accessibilityReadyState.ReadSucceeded
        };

        if (!accessibilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser inclusivity session blocked for profile '{accessibilityReadyState.ProfileId}'.";
            result.Error = accessibilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserInclusivitySessionVersion = "runtime-browser-inclusivity-session-v1";
        result.BrowserInclusivityStages =
        [
            "open-browser-inclusivity-session",
            "bind-browser-accessibility-ready-state",
            "publish-browser-inclusivity-ready"
        ];
        result.BrowserInclusivitySummary = $"Runtime browser inclusivity session prepared {result.BrowserInclusivityStages.Length} inclusivity stage(s) for profile '{accessibilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser inclusivity session ready for profile '{accessibilityReadyState.ProfileId}' with {result.BrowserInclusivityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserInclusivitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserInclusivitySessionVersion { get; set; } = string.Empty;
    public string BrowserAccessibilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAccessibilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserInclusivityStages { get; set; } = Array.Empty<string>();
    public string BrowserInclusivitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
