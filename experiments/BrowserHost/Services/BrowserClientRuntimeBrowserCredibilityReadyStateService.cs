namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCredibilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserCredibilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCredibilityReadyStateService : IBrowserClientRuntimeBrowserCredibilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserCredibilitySession _runtimeBrowserCredibilitySession;

    public BrowserClientRuntimeBrowserCredibilityReadyStateService(IBrowserClientRuntimeBrowserCredibilitySession runtimeBrowserCredibilitySession)
    {
        _runtimeBrowserCredibilitySession = runtimeBrowserCredibilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCredibilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCredibilitySessionResult credibilitySession = await _runtimeBrowserCredibilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserCredibilityReadyStateResult result = new()
        {
            ProfileId = credibilitySession.ProfileId,
            SessionId = credibilitySession.SessionId,
            SessionPath = credibilitySession.SessionPath,
            BrowserCredibilitySessionVersion = credibilitySession.BrowserCredibilitySessionVersion,
            BrowserValueReadyStateVersion = credibilitySession.BrowserValueReadyStateVersion,
            BrowserValueSessionVersion = credibilitySession.BrowserValueSessionVersion,
            LaunchMode = credibilitySession.LaunchMode,
            AssetRootPath = credibilitySession.AssetRootPath,
            ProfilesRootPath = credibilitySession.ProfilesRootPath,
            CacheRootPath = credibilitySession.CacheRootPath,
            ConfigRootPath = credibilitySession.ConfigRootPath,
            SettingsFilePath = credibilitySession.SettingsFilePath,
            StartupProfilePath = credibilitySession.StartupProfilePath,
            RequiredAssets = credibilitySession.RequiredAssets,
            ReadyAssetCount = credibilitySession.ReadyAssetCount,
            CompletedSteps = credibilitySession.CompletedSteps,
            TotalSteps = credibilitySession.TotalSteps,
            Exists = credibilitySession.Exists,
            ReadSucceeded = credibilitySession.ReadSucceeded
        };

        if (!credibilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser credibility ready state blocked for profile '{credibilitySession.ProfileId}'.";
            result.Error = credibilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCredibilityReadyStateVersion = "runtime-browser-credibility-ready-state-v1";
        result.BrowserCredibilityReadyChecks =
        [
            "browser-value-ready-state-ready",
            "browser-credibility-session-ready",
            "browser-credibility-ready"
        ];
        result.BrowserCredibilityReadySummary = $"Runtime browser credibility ready state passed {result.BrowserCredibilityReadyChecks.Length} credibility readiness check(s) for profile '{credibilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser credibility ready state ready for profile '{credibilitySession.ProfileId}' with {result.BrowserCredibilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCredibilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserCredibilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCredibilitySessionVersion { get; set; } = string.Empty;
    public string BrowserValueReadyStateVersion { get; set; } = string.Empty;
    public string BrowserValueSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCredibilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserCredibilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
