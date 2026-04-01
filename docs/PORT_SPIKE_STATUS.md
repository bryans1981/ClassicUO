# Port Spike Status

## Current Spike

We have a working experimental browser host at `experiments/BrowserHost`.

### Verified locally

- `dotnet restore .\experiments\BrowserHost\BrowserHost.csproj`
- `dotnet build .\experiments\BrowserHost\BrowserHost.csproj -c Debug`
- local HTTP probe against `http://localhost:5099` returned `200`
- `dotnet build .\src\ClassicUO.Client\ClassicUO.Client.csproj -c Debug` succeeded after the stream-based filesystem refactor

### Current browser-host capabilities

- environment probe for browser runtime and JS interop
- OPFS capability detection
- OPFS smoke test path for create, write, read, and list operations in browser storage
- browser file and folder import into OPFS under the shared `/uo` asset root, with persisted relative paths and file listing
- virtual-path browser storage probe from C# for file existence, text reads, byte-length reads, and text writes, including `/uo/...` asset-root paths
- root-aware asset manifest for `/uo` with file counts, total bytes, extension summary, and preload-state visibility
- bootstrap-readiness contract over `/uo` that checks grouped map, art, gump, localization, and core metadata requirements
- hosted local seed import path for staging large asset bundles from disk without browser file picking
- focused recommended-seed import path for bootstrap-critical files and retail UOP alternatives
- structured `tiledata.mul` probe that now parses the first land tile correctly from the imported browser asset set
- C# browser asset byte-source service used by the spike instead of relying only on JS-side binary parsing
- root-aware browser asset caching with separate raw asset cache and processed asset cache
- reusable browser asset loader harness for read, preprocess, parse, and cache flow
- browser asset readers for `tiledata.mul`, `cliloc.enu`, and `hues.mul`
- client-facing browser asset service that now drives the current test panels instead of direct probe services
- bootstrap aggregation service and runtime bootstrap subsystem with dedicated request/state model files
- browser spike UI reorganized around `Current Tests`, `Imports & Assets`, and archived diagnostics
- one-click self-test suite for batched browser validation with automatic local report saving into the repo and reduced manual reporting
- shared `BrowserFileSystem` bridge now exists for read-only bootstrap asset loading through the real seam`r`n- seam-backed browser bootstrap handoff now validates successfully with `3 / 3` ready assets and warm-cache reruns in single-digit milliseconds`r`n- startup-oriented browser adapter output now exists on top of the seam-backed handoff for a future ClassicUO browser entrypoint`r`n- launch-oriented browser entrypoint output now prepares minimal browser-side config/profile files and reports startup readiness`r`n- startup-sequence browser entrypoint shell now groups launch preparation into explicit startup steps for a future browser entrypoint`r`n- launch-state artifact step now writes startup data into browser storage for a future browser entrypoint to consume`r`n- startup artifact reader now reads the launch-state artifact back from browser storage as if a browser entrypoint were consuming it
- the startup path now creates a simulated browser launch session from the saved startup artifact, and the self-test suite records artifact write/read plus launch-session write state in a locally saved JSON report
- the startup path now also reads the saved launch session back as a resumable launch-state consumer, and the self-test suite records launch-session read state in the local JSON report
- the launch-session serialization order has been corrected so saved session JSON reflects final write-state and summary values during readback validation
- the startup path now builds a runtime launch contract from the saved launch session, and the one-click self-test records whether that contract is ready
- the startup path now builds a single startup packet from the runtime launch contract, and the one-click self-test records whether that packet is ready
- the startup path now includes an in-memory startup consumer above the startup packet, and the one-click self-test records whether that browser bootstrap handshake object is ready
- the startup path now includes a startup-session executor above the consumer, and the one-click self-test records whether that runtime-oriented startup layer is ready
- the startup path now includes a startup-session runner above the executor, and the one-click self-test records whether that simulated runtime startup pass is ready
- the startup path now includes a minimal runtime bootstrap loop above the runner, and the one-click self-test records whether that phase-based startup loop is ready
- the startup path now includes a minimal runtime invocation layer above the loop, and the one-click self-test records whether that runtime call boundary is ready
- the startup path now includes a minimal runtime startup cycle layer above the runtime invocation, and the one-click self-test records whether that persistent runtime startup-state object is ready
- the startup path now includes a minimal runtime startup state layer above the runtime startup cycle, and the one-click self-test records whether that concrete runtime startup-state holder is ready
- the startup path now includes a minimal runtime startup state machine layer above the runtime startup state, and the one-click self-test records whether that browser bootstrap state-progression object is ready
- the startup path now includes a minimal runtime startup transition driver above the runtime startup state machine, and the one-click self-test records whether that executable browser bootstrap progression layer is ready
- the startup path now includes a minimal runtime startup dispatcher above the runtime startup transition driver, and the one-click self-test records whether that bootstrap-session control layer is ready
- the startup path now includes a minimal runtime startup session controller above the runtime startup dispatcher, and the one-click self-test records whether that executable bootstrap-session control object is ready
- the startup path now includes a minimal runtime startup coordinator above the runtime startup session controller, and the one-click self-test records whether that browser bootstrap orchestration layer is ready
- the startup path now includes a minimal runtime startup orchestrator above the runtime startup coordinator, and the one-click self-test records whether that runtime-wide startup orchestration layer is ready
- the startup path now includes a first browser boot-flow controller above the runtime startup orchestrator, and the one-click self-test records whether that browser-facing boot flow layer is ready
- the startup path now includes a first browser boot session above the boot-flow controller, and the one-click self-test records whether that browser-session startup layer is ready
- the startup path now includes a first runtime launch handoff above the browser boot session, and the one-click self-test records whether that browser-to-runtime handoff layer is ready
- the startup path now includes a first runtime bootstrap consumer above the runtime launch handoff, and the one-click self-test records whether that runtime-side handoff consumer is ready
- the startup path now includes a first runtime bootstrap session above the runtime bootstrap consumer, and the one-click self-test records whether that runtime-session startup layer is ready

