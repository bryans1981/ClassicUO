namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserInclusivityReadyState
{
    ValueTask<BrowserClientRuntimeBrowserInclusivityReadyStateResult> BuildAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserInclusivityReadyStateService : IBrowserClientRuntimeBrowserInclusivityReadyState
{
    private readonly IBrowserClientRuntimeBrowserInclusivitySession _runtimeBrowserInclusivitySession;

    public BrowserClientRuntimeBrowserInclusivityReadyStateService(IBrowserClientRuntimeBrowserInclusivitySession runtimeBrowserInclusivitySession)
    {
        _runtimeBrowserInclusivitySession = runtimeBrowserInclusivitySession;
    }

    public async ValueTask<BrowserClientRuntimeBrowserInclusivityReadyStateResult> BuildAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserInclusivitySessionResult inclusivitySession = await _runtimeBrowserInclusivitySession.CreateAsync(profileId);

        BrowserClientRuntimeBrowserInclusivityReadyStateResult result = new()
        {
            ProfileId = inclusivitySession.ProfileId,
            SessionId = inclusivitySession.SessionId,
            SessionPath = inclusivitySession.SessionPath,
            BrowserInclusivitySessionVersion = inclusivitySession.BrowserInclusivitySessionVersion,
            BrowserAccessibilityReadyStateVersion = inclusivitySession.BrowserAccessibilityReadyStateVersion,
            BrowserAccessibilitySessionVersion = inclusivitySession.BrowserAccessibilitySessionVersion,
            LaunchMode = inclusivitySession.LaunchMode,
            AssetRootPath = inclusivitySession.AssetRootPath,
            ProfilesRootPath = inclusivitySession.ProfilesRootPath,
            CacheRootPath = inclusivitySession.CacheRootPath,
            ConfigRootPath = inclusivitySession.ConfigRootPath,
            SettingsFilePath = inclusivitySession.SettingsFilePath,
            StartupProfilePath = inclusivitySession.StartupProfilePath,
            RequiredAssets = inclusivitySession.RequiredAssets,
            ReadyAssetCount = inclusivitySession.ReadyAssetCount,
            CompletedSteps = inclusivitySession.CompletedSteps,
            TotalSteps = inclusivitySession.TotalSteps,
            Exists = inclusivitySession.Exists,
            ReadSucceeded = inclusivitySession.ReadSucceeded
        };

        if (!inclusivitySession.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser inclusivity ready state blocked for profile '{inclusivitySession.ProfileId}'.";
            result.Error = inclusivitySession.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserInclusivityReadyStateVersion = "runtime-browser-inclusivity-ready-state-v1";
        result.BrowserInclusivityReadyChecks =
        [
            "browser-accessibility-ready-state-ready",
            "browser-inclusivity-session-ready",
            "browser-inclusivity-ready"
        ];
        result.BrowserInclusivityReadySummary = $"Runtime browser inclusivity ready state passed {result.BrowserInclusivityReadyChecks.Length} inclusivity readiness check(s) for profile '{inclusivitySession.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser inclusivity ready state ready for profile '{inclusivitySession.ProfileId}' with {result.BrowserInclusivityReadyChecks.Length} check(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserInclusivityReadyStateResult
{
    public bool IsReady { get; set; }
    public string BrowserInclusivityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInclusivitySessionVersion { get; set; } = string.Empty;
    public string BrowserAccessibilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserAccessibilitySessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserInclusivityReadyChecks { get; set; } = Array.Empty<string>();
    public string BrowserInclusivityReadySummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
