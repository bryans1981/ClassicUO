namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCustomizabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserCustomizabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCustomizabilitySessionService : IBrowserClientRuntimeBrowserCustomizabilitySession
{
    private readonly IBrowserClientRuntimeBrowserControllabilityReadyState _runtimeBrowserControllabilityReadyState;

    public BrowserClientRuntimeBrowserCustomizabilitySessionService(IBrowserClientRuntimeBrowserControllabilityReadyState runtimeBrowserControllabilityReadyState)
    {
        _runtimeBrowserControllabilityReadyState = runtimeBrowserControllabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCustomizabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserControllabilityReadyStateResult controllabilityReadyState = await _runtimeBrowserControllabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserCustomizabilitySessionResult result = new()
        {
            ProfileId = controllabilityReadyState.ProfileId,
            SessionId = controllabilityReadyState.SessionId,
            SessionPath = controllabilityReadyState.SessionPath,
            BrowserControllabilityReadyStateVersion = controllabilityReadyState.BrowserControllabilityReadyStateVersion,
            BrowserControllabilitySessionVersion = controllabilityReadyState.BrowserControllabilitySessionVersion,
            LaunchMode = controllabilityReadyState.LaunchMode,
            AssetRootPath = controllabilityReadyState.AssetRootPath,
            ProfilesRootPath = controllabilityReadyState.ProfilesRootPath,
            CacheRootPath = controllabilityReadyState.CacheRootPath,
            ConfigRootPath = controllabilityReadyState.ConfigRootPath,
            SettingsFilePath = controllabilityReadyState.SettingsFilePath,
            StartupProfilePath = controllabilityReadyState.StartupProfilePath,
            RequiredAssets = controllabilityReadyState.RequiredAssets,
            ReadyAssetCount = controllabilityReadyState.ReadyAssetCount,
            CompletedSteps = controllabilityReadyState.CompletedSteps,
            TotalSteps = controllabilityReadyState.TotalSteps,
            Exists = controllabilityReadyState.Exists,
            ReadSucceeded = controllabilityReadyState.ReadSucceeded
        };

        if (!controllabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser customizability session blocked for profile '{controllabilityReadyState.ProfileId}'.";
            result.Error = controllabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCustomizabilitySessionVersion = "runtime-browser-customizability-session-v1";
        result.BrowserCustomizabilityStages =
        [
            "open-browser-customizability-session",
            "bind-browser-controllability-ready-state",
            "publish-browser-customizability-ready"
        ];
        result.BrowserCustomizabilitySummary = $"Runtime browser customizability session prepared {result.BrowserCustomizabilityStages.Length} customizability stage(s) for profile '{controllabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser customizability session ready for profile '{controllabilityReadyState.ProfileId}' with {result.BrowserCustomizabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCustomizabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserCustomizabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserControllabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserControllabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCustomizabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserCustomizabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