### Main-code integration progress

- the main client now routes core file access through `FileSystemHelper`
- `BrowserFileSystem` now supports an injectable `IBrowserStorageProvider`
- browser virtual paths are now normalized consistently before provider access
- an `InMemoryBrowserStorageProvider` now exists for non-browser tests and early bring-up
- the IO layer now accepts generic `Stream` input, with memory-mapped IO retained for desktop `FileStream` inputs
- the shared filesystem seam now supports read, write, append, read/write, listing, file length, copy, and delete operations
- `DefReader` now reads through the shared filesystem seam
- the shallow asset-loader file existence/open checks have been moved onto the shared seam
- profile/state XML and JSON managers now use the shared filesystem seam for read/write paths (`MacroManager`, `SkillsGroupManager`, `InfoBarManager`, `IgnoreManager`, `LastCharacterManager`)
- data tables, world-map support, container persistence, `UltimaLive`, journal logging, and screenshot saving have now been moved onto the seam at the main architectural touchpoints
- targeted browser filesystem unit tests now pass in `ClassicUO.UnitTests`
- the shared utility layer now includes a read-only binary asset provider for `/uo`-style browser asset paths

### Validation

- `dotnet build .\experiments\BrowserHost\BrowserHost.csproj -c Debug`
- `dotnet build .\src\ClassicUO.Client\ClassicUO.Client.csproj -c Debug`
- `dotnet test .\tests\ClassicUO.UnitTests\ClassicUO.UnitTests.csproj -c Debug --filter BrowserFileSystemTests` (18 passing tests)

### Current browser asset subsystem layout

- `BrowserAssetSourceService`
  - browser-backed raw byte and stream access
  - case-insensitive path resolution
  - raw asset cache
- `BrowserProcessedAssetCacheService`
  - processed and parsed asset cache
- `BrowserAssetLoaderHarnessService`
  - shared read/preprocess/parse/cache flow
- `BrowserTileDataReaderService`
- `BrowserClilocReaderService`
- `BrowserHuesReaderService`
- `BrowserBootstrapAssetService`
  - aggregates bootstrap asset reads
- `BrowserRuntimeBootstrapAssetService`
  - runtime-facing bootstrap and cache control boundary
- `BrowserClientAssetService`
  - client-facing browser asset API used by the spike UI
- `BrowserFileSystemBridgeService`
  - activates a shared read-only `BrowserFileSystem` provider from bootstrap assets and probes seam-level reads

### Operator workflow

- Use `Current Tests` for the active validation path only.
- Prefer one-click actions that write readable local reports instead of manual copy/paste from the browser.
- Move superseded checks into archived sections once they are no longer part of the active path.

### Operator commands

Start:

```powershell
.\scripts\browser-spike-start.ps1
```

Stop:

```powershell
.\scripts\browser-spike-stop.ps1
```

Test:

```powershell
.\scripts\browser-spike-test.ps1
```

### Next code objective

Extend the runtime bootstrap consumer/session layers into the next browser entrypoint step, ideally by shaping the first executable runtime client bootstrap controller around that consumed handoff state.







- Do not pause for user confirmation between routine implementation milestones; continue automatically and only stop when a browser-side test, external decision, or blocker truly requires operator input.

