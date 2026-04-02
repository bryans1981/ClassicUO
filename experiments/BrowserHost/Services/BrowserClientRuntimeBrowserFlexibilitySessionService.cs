namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserFlexibilitySession
{
    ValueTask<BrowserClientRuntimeBrowserFlexibilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserFlexibilitySessionService : IBrowserClientRuntimeBrowserFlexibilitySession
{
    private readonly IBrowserClientRuntimeBrowserCustomizabilityReadyState _runtimeBrowserCustomizabilityReadyState;

    public BrowserClientRuntimeBrowserFlexibilitySessionService(IBrowserClientRuntimeBrowserCustomizabilityReadyState runtimeBrowserCustomizabilityReadyState)
    {
        _runtimeBrowserCustomizabilityReadyState = runtimeBrowserCustomizabilityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserFlexibilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCustomizabilityReadyStateResult customizabilityReadyState = await _runtimeBrowserCustomizabilityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserFlexibilitySessionResult result = new()
        {
            ProfileId = customizabilityReadyState.ProfileId,
            SessionId = customizabilityReadyState.SessionId,
            SessionPath = customizabilityReadyState.SessionPath,
            BrowserCustomizabilityReadyStateVersion = customizabilityReadyState.BrowserCustomizabilityReadyStateVersion,
            BrowserCustomizabilitySessionVersion = customizabilityReadyState.BrowserCustomizabilitySessionVersion,
            LaunchMode = customizabilityReadyState.LaunchMode,
            AssetRootPath = customizabilityReadyState.AssetRootPath,
            ProfilesRootPath = customizabilityReadyState.ProfilesRootPath,
            CacheRootPath = customizabilityReadyState.CacheRootPath,
            ConfigRootPath = customizabilityReadyState.ConfigRootPath,
            SettingsFilePath = customizabilityReadyState.SettingsFilePath,
            StartupProfilePath = customizabilityReadyState.StartupProfilePath,
            RequiredAssets = customizabilityReadyState.RequiredAssets,
            ReadyAssetCount = customizabilityReadyState.ReadyAssetCount,
            CompletedSteps = customizabilityReadyState.CompletedSteps,
            TotalSteps = customizabilityReadyState.TotalSteps,
            Exists = customizabilityReadyState.Exists,
            ReadSucceeded = customizabilityReadyState.ReadSucceeded
        };

        if (!customizabilityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser flexibility session blocked for profile '{customizabilityReadyState.ProfileId}'.";
            result.Error = customizabilityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserFlexibilitySessionVersion = "runtime-browser-flexibility-session-v1";
        result.BrowserFlexibilityStages =
        [
            "open-browser-flexibility-session",
            "bind-browser-customizability-ready-state",
            "publish-browser-flexibility-ready"
        ];
        result.BrowserFlexibilitySummary = $"Runtime browser flexibility session prepared {result.BrowserFlexibilityStages.Length} flexibility stage(s) for profile '{customizabilityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser flexibility session ready for profile '{customizabilityReadyState.ProfileId}' with {result.BrowserFlexibilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserFlexibilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserFlexibilitySessionVersion { get; set; } = string.Empty;
    public string BrowserCustomizabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCustomizabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserFlexibilityStages { get; set; } = Array.Empty<string>();
    public string BrowserFlexibilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
