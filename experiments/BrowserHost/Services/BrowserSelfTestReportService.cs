using System.Text.Json;

namespace BrowserHost.Services;

public sealed class BrowserSelfTestReportService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly IBrowserClientAssetService _browserClientAssetService;
    private readonly BrowserFileSystemBridgeService _bridgeService;
    private readonly BrowserStorageService _storageService;
    private readonly IBrowserClientStartupArtifacts _startupArtifacts;
    private readonly IBrowserClientStartupArtifactReader _startupArtifactReader;
    private readonly IBrowserClientLaunchSession _launchSession;
    private readonly IBrowserClientLaunchSessionReader _launchSessionReader;
    private readonly IBrowserClientRuntimeLaunchContract _runtimeLaunchContract;
    private readonly IBrowserClientStartupPacket _startupPacket;
    private readonly IBrowserClientStartupConsumer _startupConsumer;
    private readonly IBrowserClientStartupSessionExecutor _startupSessionExecutor;
    private readonly IBrowserClientStartupSessionRunner _startupSessionRunner;
    private readonly IBrowserClientRuntimeBootstrapLoop _runtimeBootstrapLoop;
    private readonly IBrowserClientRuntimeInvocation _runtimeInvocation;
    private readonly IBrowserClientRuntimeStartupCycle _runtimeStartupCycle;
    private readonly IBrowserClientRuntimeStartupState _runtimeStartupState;
    private readonly IBrowserClientRuntimeStartupStateMachine _runtimeStartupStateMachine;
    private readonly IBrowserClientRuntimeStartupTransitionDriver _runtimeStartupTransitionDriver;
    private readonly IBrowserClientRuntimeStartupDispatcher _runtimeStartupDispatcher;
    private readonly IBrowserClientRuntimeStartupSessionController _runtimeStartupSessionController;
    private readonly IBrowserClientRuntimeStartupCoordinator _runtimeStartupCoordinator;
    private readonly IBrowserClientRuntimeStartupOrchestrator _runtimeStartupOrchestrator;
    private readonly IBrowserClientBootFlowController _browserClientBootFlowController;
    private readonly IBrowserClientBootSession _browserClientBootSession;
    private readonly IBrowserClientRuntimeLaunchHandoff _runtimeLaunchHandoff;
    private readonly IBrowserClientRuntimeBootstrapConsumer _runtimeBootstrapConsumer;
    private readonly IBrowserClientRuntimeBootstrapSession _runtimeBootstrapSession;
    private readonly IBrowserClientRuntimeClientBootstrapController _runtimeClientBootstrapController;
    private readonly IBrowserClientRuntimeStartupReadinessGate _runtimeStartupReadinessGate;
    private readonly IBrowserClientRuntimeReadySignal _runtimeReadySignal;
    private readonly IBrowserClientRuntimeLaunchController _runtimeLaunchController;
    private readonly IBrowserClientRuntimeClientReadyState _runtimeClientReadyState;
    private readonly IBrowserClientRuntimeClientLaunchSession _runtimeClientLaunchSession;
    private readonly IBrowserClientRuntimeClientActivation _runtimeClientActivation;
    private readonly IBrowserClientRuntimeClientRunState _runtimeClientRunState;
    private readonly IBrowserClientRuntimeClientLoopState _runtimeClientLoopState;
    private readonly IBrowserClientRuntimeHostSession _runtimeHostSession;
    private readonly IBrowserClientRuntimeHostLoop _runtimeHostLoop;
    private readonly IBrowserClientRuntimeHostReadyState _runtimeHostReadyState;
    private readonly IBrowserClientRuntimePlatformSession _runtimePlatformSession;
    private readonly IBrowserClientRuntimePlatformReadyState _runtimePlatformReadyState;
    private readonly IBrowserClientRuntimePlatformLoop _runtimePlatformLoop;
    private readonly IBrowserClientRuntimePlatformLaunchGate _runtimePlatformLaunchGate;
    private readonly IBrowserClientRuntimeBrowserShellSession _runtimeBrowserShellSession;
    private readonly IBrowserClientRuntimeBrowserShellReadyState _runtimeBrowserShellReadyState;
    private readonly IBrowserClientRuntimeBrowserSurfaceSession _runtimeBrowserSurfaceSession;
    private readonly IBrowserClientRuntimeBrowserSurfaceReadyState _runtimeBrowserSurfaceReadyState;
    private readonly IBrowserClientRuntimeBrowserWindowSession _runtimeBrowserWindowSession;
    private readonly IBrowserClientRuntimeBrowserWindowReadyState _runtimeBrowserWindowReadyState;
    private readonly IBrowserClientRuntimeBrowserFrameSession _runtimeBrowserFrameSession;
    private readonly IBrowserClientRuntimeBrowserFrameReadyState _runtimeBrowserFrameReadyState;
    private readonly IBrowserClientRuntimeBrowserCanvasSession _runtimeBrowserCanvasSession;
    private readonly IBrowserClientRuntimeBrowserCanvasReadyState _runtimeBrowserCanvasReadyState;
    private readonly IBrowserClientRuntimeBrowserRenderSession _runtimeBrowserRenderSession;
    private readonly IBrowserClientRuntimeBrowserRenderReadyState _runtimeBrowserRenderReadyState;
    private readonly IBrowserClientRuntimeBrowserPresentSession _runtimeBrowserPresentSession;
    private readonly IBrowserClientRuntimeBrowserPresentReadyState _runtimeBrowserPresentReadyState;
    private readonly IBrowserClientRuntimeBrowserDisplaySession _runtimeBrowserDisplaySession;
    private readonly IBrowserClientRuntimeBrowserDisplayReadyState _runtimeBrowserDisplayReadyState;
    private readonly IBrowserClientRuntimeBrowserViewportSession _runtimeBrowserViewportSession;
    private readonly IBrowserClientRuntimeBrowserViewportReadyState _runtimeBrowserViewportReadyState;
    private readonly IBrowserClientRuntimeBrowserSceneSession _runtimeBrowserSceneSession;
    private readonly IBrowserClientRuntimeBrowserSceneReadyState _runtimeBrowserSceneReadyState;
    private readonly IBrowserClientRuntimeBrowserInputSession _runtimeBrowserInputSession;
    private readonly IBrowserClientRuntimeBrowserInputReadyState _runtimeBrowserInputReadyState;
    private readonly IBrowserClientRuntimeBrowserEventSession _runtimeBrowserEventSession;
    private readonly IBrowserClientRuntimeBrowserEventReadyState _runtimeBrowserEventReadyState;
    private readonly IBrowserClientRuntimeBrowserInteractionSession _runtimeBrowserInteractionSession;
    private readonly IBrowserClientRuntimeBrowserInteractionReadyState _runtimeBrowserInteractionReadyState;
    private readonly IBrowserClientRuntimeBrowserFocusSession _runtimeBrowserFocusSession;
    private readonly IBrowserClientRuntimeBrowserFocusReadyState _runtimeBrowserFocusReadyState;
    private readonly IBrowserClientRuntimeBrowserShortcutSession _runtimeBrowserShortcutSession;
    private readonly IBrowserClientRuntimeBrowserShortcutReadyState _runtimeBrowserShortcutReadyState;
    private readonly IBrowserClientRuntimeBrowserPointerSession _runtimeBrowserPointerSession;
    private readonly IBrowserClientRuntimeBrowserPointerReadyState _runtimeBrowserPointerReadyState;
    private readonly IBrowserClientRuntimeBrowserCommandSession _runtimeBrowserCommandSession;
    private readonly IBrowserClientRuntimeBrowserCommandReadyState _runtimeBrowserCommandReadyState;
    private readonly IBrowserClientRuntimeBrowserGestureSession _runtimeBrowserGestureSession;
    private readonly IBrowserClientRuntimeBrowserGestureReadyState _runtimeBrowserGestureReadyState;
    private readonly IBrowserClientRuntimeBrowserLifecycleSession _runtimeBrowserLifecycleSession;
    private readonly IBrowserClientRuntimeBrowserLifecycleReadyState _runtimeBrowserLifecycleReadyState;
    private readonly IBrowserClientRuntimeBrowserRouteSession _runtimeBrowserRouteSession;
    private readonly IBrowserClientRuntimeBrowserRouteReadyState _runtimeBrowserRouteReadyState;
    private readonly IBrowserClientRuntimeBrowserStateSyncSession _runtimeBrowserStateSyncSession;
    private readonly IBrowserClientRuntimeBrowserStateSyncReadyState _runtimeBrowserStateSyncReadyState;
    private readonly IBrowserClientRuntimeBrowserRestoreSession _runtimeBrowserRestoreSession;
    private readonly IBrowserClientRuntimeBrowserRestoreReadyState _runtimeBrowserRestoreReadyState;
    private readonly IBrowserClientRuntimeBrowserResumeSession _runtimeBrowserResumeSession;
    private readonly IBrowserClientRuntimeBrowserResumeReadyState _runtimeBrowserResumeReadyState;

    public BrowserSelfTestReportService(
        IBrowserClientAssetService browserClientAssetService,
        BrowserFileSystemBridgeService bridgeService,
        BrowserStorageService storageService,
        IBrowserClientStartupArtifacts startupArtifacts,
        IBrowserClientStartupArtifactReader startupArtifactReader,
        IBrowserClientLaunchSession launchSession,
        IBrowserClientLaunchSessionReader launchSessionReader,
        IBrowserClientRuntimeLaunchContract runtimeLaunchContract,
        IBrowserClientStartupPacket startupPacket,
        IBrowserClientStartupConsumer startupConsumer,
        IBrowserClientStartupSessionExecutor startupSessionExecutor,
        IBrowserClientStartupSessionRunner startupSessionRunner,
        IBrowserClientRuntimeBootstrapLoop runtimeBootstrapLoop,
        IBrowserClientRuntimeInvocation runtimeInvocation,
        IBrowserClientRuntimeStartupCycle runtimeStartupCycle,
        IBrowserClientRuntimeStartupState runtimeStartupState,
        IBrowserClientRuntimeStartupStateMachine runtimeStartupStateMachine,
        IBrowserClientRuntimeStartupTransitionDriver runtimeStartupTransitionDriver,
        IBrowserClientRuntimeStartupDispatcher runtimeStartupDispatcher,
        IBrowserClientRuntimeStartupSessionController runtimeStartupSessionController,
        IBrowserClientRuntimeStartupCoordinator runtimeStartupCoordinator,
        IBrowserClientRuntimeStartupOrchestrator runtimeStartupOrchestrator,
        IBrowserClientBootFlowController browserClientBootFlowController,
        IBrowserClientBootSession browserClientBootSession,
        IBrowserClientRuntimeLaunchHandoff runtimeLaunchHandoff,
        IBrowserClientRuntimeBootstrapConsumer runtimeBootstrapConsumer,
        IBrowserClientRuntimeBootstrapSession runtimeBootstrapSession,
        IBrowserClientRuntimeClientBootstrapController runtimeClientBootstrapController,
        IBrowserClientRuntimeStartupReadinessGate runtimeStartupReadinessGate,
        IBrowserClientRuntimeReadySignal runtimeReadySignal,
        IBrowserClientRuntimeLaunchController runtimeLaunchController,
        IBrowserClientRuntimeClientReadyState runtimeClientReadyState,
        IBrowserClientRuntimeClientLaunchSession runtimeClientLaunchSession,
        IBrowserClientRuntimeClientActivation runtimeClientActivation,
        IBrowserClientRuntimeClientRunState runtimeClientRunState,
        IBrowserClientRuntimeClientLoopState runtimeClientLoopState,
        IBrowserClientRuntimeHostSession runtimeHostSession,
        IBrowserClientRuntimeHostLoop runtimeHostLoop,
        IBrowserClientRuntimeHostReadyState runtimeHostReadyState,
        IBrowserClientRuntimePlatformSession runtimePlatformSession,
        IBrowserClientRuntimePlatformReadyState runtimePlatformReadyState,
        IBrowserClientRuntimePlatformLoop runtimePlatformLoop,
        IBrowserClientRuntimePlatformLaunchGate runtimePlatformLaunchGate,
        IBrowserClientRuntimeBrowserShellSession runtimeBrowserShellSession,
        IBrowserClientRuntimeBrowserShellReadyState runtimeBrowserShellReadyState,
        IBrowserClientRuntimeBrowserSurfaceSession runtimeBrowserSurfaceSession,
        IBrowserClientRuntimeBrowserSurfaceReadyState runtimeBrowserSurfaceReadyState,
        IBrowserClientRuntimeBrowserWindowSession runtimeBrowserWindowSession,
        IBrowserClientRuntimeBrowserWindowReadyState runtimeBrowserWindowReadyState,
        IBrowserClientRuntimeBrowserFrameSession runtimeBrowserFrameSession,
        IBrowserClientRuntimeBrowserFrameReadyState runtimeBrowserFrameReadyState,
        IBrowserClientRuntimeBrowserCanvasSession runtimeBrowserCanvasSession,
        IBrowserClientRuntimeBrowserCanvasReadyState runtimeBrowserCanvasReadyState,
        IBrowserClientRuntimeBrowserRenderSession runtimeBrowserRenderSession,
        IBrowserClientRuntimeBrowserRenderReadyState runtimeBrowserRenderReadyState,
        IBrowserClientRuntimeBrowserPresentSession runtimeBrowserPresentSession,
        IBrowserClientRuntimeBrowserPresentReadyState runtimeBrowserPresentReadyState,
        IBrowserClientRuntimeBrowserDisplaySession runtimeBrowserDisplaySession,
        IBrowserClientRuntimeBrowserDisplayReadyState runtimeBrowserDisplayReadyState,
        IBrowserClientRuntimeBrowserViewportSession runtimeBrowserViewportSession,
        IBrowserClientRuntimeBrowserViewportReadyState runtimeBrowserViewportReadyState,
        IBrowserClientRuntimeBrowserSceneSession runtimeBrowserSceneSession,
        IBrowserClientRuntimeBrowserSceneReadyState runtimeBrowserSceneReadyState,
        IBrowserClientRuntimeBrowserInputSession runtimeBrowserInputSession,
        IBrowserClientRuntimeBrowserInputReadyState runtimeBrowserInputReadyState,
        IBrowserClientRuntimeBrowserEventSession runtimeBrowserEventSession,
        IBrowserClientRuntimeBrowserEventReadyState runtimeBrowserEventReadyState,
        IBrowserClientRuntimeBrowserInteractionSession runtimeBrowserInteractionSession,
        IBrowserClientRuntimeBrowserInteractionReadyState runtimeBrowserInteractionReadyState,
        IBrowserClientRuntimeBrowserFocusSession runtimeBrowserFocusSession,
        IBrowserClientRuntimeBrowserFocusReadyState runtimeBrowserFocusReadyState,
        IBrowserClientRuntimeBrowserShortcutSession runtimeBrowserShortcutSession,
        IBrowserClientRuntimeBrowserShortcutReadyState runtimeBrowserShortcutReadyState,
        IBrowserClientRuntimeBrowserPointerSession runtimeBrowserPointerSession,
        IBrowserClientRuntimeBrowserPointerReadyState runtimeBrowserPointerReadyState,
        IBrowserClientRuntimeBrowserCommandSession runtimeBrowserCommandSession,
        IBrowserClientRuntimeBrowserCommandReadyState runtimeBrowserCommandReadyState,
        IBrowserClientRuntimeBrowserGestureSession runtimeBrowserGestureSession,
        IBrowserClientRuntimeBrowserGestureReadyState runtimeBrowserGestureReadyState,
        IBrowserClientRuntimeBrowserLifecycleSession runtimeBrowserLifecycleSession,
        IBrowserClientRuntimeBrowserLifecycleReadyState runtimeBrowserLifecycleReadyState,
        IBrowserClientRuntimeBrowserRouteSession runtimeBrowserRouteSession,
        IBrowserClientRuntimeBrowserRouteReadyState runtimeBrowserRouteReadyState,
        IBrowserClientRuntimeBrowserStateSyncSession runtimeBrowserStateSyncSession,
        IBrowserClientRuntimeBrowserStateSyncReadyState runtimeBrowserStateSyncReadyState,
        IBrowserClientRuntimeBrowserRestoreSession runtimeBrowserRestoreSession,
        IBrowserClientRuntimeBrowserRestoreReadyState runtimeBrowserRestoreReadyState,
        IBrowserClientRuntimeBrowserResumeSession runtimeBrowserResumeSession,
        IBrowserClientRuntimeBrowserResumeReadyState runtimeBrowserResumeReadyState
    )
    {
        _browserClientAssetService = browserClientAssetService;
        _bridgeService = bridgeService;
        _storageService = storageService;
        _startupArtifacts = startupArtifacts;
        _startupArtifactReader = startupArtifactReader;
        _launchSession = launchSession;
        _launchSessionReader = launchSessionReader;
        _runtimeLaunchContract = runtimeLaunchContract;
        _startupPacket = startupPacket;
        _startupConsumer = startupConsumer;
        _startupSessionExecutor = startupSessionExecutor;
        _startupSessionRunner = startupSessionRunner;
        _runtimeBootstrapLoop = runtimeBootstrapLoop;
        _runtimeInvocation = runtimeInvocation;
        _runtimeStartupCycle = runtimeStartupCycle;
        _runtimeStartupState = runtimeStartupState;
        _runtimeStartupStateMachine = runtimeStartupStateMachine;
        _runtimeStartupTransitionDriver = runtimeStartupTransitionDriver;
        _runtimeStartupDispatcher = runtimeStartupDispatcher;
        _runtimeStartupSessionController = runtimeStartupSessionController;
        _runtimeStartupCoordinator = runtimeStartupCoordinator;
        _runtimeStartupOrchestrator = runtimeStartupOrchestrator;
        _browserClientBootFlowController = browserClientBootFlowController;
        _browserClientBootSession = browserClientBootSession;
        _runtimeLaunchHandoff = runtimeLaunchHandoff;
        _runtimeBootstrapConsumer = runtimeBootstrapConsumer;
        _runtimeBootstrapSession = runtimeBootstrapSession;
        _runtimeClientBootstrapController = runtimeClientBootstrapController;
        _runtimeStartupReadinessGate = runtimeStartupReadinessGate;
        _runtimeReadySignal = runtimeReadySignal;
        _runtimeLaunchController = runtimeLaunchController;
        _runtimeClientReadyState = runtimeClientReadyState;
        _runtimeClientLaunchSession = runtimeClientLaunchSession;
        _runtimeClientActivation = runtimeClientActivation;
        _runtimeClientRunState = runtimeClientRunState;
        _runtimeClientLoopState = runtimeClientLoopState;
        _runtimeHostSession = runtimeHostSession;
        _runtimeHostLoop = runtimeHostLoop;
        _runtimeHostReadyState = runtimeHostReadyState;
        _runtimePlatformSession = runtimePlatformSession;
        _runtimePlatformReadyState = runtimePlatformReadyState;
        _runtimePlatformLoop = runtimePlatformLoop;
        _runtimePlatformLaunchGate = runtimePlatformLaunchGate;
        _runtimeBrowserShellSession = runtimeBrowserShellSession;
        _runtimeBrowserShellReadyState = runtimeBrowserShellReadyState;
        _runtimeBrowserSurfaceSession = runtimeBrowserSurfaceSession;
        _runtimeBrowserSurfaceReadyState = runtimeBrowserSurfaceReadyState;
        _runtimeBrowserWindowSession = runtimeBrowserWindowSession;
        _runtimeBrowserWindowReadyState = runtimeBrowserWindowReadyState;
        _runtimeBrowserFrameSession = runtimeBrowserFrameSession;
        _runtimeBrowserFrameReadyState = runtimeBrowserFrameReadyState;
        _runtimeBrowserCanvasSession = runtimeBrowserCanvasSession;
        _runtimeBrowserCanvasReadyState = runtimeBrowserCanvasReadyState;
        _runtimeBrowserRenderSession = runtimeBrowserRenderSession;
        _runtimeBrowserRenderReadyState = runtimeBrowserRenderReadyState;
        _runtimeBrowserPresentSession = runtimeBrowserPresentSession;
        _runtimeBrowserPresentReadyState = runtimeBrowserPresentReadyState;
        _runtimeBrowserDisplaySession = runtimeBrowserDisplaySession;
        _runtimeBrowserDisplayReadyState = runtimeBrowserDisplayReadyState;
        _runtimeBrowserViewportSession = runtimeBrowserViewportSession;
        _runtimeBrowserViewportReadyState = runtimeBrowserViewportReadyState;
        _runtimeBrowserSceneSession = runtimeBrowserSceneSession;
        _runtimeBrowserSceneReadyState = runtimeBrowserSceneReadyState;
        _runtimeBrowserInputSession = runtimeBrowserInputSession;
        _runtimeBrowserInputReadyState = runtimeBrowserInputReadyState;
        _runtimeBrowserEventSession = runtimeBrowserEventSession;
        _runtimeBrowserEventReadyState = runtimeBrowserEventReadyState;
        _runtimeBrowserInteractionSession = runtimeBrowserInteractionSession;
        _runtimeBrowserInteractionReadyState = runtimeBrowserInteractionReadyState;
        _runtimeBrowserFocusSession = runtimeBrowserFocusSession;
        _runtimeBrowserFocusReadyState = runtimeBrowserFocusReadyState;
        _runtimeBrowserShortcutSession = runtimeBrowserShortcutSession;
        _runtimeBrowserShortcutReadyState = runtimeBrowserShortcutReadyState;
        _runtimeBrowserPointerSession = runtimeBrowserPointerSession;
        _runtimeBrowserPointerReadyState = runtimeBrowserPointerReadyState;
        _runtimeBrowserCommandSession = runtimeBrowserCommandSession;
        _runtimeBrowserCommandReadyState = runtimeBrowserCommandReadyState;
        _runtimeBrowserGestureSession = runtimeBrowserGestureSession;
        _runtimeBrowserGestureReadyState = runtimeBrowserGestureReadyState;
        _runtimeBrowserLifecycleSession = runtimeBrowserLifecycleSession;
        _runtimeBrowserLifecycleReadyState = runtimeBrowserLifecycleReadyState;
        _runtimeBrowserRouteSession = runtimeBrowserRouteSession;
        _runtimeBrowserRouteReadyState = runtimeBrowserRouteReadyState;
        _runtimeBrowserStateSyncSession = runtimeBrowserStateSyncSession;
        _runtimeBrowserStateSyncReadyState = runtimeBrowserStateSyncReadyState;
        _runtimeBrowserRestoreSession = runtimeBrowserRestoreSession;
        _runtimeBrowserRestoreReadyState = runtimeBrowserRestoreReadyState;
        _runtimeBrowserResumeSession = runtimeBrowserResumeSession;
        _runtimeBrowserResumeReadyState = runtimeBrowserResumeReadyState;
    }

    public async ValueTask<BrowserSelfTestReport> RunAsync(BrowserRuntimeBootstrapRequest? request = null)
    {
        BrowserRuntimeBootstrapRequest effectiveRequest = request ?? new BrowserRuntimeBootstrapRequest();
        DateTimeOffset started = DateTimeOffset.UtcNow;

        BrowserAssetManifestResult initialManifest = await _storageService.GetAssetManifestAsync();
        BrowserAssetImportResult? importResult = null;

        if (!initialManifest.Bootstrap.IsReady)
        {
            importResult = await _storageService.ImportHostedRecommendedSeedAsync();
        }

        await _bridgeService.ActivateBootstrapAssetsAsync(effectiveRequest);

        BrowserRuntimeBootstrapState directBootstrap = await _browserClientAssetService.GetBootstrapStateAsync(effectiveRequest);
        BrowserSharedSeamRuntimeBootstrapState seamBootstrap = await _browserClientAssetService.GetSharedSeamBootstrapStateAsync(effectiveRequest);
        BrowserClientBootstrapHandoff handoff = await _browserClientAssetService.GetBrowserBootstrapHandoffAsync(effectiveRequest);
        BrowserRuntimeCacheState cacheState = await _browserClientAssetService.GetCacheStateAsync();
        BrowserFileSystemBridgeState bridgeState = await _bridgeService.GetStateAsync();
        BrowserClientStartupArtifact startupArtifact = await _startupArtifacts.CreateStartupArtifactAsync(effectiveRequest);
        BrowserClientStartupArtifactRead startupArtifactRead = await _startupArtifactReader.ReadStartupArtifactAsync();
        BrowserClientLaunchSession launchSession = await _launchSession.CreateLaunchSessionAsync(effectiveRequest);
        BrowserClientLaunchSessionRead launchSessionRead = await _launchSessionReader.ReadLaunchSessionAsync();
        BrowserClientRuntimeLaunchContractResult runtimeLaunchContract = await _runtimeLaunchContract.BuildAsync();
        BrowserClientStartupPacketResult startupPacket = await _startupPacket.BuildAsync();
        BrowserClientStartupConsumerResult startupConsumer = await _startupConsumer.ConsumeAsync();
        BrowserClientStartupSessionExecutorResult startupSessionExecutor = await _startupSessionExecutor.ExecuteAsync();
        BrowserClientStartupSessionRunnerResult startupSessionRunner = await _startupSessionRunner.RunAsync();
        BrowserClientRuntimeBootstrapLoopResult runtimeBootstrapLoop = await _runtimeBootstrapLoop.RunAsync();
        BrowserClientRuntimeInvocationResult runtimeInvocation = await _runtimeInvocation.InvokeAsync();
        BrowserClientRuntimeStartupCycleResult runtimeStartupCycle = await _runtimeStartupCycle.BuildAsync();
        BrowserClientRuntimeStartupStateResult runtimeStartupState = await _runtimeStartupState.BuildAsync();
        BrowserClientRuntimeStartupStateMachineResult runtimeStartupStateMachine = await _runtimeStartupStateMachine.BuildAsync();
        BrowserClientRuntimeStartupTransitionDriverResult runtimeStartupTransitionDriver = await _runtimeStartupTransitionDriver.DriveAsync();
        BrowserClientRuntimeStartupDispatcherResult runtimeStartupDispatcher = await _runtimeStartupDispatcher.DispatchAsync();
        BrowserClientRuntimeStartupSessionControllerResult runtimeStartupSessionController = await _runtimeStartupSessionController.ControlAsync();
        BrowserClientRuntimeStartupCoordinatorResult runtimeStartupCoordinator = await _runtimeStartupCoordinator.CoordinateAsync();
        BrowserClientRuntimeStartupOrchestratorResult runtimeStartupOrchestrator = await _runtimeStartupOrchestrator.OrchestrateAsync();
        BrowserClientBootFlowControllerResult browserBootFlowController = await _browserClientBootFlowController.ControlAsync();
        BrowserClientBootSessionResult browserBootSession = await _browserClientBootSession.CreateAsync();
        BrowserClientRuntimeLaunchHandoffResult runtimeLaunchHandoff = await _runtimeLaunchHandoff.BuildAsync();
        BrowserClientRuntimeBootstrapConsumerResult runtimeBootstrapConsumer = await _runtimeBootstrapConsumer.ConsumeAsync();
        BrowserClientRuntimeBootstrapSessionResult runtimeBootstrapSession = await _runtimeBootstrapSession.CreateAsync();
        BrowserClientRuntimeClientBootstrapControllerResult runtimeClientBootstrapController = await _runtimeClientBootstrapController.ControlAsync();
        BrowserClientRuntimeStartupReadinessGateResult runtimeStartupReadinessGate = await _runtimeStartupReadinessGate.EvaluateAsync();
        BrowserClientRuntimeReadySignalResult runtimeReadySignal = await _runtimeReadySignal.SignalAsync();
        BrowserClientRuntimeLaunchControllerResult runtimeLaunchController = await _runtimeLaunchController.ControlAsync();
        BrowserClientRuntimeClientReadyStateResult runtimeClientReadyState = await _runtimeClientReadyState.BuildAsync();
        BrowserClientRuntimeClientLaunchSessionResult runtimeClientLaunchSession = await _runtimeClientLaunchSession.CreateAsync();
        BrowserClientRuntimeClientActivationResult runtimeClientActivation = await _runtimeClientActivation.ActivateAsync();
        BrowserClientRuntimeClientRunStateResult runtimeClientRunState = await _runtimeClientRunState.BuildAsync();
        BrowserClientRuntimeClientLoopStateResult runtimeClientLoopState = await _runtimeClientLoopState.BuildAsync();
        BrowserClientRuntimeHostSessionResult runtimeHostSession = await _runtimeHostSession.CreateAsync();
        BrowserClientRuntimeHostLoopResult runtimeHostLoop = await _runtimeHostLoop.RunAsync();
        BrowserClientRuntimeHostReadyStateResult runtimeHostReadyState = await _runtimeHostReadyState.BuildAsync();
        BrowserClientRuntimePlatformSessionResult runtimePlatformSession = await _runtimePlatformSession.CreateAsync();
        BrowserClientRuntimePlatformReadyStateResult runtimePlatformReadyState = await _runtimePlatformReadyState.BuildAsync();
        BrowserClientRuntimePlatformLoopResult runtimePlatformLoop = await _runtimePlatformLoop.RunAsync();
        BrowserClientRuntimePlatformLaunchGateResult runtimePlatformLaunchGate = await _runtimePlatformLaunchGate.EvaluateAsync();
        BrowserClientRuntimeBrowserShellSessionResult runtimeBrowserShellSession = await _runtimeBrowserShellSession.CreateAsync();
        BrowserClientRuntimeBrowserShellReadyStateResult runtimeBrowserShellReadyState = await _runtimeBrowserShellReadyState.BuildAsync();
        BrowserClientRuntimeBrowserSurfaceSessionResult runtimeBrowserSurfaceSession = await _runtimeBrowserSurfaceSession.CreateAsync();
        BrowserClientRuntimeBrowserSurfaceReadyStateResult runtimeBrowserSurfaceReadyState = await _runtimeBrowserSurfaceReadyState.BuildAsync();
        BrowserClientRuntimeBrowserWindowSessionResult runtimeBrowserWindowSession = await _runtimeBrowserWindowSession.CreateAsync();
        BrowserClientRuntimeBrowserWindowReadyStateResult runtimeBrowserWindowReadyState = await _runtimeBrowserWindowReadyState.BuildAsync();
        BrowserClientRuntimeBrowserFrameSessionResult runtimeBrowserFrameSession = await _runtimeBrowserFrameSession.CreateAsync();
        BrowserClientRuntimeBrowserFrameReadyStateResult runtimeBrowserFrameReadyState = await _runtimeBrowserFrameReadyState.BuildAsync();
        BrowserClientRuntimeBrowserCanvasSessionResult runtimeBrowserCanvasSession = await _runtimeBrowserCanvasSession.CreateAsync();
        BrowserClientRuntimeBrowserCanvasReadyStateResult runtimeBrowserCanvasReadyState = await _runtimeBrowserCanvasReadyState.BuildAsync();
        BrowserClientRuntimeBrowserRenderSessionResult runtimeBrowserRenderSession = await _runtimeBrowserRenderSession.CreateAsync();
        BrowserClientRuntimeBrowserRenderReadyStateResult runtimeBrowserRenderReadyState = await _runtimeBrowserRenderReadyState.BuildAsync();
        BrowserClientRuntimeBrowserPresentSessionResult runtimeBrowserPresentSession = await _runtimeBrowserPresentSession.CreateAsync();
        BrowserClientRuntimeBrowserPresentReadyStateResult runtimeBrowserPresentReadyState = await _runtimeBrowserPresentReadyState.BuildAsync();
        BrowserClientRuntimeBrowserDisplaySessionResult runtimeBrowserDisplaySession = await _runtimeBrowserDisplaySession.CreateAsync();
        BrowserClientRuntimeBrowserDisplayReadyStateResult runtimeBrowserDisplayReadyState = await _runtimeBrowserDisplayReadyState.BuildAsync();
        BrowserClientRuntimeBrowserViewportSessionResult runtimeBrowserViewportSession = await _runtimeBrowserViewportSession.CreateAsync();
        BrowserClientRuntimeBrowserViewportReadyStateResult runtimeBrowserViewportReadyState = await _runtimeBrowserViewportReadyState.BuildAsync();
        BrowserClientRuntimeBrowserSceneSessionResult runtimeBrowserSceneSession = await _runtimeBrowserSceneSession.CreateAsync();
        BrowserClientRuntimeBrowserSceneReadyStateResult runtimeBrowserSceneReadyState = await _runtimeBrowserSceneReadyState.BuildAsync();
        BrowserClientRuntimeBrowserInputSessionResult runtimeBrowserInputSession = await _runtimeBrowserInputSession.CreateAsync();
        BrowserClientRuntimeBrowserInputReadyStateResult runtimeBrowserInputReadyState = await _runtimeBrowserInputReadyState.BuildAsync();
        BrowserClientRuntimeBrowserEventSessionResult runtimeBrowserEventSession = await _runtimeBrowserEventSession.CreateAsync();
        BrowserClientRuntimeBrowserEventReadyStateResult runtimeBrowserEventReadyState = await _runtimeBrowserEventReadyState.BuildAsync();
        BrowserClientRuntimeBrowserInteractionSessionResult runtimeBrowserInteractionSession = await _runtimeBrowserInteractionSession.CreateAsync();
        BrowserClientRuntimeBrowserInteractionReadyStateResult runtimeBrowserInteractionReadyState = await _runtimeBrowserInteractionReadyState.BuildAsync();
        BrowserClientRuntimeBrowserFocusSessionResult runtimeBrowserFocusSession = await _runtimeBrowserFocusSession.CreateAsync();
        BrowserClientRuntimeBrowserFocusReadyStateResult runtimeBrowserFocusReadyState = await _runtimeBrowserFocusReadyState.BuildAsync();
        BrowserClientRuntimeBrowserShortcutSessionResult runtimeBrowserShortcutSession = await _runtimeBrowserShortcutSession.CreateAsync();
        BrowserClientRuntimeBrowserShortcutReadyStateResult runtimeBrowserShortcutReadyState = await _runtimeBrowserShortcutReadyState.BuildAsync();
        BrowserClientRuntimeBrowserPointerSessionResult runtimeBrowserPointerSession = await _runtimeBrowserPointerSession.CreateAsync();
        BrowserClientRuntimeBrowserPointerReadyStateResult runtimeBrowserPointerReadyState = await _runtimeBrowserPointerReadyState.BuildAsync();
        BrowserClientRuntimeBrowserCommandSessionResult runtimeBrowserCommandSession = await _runtimeBrowserCommandSession.CreateAsync();
        BrowserClientRuntimeBrowserCommandReadyStateResult runtimeBrowserCommandReadyState = await _runtimeBrowserCommandReadyState.BuildAsync();
        BrowserClientRuntimeBrowserGestureSessionResult runtimeBrowserGestureSession = await _runtimeBrowserGestureSession.CreateAsync();
        BrowserClientRuntimeBrowserGestureReadyStateResult runtimeBrowserGestureReadyState = await _runtimeBrowserGestureReadyState.BuildAsync();
        BrowserClientRuntimeBrowserLifecycleSessionResult runtimeBrowserLifecycleSession = await _runtimeBrowserLifecycleSession.CreateAsync();
        BrowserClientRuntimeBrowserLifecycleReadyStateResult runtimeBrowserLifecycleReadyState = await _runtimeBrowserLifecycleReadyState.BuildAsync();
        BrowserClientRuntimeBrowserRouteSessionResult runtimeBrowserRouteSession = await _runtimeBrowserRouteSession.CreateAsync();
        BrowserClientRuntimeBrowserRouteReadyStateResult runtimeBrowserRouteReadyState = await _runtimeBrowserRouteReadyState.BuildAsync();
        BrowserClientRuntimeBrowserStateSyncSessionResult runtimeBrowserStateSyncSession = await _runtimeBrowserStateSyncSession.CreateAsync();
        BrowserClientRuntimeBrowserStateSyncReadyStateResult runtimeBrowserStateSyncReadyState = await _runtimeBrowserStateSyncReadyState.BuildAsync();
        BrowserClientRuntimeBrowserRestoreSessionResult runtimeBrowserRestoreSession = await _runtimeBrowserRestoreSession.CreateAsync();
        BrowserClientRuntimeBrowserRestoreReadyStateResult runtimeBrowserRestoreReadyState = await _runtimeBrowserRestoreReadyState.BuildAsync();
        BrowserClientRuntimeBrowserResumeSessionResult runtimeBrowserResumeSession = await _runtimeBrowserResumeSession.CreateAsync();
        BrowserClientRuntimeBrowserResumeReadyStateResult runtimeBrowserResumeReadyState = await _runtimeBrowserResumeReadyState.BuildAsync();

        BrowserSelfTestReport report = new()
        {
            GeneratedAtUtc = started,
            Request = new BrowserSelfTestRequest
            {
                TileDataPath = effectiveRequest.TileDataPath,
                ClilocPath = effectiveRequest.ClilocPath,
                HuesPath = effectiveRequest.HuesPath
            },
            SeedImportAttempted = importResult is not null,
            SeedImportCount = importResult?.ImportedCount ?? 0,
            DirectBootstrap = directBootstrap,
            SeamBootstrap = seamBootstrap,
            Handoff = handoff,
            CacheState = cacheState,
            BridgeState = bridgeState,
            StartupArtifact = startupArtifact,
            StartupArtifactRead = startupArtifactRead,
            LaunchSession = launchSession,
            LaunchSessionRead = launchSessionRead,
            RuntimeLaunchContract = runtimeLaunchContract,
            StartupPacket = startupPacket,
            StartupConsumer = startupConsumer,
            StartupSessionExecutor = startupSessionExecutor,
            StartupSessionRunner = startupSessionRunner,
            RuntimeBootstrapLoop = runtimeBootstrapLoop,
            RuntimeInvocation = runtimeInvocation,
            RuntimeStartupCycle = runtimeStartupCycle,
            RuntimeStartupState = runtimeStartupState,
            RuntimeStartupStateMachine = runtimeStartupStateMachine,
            RuntimeStartupTransitionDriver = runtimeStartupTransitionDriver,
            RuntimeStartupDispatcher = runtimeStartupDispatcher,
            RuntimeStartupSessionController = runtimeStartupSessionController,
            RuntimeStartupCoordinator = runtimeStartupCoordinator,
            RuntimeStartupOrchestrator = runtimeStartupOrchestrator,
            BrowserBootFlowController = browserBootFlowController,
            BrowserBootSession = browserBootSession,
            RuntimeLaunchHandoff = runtimeLaunchHandoff,
            RuntimeBootstrapConsumer = runtimeBootstrapConsumer,
            RuntimeBootstrapSession = runtimeBootstrapSession,
            RuntimeClientBootstrapController = runtimeClientBootstrapController,
            RuntimeStartupReadinessGate = runtimeStartupReadinessGate,
            RuntimeReadySignal = runtimeReadySignal,
            RuntimeLaunchController = runtimeLaunchController,
            RuntimeClientReadyState = runtimeClientReadyState,
            RuntimeClientLaunchSession = runtimeClientLaunchSession,
            RuntimeClientActivation = runtimeClientActivation,
            RuntimeClientRunState = runtimeClientRunState,
            RuntimeClientLoopState = runtimeClientLoopState,
            RuntimeHostSession = runtimeHostSession,
            RuntimeHostLoop = runtimeHostLoop,
            RuntimeHostReadyState = runtimeHostReadyState,
            RuntimePlatformSession = runtimePlatformSession,
            RuntimePlatformReadyState = runtimePlatformReadyState,
            RuntimePlatformLoop = runtimePlatformLoop,
            RuntimePlatformLaunchGate = runtimePlatformLaunchGate,
            RuntimeBrowserShellSession = runtimeBrowserShellSession,
            RuntimeBrowserShellReadyState = runtimeBrowserShellReadyState,
            RuntimeBrowserSurfaceSession = runtimeBrowserSurfaceSession,
            RuntimeBrowserSurfaceReadyState = runtimeBrowserSurfaceReadyState,
            RuntimeBrowserWindowSession = runtimeBrowserWindowSession,
            RuntimeBrowserWindowReadyState = runtimeBrowserWindowReadyState,
            RuntimeBrowserFrameSession = runtimeBrowserFrameSession,
            RuntimeBrowserFrameReadyState = runtimeBrowserFrameReadyState,
            RuntimeBrowserCanvasSession = runtimeBrowserCanvasSession,
            RuntimeBrowserCanvasReadyState = runtimeBrowserCanvasReadyState,
            RuntimeBrowserRenderSession = runtimeBrowserRenderSession,
            RuntimeBrowserRenderReadyState = runtimeBrowserRenderReadyState,
            RuntimeBrowserPresentSession = runtimeBrowserPresentSession,
            RuntimeBrowserPresentReadyState = runtimeBrowserPresentReadyState,
            RuntimeBrowserDisplaySession = runtimeBrowserDisplaySession,
            RuntimeBrowserDisplayReadyState = runtimeBrowserDisplayReadyState,
            RuntimeBrowserViewportSession = runtimeBrowserViewportSession,
            RuntimeBrowserViewportReadyState = runtimeBrowserViewportReadyState,
            RuntimeBrowserSceneSession = runtimeBrowserSceneSession,
            RuntimeBrowserSceneReadyState = runtimeBrowserSceneReadyState,
            RuntimeBrowserInputSession = runtimeBrowserInputSession,
            RuntimeBrowserInputReadyState = runtimeBrowserInputReadyState,
            RuntimeBrowserEventSession = runtimeBrowserEventSession,
            RuntimeBrowserEventReadyState = runtimeBrowserEventReadyState,
            RuntimeBrowserInteractionSession = runtimeBrowserInteractionSession,
            RuntimeBrowserInteractionReadyState = runtimeBrowserInteractionReadyState,
            RuntimeBrowserFocusSession = runtimeBrowserFocusSession,
            RuntimeBrowserFocusReadyState = runtimeBrowserFocusReadyState,
            RuntimeBrowserShortcutSession = runtimeBrowserShortcutSession,
            RuntimeBrowserShortcutReadyState = runtimeBrowserShortcutReadyState,
            RuntimeBrowserPointerSession = runtimeBrowserPointerSession,
            RuntimeBrowserPointerReadyState = runtimeBrowserPointerReadyState,
            RuntimeBrowserCommandSession = runtimeBrowserCommandSession,
            RuntimeBrowserCommandReadyState = runtimeBrowserCommandReadyState,
            RuntimeBrowserGestureSession = runtimeBrowserGestureSession,
            RuntimeBrowserGestureReadyState = runtimeBrowserGestureReadyState,
            RuntimeBrowserLifecycleSession = runtimeBrowserLifecycleSession,
            RuntimeBrowserLifecycleReadyState = runtimeBrowserLifecycleReadyState,
            RuntimeBrowserRouteSession = runtimeBrowserRouteSession,
            RuntimeBrowserRouteReadyState = runtimeBrowserRouteReadyState,
            RuntimeBrowserStateSyncSession = runtimeBrowserStateSyncSession,
            RuntimeBrowserStateSyncReadyState = runtimeBrowserStateSyncReadyState,
            RuntimeBrowserRestoreSession = runtimeBrowserRestoreSession,
            RuntimeBrowserRestoreReadyState = runtimeBrowserRestoreReadyState,
            RuntimeBrowserResumeSession = runtimeBrowserResumeSession,
            RuntimeBrowserResumeReadyState = runtimeBrowserResumeReadyState
        };

        report.Summary = BuildSummary(report);
        report.Json = JsonSerializer.Serialize(report, JsonOptions);
        return report;
    }

    private static string BuildSummary(BrowserSelfTestReport report)
    {
        return string.Join(
            " | ",
            $"handoff={(report.Handoff.IsReady ? "ready" : "not-ready")}",
            $"direct={report.DirectBootstrap.LoadedCount}/3",
            $"seam={report.SeamBootstrap.LoadedCount}/3",
            $"handoffCacheHits={report.Handoff.CacheHits}",
            $"rawCache={report.CacheState.RawAssetCache.EntryCount}",
            $"processedCache={report.CacheState.ProcessedAssetCache.EntryCount}",
            $"bridgeLoaded={report.BridgeState.LoadedCount}",
            $"seedImport={(report.SeedImportAttempted ? report.SeedImportCount.ToString() : "skip")}",
            $"artifactWrite={(report.StartupArtifact.WriteSucceeded ? "ok" : "fail")}",
            $"artifactRead={(report.StartupArtifactRead.ReadSucceeded ? "ok" : "fail")}",
            $"launchSession={(report.LaunchSession.WriteSucceeded ? "ok" : "fail")}",
            $"launchSessionRead={(report.LaunchSessionRead.ReadSucceeded ? "ok" : "fail")}",
            $"runtimeContract={(report.RuntimeLaunchContract.IsReady ? "ok" : "fail")}",
            $"startupPacket={(report.StartupPacket.IsReady ? "ok" : "fail")}",
            $"startupConsumer={(report.StartupConsumer.IsReady ? "ok" : "fail")}",
            $"startupExecutor={(report.StartupSessionExecutor.IsReady ? "ok" : "fail")}",
            $"startupRunner={(report.StartupSessionRunner.IsReady ? "ok" : "fail")}",
            $"runtimeLoop={(report.RuntimeBootstrapLoop.IsReady ? "ok" : "fail")}",
            $"runtimeInvoke={(report.RuntimeInvocation.IsReady ? "ok" : "fail")}",
            $"runtimeCycle={(report.RuntimeStartupCycle.IsReady ? "ok" : "fail")}",
            $"runtimeState={(report.RuntimeStartupState.IsReady ? "ok" : "fail")}",
            $"runtimeStateMachine={(report.RuntimeStartupStateMachine.IsReady ? "ok" : "fail")}",
            $"runtimeTransitionDriver={(report.RuntimeStartupTransitionDriver.IsReady ? "ok" : "fail")}",
            $"runtimeDispatcher={(report.RuntimeStartupDispatcher.IsReady ? "ok" : "fail")}",
            $"runtimeSessionController={(report.RuntimeStartupSessionController.IsReady ? "ok" : "fail")}",
            $"runtimeCoordinator={(report.RuntimeStartupCoordinator.IsReady ? "ok" : "fail")}",
            $"runtimeOrchestrator={(report.RuntimeStartupOrchestrator.IsReady ? "ok" : "fail")}",
            $"bootFlow={(report.BrowserBootFlowController.IsReady ? "ok" : "fail")}",
            $"bootSession={(report.BrowserBootSession.IsReady ? "ok" : "fail")}",
            $"launchHandoff={(report.RuntimeLaunchHandoff.IsReady ? "ok" : "fail")}",
            $"bootstrapConsumer={(report.RuntimeBootstrapConsumer.IsReady ? "ok" : "fail")}",
            $"bootstrapSession={(report.RuntimeBootstrapSession.IsReady ? "ok" : "fail")}",
            $"clientBootstrap={(report.RuntimeClientBootstrapController.IsReady ? "ok" : "fail")}",
            $"readinessGate={(report.RuntimeStartupReadinessGate.IsReady ? "ok" : "fail")}",
            $"readySignal={(report.RuntimeReadySignal.IsReady ? "ok" : "fail")}",
            $"launchController={(report.RuntimeLaunchController.IsReady ? "ok" : "fail")}",
            $"clientReadyState={(report.RuntimeClientReadyState.IsReady ? "ok" : "fail")}",
            $"clientLaunchSession={(report.RuntimeClientLaunchSession.IsReady ? "ok" : "fail")}",
            $"clientActivation={(report.RuntimeClientActivation.IsReady ? "ok" : "fail")}",
            $"clientRunState={(report.RuntimeClientRunState.IsReady ? "ok" : "fail")}",
            $"clientLoopState={(report.RuntimeClientLoopState.IsReady ? "ok" : "fail")}",
            $"hostSession={(report.RuntimeHostSession.IsReady ? "ok" : "fail")}",
            $"hostLoop={(report.RuntimeHostLoop.IsReady ? "ok" : "fail")}",
            $"hostReadyState={(report.RuntimeHostReadyState.IsReady ? "ok" : "fail")}",
            $"platformSession={(report.RuntimePlatformSession.IsReady ? "ok" : "fail")}",
            $"platformReadyState={(report.RuntimePlatformReadyState.IsReady ? "ok" : "fail")}",
            $"platformLoop={(report.RuntimePlatformLoop.IsReady ? "ok" : "fail")}",
            $"platformLaunchGate={(report.RuntimePlatformLaunchGate.IsReady ? "ok" : "fail")}",
            $"browserShellSession={(report.RuntimeBrowserShellSession.IsReady ? "ok" : "fail")}",
            $"browserShellReadyState={(report.RuntimeBrowserShellReadyState.IsReady ? "ok" : "fail")}",
            $"browserSurfaceSession={(report.RuntimeBrowserSurfaceSession.IsReady ? "ok" : "fail")}",
            $"browserSurfaceReadyState={(report.RuntimeBrowserSurfaceReadyState.IsReady ? "ok" : "fail")}",
            $"browserWindowSession={(report.RuntimeBrowserWindowSession.IsReady ? "ok" : "fail")}",
            $"browserWindowReadyState={(report.RuntimeBrowserWindowReadyState.IsReady ? "ok" : "fail")}",
            $"browserFrameSession={(report.RuntimeBrowserFrameSession.IsReady ? "ok" : "fail")}",
            $"browserFrameReadyState={(report.RuntimeBrowserFrameReadyState.IsReady ? "ok" : "fail")}",
            $"browserCanvasSession={(report.RuntimeBrowserCanvasSession.IsReady ? "ok" : "fail")}",
            $"browserCanvasReadyState={(report.RuntimeBrowserCanvasReadyState.IsReady ? "ok" : "fail")}",
            $"browserRenderSession={(report.RuntimeBrowserRenderSession.IsReady ? "ok" : "fail")}",
            $"browserRenderReadyState={(report.RuntimeBrowserRenderReadyState.IsReady ? "ok" : "fail")}",
            $"browserPresentSession={(report.RuntimeBrowserPresentSession.IsReady ? "ok" : "fail")}",
            $"browserPresentReadyState={(report.RuntimeBrowserPresentReadyState.IsReady ? "ok" : "fail")}",
            $"browserDisplaySession={(report.RuntimeBrowserDisplaySession.IsReady ? "ok" : "fail")}",
            $"browserDisplayReadyState={(report.RuntimeBrowserDisplayReadyState.IsReady ? "ok" : "fail")}",
            $"browserViewportSession={(report.RuntimeBrowserViewportSession.IsReady ? "ok" : "fail")}",
            $"browserViewportReadyState={(report.RuntimeBrowserViewportReadyState.IsReady ? "ok" : "fail")}",
            $"browserSceneSession={(report.RuntimeBrowserSceneSession.IsReady ? "ok" : "fail")}",
            $"browserSceneReadyState={(report.RuntimeBrowserSceneReadyState.IsReady ? "ok" : "fail")}",
            $"browserInputSession={(report.RuntimeBrowserInputSession.IsReady ? "ok" : "fail")}",
            $"browserInputReadyState={(report.RuntimeBrowserInputReadyState.IsReady ? "ok" : "fail")}",
            $"browserEventSession={(report.RuntimeBrowserEventSession.IsReady ? "ok" : "fail")}",
            $"browserEventReadyState={(report.RuntimeBrowserEventReadyState.IsReady ? "ok" : "fail")}",
            $"browserInteractionSession={(report.RuntimeBrowserInteractionSession.IsReady ? "ok" : "fail")}",
            $"browserInteractionReadyState={(report.RuntimeBrowserInteractionReadyState.IsReady ? "ok" : "fail")}",
            $"browserFocusSession={(report.RuntimeBrowserFocusSession.IsReady ? "ok" : "fail")}",
            $"browserFocusReadyState={(report.RuntimeBrowserFocusReadyState.IsReady ? "ok" : "fail")}",
            $"browserShortcutSession={(report.RuntimeBrowserShortcutSession.IsReady ? "ok" : "fail")}",
            $"browserShortcutReadyState={(report.RuntimeBrowserShortcutReadyState.IsReady ? "ok" : "fail")}",
            $"browserPointerSession={(report.RuntimeBrowserPointerSession.IsReady ? "ok" : "fail")}",
            $"browserPointerReadyState={(report.RuntimeBrowserPointerReadyState.IsReady ? "ok" : "fail")}",
            $"browserCommandSession={(report.RuntimeBrowserCommandSession.IsReady ? "ok" : "fail")}",
            $"browserCommandReadyState={(report.RuntimeBrowserCommandReadyState.IsReady ? "ok" : "fail")}",
            $"browserGestureSession={(report.RuntimeBrowserGestureSession.IsReady ? "ok" : "fail")}",
            $"browserGestureReadyState={(report.RuntimeBrowserGestureReadyState.IsReady ? "ok" : "fail")}",
            $"browserLifecycleSession={(report.RuntimeBrowserLifecycleSession.IsReady ? "ok" : "fail")}",
            $"browserLifecycleReadyState={(report.RuntimeBrowserLifecycleReadyState.IsReady ? "ok" : "fail")}",
            $"browserRouteSession={(report.RuntimeBrowserRouteSession.IsReady ? "ok" : "fail")}",
            $"browserRouteReadyState={(report.RuntimeBrowserRouteReadyState.IsReady ? "ok" : "fail")}",
            $"browserStateSyncSession={(report.RuntimeBrowserStateSyncSession.IsReady ? "ok" : "fail")}",
            $"browserStateSyncReadyState={(report.RuntimeBrowserStateSyncReadyState.IsReady ? "ok" : "fail")}",
            $"browserRestoreSession={(report.RuntimeBrowserRestoreSession.IsReady ? "ok" : "fail")}",
            $"browserRestoreReadyState={(report.RuntimeBrowserRestoreReadyState.IsReady ? "ok" : "fail")}",
            $"browserResumeSession={(report.RuntimeBrowserResumeSession.IsReady ? "ok" : "fail")}",
            $"browserResumeReadyState={(report.RuntimeBrowserResumeReadyState.IsReady ? "ok" : "fail")}"
        );
    }
}

