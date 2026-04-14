namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSteadyOperationSession
{
    ValueTask<BrowserClientRuntimeBrowserSteadyOperationSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSteadyOperationSessionService : IBrowserClientRuntimeBrowserSteadyOperationSession
{
    private readonly IBrowserClientRuntimeBrowserServiceActivationReadyState _runtimeBrowserServiceActivationReadyState;

    public BrowserClientRuntimeBrowserSteadyOperationSessionService(IBrowserClientRuntimeBrowserServiceActivationReadyState runtimeBrowserServiceActivationReadyState)
    {
        _runtimeBrowserServiceActivationReadyState = runtimeBrowserServiceActivationReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSteadyOperationSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserServiceActivationReadyStateResult serviceactivationReadyState = await _runtimeBrowserServiceActivationReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserSteadyOperationSessionResult result = new()
        {
            ProfileId = serviceactivationReadyState.ProfileId,
            SessionId = serviceactivationReadyState.SessionId,
            SessionPath = serviceactivationReadyState.SessionPath,
            BrowserServiceActivationReadyStateVersion = serviceactivationReadyState.BrowserServiceActivationReadyStateVersion,
            BrowserServiceActivationSessionVersion = serviceactivationReadyState.BrowserServiceActivationSessionVersion,
            LaunchMode = serviceactivationReadyState.LaunchMode,
            AssetRootPath = serviceactivationReadyState.AssetRootPath,
            ProfilesRootPath = serviceactivationReadyState.ProfilesRootPath,
            CacheRootPath = serviceactivationReadyState.CacheRootPath,
            ConfigRootPath = serviceactivationReadyState.ConfigRootPath,
            SettingsFilePath = serviceactivationReadyState.SettingsFilePath,
            StartupProfilePath = serviceactivationReadyState.StartupProfilePath,
            RequiredAssets = serviceactivationReadyState.RequiredAssets,
            ReadyAssetCount = serviceactivationReadyState.ReadyAssetCount,
            CompletedSteps = serviceactivationReadyState.CompletedSteps,
            TotalSteps = serviceactivationReadyState.TotalSteps,
            Exists = serviceactivationReadyState.Exists,
            ReadSucceeded = serviceactivationReadyState.ReadSucceeded
        };

        if (!serviceactivationReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser steadyoperation session blocked for profile '{serviceactivationReadyState.ProfileId}'.";
            result.Error = serviceactivationReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSteadyOperationSessionVersion = "runtime-browser-steadyoperation-session-v1";
        result.BrowserSteadyOperationStages =
        [
            "open-browser-steadyoperation-session",
            "bind-browser-serviceactivation-ready-state",
            "publish-browser-steadyoperation-ready"
        ];
        result.BrowserSteadyOperationSummary = $"Runtime browser steadyoperation session prepared {result.BrowserSteadyOperationStages.Length} steadyoperation stage(s) for profile '{serviceactivationReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser steadyoperation session ready for profile '{serviceactivationReadyState.ProfileId}' with {result.BrowserSteadyOperationStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSteadyOperationSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserSteadyOperationSessionVersion { get; set; } = string.Empty;
    public string BrowserServiceActivationReadyStateVersion { get; set; } = string.Empty;
    public string BrowserServiceActivationSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSteadyOperationStages { get; set; } = Array.Empty<string>();
    public string BrowserSteadyOperationSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
