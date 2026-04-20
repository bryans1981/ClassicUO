using ClassicUO.Utility;

namespace BrowserHost.Services;

public interface IBrowserClientNativeBrowserRuntimeLaunchSnapshot
{
    ValueTask<BrowserClientNativeBrowserRuntimeLaunchSnapshotResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default");
}

public sealed class BrowserClientNativeBrowserRuntimeLaunchSnapshotService : IBrowserClientNativeBrowserRuntimeLaunchSnapshot
{
    private readonly IBrowserClientNativeBrowserRuntimeSessionAssurance _nativeBrowserRuntimeSessionAssurance;
    private readonly IBrowserClientRuntimeLaunchContract _runtimeLaunchContract;

    public BrowserClientNativeBrowserRuntimeLaunchSnapshotService(
        IBrowserClientNativeBrowserRuntimeSessionAssurance nativeBrowserRuntimeSessionAssurance,
        IBrowserClientRuntimeLaunchContract runtimeLaunchContract
    )
    {
        _nativeBrowserRuntimeSessionAssurance = nativeBrowserRuntimeSessionAssurance;
        _runtimeLaunchContract = runtimeLaunchContract;
    }

    public async ValueTask<BrowserClientNativeBrowserRuntimeLaunchSnapshotResult> PrepareAsync(BrowserRuntimeBootstrapRequest? request = null, string profileId = "default")
    {
        DateTimeOffset started = DateTimeOffset.UtcNow;
        BrowserClientNativeBrowserRuntimeSessionAssuranceResult nativeBrowserRuntimeSessionAssurance = await _nativeBrowserRuntimeSessionAssurance.PrepareAsync(request, profileId);
        BrowserClientRuntimeLaunchContractResult runtimeLaunchContract = await _runtimeLaunchContract.BuildAsync(profileId);

        BrowserClientNativeBrowserRuntimeLaunchSnapshotResult result = new()
        {
            ProfileId = nativeBrowserRuntimeSessionAssurance.ProfileId,
            BrowserCanvasContainerId = nativeBrowserRuntimeSessionAssurance.BrowserCanvasContainerId,
            BrowserCanvasElementId = nativeBrowserRuntimeSessionAssurance.BrowserCanvasElementId,
            BrowserCanvasWidth = nativeBrowserRuntimeSessionAssurance.BrowserCanvasWidth,
            BrowserCanvasHeight = nativeBrowserRuntimeSessionAssurance.BrowserCanvasHeight,
            BrowserCanvasContextType = nativeBrowserRuntimeSessionAssurance.BrowserCanvasContextType,
            BrowserFrameRendered = nativeBrowserRuntimeSessionAssurance.BrowserFrameRendered,
            BrowserFrameContextType = nativeBrowserRuntimeSessionAssurance.BrowserFrameContextType,
            BrowserFrameText = nativeBrowserRuntimeSessionAssurance.BrowserFrameText,
            BrowserFrameError = nativeBrowserRuntimeSessionAssurance.BrowserFrameError,
            NativeBrowserCanvasFrameReady = nativeBrowserRuntimeSessionAssurance.NativeBrowserCanvasFrameReady,
            NativeBrowserCanvasFrameVersion = nativeBrowserRuntimeSessionAssurance.NativeBrowserCanvasFrameVersion,
            NativeBrowserCanvasFrameSummary = nativeBrowserRuntimeSessionAssurance.NativeBrowserCanvasFrameSummary,
            NativeBrowserFramePumpReady = nativeBrowserRuntimeSessionAssurance.NativeBrowserFramePumpReady,
            NativeBrowserFramePumpVersion = nativeBrowserRuntimeSessionAssurance.NativeBrowserFramePumpVersion,
            NativeBrowserFramePumpSummary = nativeBrowserRuntimeSessionAssurance.NativeBrowserFramePumpSummary,
            BrowserInputReady = nativeBrowserRuntimeSessionAssurance.BrowserInputReady,
            BrowserInputReadyStateVersion = nativeBrowserRuntimeSessionAssurance.BrowserInputReadyStateVersion,
            BrowserInputSummary = nativeBrowserRuntimeSessionAssurance.BrowserInputSummary,
            NativeBrowserRuntimeLoopReady = nativeBrowserRuntimeSessionAssurance.NativeBrowserRuntimeLoopReady,
            NativeBrowserRuntimeLoopVersion = nativeBrowserRuntimeSessionAssurance.NativeBrowserRuntimeLoopVersion,
            NativeBrowserRuntimeLoopSummary = nativeBrowserRuntimeSessionAssurance.NativeBrowserRuntimeLoopSummary,
            NativeBrowserRuntimeExecutionSliceReady = nativeBrowserRuntimeSessionAssurance.NativeBrowserRuntimeExecutionSliceReady,
            NativeBrowserRuntimeExecutionSliceVersion = nativeBrowserRuntimeSessionAssurance.NativeBrowserRuntimeExecutionSliceVersion,
            NativeBrowserRuntimeExecutionSliceSummary = nativeBrowserRuntimeSessionAssurance.NativeBrowserRuntimeExecutionSliceSummary,
            NativeBrowserNetworkReady = nativeBrowserRuntimeSessionAssurance.NativeBrowserNetworkReady,
            NativeBrowserNetworkVersion = nativeBrowserRuntimeSessionAssurance.NativeBrowserNetworkVersion,
            NativeBrowserNetworkSummary = nativeBrowserRuntimeSessionAssurance.NativeBrowserNetworkSummary,
            NativeBrowserTransportReady = nativeBrowserRuntimeSessionAssurance.NativeBrowserTransportReady,
            NativeBrowserTransportVersion = nativeBrowserRuntimeSessionAssurance.NativeBrowserTransportVersion,
            NativeBrowserTransportSummary = nativeBrowserRuntimeSessionAssurance.NativeBrowserTransportSummary,
            TransportEndpoint = nativeBrowserRuntimeSessionAssurance.TransportEndpoint,
            TransportMode = nativeBrowserRuntimeSessionAssurance.TransportMode,
            LaunchMode = nativeBrowserRuntimeSessionAssurance.LaunchMode,
            ContractVersion = nativeBrowserRuntimeSessionAssurance.ContractVersion,
            WebSocketRuntimeExecutionReady = nativeBrowserRuntimeSessionAssurance.WebSocketRuntimeExecutionReady,
            WebSocketRuntimeExecutionEndpoint = nativeBrowserRuntimeSessionAssurance.WebSocketRuntimeExecutionEndpoint,
            WebSocketRuntimeExecutionProtocol = nativeBrowserRuntimeSessionAssurance.WebSocketRuntimeExecutionProtocol,
            WebSocketRuntimeExecutionMode = nativeBrowserRuntimeSessionAssurance.WebSocketRuntimeExecutionMode,
            WebSocketRuntimeExecutionVersion = nativeBrowserRuntimeSessionAssurance.WebSocketRuntimeExecutionVersion,
            WebSocketRuntimeExecutionSummary = nativeBrowserRuntimeSessionAssurance.WebSocketRuntimeExecutionSummary,
            BrowserSessionReady = nativeBrowserRuntimeSessionAssurance.BrowserSessionReady,
            BrowserSessionVersion = nativeBrowserRuntimeSessionAssurance.BrowserSessionVersion,
            BrowserSessionSummary = nativeBrowserRuntimeSessionAssurance.BrowserSessionSummary,
            BrowserSessionStabilityReady = nativeBrowserRuntimeSessionAssurance.BrowserSessionStabilityReady,
            BrowserSessionStabilityReadyStateVersion = nativeBrowserRuntimeSessionAssurance.BrowserSessionStabilityReadyStateVersion,
            BrowserSessionStabilitySummary = nativeBrowserRuntimeSessionAssurance.BrowserSessionStabilitySummary,
            WebSocketSessionEndpoint = nativeBrowserRuntimeSessionAssurance.WebSocketSessionEndpoint,
            WebSocketSessionScheme = nativeBrowserRuntimeSessionAssurance.WebSocketSessionScheme,
            WebSocketSessionProtocol = nativeBrowserRuntimeSessionAssurance.WebSocketSessionProtocol,
            WebSocketSessionReady = nativeBrowserRuntimeSessionAssurance.WebSocketSessionReady,
            WebSocketSessionVersion = nativeBrowserRuntimeSessionAssurance.WebSocketSessionVersion,
            WebSocketSessionSummary = nativeBrowserRuntimeSessionAssurance.WebSocketSessionSummary,
            ReadyAssetCount = nativeBrowserRuntimeSessionAssurance.ReadyAssetCount,
            CacheHits = nativeBrowserRuntimeSessionAssurance.CacheHits,
            AssetRootPath = nativeBrowserRuntimeSessionAssurance.AssetRootPath,
            ProfilesRootPath = nativeBrowserRuntimeSessionAssurance.ProfilesRootPath,
            CacheRootPath = nativeBrowserRuntimeSessionAssurance.CacheRootPath,
            ConfigRootPath = nativeBrowserRuntimeSessionAssurance.ConfigRootPath,
            SettingsFilePath = nativeBrowserRuntimeSessionAssurance.SettingsFilePath,
            StartupProfilePath = nativeBrowserRuntimeSessionAssurance.StartupProfilePath,
            RequiredAssets = nativeBrowserRuntimeSessionAssurance.RequiredAssets,
            LaunchContractIsReady = runtimeLaunchContract.IsReady,
            LaunchContractVersion = runtimeLaunchContract.ContractVersion,
            LaunchContractMode = runtimeLaunchContract.LaunchMode,
            LaunchContractSessionId = runtimeLaunchContract.SessionId,
            LaunchContractSessionPath = runtimeLaunchContract.SessionPath,
            LaunchContractArtifactPath = runtimeLaunchContract.ArtifactPath,
            LaunchContractSummary = runtimeLaunchContract.Summary,
            ReadinessChecks =
            [
                nativeBrowserRuntimeSessionAssurance.IsReady ? "native-browser-runtime-session-assurance-ready" : "native-browser-runtime-session-assurance-blocked",
                runtimeLaunchContract.IsReady ? "runtime-launch-contract-ready" : "runtime-launch-contract-blocked"
            ]
        };

        result.IsReady = result.ReadinessChecks.All(static check => check.EndsWith("-ready", StringComparison.Ordinal));
        result.NativeBrowserRuntimeLaunchSnapshotVersion = "browser-native-browser-runtime-launch-snapshot-v1";
        result.NativeBrowserRuntimeLaunchSnapshotStages =
        [
            "bind-native-browser-runtime-session-assurance",
            "bind-runtime-launch-contract",
            "publish-browser-native-browser-runtime-launch-snapshot"
        ];
        result.TotalMs = (DateTimeOffset.UtcNow - started).TotalMilliseconds;
        result.Summary = result.IsReady
            ? $"Browser-native browser runtime launch snapshot ready for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeLaunchSnapshotStages.Length} stage(s) and launch mode {result.LaunchContractMode}."
            : $"Browser-native browser runtime launch snapshot blocked for profile '{result.ProfileId}' with {result.NativeBrowserRuntimeLaunchSnapshotStages.Length} stage(s).";

        return result;
    }
}

