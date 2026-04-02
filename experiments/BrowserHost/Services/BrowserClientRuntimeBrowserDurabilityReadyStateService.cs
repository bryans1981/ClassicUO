namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserDurabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserDurabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserDurabilityReadyStateService : IBrowserClientRuntimeBrowserDurabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserDurabilitySession _runtimeBrowserDurabilitySession;

    public BrowserClientRuntimeBrowserDurabilityReadyStateService(IBrowserClientRuntimeBrowserDurabilitySession runtimeBrowserDurabilitySession)
    {
        _runtimeBrowserDurabilitySession = runtimeBrowserDurabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserDurabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserDurabilitySessionResult durabilitySession = await _runtimeBrowserDurabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserDurabilityReadyStateResult result = new()
        {
            ProfileId = durabilitySession.ProfileId,
            SessionId = durabilitySession.SessionId,
            SessionPath = durabilitySession.SessionPath,
            BrowserDurabilitySessionVersion = durabilitySession.BrowserDurabilitySessionVersion,
            BrowserContinuityReadyStateVersion = durabilitySession.BrowserContinuityReadyStateVersion,
            BrowserContinuitySessionVersion = durabilitySession.BrowserContinuitySessionVersion,
            LaunchMode = durabilitySession.LaunchMode,
            AssetRootPath = durabilitySession.AssetRootPath,
            ProfilesRootPath = durabilitySession.ProfilesRootPath,
            CacheRootPath = durabilitySession.CacheRootPath,
            ConfigRootPath = durabilitySession.ConfigRootPath,
            SettingsFilePath = durabilitySession.SettingsFilePath,
            StartupProfilePath = durabilitySession.StartupProfilePath,
            RequiredAssets = durabilitySession.RequiredAssets,
            ReadyAssetCount = durabilitySession.ReadyAssetCount,
            CompletedSteps = durabilitySession.CompletedSteps,
            TotalSteps = durabilitySession.TotalSteps,
            Exists = durabilitySession.Exists,
            ReadSucceeded = durabilitySession.ReadSucceeded
        };

        if (!durabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser durability ready state blocked for profile '{durabilitySession.ProfileId}'.";
            result.Error = durabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserDurabilityReadyStateVersion = "runtime-browser-durability-ready-state-v1";
        result.BrowserDurabilityReadyChecks =
        [
            "browser-continuity-ready-state-ready",
            "browser-durability-session-ready",
            "browser-durability-ready"
        ];
        result.BrowserDurabilityReadySummary = $"Runtime browser durability ready state passed {result.BrowserDurabilityReadyChecks.Length} durability readiness check(s) for profile '{durabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser durability ready state ready for profile '{durabilitySession.ProfileId}' with {result.BrowserDurabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserDurabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserDurabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDurabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserContinuityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserContinuitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserDurabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserDurabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
