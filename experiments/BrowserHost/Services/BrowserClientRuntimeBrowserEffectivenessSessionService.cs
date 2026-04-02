namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserEffectivenessSession
{
    ValueTask<BrowserClientRuntimeBrowserEffectivenessSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserEffectivenessSessionService : IBrowserClientRuntimeBrowserEffectivenessSession
{
    private readonly IBrowserClientRuntimeBrowserEfficiencyReadyState _runtimeBrowserEfficiencyReadyState;

    public BrowserClientRuntimeBrowserEffectivenessSessionService(IBrowserClientRuntimeBrowserEfficiencyReadyState runtimeBrowserEfficiencyReadyState)
    {
        _runtimeBrowserEfficiencyReadyState = runtimeBrowserEfficiencyReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserEffectivenessSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserEfficiencyReadyStateResult efficiencyReadyState = await _runtimeBrowserEfficiencyReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserEffectivenessSessionResult result = new()
        {
            ProfileId = efficiencyReadyState.ProfileId,
            SessionId = efficiencyReadyState.SessionId,
            SessionPath = efficiencyReadyState.SessionPath,
            BrowserEfficiencyReadyStateVersion = efficiencyReadyState.BrowserEfficiencyReadyStateVersion,
            BrowserEfficiencySessionVersion = efficiencyReadyState.BrowserEfficiencySessionVersion,
            LaunchMode = efficiencyReadyState.LaunchMode,
            AssetRootPath = efficiencyReadyState.AssetRootPath,
            ProfilesRootPath = efficiencyReadyState.ProfilesRootPath,
            CacheRootPath = efficiencyReadyState.CacheRootPath,
            ConfigRootPath = efficiencyReadyState.ConfigRootPath,
            SettingsFilePath = efficiencyReadyState.SettingsFilePath,
            StartupProfilePath = efficiencyReadyState.StartupProfilePath,
            RequiredAssets = efficiencyReadyState.RequiredAssets,
            ReadyAssetCount = efficiencyReadyState.ReadyAssetCount,
            CompletedSteps = efficiencyReadyState.CompletedSteps,
            TotalSteps = efficiencyReadyState.TotalSteps,
            Exists = efficiencyReadyState.Exists,
            ReadSucceeded = efficiencyReadyState.ReadSucceeded
        };

        if (!efficiencyReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser effectiveness session blocked for profile '{efficiencyReadyState.ProfileId}'.";
            result.Error = efficiencyReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserEffectivenessSessionVersion = "runtime-browser-effectiveness-session-v1";
        result.BrowserEffectivenessStages =
        [
            "open-browser-effectiveness-session",
            "bind-browser-efficiency-ready-state",
            "publish-browser-effectiveness-ready"
        ];
        result.BrowserEffectivenessSummary = $"Runtime browser effectiveness session prepared {result.BrowserEffectivenessStages.Length} effectiveness stage(s) for profile '{efficiencyReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser effectiveness session ready for profile '{efficiencyReadyState.ProfileId}' with {result.BrowserEffectivenessStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserEffectivenessSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserEffectivenessSessionVersion { get; set; } = string.Empty;
    public string BrowserEfficiencyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserEfficiencySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserEffectivenessStages { get; set; } = Array.Empty<string>();
    public string BrowserEffectivenessSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
