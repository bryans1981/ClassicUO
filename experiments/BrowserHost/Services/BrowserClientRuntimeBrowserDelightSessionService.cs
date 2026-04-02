namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDelightSession
{
    ValueTask<BrowserClientRuntimeBrowserDelightSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDelightSessionService : IBrowserClientRuntimeBrowserDelightSession
{
    private readonly IBrowserClientRuntimeBrowserSatisfactionReadyState _runtimeBrowserSatisfactionReadyState;

    public BrowserClientRuntimeBrowserDelightSessionService(IBrowserClientRuntimeBrowserSatisfactionReadyState runtimeBrowserSatisfactionReadyState)
    {
        _runtimeBrowserSatisfactionReadyState = runtimeBrowserSatisfactionReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDelightSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSatisfactionReadyStateResult satisfactionReadyState = await _runtimeBrowserSatisfactionReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserDelightSessionResult result = new()
        {
            ProfileId = satisfactionReadyState.ProfileId,
            SessionId = satisfactionReadyState.SessionId,
            SessionPath = satisfactionReadyState.SessionPath,
            BrowserSatisfactionReadyStateVersion = satisfactionReadyState.BrowserSatisfactionReadyStateVersion,
            BrowserSatisfactionSessionVersion = satisfactionReadyState.BrowserSatisfactionSessionVersion,
            LaunchMode = satisfactionReadyState.LaunchMode,
            AssetRootPath = satisfactionReadyState.AssetRootPath,
            ProfilesRootPath = satisfactionReadyState.ProfilesRootPath,
            CacheRootPath = satisfactionReadyState.CacheRootPath,
            ConfigRootPath = satisfactionReadyState.ConfigRootPath,
            SettingsFilePath = satisfactionReadyState.SettingsFilePath,
            StartupProfilePath = satisfactionReadyState.StartupProfilePath,
            RequiredAssets = satisfactionReadyState.RequiredAssets,
            ReadyAssetCount = satisfactionReadyState.ReadyAssetCount,
            CompletedSteps = satisfactionReadyState.CompletedSteps,
            TotalSteps = satisfactionReadyState.TotalSteps,
            Exists = satisfactionReadyState.Exists,
            ReadSucceeded = satisfactionReadyState.ReadSucceeded
        };

        if (!satisfactionReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser delight session blocked for profile '{satisfactionReadyState.ProfileId}'.";
            result.Error = satisfactionReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDelightSessionVersion = "runtime-browser-delight-session-v1";
        result.BrowserDelightStages =
        [
            "open-browser-delight-session",
            "bind-browser-satisfaction-ready-state",
            "publish-browser-delight-ready"
        ];
        result.BrowserDelightSummary = $"Runtime browser delight session prepared {result.BrowserDelightStages.Length} delight stage(s) for profile '{satisfactionReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser delight session ready for profile '{satisfactionReadyState.ProfileId}' with {result.BrowserDelightStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDelightSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserDelightSessionVersion { get; set; } = string.Empty;
    public string BrowserSatisfactionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSatisfactionSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDelightStages { get; set; } = Array.Empty<string>();
    public string BrowserDelightSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
