namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAssurabilityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserAssurabilityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAssurabilityReadyStateService : IBrowserClientRuntimeBrowserAssurabilityReadyState
{
    private readonly IBrowserClientRuntimeBrowserAssurabilitySession _runtimeBrowserAssurabilitySession;

    public BrowserClientRuntimeBrowserAssurabilityReadyStateService(IBrowserClientRuntimeBrowserAssurabilitySession runtimeBrowserAssurabilitySession)
    {
        _runtimeBrowserAssurabilitySession = runtimeBrowserAssurabilitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAssurabilityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserAssurabilitySessionResult assurabilitySession = await _runtimeBrowserAssurabilitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserAssurabilityReadyStateResult result = new()
        {
            ProfileId = assurabilitySession.ProfileId,
            SessionId = assurabilitySession.SessionId,
            SessionPath = assurabilitySession.SessionPath,
            BrowserAssurabilitySessionVersion = assurabilitySession.BrowserAssurabilitySessionVersion,
            BrowserDependabilityReadyStateVersion = assurabilitySession.BrowserDependabilityReadyStateVersion,
            BrowserDependabilitySessionVersion = assurabilitySession.BrowserDependabilitySessionVersion,
            LaunchMode = assurabilitySession.LaunchMode,
            AssetRootPath = assurabilitySession.AssetRootPath,
            ProfilesRootPath = assurabilitySession.ProfilesRootPath,
            CacheRootPath = assurabilitySession.CacheRootPath,
            ConfigRootPath = assurabilitySession.ConfigRootPath,
            SettingsFilePath = assurabilitySession.SettingsFilePath,
            StartupProfilePath = assurabilitySession.StartupProfilePath,
            RequiredAssets = assurabilitySession.RequiredAssets,
            ReadyAssetCount = assurabilitySession.ReadyAssetCount,
            CompletedSteps = assurabilitySession.CompletedSteps,
            TotalSteps = assurabilitySession.TotalSteps,
            Exists = assurabilitySession.Exists,
            ReadSucceeded = assurabilitySession.ReadSucceeded
        };

        if (!assurabilitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser assurability ready state blocked for profile '{assurabilitySession.ProfileId}'.";
            result.Error = assurabilitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAssurabilityReadyStateVersion = "runtime-browser-assurability-ready-state-v1";
        result.BrowserAssurabilityReadyChecks =
        [
            "browser-dependability-ready-state-ready",
            "browser-assurability-session-ready",
            "browser-assurability-ready"
        ];
        result.BrowserAssurabilityReadySummary = $"Runtime browser assurability ready state passed {result.BrowserAssurabilityReadyChecks.Length} assurability readiness check(s) for profile '{assurabilitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser assurability ready state ready for profile '{assurabilitySession.ProfileId}' with {result.BrowserAssurabilityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAssurabilityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserAssurabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAssurabilitySessionVersion { get; set; } = string.Empty;
    public string BrowserDependabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserDependabilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAssurabilityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserAssurabilityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