public sealed class BrowserSelfTestReport
{
    public DateTimeOffset GeneratedAtUtc { get; set; }
    public BrowserSelfTestRequest Request { get; set; } = new();
    public BrowserRuntimeBootstrapState DirectBootstrap { get; set; } = new();
    public BrowserSharedSeamRuntimeBootstrapState SeamBootstrap { get; set; } = new();
    public BrowserClientBootstrapHandoff Handoff { get; set; } = new();
    public BrowserRuntimeCacheState CacheState { get; set; } = new();
    public BrowserFileSystemBridgeState BridgeState { get; set; } = new();
    public bool SeedImportAttempted { get; set; }
    public int SeedImportCount { get; set; }
    public BrowserClientStartupArtifact StartupArtifact { get; set; } = new();
    public BrowserClientStartupArtifactRead StartupArtifactRead { get; set; } = new();
    public BrowserClientLaunchSession LaunchSession { get; set; } = new();
    public BrowserClientLaunchSessionRead LaunchSessionRead { get; set; } = new();
    public BrowserClientRuntimeLaunchContractResult RuntimeLaunchContract { get; set; } = new();
    public BrowserClientStartupPacketResult StartupPacket { get; set; } = new();
    public BrowserClientStartupConsumerResult StartupConsumer { get; set; } = new();
    public BrowserClientStartupSessionExecutorResult StartupSessionExecutor { get; set; } = new();
    public BrowserClientStartupSessionRunnerResult StartupSessionRunner { get; set; } = new();
    public BrowserClientRuntimeBootstrapLoopResult RuntimeBootstrapLoop { get; set; } = new();
    public BrowserClientRuntimeInvocationResult RuntimeInvocation { get; set; } = new();
    public BrowserClientRuntimeStartupCycleResult RuntimeStartupCycle { get; set; } = new();
    public BrowserClientRuntimeStartupStateResult RuntimeStartupState { get; set; } = new();
    public BrowserClientRuntimeStartupStateMachineResult RuntimeStartupStateMachine { get; set; } = new();
    public BrowserClientRuntimeStartupTransitionDriverResult RuntimeStartupTransitionDriver { get; set; } = new();
    public BrowserClientRuntimeStartupDispatcherResult RuntimeStartupDispatcher { get; set; } = new();
    public BrowserClientRuntimeStartupSessionControllerResult RuntimeStartupSessionController { get; set; } = new();
    public BrowserClientRuntimeStartupCoordinatorResult RuntimeStartupCoordinator { get; set; } = new();
    public BrowserClientRuntimeStartupOrchestratorResult RuntimeStartupOrchestrator { get; set; } = new();
    public BrowserClientBootFlowControllerResult BrowserBootFlowController { get; set; } = new();
    public BrowserClientBootSessionResult BrowserBootSession { get; set; } = new();
    public BrowserClientRuntimeLaunchHandoffResult RuntimeLaunchHandoff { get; set; } = new();
    public BrowserClientRuntimeBootstrapConsumerResult RuntimeBootstrapConsumer { get; set; } = new();
    public BrowserClientRuntimeBootstrapSessionResult RuntimeBootstrapSession { get; set; } = new();
    public BrowserClientRuntimeClientBootstrapControllerResult RuntimeClientBootstrapController { get; set; } = new();
    public BrowserClientRuntimeStartupReadinessGateResult RuntimeStartupReadinessGate { get; set; } = new();
    public BrowserClientRuntimeReadySignalResult RuntimeReadySignal { get; set; } = new();
    public BrowserClientRuntimeLaunchControllerResult RuntimeLaunchController { get; set; } = new();
    public BrowserClientRuntimeClientReadyStateResult RuntimeClientReadyState { get; set; } = new();
    public BrowserClientRuntimeClientLaunchSessionResult RuntimeClientLaunchSession { get; set; } = new();
    public BrowserClientRuntimeClientActivationResult RuntimeClientActivation { get; set; } = new();
    public BrowserClientRuntimeClientRunStateResult RuntimeClientRunState { get; set; } = new();
    public BrowserClientRuntimeClientLoopStateResult RuntimeClientLoopState { get; set; } = new();
    public BrowserClientRuntimeHostSessionResult RuntimeHostSession { get; set; } = new();
    public BrowserClientRuntimeHostLoopResult RuntimeHostLoop { get; set; } = new();
    public BrowserClientRuntimeHostReadyStateResult RuntimeHostReadyState { get; set; } = new();
    public BrowserClientRuntimePlatformSessionResult RuntimePlatformSession { get; set; } = new();
    public BrowserClientRuntimePlatformReadyStateResult RuntimePlatformReadyState { get; set; } = new();
    public BrowserClientRuntimePlatformLoopResult RuntimePlatformLoop { get; set; } = new();
    public BrowserClientRuntimePlatformLaunchGateResult RuntimePlatformLaunchGate { get; set; } = new();
    public BrowserClientRuntimeBrowserShellSessionResult RuntimeBrowserShellSession { get; set; } = new();
    public BrowserClientRuntimeBrowserShellReadyStateResult RuntimeBrowserShellReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserSurfaceSessionResult RuntimeBrowserSurfaceSession { get; set; } = new();
    public BrowserClientRuntimeBrowserSurfaceReadyStateResult RuntimeBrowserSurfaceReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserWindowSessionResult RuntimeBrowserWindowSession { get; set; } = new();
    public BrowserClientRuntimeBrowserWindowReadyStateResult RuntimeBrowserWindowReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserFrameSessionResult RuntimeBrowserFrameSession { get; set; } = new();
    public BrowserClientRuntimeBrowserFrameReadyStateResult RuntimeBrowserFrameReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserCanvasSessionResult RuntimeBrowserCanvasSession { get; set; } = new();
    public BrowserClientRuntimeBrowserCanvasReadyStateResult RuntimeBrowserCanvasReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserRenderSessionResult RuntimeBrowserRenderSession { get; set; } = new();
    public BrowserClientRuntimeBrowserRenderReadyStateResult RuntimeBrowserRenderReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserPresentSessionResult RuntimeBrowserPresentSession { get; set; } = new();
    public BrowserClientRuntimeBrowserPresentReadyStateResult RuntimeBrowserPresentReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserDisplaySessionResult RuntimeBrowserDisplaySession { get; set; } = new();
    public BrowserClientRuntimeBrowserDisplayReadyStateResult RuntimeBrowserDisplayReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserViewportSessionResult RuntimeBrowserViewportSession { get; set; } = new();
    public BrowserClientRuntimeBrowserViewportReadyStateResult RuntimeBrowserViewportReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserSceneSessionResult RuntimeBrowserSceneSession { get; set; } = new();
    public BrowserClientRuntimeBrowserSceneReadyStateResult RuntimeBrowserSceneReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserInputSessionResult RuntimeBrowserInputSession { get; set; } = new();
    public BrowserClientRuntimeBrowserInputReadyStateResult RuntimeBrowserInputReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserEventSessionResult RuntimeBrowserEventSession { get; set; } = new();
    public BrowserClientRuntimeBrowserEventReadyStateResult RuntimeBrowserEventReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserInteractionSessionResult RuntimeBrowserInteractionSession { get; set; } = new();
    public BrowserClientRuntimeBrowserInteractionReadyStateResult RuntimeBrowserInteractionReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserFocusSessionResult RuntimeBrowserFocusSession { get; set; } = new();
    public BrowserClientRuntimeBrowserFocusReadyStateResult RuntimeBrowserFocusReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserShortcutSessionResult RuntimeBrowserShortcutSession { get; set; } = new();
    public BrowserClientRuntimeBrowserShortcutReadyStateResult RuntimeBrowserShortcutReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserPointerSessionResult RuntimeBrowserPointerSession { get; set; } = new();
    public BrowserClientRuntimeBrowserPointerReadyStateResult RuntimeBrowserPointerReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserCommandSessionResult RuntimeBrowserCommandSession { get; set; } = new();
    public BrowserClientRuntimeBrowserCommandReadyStateResult RuntimeBrowserCommandReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserGestureSessionResult RuntimeBrowserGestureSession { get; set; } = new();
    public BrowserClientRuntimeBrowserGestureReadyStateResult RuntimeBrowserGestureReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserLifecycleSessionResult RuntimeBrowserLifecycleSession { get; set; } = new();
    public BrowserClientRuntimeBrowserLifecycleReadyStateResult RuntimeBrowserLifecycleReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserRouteSessionResult RuntimeBrowserRouteSession { get; set; } = new();
    public BrowserClientRuntimeBrowserRouteReadyStateResult RuntimeBrowserRouteReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserStateSyncSessionResult RuntimeBrowserStateSyncSession { get; set; } = new();
    public BrowserClientRuntimeBrowserStateSyncReadyStateResult RuntimeBrowserStateSyncReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserRestoreSessionResult RuntimeBrowserRestoreSession { get; set; } = new();
    public BrowserClientRuntimeBrowserRestoreReadyStateResult RuntimeBrowserRestoreReadyState { get; set; } = new();
    public BrowserClientRuntimeBrowserResumeSessionResult RuntimeBrowserResumeSession { get; set; } = new();
    public BrowserClientRuntimeBrowserResumeReadyStateResult RuntimeBrowserResumeReadyState { get; set; } = new();
    public string Summary { get; set; } = string.Empty;
    public string Json { get; set; } = string.Empty;
}

public sealed class BrowserSelfTestRequest
{
    public string TileDataPath { get; set; } = string.Empty;
    public string ClilocPath { get; set; } = string.Empty;
    public string HuesPath { get; set; } = string.Empty;
}
