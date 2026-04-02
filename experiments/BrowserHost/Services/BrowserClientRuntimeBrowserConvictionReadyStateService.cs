namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserConvictionReadyState
{
    ValueTask<BrowserClientRuntimeBrowserConvictionReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserConvictionReadyStateService : IBrowserClientRuntimeBrowserConvictionReadyState
{
    private readonly IBrowserClientRuntimeBrowserConvictionSession _runtimeBrowserConvictionSession;

    public BrowserClientRuntimeBrowserConvictionReadyStateService(IBrowserClientRuntimeBrowserConvictionSession runtimeBrowserConvictionSession)
    {
        _runtimeBrowserConvictionSession = runtimeBrowserConvictionSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserConvictionReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserConvictionSessionResult convictionSession = await _runtimeBrowserConvictionSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserConvictionReadyStateResult result = new()
        {
            ProfileId = convictionSession.ProfileId,
            SessionId = convictionSession.SessionId,
            SessionPath = convictionSession.SessionPath,
            BrowserConvictionSessionVersion = convictionSession.BrowserConvictionSessionVersion,
            BrowserAssurabilityReadyStateVersion = convictionSession.BrowserAssurabilityReadyStateVersion,
            BrowserAssurabilitySessionVersion = convictionSession.BrowserAssurabilitySessionVersion,
            LaunchMode = convictionSession.LaunchMode,
            AssetRootPath = convictionSession.AssetRootPath,
            ProfilesRootPath = convictionSession.ProfilesRootPath,
            CacheRootPath = convictionSession.CacheRootPath,
            ConfigRootPath = convictionSession.ConfigRootPath,
            SettingsFilePath = convictionSession.SettingsFilePath,
            StartupProfilePath = convictionSession.StartupProfilePath,
            RequiredAssets = convictionSession.RequiredAssets,
            ReadyAssetCount = convictionSession.ReadyAssetCount,
            CompletedSteps = convictionSession.CompletedSteps,
            TotalSteps = convictionSession.TotalSteps,
            Exists = convictionSession.Exists,
            ReadSucceeded = convictionSession.ReadSucceeded
        };

        if (!convictionSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser conviction ready state blocked for profile '{convictionSession.ProfileId}'.";
            result.Error = convictionSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserConvictionReadyStateVersion = "runtime-browser-conviction-ready-state-v1";
        result.BrowserConvictionReadyChecks =
        [
            "browser-assurability-ready-state-ready",
            "browser-conviction-session-ready",
            "browser-conviction-ready"
        ];
        result.BrowserConvictionReadySummary = $"Runtime browser conviction ready state passed {result.BrowserConvictionReadyChecks.Length} conviction readiness check(s) for profile '{convictionSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser conviction ready state ready for profile '{convictionSession.ProfileId}' with {result.BrowserConvictionReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserConvictionReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserConvictionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserConvictionSessionVersion { get; set; } = string.Empty;
    public string BrowserAssurabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAssurabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserConvictionReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserConvictionReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
