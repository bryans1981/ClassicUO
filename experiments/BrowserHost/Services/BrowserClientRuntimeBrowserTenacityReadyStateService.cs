namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTenacityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserTenacityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTenacityReadyStateService : IBrowserClientRuntimeBrowserTenacityReadyState
{
    private readonly IBrowserClientRuntimeBrowserTenacitySession _runtimeBrowserTenacitySession;

    public BrowserClientRuntimeBrowserTenacityReadyStateService(IBrowserClientRuntimeBrowserTenacitySession runtimeBrowserTenacitySession)
    {
        _runtimeBrowserTenacitySession = runtimeBrowserTenacitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTenacityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserTenacitySessionResult tenacitySession = await _runtimeBrowserTenacitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserTenacityReadyStateResult result = new()
        {
            ProfileId = tenacitySession.ProfileId,
            SessionId = tenacitySession.SessionId,
            SessionPath = tenacitySession.SessionPath,
            BrowserTenacitySessionVersion = tenacitySession.BrowserTenacitySessionVersion,
            BrowserPersistenceReadyStateVersion = tenacitySession.BrowserPersistenceReadyStateVersion,
            BrowserPersistenceSessionVersion = tenacitySession.BrowserPersistenceSessionVersion,
            LaunchMode = tenacitySession.LaunchMode,
            AssetRootPath = tenacitySession.AssetRootPath,
            ProfilesRootPath = tenacitySession.ProfilesRootPath,
            CacheRootPath = tenacitySession.CacheRootPath,
            ConfigRootPath = tenacitySession.ConfigRootPath,
            SettingsFilePath = tenacitySession.SettingsFilePath,
            StartupProfilePath = tenacitySession.StartupProfilePath,
            RequiredAssets = tenacitySession.RequiredAssets,
            ReadyAssetCount = tenacitySession.ReadyAssetCount,
            CompletedSteps = tenacitySession.CompletedSteps,
            TotalSteps = tenacitySession.TotalSteps,
            Exists = tenacitySession.Exists,
            ReadSucceeded = tenacitySession.ReadSucceeded
        };

        if (!tenacitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser tenacity ready state blocked for profile '{tenacitySession.ProfileId}'.";
            result.Error = tenacitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTenacityReadyStateVersion = "runtime-browser-tenacity-ready-state-v1";
        result.BrowserTenacityReadyChecks =
        [
            "browser-persistence-ready-state-ready",
            "browser-tenacity-session-ready",
            "browser-tenacity-ready"
        ];
        result.BrowserTenacityReadySummary = $"Runtime browser tenacity ready state passed {result.BrowserTenacityReadyChecks.Length} tenacity readiness check(s) for profile '{tenacitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser tenacity ready state ready for profile '{tenacitySession.ProfileId}' with {result.BrowserTenacityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTenacityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserTenacityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserTenacitySessionVersion { get; set; } = string.Empty;
    public string BrowserPersistenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPersistenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserTenacityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserTenacityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
