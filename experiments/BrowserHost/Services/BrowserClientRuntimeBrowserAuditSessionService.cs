namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAuditSession
{
    ValueTask<BrowserClientRuntimeBrowserAuditSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAuditSessionService : IBrowserClientRuntimeBrowserAuditSession
{
    private readonly IBrowserClientRuntimeBrowserPolicyReadyState _runtimeBrowserPolicyReadyState;

    public BrowserClientRuntimeBrowserAuditSessionService(IBrowserClientRuntimeBrowserPolicyReadyState runtimeBrowserPolicyReadyState)
    {
        _runtimeBrowserPolicyReadyState = runtimeBrowserPolicyReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAuditSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserPolicyReadyStateResult policyReadyState = await _runtimeBrowserPolicyReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAuditSessionResult result = new()
        {
            ProfileId = policyReadyState.ProfileId,
            SessionId = policyReadyState.SessionId,
            SessionPath = policyReadyState.SessionPath,
            BrowserPolicyReadyStateVersion = policyReadyState.BrowserPolicyReadyStateVersion,
            BrowserPolicySessionVersion = policyReadyState.BrowserPolicySessionVersion,
            LaunchMode = policyReadyState.LaunchMode,
            AssetRootPath = policyReadyState.AssetRootPath,
            ProfilesRootPath = policyReadyState.ProfilesRootPath,
            CacheRootPath = policyReadyState.CacheRootPath,
            ConfigRootPath = policyReadyState.ConfigRootPath,
            SettingsFilePath = policyReadyState.SettingsFilePath,
            StartupProfilePath = policyReadyState.StartupProfilePath,
            RequiredAssets = policyReadyState.RequiredAssets,
            ReadyAssetCount = policyReadyState.ReadyAssetCount,
            CompletedSteps = policyReadyState.CompletedSteps,
            TotalSteps = policyReadyState.TotalSteps,
            Exists = policyReadyState.Exists,
            ReadSucceeded = policyReadyState.ReadSucceeded,
            BrowserPolicyReadyState = policyReadyState
        };

        if (!policyReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser audit session blocked for profile '{policyReadyState.ProfileId}'.";
            result.Error = policyReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAuditSessionVersion = "runtime-browser-audit-session-v1";
        result.BrowserAuditStages =
        [
            "open-browser-audit-session",
            "bind-browser-policy-ready-state",
            "publish-browser-audit-ready"
        ];
        result.BrowserAuditSummary = $"Runtime browser audit session prepared {result.BrowserAuditStages.Length} audit stage(s) for profile '{policyReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser audit session ready for profile '{policyReadyState.ProfileId}' with {result.BrowserAuditStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAuditSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAuditSessionVersion { get; set; } = string.Empty;
    public string BrowserPolicyReadyStateVersion { get; set; } = string.Empty;
    public string BrowserPolicySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAuditStages { get; set; } = Array.Empty<string>();
    public string BrowserAuditSummary { get; set; } = string.Empty;
    public BrowserClientRuntimeBrowserPolicyReadyStateResult BrowserPolicyReadyState { get; set; } = new();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
