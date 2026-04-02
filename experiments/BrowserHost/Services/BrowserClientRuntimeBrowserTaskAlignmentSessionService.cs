namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserTaskAlignmentSession
{
    ValueTask<BrowserClientRuntimeBrowserTaskAlignmentSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserTaskAlignmentSessionService : IBrowserClientRuntimeBrowserTaskAlignmentSession
{
    private readonly IBrowserClientRuntimeBrowserNavigationalConfidenceReadyState _runtimeBrowserNavigationalConfidenceReadyState;

    public BrowserClientRuntimeBrowserTaskAlignmentSessionService(IBrowserClientRuntimeBrowserNavigationalConfidenceReadyState runtimeBrowserNavigationalConfidenceReadyState)
    {
        _runtimeBrowserNavigationalConfidenceReadyState = runtimeBrowserNavigationalConfidenceReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserTaskAlignmentSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserNavigationalConfidenceReadyStateResult navigationalconfidenceReadyState = await _runtimeBrowserNavigationalConfidenceReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserTaskAlignmentSessionResult result = new()
        {
            ProfileId = navigationalconfidenceReadyState.ProfileId,
            SessionId = navigationalconfidenceReadyState.SessionId,
            SessionPath = navigationalconfidenceReadyState.SessionPath,
            BrowserNavigationalConfidenceReadyStateVersion = navigationalconfidenceReadyState.BrowserNavigationalConfidenceReadyStateVersion,
            BrowserNavigationalConfidenceSessionVersion = navigationalconfidenceReadyState.BrowserNavigationalConfidenceSessionVersion,
            LaunchMode = navigationalconfidenceReadyState.LaunchMode,
            AssetRootPath = navigationalconfidenceReadyState.AssetRootPath,
            ProfilesRootPath = navigationalconfidenceReadyState.ProfilesRootPath,
            CacheRootPath = navigationalconfidenceReadyState.CacheRootPath,
            ConfigRootPath = navigationalconfidenceReadyState.ConfigRootPath,
            SettingsFilePath = navigationalconfidenceReadyState.SettingsFilePath,
            StartupProfilePath = navigationalconfidenceReadyState.StartupProfilePath,
            RequiredAssets = navigationalconfidenceReadyState.RequiredAssets,
            ReadyAssetCount = navigationalconfidenceReadyState.ReadyAssetCount,
            CompletedSteps = navigationalconfidenceReadyState.CompletedSteps,
            TotalSteps = navigationalconfidenceReadyState.TotalSteps,
            Exists = navigationalconfidenceReadyState.Exists,
            ReadSucceeded = navigationalconfidenceReadyState.ReadSucceeded
        };

        if (!navigationalconfidenceReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser taskalignment session blocked for profile '{navigationalconfidenceReadyState.ProfileId}'.";
            result.Error = navigationalconfidenceReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserTaskAlignmentSessionVersion = "runtime-browser-taskalignment-session-v1";
        result.BrowserTaskAlignmentStages =
        [
            "open-browser-taskalignment-session",
            "bind-browser-navigationalconfidence-ready-state",
            "publish-browser-taskalignment-ready"
        ];
        result.BrowserTaskAlignmentSummary = $"Runtime browser taskalignment session prepared {result.BrowserTaskAlignmentStages.Length} taskalignment stage(s) for profile '{navigationalconfidenceReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser taskalignment session ready for profile '{navigationalconfidenceReadyState.ProfileId}' with {result.BrowserTaskAlignmentStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserTaskAlignmentSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserTaskAlignmentSessionVersion { get; set; } = string.Empty;
    public string BrowserNavigationalConfidenceReadyStateVersion { get; set; } = string.Empty;
    public string BrowserNavigationalConfidenceSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserTaskAlignmentStages { get; set; } = Array.Empty<string>();
    public string BrowserTaskAlignmentSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
