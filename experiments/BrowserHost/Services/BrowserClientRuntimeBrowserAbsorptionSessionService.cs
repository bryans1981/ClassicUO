namespace BrowserHost.Services;

public interface IBrowserClientRuntimeBrowserAbsorptionSession
{
    ValueTask<BrowserClientRuntimeBrowserAbsorptionSessionResult> CreateAsync(string profileId = "default");
}

public sealed class BrowserClientRuntimeBrowserAbsorptionSessionService : IBrowserClientRuntimeBrowserAbsorptionSession
{
    private readonly IBrowserClientRuntimeBrowserInvolvementReadyState _runtimeBrowserInvolvementReadyState;

    public BrowserClientRuntimeBrowserAbsorptionSessionService(IBrowserClientRuntimeBrowserInvolvementReadyState runtimeBrowserInvolvementReadyState)
    {
        _runtimeBrowserInvolvementReadyState = runtimeBrowserInvolvementReadyState;
    }

    public async ValueTask<BrowserClientRuntimeBrowserAbsorptionSessionResult> CreateAsync(string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientRuntimeBrowserInvolvementReadyStateResult involvementReadyState = await _runtimeBrowserInvolvementReadyState.BuildAsync(profileId);

        BrowserClientRuntimeBrowserAbsorptionSessionResult result = new()
        {
            ProfileId = involvementReadyState.ProfileId,
            SessionId = involvementReadyState.SessionId,
            SessionPath = involvementReadyState.SessionPath,
            BrowserInvolvementReadyStateVersion = involvementReadyState.BrowserInvolvementReadyStateVersion,
            BrowserInvolvementSessionVersion = involvementReadyState.BrowserInvolvementSessionVersion,
            LaunchMode = involvementReadyState.LaunchMode,
            AssetRootPath = involvementReadyState.AssetRootPath,
            ProfilesRootPath = involvementReadyState.ProfilesRootPath,
            CacheRootPath = involvementReadyState.CacheRootPath,
            ConfigRootPath = involvementReadyState.ConfigRootPath,
            SettingsFilePath = involvementReadyState.SettingsFilePath,
            StartupProfilePath = involvementReadyState.StartupProfilePath,
            RequiredAssets = involvementReadyState.RequiredAssets,
            ReadyAssetCount = involvementReadyState.ReadyAssetCount,
            CompletedSteps = involvementReadyState.CompletedSteps,
            TotalSteps = involvementReadyState.TotalSteps,
            Exists = involvementReadyState.Exists,
            ReadSucceeded = involvementReadyState.ReadSucceeded
        };

        if (!involvementReadyState.IsReady)
        {
            result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
            result.Summary = $"Runtime browser absorption session blocked for profile '{involvementReadyState.ProfileId}'.";
            result.Error = involvementReadyState.Error;
            return result;
        }

        result.IsReady = true;
        result.BrowserAbsorptionSessionVersion = "runtime-browser-absorption-session-v1";
        result.BrowserAbsorptionStages =
        [
            "open-browser-absorption-session",
            "bind-browser-involvement-ready-state",
            "publish-browser-absorption-ready"
        ];
        result.BrowserAbsorptionSummary = $"Runtime browser absorption session prepared {result.BrowserAbsorptionStages.Length} absorption stage(s) for profile '{involvementReadyState.ProfileId}'.";
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = $"Runtime browser absorption session ready for profile '{involvementReadyState.ProfileId}' with {result.BrowserAbsorptionStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientRuntimeBrowserAbsorptionSessionResult
{
    public bool IsReady { get; set; }
    public string BrowserAbsorptionSessionVersion { get; set; } = string.Empty;
    public string BrowserInvolvementReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInvolvementSessionVersion { get; set; } = string.Empty;
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
    public string[] BrowserAbsorptionStages { get; set; } = Array.Empty<string>();
    public string BrowserAbsorptionSummary { get; set; } = string.Empty;
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
