namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserReadabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserReadabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserReadabilityReadyStateService : IBrowserClientRuntimeBrowserReadabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserReadabilitySession _runtimeBrowserReadabilitySession;

    public BrowserClientRuntimeBrowserReadabilityReadyStateService(IBrowserClientRuntimeBrowserReadabilitySession runtimeBrowserReadabilitySession)
    {
        _runtimeBrowserReadabilitySession = runtimeBrowserReadabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserReadabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserReadabilitySessionResult readabilitySession = await _runtimeBrowserReadabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserReadabilityReadyStateResult result = new()
        {
            ProfileId = readabilitySession.ProfileId,
            SessionId = readabilitySession.SessionId,
            SessionPath = readabilitySession.SessionPath,
            BrowserReadabilitySessionVersion = readabilitySession.BrowserReadabilitySessionVersion,
            BrowserHarmonyReadyStateVersion = readabilitySession.BrowserHarmonyReadyStateVersion,
            BrowserHarmonySessionVersion = readabilitySession.BrowserHarmonySessionVersion,
            LaunchMode = readabilitySession.LaunchMode,
            AssetRootPath = readabilitySession.AssetRootPath,
            ProfilesRootPath = readabilitySession.ProfilesRootPath,
            CacheRootPath = readabilitySession.CacheRootPath,
            ConfigRootPath = readabilitySession.ConfigRootPath,
            SettingsFilePath = readabilitySession.SettingsFilePath,
            StartupProfilePath = readabilitySession.StartupProfilePath,
            RequiredAssets = readabilitySession.RequiredAssets,
            ReadyAssetCount = readabilitySession.ReadyAssetCount,
            CompletedSteps = readabilitySession.CompletedSteps,
            TotalSteps = readabilitySession.TotalSteps,
            Exists = readabilitySession.Exists,
            ReadSucceeded = readabilitySession.ReadSucceeded
        };

        if (!readabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser readability ready state blocked for profile '{readabilitySession.ProfileId}'.";
            result.Error = readabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserReadabilityReadyStateVersion = "runtime-browser-readability-ready-state-v1";
        result.BrowserReadabilityReadyChecks =
        [
            "browser-harmony-ready-state-ready",
            "browser-readability-session-ready",
            "browser-readability-ready"
        ];
        result.BrowserReadabilityReadySummary = $"Runtime browser readability ready state passed {result.BrowserReadabilityReadyChecks.Length} readability readiness check(s) for profile '{readabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser readability ready state ready for profile '{readabilitySession.ProfileId}' with {result.BrowserReadabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserReadabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserReadabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserReadabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserHarmonyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserHarmonySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserReadabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserReadabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}