public sealed class BrowserClientNativeBrowserRuntimeLaunchSnapshotResult
{
    public bool IsReady { get; set; }
    public string NativeBrowserRuntimeLaunchSnapshotVersion { get; set; } = string.Empty;
    public string[] NativeBrowserRuntimeLaunchSnapshotStages { get; set; } = Array.Empty<string>();
    public string ProfileId { get; set; } = "default";
    public string BrowserCanvasContainerId { get; set; } = string.Empty;
    public string BrowserCanvasElementId { get; set; } = string.Empty;
    public int BrowserCanvasWidth { get; set; }
    public int BrowserCanvasHeight { get; set; }
    public string BrowserCanvasContextType { get; set; } = string.Empty;
    public bool BrowserFrameRendered { get; set; }
    public string BrowserFrameContextType { get; set; } = string.Empty;
    public string BrowserFrameText { get; set; } = string.Empty;
    public string BrowserFrameError { get; set; } = string.Empty;
    public bool NativeBrowserCanvasFrameReady { get; set; }
    public string NativeBrowserCanvasFrameVersion { get; set; } = string.Empty;
    public string NativeBrowserCanvasFrameSummary { get; set; } = string.Empty;
    public bool NativeBrowserFramePumpReady { get; set; }
    public string NativeBrowserFramePumpVersion { get; set; } = string.Empty;
    public string NativeBrowserFramePumpSummary { get; set; } = string.Empty;
    public bool BrowserInputReady { get; set; }
    public string BrowserInputReadyStateVersion { get; set; } = string.Empty;
    public string BrowserInputSummary { get; set; } = string.Empty;
    public bool NativeBrowserRuntimeLoopReady { get; set; }
    public string NativeBrowserRuntimeLoopVersion { get; set; } = string.Empty;
    public string NativeBrowserRuntimeLoopSummary { get; set; } = string.Empty;
    public bool NativeBrowserRuntimeExecutionSliceReady { get; set; }
    public string NativeBrowserRuntimeExecutionSliceVersion { get; set; } = string.Empty;
    public string NativeBrowserRuntimeExecutionSliceSummary { get; set; } = string.Empty;
    public bool NativeBrowserNetworkReady { get; set; }
    public string NativeBrowserNetworkVersion { get; set; } = string.Empty;
    public string NativeBrowserNetworkSummary { get; set; } = string.Empty;
    public bool NativeBrowserTransportReady { get; set; }
    public string NativeBrowserTransportVersion { get; set; } = string.Empty;
    public string NativeBrowserTransportSummary { get; set; } = string.Empty;
    public string TransportEndpoint { get; set; } = string.Empty;
    public string TransportMode { get; set; } = string.Empty;
    public string LaunchMode { get; set; } = string.Empty;
    public string ContractVersion { get; set; } = string.Empty;
    public bool WebSocketRuntimeExecutionReady { get; set; }
    public string WebSocketRuntimeExecutionEndpoint { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionProtocol { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionMode { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionVersion { get; set; } = string.Empty;
    public string WebSocketRuntimeExecutionSummary { get; set; } = string.Empty;
    public bool BrowserSessionReady { get; set; }
    public string BrowserSessionVersion { get; set; } = string.Empty;
    public string BrowserSessionSummary { get; set; } = string.Empty;
    public bool BrowserSessionStabilityReady { get; set; }
    public string BrowserSessionStabilityReadyStateVersion { get; set; } = string.Empty;
    public string BrowserSessionStabilitySummary { get; set; } = string.Empty;
    public bool WebSocketSessionReady { get; set; }
    public string WebSocketSessionEndpoint { get; set; } = string.Empty;
    public string WebSocketSessionScheme { get; set; } = string.Empty;
    public string WebSocketSessionProtocol { get; set; } = string.Empty;
    public string WebSocketSessionVersion { get; set; } = string.Empty;
    public string WebSocketSessionSummary { get; set; } = string.Empty;
    public string AssetRootPath { get; set; } = BrowserVirtualPaths.AssetsRoot;
    public string ProfilesRootPath { get; set; } = BrowserVirtualPaths.ProfilesRoot;
    public string CacheRootPath { get; set; } = BrowserVirtualPaths.CacheRoot;
    public string ConfigRootPath { get; set; } = BrowserVirtualPaths.ConfigRoot;
    public string SettingsFilePath { get; set; } = string.Empty;
    public string StartupProfilePath { get; set; } = string.Empty;
    public string[] RequiredAssets { get; set; } = Array.Empty<string>();
    public int ReadyAssetCount { get; set; }
    public int CacheHits { get; set; }
    public bool LaunchContractIsReady { get; set; }
    public string LaunchContractVersion { get; set; } = string.Empty;
    public string LaunchContractMode { get; set; } = string.Empty;
    public string LaunchContractSessionId { get; set; } = string.Empty;
    public string LaunchContractSessionPath { get; set; } = string.Empty;
    public string LaunchContractArtifactPath { get; set; } = string.Empty;
    public string LaunchContractSummary { get; set; } = string.Empty;
    public string[] ReadinessChecks { get; set; } = Array.Empty<string>();
    public double TotalMs { get; set; }
    public string Summary { get; set; } = string.Empty;
}
