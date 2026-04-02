namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserIntentionalityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserIntentionalityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserIntentionalityReadyStateService : IBrowserClientRuntimeBrowserIntentionalityReadyState
{
    private readonly IBrowserClientRuntimeBrowserIntentionalitySession _runtimeBrowserIntentionalitySession;

    public BrowserClientRuntimeBrowserIntentionalityReadyStateService(IBrowserClientRuntimeBrowserIntentionalitySession runtimeBrowserIntentionalitySession)
    {
        _runtimeBrowserIntentionalitySession = runtimeBrowserIntentionalitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserIntentionalityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserIntentionalitySessionResult intentionalitySession = await _runtimeBrowserIntentionalitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserIntentionalityReadyStateResult result = new()
        {
            ProfileId = intentionalitySession.ProfileId,
            SessionId = intentionalitySession.SessionId,
            SessionPath = intentionalitySession.SessionPath,
            BrowserIntentionalitySessionVersion = intentionalitySession.BrowserIntentionalitySessionVersion,
            BrowserClarityOfPurposeReadyStateVersion = intentionalitySession.BrowserClarityOfPurposeReadyStateVersion,
            BrowserClarityOfPurposeSessionVersion = intentionalitySession.BrowserClarityOfPurposeSessionVersion,
            LaunchMode = intentionalitySession.LaunchMode,
            AssetRootPath = intentionalitySession.AssetRootPath,
            ProfilesRootPath = intentionalitySession.ProfilesRootPath,
            CacheRootPath = intentionalitySession.CacheRootPath,
            ConfigRootPath = intentionalitySession.ConfigRootPath,
            SettingsFilePath = intentionalitySession.SettingsFilePath,
            StartupProfilePath = intentionalitySession.StartupProfilePath,
            RequiredAssets = intentionalitySession.RequiredAssets,
            ReadyAssetCount = intentionalitySession.ReadyAssetCount,
            CompletedSteps = intentionalitySession.CompletedSteps,
            TotalSteps = intentionalitySession.TotalSteps,
            Exists = intentionalitySession.Exists,
            ReadSucceeded = intentionalitySession.ReadSucceeded
        };

        if (!intentionalitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser intentionality ready state blocked for profile '{intentionalitySession.ProfileId}'.";
            result.Error = intentionalitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserIntentionalityReadyStateVersion = "runtime-browser-intentionality-ready-state-v1";
        result.BrowserIntentionalityReadyChecks =
        [
            "browser-clarityofpurpose-ready-state-ready",
            "browser-intentionality-session-ready",
            "browser-intentionality-ready"
        ];
        result.BrowserIntentionalityReadySummary = $"Runtime browser intentionality ready state passed {result.BrowserIntentionalityReadyChecks.Length} intentionality readiness check(s) for profile '{intentionalitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser intentionality ready state ready for profile '{intentionalitySession.ProfileId}' with {result.BrowserIntentionalityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserIntentionalityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserIntentionalityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserIntentionalitySessionVersion { get; set; } = string.Empty;
    public string BrowserClarityOfPurposeReadyStateVersion { get; set; } = string.Empty;
    public string BrowserClarityOfPurposeSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserIntentionalityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserIntentionalityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
