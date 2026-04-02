namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserUnderstandabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserUnderstandabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserUnderstandabilityReadyStateService : IBrowserClientRuntimeBrowserUnderstandabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserUnderstandabilitySession _runtimeBrowserUnderstandabilitySession;

    public BrowserClientRuntimeBrowserUnderstandabilityReadyStateService(IBrowserClientRuntimeBrowserUnderstandabilitySession runtimeBrowserUnderstandabilitySession)
    {
        _runtimeBrowserUnderstandabilitySession = runtimeBrowserUnderstandabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserUnderstandabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserUnderstandabilitySessionResult understandabilitySession = await _runtimeBrowserUnderstandabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserUnderstandabilityReadyStateResult result = new()
        {
            ProfileId = understandabilitySession.ProfileId,
            SessionId = understandabilitySession.SessionId,
            SessionPath = understandabilitySession.SessionPath,
            BrowserUnderstandabilitySessionVersion = understandabilitySession.BrowserUnderstandabilitySessionVersion,
            BrowserSimplicityReadyStateVersion = understandabilitySession.BrowserSimplicityReadyStateVersion,
            BrowserSimplicitySessionVersion = understandabilitySession.BrowserSimplicitySessionVersion,
            LaunchMode = understandabilitySession.LaunchMode,
            AssetRootPath = understandabilitySession.AssetRootPath,
            ProfilesRootPath = understandabilitySession.ProfilesRootPath,
            CacheRootPath = understandabilitySession.CacheRootPath,
            ConfigRootPath = understandabilitySession.ConfigRootPath,
            SettingsFilePath = understandabilitySession.SettingsFilePath,
            StartupProfilePath = understandabilitySession.StartupProfilePath,
            RequiredAssets = understandabilitySession.RequiredAssets,
            ReadyAssetCount = understandabilitySession.ReadyAssetCount,
            CompletedSteps = understandabilitySession.CompletedSteps,
            TotalSteps = understandabilitySession.TotalSteps,
            Exists = understandabilitySession.Exists,
            ReadSucceeded = understandabilitySession.ReadSucceeded
        };

        if (!understandabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser understandability ready state blocked for profile '{understandabilitySession.ProfileId}'.";
            result.Error = understandabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserUnderstandabilityReadyStateVersion = "runtime-browser-understandability-ready-state-v1";
        result.BrowserUnderstandabilityReadyChecks =
        [
            "browser-simplicity-ready-state-ready",
            "browser-understandability-session-ready",
            "browser-understandability-ready"
        ];
        result.BrowserUnderstandabilityReadySummary = $"Runtime browser understandability ready state passed {result.BrowserUnderstandabilityReadyChecks.Length} understandability readiness check(s) for profile '{understandabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser understandability ready state ready for profile '{understandabilitySession.ProfileId}' with {result.BrowserUnderstandabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserUnderstandabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserUnderstandabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserUnderstandabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSimplicityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSimplicitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserUnderstandabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserUnderstandabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

