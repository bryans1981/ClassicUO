namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserControllabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserControllabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserControllabilitySessionService : IBrowserClientRuntimeBrowserControllabilitySession
{
    private readonly IBrowserClientRuntimeBrowserEffectivenessReadyState _runtimeBrowserEffectivenessReadyState;

    public BrowserClientRuntimeBrowserControllabilitySessionService(IBrowserClientRuntimeBrowserEffectivenessReadyState runtimeBrowserEffectivenessReadyState)
    {
        _runtimeBrowserEffectivenessReadyState = runtimeBrowserEffectivenessReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserControllabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserEffectivenessReadyStateResult effectivenessReadyState = await _runtimeBrowserEffectivenessReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserControllabilitySessionResult result = new()
        {
            ProfileId = effectivenessReadyState.ProfileId,
            SessionId = effectivenessReadyState.SessionId,
            SessionPath = effectivenessReadyState.SessionPath,
            BrowserEffectivenessReadyStateVersion = effectivenessReadyState.BrowserEffectivenessReadyStateVersion,
            BrowserEffectivenessSessionVersion = effectivenessReadyState.BrowserEffectivenessSessionVersion,
            LaunchMode = effectivenessReadyState.LaunchMode,
            AssetRootPath = effectivenessReadyState.AssetRootPath,
            ProfilesRootPath = effectivenessReadyState.ProfilesRootPath,
            CacheRootPath = effectivenessReadyState.CacheRootPath,
            ConfigRootPath = effectivenessReadyState.ConfigRootPath,
            SettingsFilePath = effectivenessReadyState.SettingsFilePath,
            StartupProfilePath = effectivenessReadyState.StartupProfilePath,
            RequiredAssets = effectivenessReadyState.RequiredAssets,
            ReadyAssetCount = effectivenessReadyState.ReadyAssetCount,
            CompletedSteps = effectivenessReadyState.CompletedSteps,
            TotalSteps = effectivenessReadyState.TotalSteps,
            Exists = effectivenessReadyState.Exists,
            ReadSucceeded = effectivenessReadyState.ReadSucceeded
        };

        if (!effectivenessReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser controllability session blocked for profile '{effectivenessReadyState.ProfileId}'.";
            result.Error = effectivenessReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserControllabilitySessionVersion = "runtime-browser-controllability-session-v1";
        result.BrowserControllabilityStages =
        [
            "open-browser-controllability-session",
            "bind-browser-effectiveness-ready-state",
            "publish-browser-controllability-ready"
        ];
        result.BrowserControllabilitySummary = $"Runtime browser controllability session prepared {result.BrowserControllabilityStages.Length} controllability stage(s) for profile '{effectivenessReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser controllability session ready for profile '{effectivenessReadyState.ProfileId}' with {result.BrowserControllabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserControllabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserControllabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserEffectivenessReadyStateVersion { get; set; } = string.Empty;
    public string BrowserEffectivenessSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserControllabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserControllabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
