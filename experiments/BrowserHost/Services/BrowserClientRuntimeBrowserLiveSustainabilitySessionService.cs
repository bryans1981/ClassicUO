namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserLiveSustainabilitySession
{
    ValueTask<BrowserClientRuntimeBrowserLiveSustainabilitySessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserLiveSustainabilitySessionService : IBrowserClientRuntimeBrowserLiveSustainabilitySession
{
    private readonly IBrowserClientRuntimeBrowserSustainmentAssuranceReadyState _runtimeBrowserSustainmentAssuranceReadyState;

    public BrowserClientRuntimeBrowserLiveSustainabilitySessionService(IBrowserClientRuntimeBrowserSustainmentAssuranceReadyState runtimeBrowserSustainmentAssuranceReadyState)
    {
        _runtimeBrowserSustainmentAssuranceReadyState = runtimeBrowserSustainmentAssuranceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserLiveSustainabilitySessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserSustainmentAssuranceReadyStateResult prevReadyState = await _runtimeBrowserSustainmentAssuranceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserLiveSustainabilitySessionResult result = new()
        {
            ProfileId = prevReadyState.ProfileId,
            SessionId = prevReadyState.SessionId,
            SessionPath = prevReadyState.SessionPath,
            BrowserSustainmentAssuranceReadyStateVersion = prevReadyState.BrowserSustainmentAssuranceReadyStateVersion,
            BrowserSustainmentAssuranceSessionVersion = prevReadyState.BrowserSustainmentAssuranceSessionVersion,
            LaunchMode = prevReadyState.LaunchMode,
            AssetRootPath = prevReadyState.AssetRootPath,
            ProfilesRootPath = prevReadyState.ProfilesRootPath,
            CacheRootPath = prevReadyState.CacheRootPath,
            ConfigRootPath = prevReadyState.ConfigRootPath,
            SettingsFilePath = prevReadyState.SettingsFilePath,
            StartupProfilePath = prevReadyState.StartupProfilePath,
            RequiredAssets = prevReadyState.RequiredAssets,
            ReadyAssetCount = prevReadyState.ReadyAssetCount,
            CompletedSteps = prevReadyState.CompletedSteps,
            TotalSteps = prevReadyState.TotalSteps,
            Exists = prevReadyState.Exists,
            ReadSucceeded = prevReadyState.ReadSucceeded
        };

        if (!prevReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser livesustainability session blocked for profile '{prevReadyState.ProfileId}'.";
            result.Error = prevReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserLiveSustainabilitySessionVersion = "runtime-browser-livesustainability-session-v1";
        result.BrowserLiveSustainabilityStages =
        [
            "open-browser-livesustainability-session",
            "bind-browser-sustainmentassurance-ready-state",
            "publish-browser-livesustainability-ready"
        ];
        result.BrowserLiveSustainabilitySummary = $"Runtime browser livesustainability session prepared {result.BrowserLiveSustainabilityStages.Length} livesustainability stage(s) for profile '{prevReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser livesustainability session ready for profile '{prevReadyState.ProfileId}' with {result.BrowserLiveSustainabilityStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserLiveSustainabilitySessionResult
{
    public bool IsReady { get; set; }
    public string BrowserLiveSustainabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserSustainmentAssuranceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSustainmentAssuranceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserLiveSustainabilityStages { get; set; } = Array.Empty<string>();
    public string BrowserLiveSustainabilitySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