- Do not ask the operator to reply with "next" or any equivalent continue signal; continue implementation automatically after routine milestones and only request input for an actual test, external decision, or blocker.
- Added `BrowserClientRuntimeReadySignalService` and `BrowserClientRuntimeLaunchControllerService` and wired both into the one-click self-test/report path.
- Current self-test summary now reports `readySignal` and `launchController` readiness alongside the existing runtime chain.
- Added `BrowserClientRuntimeClientReadyStateService` and `BrowserClientRuntimeClientLaunchSessionService` and wired both into the one-click self-test/report path.
- Current self-test summary now reports `clientReadyState` and `clientLaunchSession` readiness.
- Added `BrowserClientRuntimeClientActivationService` and `BrowserClientRuntimeClientRunStateService` and wired both into the one-click self-test/report path.
- Current self-test summary now reports `clientActivation` and `clientRunState` readiness.
- Added `BrowserClientRuntimeClientLoopStateService` and `BrowserClientRuntimeHostSessionService` and wired both into the one-click self-test/report path.
- Current self-test summary now reports `clientLoopState` and `hostSession` readiness.
- Added `BrowserClientRuntimeHostLoopService` and `BrowserClientRuntimeHostReadyStateService` and wired both into the one-click self-test/report path.
- Current self-test summary now reports `hostLoop` and `hostReadyState` readiness.
- Added `BrowserClientRuntimePlatformSessionService` and `BrowserClientRuntimePlatformReadyStateService` and wired both into the one-click self-test/report path.
- Current self-test summary now reports `platformSession` and `platformReadyState` readiness.
- Added `BrowserClientRuntimePlatformLoopService` and `BrowserClientRuntimePlatformLaunchGateService` and wired both into the one-click self-test/report path.
- Current self-test summary now reports `platformLoop` and `platformLaunchGate` readiness.

- Added BrowserClientRuntimeBrowserShellSessionService and BrowserClientRuntimeBrowserShellReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserShellSession and rowserShellReadyState readiness.
- Added BrowserClientRuntimeBrowserSurfaceSessionService and BrowserClientRuntimeBrowserSurfaceReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserSurfaceSession and rowserSurfaceReadyState readiness.
- Added BrowserClientRuntimeBrowserWindowSessionService and BrowserClientRuntimeBrowserWindowReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserWindowSession and rowserWindowReadyState readiness.
- Added BrowserClientRuntimeBrowserFrameSessionService and BrowserClientRuntimeBrowserFrameReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserFrameSession and rowserFrameReadyState readiness.
- Added BrowserClientRuntimeBrowserCanvasSessionService and BrowserClientRuntimeBrowserCanvasReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserCanvasSession and rowserCanvasReadyState readiness.
- Added BrowserClientRuntimeBrowserRenderSessionService and BrowserClientRuntimeBrowserRenderReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserRenderSession and rowserRenderReadyState readiness.
- Added BrowserClientRuntimeBrowserPresentSessionService and BrowserClientRuntimeBrowserPresentReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserPresentSession and rowserPresentReadyState readiness.
- Added BrowserClientRuntimeBrowserDisplaySessionService and BrowserClientRuntimeBrowserDisplayReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserDisplaySession and rowserDisplayReadyState readiness.
- Added BrowserClientRuntimeBrowserViewportSessionService and BrowserClientRuntimeBrowserViewportReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserViewportSession and rowserViewportReadyState readiness.

- Added BrowserClientRuntimeBrowserSceneSessionService and BrowserClientRuntimeBrowserSceneReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserSceneSession and rowserSceneReadyState readiness.
- Added BrowserClientRuntimeBrowserInputSessionService and BrowserClientRuntimeBrowserInputReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserInputSession and rowserInputReadyState readiness.

- Added BrowserClientRuntimeBrowserEventSessionService and BrowserClientRuntimeBrowserEventReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserEventSession and rowserEventReadyState readiness.
- Added BrowserClientRuntimeBrowserInteractionSessionService and BrowserClientRuntimeBrowserInteractionReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserInteractionSession and rowserInteractionReadyState readiness.

- Added BrowserClientRuntimeBrowserFocusSessionService and BrowserClientRuntimeBrowserFocusReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserFocusSession and rowserFocusReadyState readiness.
- Added BrowserClientRuntimeBrowserShortcutSessionService and BrowserClientRuntimeBrowserShortcutReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserShortcutSession and rowserShortcutReadyState readiness.

- Added BrowserClientRuntimeBrowserPointerSessionService and BrowserClientRuntimeBrowserPointerReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserPointerSession and rowserPointerReadyState readiness.
- Added BrowserClientRuntimeBrowserCommandSessionService and BrowserClientRuntimeBrowserCommandReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserCommandSession and rowserCommandReadyState readiness.

- Added BrowserClientRuntimeBrowserGestureSessionService and BrowserClientRuntimeBrowserGestureReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserGestureSession and rowserGestureReadyState readiness.
- Added BrowserClientRuntimeBrowserLifecycleSessionService and BrowserClientRuntimeBrowserLifecycleReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserLifecycleSession and rowserLifecycleReadyState readiness.

- Added BrowserClientRuntimeBrowserRouteSessionService and BrowserClientRuntimeBrowserRouteReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserRouteSession and rowserRouteReadyState readiness.
- Added BrowserClientRuntimeBrowserStateSyncSessionService and BrowserClientRuntimeBrowserStateSyncReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports rowserStateSyncSession and rowserStateSyncReadyState readiness.

- Added BrowserClientRuntimeBrowserRestoreSessionService and BrowserClientRuntimeBrowserRestoreReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserRestoreSession and browserRestoreReadyState readiness.
- Added BrowserClientRuntimeBrowserResumeSessionService and BrowserClientRuntimeBrowserResumeReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserResumeSession and browserResumeReadyState readiness.
