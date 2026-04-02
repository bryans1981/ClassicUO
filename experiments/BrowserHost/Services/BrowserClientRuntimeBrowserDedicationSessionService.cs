namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDedicationSession
{
    ValueTask<BrowserClientRuntimeBrowserDedicationSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDedicationSessionService : IBrowserClientRuntimeBrowserDedicationSession
{
    private readonly IBrowserClientRuntimeBrowserCommitmentReadyState _runtimeBrowserCommitmentReadyState;

    public BrowserClientRuntimeBrowserDedicationSessionService(IBrowserClientRuntimeBrowserCommitmentReadyState runtimeBrowserCommitmentReadyState)
    {
        _runtimeBrowserCommitmentReadyState = runtimeBrowserCommitmentReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDedicationSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCommitmentReadyStateResult commitmentReadyState = await _runtimeBrowserCommitmentReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserDedicationSessionResult result = new()
        {
            ProfileId = commitmentReadyState.ProfileId,
            SessionId = commitmentReadyState.SessionId,
            SessionPath = commitmentReadyState.SessionPath,
            BrowserCommitmentReadyStateVersion = commitmentReadyState.BrowserCommitmentReadyStateVersion,
            BrowserCommitmentSessionVersion = commitmentReadyState.BrowserCommitmentSessionVersion,
            LaunchMode = commitmentReadyState.LaunchMode,
            AssetRootPath = commitmentReadyState.AssetRootPath,
            ProfilesRootPath = commitmentReadyState.ProfilesRootPath,
            CacheRootPath = commitmentReadyState.CacheRootPath,
            ConfigRootPath = commitmentReadyState.ConfigRootPath,
            SettingsFilePath = commitmentReadyState.SettingsFilePath,
            StartupProfilePath = commitmentReadyState.StartupProfilePath,
            RequiredAssets = commitmentReadyState.RequiredAssets,
            ReadyAssetCount = commitmentReadyState.ReadyAssetCount,
            CompletedSteps = commitmentReadyState.CompletedSteps,
            TotalSteps = commitmentReadyState.TotalSteps,
            Exists = commitmentReadyState.Exists,
            ReadSucceeded = commitmentReadyState.ReadSucceeded
        };

        if (!commitmentReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser dedication session blocked for profile '{commitmentReadyState.ProfileId}'.";
            result.Error = commitmentReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDedicationSessionVersion = "runtime-browser-dedication-session-v1";
        result.BrowserDedicationStages =
        [
            "open-browser-dedication-session",
            "bind-browser-commitment-ready-state",
            "publish-browser-dedication-ready"
        ];
        result.BrowserDedicationSummary = $"Runtime browser dedication session prepared {result.BrowserDedicationStages.Length} dedication stage(s) for profile '{commitmentReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser dedication session ready for profile '{commitmentReadyState.ProfileId}' with {result.BrowserDedicationStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDedicationSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserDedicationSessionVersion { get; set; } = string.Empty;
    public string BrowserCommitmentReadyStateVersion { get; set; } = string.Empty;
    public string BrowserCommitmentSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDedicationStages { get; set; } = Array.Empty<string>();
    public string BrowserDedicationSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
