namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAdaptabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserAdaptabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAdaptabilitySessionService : IBrowserClientRuntimeBrowserAdaptabilitySession
{
    private readonly IBrowserClientRuntimeBrowserInclusivityReadyState _runtimeBrowserInclusivityReadyState;

    public BrowserClientRuntimeBrowserAdaptabilitySessionService(IBrowserClientRuntimeBrowserInclusivityReadyState runtimeBrowserInclusivityReadyState)
    {
        _runtimeBrowserInclusivityReadyState = runtimeBrowserInclusivityReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAdaptabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserInclusivityReadyStateResult inclusivityReadyState = await _runtimeBrowserInclusivityReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAdaptabilitySessionResult result = new()
        {
            ProfileId = inclusivityReadyState.ProfileId,
            SessionId = inclusivityReadyState.SessionId,
            SessionPath = inclusivityReadyState.SessionPath,
            BrowserInclusivityReadyStateVersion = inclusivityReadyState.BrowserInclusivityReadyStateVersion,
            BrowserInclusivitySessionVersion = inclusivityReadyState.BrowserInclusivitySessionVersion,
            LaunchMode = inclusivityReadyState.LaunchMode,
            AssetRootPath = inclusivityReadyState.AssetRootPath,
            ProfilesRootPath = inclusivityReadyState.ProfilesRootPath,
            CacheRootPath = inclusivityReadyState.CacheRootPath,
            ConfigRootPath = inclusivityReadyState.ConfigRootPath,
            SettingsFilePath = inclusivityReadyState.SettingsFilePath,
            StartupProfilePath = inclusivityReadyState.StartupProfilePath,
            RequiredAssets = inclusivityReadyState.RequiredAssets,
            ReadyAssetCount = inclusivityReadyState.ReadyAssetCount,
            CompletedSteps = inclusivityReadyState.CompletedSteps,
            TotalSteps = inclusivityReadyState.TotalSteps,
            Exists = inclusivityReadyState.Exists,
            ReadSucceeded = inclusivityReadyState.ReadSucceeded
        };

        if (!inclusivityReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser adaptability session blocked for profile '{inclusivityReadyState.ProfileId}'.";
            result.Error = inclusivityReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAdaptabilitySessionVersion = "runtime-browser-adaptability-session-v1";
        result.BrowserAdaptabilityStages =
        [
            "open-browser-adaptability-session",
            "bind-browser-inclusivity-ready-state",
            "publish-browser-adaptability-ready"
        ];
        result.BrowserAdaptabilitySummary = $"Runtime browser adaptability session prepared {result.BrowserAdaptabilityStages.Length} adaptability stage(s) for profile '{inclusivityReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser adaptability session ready for profile '{inclusivityReadyState.ProfileId}' with {result.BrowserAdaptabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAdaptabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAdaptabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserInclusivityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInclusivitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAdaptabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserAdaptabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
