namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCommitmentSession
{
    ValueTask<BrowserClientRuntimeBrowserCommitmentSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCommitmentSessionService : IBrowserClientRuntimeBrowserCommitmentSession
{
    private readonly IBrowserClientRuntimeBrowserAdoptionReadyState _runtimeBrowserAdoptionReadyState;

    public BrowserClientRuntimeBrowserCommitmentSessionService(IBrowserClientRuntimeBrowserAdoptionReadyState runtimeBrowserAdoptionReadyState)
    {
        _runtimeBrowserAdoptionReadyState = runtimeBrowserAdoptionReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCommitmentSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAdoptionReadyStateResult adoptionReadyState = await _runtimeBrowserAdoptionReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserCommitmentSessionResult result = new()
        {
            ProfileId = adoptionReadyState.ProfileId,
            SessionId = adoptionReadyState.SessionId,
            SessionPath = adoptionReadyState.SessionPath,
            BrowserAdoptionReadyStateVersion = adoptionReadyState.BrowserAdoptionReadyStateVersion,
            BrowserAdoptionSessionVersion = adoptionReadyState.BrowserAdoptionSessionVersion,
            LaunchMode = adoptionReadyState.LaunchMode,
            AssetRootPath = adoptionReadyState.AssetRootPath,
            ProfilesRootPath = adoptionReadyState.ProfilesRootPath,
            CacheRootPath = adoptionReadyState.CacheRootPath,
            ConfigRootPath = adoptionReadyState.ConfigRootPath,
            SettingsFilePath = adoptionReadyState.SettingsFilePath,
            StartupProfilePath = adoptionReadyState.StartupProfilePath,
            RequiredAssets = adoptionReadyState.RequiredAssets,
            ReadyAssetCount = adoptionReadyState.ReadyAssetCount,
            CompletedSteps = adoptionReadyState.CompletedSteps,
            TotalSteps = adoptionReadyState.TotalSteps,
            Exists = adoptionReadyState.Exists,
            ReadSucceeded = adoptionReadyState.ReadSucceeded
        };

        if (!adoptionReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser commitment session blocked for profile '{adoptionReadyState.ProfileId}'.";
            result.Error = adoptionReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCommitmentSessionVersion = "runtime-browser-commitment-session-v1";
        result.BrowserCommitmentStages =
        [
            "open-browser-commitment-session",
            "bind-browser-adoption-ready-state",
            "publish-browser-commitment-ready"
        ];
        result.BrowserCommitmentSummary = $"Runtime browser commitment session prepared {result.BrowserCommitmentStages.Length} commitment stage(s) for profile '{adoptionReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser commitment session ready for profile '{adoptionReadyState.ProfileId}' with {result.BrowserCommitmentStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCommitmentSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserCommitmentSessionVersion { get; set; } = string.Empty;
    public string BrowserAdoptionReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAdoptionSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserCommitmentStages { get; set; } = Array.Empty<string>();
    public string BrowserCommitmentSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
