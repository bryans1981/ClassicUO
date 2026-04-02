namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserCommitmentReadyState
{
    ValueTask<BrowserClientRuntimeBrowserCommitmentReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserCommitmentReadyStateService : IBrowserClientRuntimeBrowserCommitmentReadyState
{
    private readonly IBrowserClientRuntimeBrowserCommitmentSession _runtimeBrowserCommitmentSession;

    public BrowserClientRuntimeBrowserCommitmentReadyStateService(IBrowserClientRuntimeBrowserCommitmentSession runtimeBrowserCommitmentSession)
    {
        _runtimeBrowserCommitmentSession = runtimeBrowserCommitmentSession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserCommitmentReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserCommitmentSessionResult commitmentSession = await _runtimeBrowserCommitmentSession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserCommitmentReadyStateResult result = new()
        {
            ProfileId = commitmentSession.ProfileId,
            SessionId = commitmentSession.SessionId,
            SessionPath = commitmentSession.SessionPath,
            BrowserCommitmentSessionVersion = commitmentSession.BrowserCommitmentSessionVersion,
            BrowserAdoptionReadyStateVersion = commitmentSession.BrowserAdoptionReadyStateVersion,
            BrowserAdoptionSessionVersion = commitmentSession.BrowserAdoptionSessionVersion,
            LaunchMode = commitmentSession.LaunchMode,
            AssetRootPath = commitmentSession.AssetRootPath,
            ProfilesRootPath = commitmentSession.ProfilesRootPath,
            CacheRootPath = commitmentSession.CacheRootPath,
            ConfigRootPath = commitmentSession.ConfigRootPath,
            SettingsFilePath = commitmentSession.SettingsFilePath,
            StartupProfilePath = commitmentSession.StartupProfilePath,
            RequiredAssets = commitmentSession.RequiredAssets,
            ReadyAssetCount = commitmentSession.ReadyAssetCount,
            CompletedSteps = commitmentSession.CompletedSteps,
            TotalSteps = commitmentSession.TotalSteps,
            Exists = commitmentSession.Exists,
            ReadSucceeded = commitmentSession.ReadSucceeded
        };

        if (!commitmentSession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser commitment ready state blocked for profile '{commitmentSession.ProfileId}'.";
            result.Error = commitmentSession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserCommitmentReadyStateVersion = "runtime-browser-commitment-ready-state-v1";
        result.BrowserCommitmentReadyChecks =
        [
            "browser-adoption-ready-state-ready",
            "browser-commitment-session-ready",
            "browser-commitment-ready"
        ];
        result.BrowserCommitmentReadySummary = $"Runtime browser commitment ready state passed {result.BrowserCommitmentReadyChecks.Length} commitment readiness check(s) for profile '{commitmentSession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser commitment ready state ready for profile '{commitmentSession.ProfileId}' with {result.BrowserCommitmentReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserCommitmentReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserCommitmentReadyStateVersion { get; set; } = string.Empty;
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
    public string[] BrowserCommitmentReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserCommitmentReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
