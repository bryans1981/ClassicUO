namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserSustainabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserSustainabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserSustainabilityReadyStateService : IBrowserClientRuntimeBrowserSustainabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserSustainabilitySession _runtimeBrowserSustainabilitySession;

    public BrowserClientRuntimeBrowserSustainabilityReadyStateService(IBrowserClientRuntimeBrowserSustainabilitySession runtimeBrowserSustainabilitySession)
    {
        _runtimeBrowserSustainabilitySession = runtimeBrowserSustainabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserSustainabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSustainabilitySessionResult sustainabilitySession = await _runtimeBrowserSustainabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserSustainabilityReadyStateResult result = new()
        {
            ProfileId = sustainabilitySession.ProfileId,
            SessionId = sustainabilitySession.SessionId,
            SessionPath = sustainabilitySession.SessionPath,
            BrowserSustainabilitySessionVersion = sustainabilitySession.BrowserSustainabilitySessionVersion,
            BrowserDurabilityReadyStateVersion = sustainabilitySession.BrowserDurabilityReadyStateVersion,
            BrowserDurabilitySessionVersion = sustainabilitySession.BrowserDurabilitySessionVersion,
            LaunchMode = sustainabilitySession.LaunchMode,
            AssetRootPath = sustainabilitySession.AssetRootPath,
            ProfilesRootPath = sustainabilitySession.ProfilesRootPath,
            CacheRootPath = sustainabilitySession.CacheRootPath,
            ConfigRootPath = sustainabilitySession.ConfigRootPath,
            SettingsFilePath = sustainabilitySession.SettingsFilePath,
            StartupProfilePath = sustainabilitySession.StartupProfilePath,
            RequiredAssets = sustainabilitySession.RequiredAssets,
            ReadyAssetCount = sustainabilitySession.ReadyAssetCount,
            CompletedSteps = sustainabilitySession.CompletedSteps,
            TotalSteps = sustainabilitySession.TotalSteps,
            Exists = sustainabilitySession.Exists,
            ReadSucceeded = sustainabilitySession.ReadSucceeded
        };

        if (!sustainabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser sustainability ready state blocked for profile '{sustainabilitySession.ProfileId}'.";
            result.Error = sustainabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserSustainabilityReadyStateVersion = "runtime-browser-sustainability-ready-state-v1";
        result.BrowserSustainabilityReadyChecks =
        [
            "browser-durability-ready-state-ready",
            "browser-sustainability-session-ready",
            "browser-sustainability-ready"
        ];
        result.BrowserSustainabilityReadySummary = $"Runtime browser sustainability ready state passed {result.BrowserSustainabilityReadyChecks.Length} sustainability readiness check(s) for profile '{sustainabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser sustainability ready state ready for profile '{sustainabilitySession.ProfileId}' with {result.BrowserSustainabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserSustainabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserSustainabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSustainabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserDurabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDurabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserSustainabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserSustainabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
