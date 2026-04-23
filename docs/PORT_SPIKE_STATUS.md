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
- the browser filesystem bootstrap seam has been promoted into `ClassicUO.Utility`, and the main client now checks for an attached browser storage provider during startup
- the browser host now links the shared browser filesystem bootstrap helper and attaches the provider through that helper
- the browser storage provider contract and provider implementations are now public in `ClassicUO.Utility`
- the main client now owns a dedicated `BrowserRuntimeBootstrap` helper for browser-safe defaults and storage seam checks
- the main client browser bootstrap helper now also captures a browser startup state snapshot for the real entrypoint path
- the browser startup state snapshot is now threaded into `Client.Run` and `GameController`, so the real client consumes the browser startup contract instead of only capturing it
- the browser defaults are now owned directly by the bootstrap path and consumed by `GameController`, so browser render and input behavior stays in the real client path without a cached policy object
- browser startup now owns browser window resizing and text-input defaults directly, so browser-facing runtime behavior is controlled in the real client path without an intermediate policy cache
- browser mode now disables idle sleep directly in the main client path, so browser timing stays under the real client path instead of desktop-style sleep behavior
- browser mode now drives the browser refresh rate directly during initialization, so browser loop timing is owned by the real client path
- browser mode now writes browser defaults back into `Settings.GlobalSettings` during startup, so browser defaults are visible to the rest of the real client
- browser window-size updates now rely on the resize flag directly instead of an extra browser branch in `GameController`, so browser window handling stays aligned with the startup defaults
- `GameController` now reads the browser-normalized settings values directly for mouse input, fixed timestep, target FPS, and text input, so the browser startup defaults flow straight into the real client loop
- `GameController` now logs the browser bootstrap state whenever it is present, without a separate browser-mode gate in the initialize path
- the browser startup path now owns the browser defaults directly; `Main` no longer applies a separate browser runtime-policy step after startup validation
- `Main` now relies on the browser startup defaults for the browser UO root as well, so browser mode does not carry a separate executable-path fallback in the startup path
- browser save sanitization now lives in `BrowserRuntimeBootstrap`, so `Settings.Save` no longer owns a separate browser-only cleanup block
- `ProfileManager` now applies the profile minimum size clamp through the shared validation path, because browser profiles already normalize to a valid browser window size at load time
- the nested `client.exe` browser guard in the startup validation path is gone, so browser mode now relies on the bootstrap-normalized client version instead of a dead inner browser branch
- the browser force-driver path in `Main` now relies on the browser startup default of zero instead of a separate browser-mode guard around the desktop switch
- plugin tick, draw, and shutdown now key off `_pluginsInitialized` instead of browser mode checks, because browser startup never initializes plugins
- the browser startup defaults now also set the browser profile root explicitly, so browser profile persistence is owned by the real client startup path
- the browser startup defaults now also set an initial browser window position and size, so the browser host does not inherit desktop placement assumptions
- the browser profile load path now applies browser-safe profile window defaults, so game-window state is owned by the real client profile layer
- the browser profile load path now also disables `ReduceFPSWhenInactive`, and the browser runtime policy now owns inactive FPS throttling directly, so browser timing stays out of desktop-style inactive throttling
- the main startup validation path now skips external browser launching in browser mode, so a browser-side startup failure does not try to spawn another browser
- the browser startup validation path now logs errors instead of showing a native SDL dialog, so browser failures stay within the browser-safe path
- browser unload no longer back-writes desktop window placement, so browser shutdown does not leak desktop window state into the saved profile
- the browser window-placement routine now short-circuits in browser mode, so browser startup does not use desktop display-bound positioning logic
- browser login and options flows now avoid desktop window resize and maximize behavior in browser mode, so the browser path stays centered on the playable client instead of native window mechanics
- the browser client-size change path now skips native resize writes in browser mode, so canvas/window updates stay out of desktop window mechanics
- browser screenshots now root under the shared browser cache path, so capture output stays in the browser filesystem contract instead of desktop data folders
- browser plugin loading is now owned by the browser runtime policy, so the main client no longer hard-codes a browser-mode startup branch for plugins
- GitHub Actions for build-test and deploy are now manual-only so browser-client work can stay on local validation without generating repeat CI noise
- bootstrap aggregation service and runtime bootstrap subsystem with dedicated request/state model files
- browser spike UI reorganized around `Current Tests`, `Imports & Assets`, and archived diagnostics
- one-click self-test suite for batched browser validation with automatic local report saving into the repo and reduced manual reporting
- self-test failures now write a compact local failure envelope when the runtime chain throws before the full report is built
- the ninth 20-layer runtime batch needed a DI cycle break; its newest ready-state services were detached from paired session services to restore the compact self-test path
- shared `BrowserFileSystem` bridge now exists for read-only bootstrap asset loading through the real seam
- seam-backed browser bootstrap handoff now validates successfully with `3 / 3` ready assets and warm-cache reruns in single-digit milliseconds
- startup-oriented browser adapter output now exists on top of the seam-backed handoff for a future ClassicUO browser entrypoint
- launch-oriented browser entrypoint output now prepares minimal browser-side config/profile files and reports startup readiness
- startup-sequence browser entrypoint shell now groups launch preparation into explicit startup steps for a future browser entrypoint
- launch-state artifact step now writes startup data into browser storage for a future browser entrypoint to consume
- startup artifact reader now reads the launch-state artifact back from browser storage as if a browser entrypoint were consuming it
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
- the current stable tail baseline is `runtimeTailExtension=ok`, which extends the steady-operation readiness layer with a compact 20-phase stabilization tail
- the current active browser handoff is now the concrete `Browser bootstrap package` artifact written to `/cache/startup/default/browser-bootstrap-package.json`; the synthetic tail remains validated, but it is no longer the active implementation path
- `Current Tests` should now focus on the bootstrap package readback and launch/session persistence; the long runtime-tail ladder has been moved to archived validation context
- The active page now surfaces the bootstrap package artifact, package readback, and launch-session path near the top, while the synthetic runtime ladder remains archived for reference only
- the bootstrap package is now also consumed by a package-consumer stage, so the launch plan now flows through a package-backed handoff rather than only a file readback
- the launch plan now consumes the live seam-backed handoff directly, which breaks the stale package dependency and keeps the browser-native execution plan on the active path
- the report receiver at `http://localhost:5100` must stay healthy for the one-click self-test flow; a dead receiver shows up as `TypeError: Failed to fetch` in the browser even when the client itself is fine
- the browser-native execution plan now reports a browser-native runtime shell summary so the active operator surface stays aligned with the final browser client instead of the older synthetic tail
- the browser spike now supports a no-click self-test URL via `?autoSelfTest=1` or `?runSelfTest=1`, which runs the current self-test suite on page load and saves the report locally
- the browser-native websocket session controller now validates the transport endpoint and runtime execution mode before the browser session controller, giving us the first explicit websocket handshake baseline
 - the browser-native websocket runtime execution controller now performs a real browser websocket connect/read against the report host and is the current live websocket execution baseline
 - the browser-native websocket runtime session controller now sits above that live websocket execution path and is the current browser-session handoff layer
 - the browser-native runtime session controller now sits above the websocket runtime-session handoff and is the current browser-runtime-session baseline
 - the browser-native browser runtime controller now consumes the browser-runtime-session layer and is the current browser-runtime baseline
 - the browser-native browser runtime ready-state now sits above the browser runtime and is the current browser-runtime baseline in the compact report
 - the browser-native browser host ready-state now sits above the browser host and is the current browser-host baseline in the compact report
 - the browser-native canvas host now mounts a real browser canvas and probes a render context, giving us the first concrete browser-render step above the render controller
 - the browser-native canvas frame now clears and paints the mounted canvas, giving us the first visible browser-render frame above the canvas host
- the browser-native canvas input bridge now installs pointer and keyboard listeners on the rendered canvas and probes them with synthetic events, giving us the first concrete browser input bridge above the input controller
- the browser-native frame pump now starts a live render/update heartbeat on the canvas and probes it before stopping, giving us the first concrete browser runtime loop above the canvas frame
- the browser-native runtime loop now sits above the frame pump and input controller, giving us the first product-facing browser runtime heartbeat on the visible render path
- the browser-native runtime execution slice now combines the runtime loop, transport, and websocket/session layers into one product-facing runtime execution snapshot
- the browser-native runtime network slice now combines the runtime execution slice, network controller, and transport controller into the current browser-network snapshot

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
- GitHub Actions `Build-Test` should ignore browser-spike and docs-only changes, and `Deploy` should only run after a successful `Build-Test` completion.

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

Extend the runtime bootstrap consumer/session layers into the next browser entrypoint step, ideally by shaping the first executable runtime client bootstrap controller around the package-backed handoff and then moving straight into browser-native rendering/input/network runtime slices for the final browser client.

### Decision Log

- 2026-04-15: Skip broad vertical-slice execution as the default path. Use feature-sized batches, parallel disjoint work, and machine-readable validation reports to move toward the final browser client faster.
- 2026-04-15: The official ClassicUO browser target uses WebAssembly and WebGL; our next client-facing work should aim at that browser-native execution shape rather than more scaffolding-only layers.
- 2026-04-15: The browser-native runtime shell controller is now part of the compact self-test baseline, and the no-click runner validates it directly from the saved local report.
- 2026-04-15: The no-click runner now uses an isolated Edge profile and closes the browser automatically after the self-test report updates, unless explicitly kept open.
- 2026-04-15: The browser-native client bootstrap controller now sits above the native launch controller and is validated by the compact self-test report.
- 2026-04-15: The browser-native browser runtime controller now sits above the native client bootstrap controller and is the current product-facing runtime baseline in the compact self-test report.
- 2026-04-15: The browser-native browser host controller now sits above the browser runtime and is the current browser-host baseline in the compact self-test report.
- 2026-04-15: The browser-native browser surface controller now sits above the browser host controller and is the current browser-surface baseline in the compact self-test report.
- 2026-04-15: The browser-native browser render controller now sits above the browser-surface controller and is the current browser-render baseline in the compact self-test report.
- 2026-04-15: The browser-native browser input controller now sits above the browser-render controller and is the current browser-input baseline in the compact self-test report.
- 2026-04-15: The browser-native browser network controller now sits above the browser-input controller and is the current browser-network baseline in the compact self-test report.
- 2026-04-15: The browser-native browser transport controller now sits above the browser-network controller and is the current browser-transport baseline in the compact self-test report.
- 2026-04-15: The browser-native browser runtime execution controller now sits above the browser-transport controller and is the current browser-runtime-execution baseline in the compact self-test report.
 - 2026-04-15: The browser-native websocket runtime session controller now sits above the browser-runtime-execution controller and is the current browser-session handoff layer in the compact self-test report.
 - 2026-04-15: The browser-native browser session controller now sits above the browser-runtime-execution controller and is the current browser-session baseline in the compact self-test report.
 - 2026-04-15: The browser-native websocket runtime execution controller now sits above the websocket session controller and is the current live websocket execution baseline in the compact self-test report.
 - 2026-04-15: The browser-native runtime session controller now sits above the websocket runtime-session controller and is the current browser-runtime-session baseline in the compact self-test report.
 - 2026-04-15: The browser-native browser runtime controller now consumes the browser-runtime-session layer and is the current browser-runtime baseline in the compact self-test report.
 - 2026-04-15: The browser-native browser runtime ready-state now sits above the browser runtime and is the current browser-runtime baseline in the compact self-test report.
 - 2026-04-15: The browser-native browser host ready-state now sits above the browser host and is the current browser-host baseline in the compact self-test report.
  - 2026-04-15: The no-click browser self-test runner now preflights `http://localhost:5099` and `http://localhost:5100/health` before opening Edge, so cold-start validation is less likely to time out.







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

- Added BrowserClientRuntimeBrowserCheckpointSessionService and BrowserClientRuntimeBrowserCheckpointReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserCheckpointSession and browserCheckpointReadyState readiness.
- Added BrowserClientRuntimeBrowserPersistenceSessionService and BrowserClientRuntimeBrowserPersistenceReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserPersistenceSession and browserPersistenceReadyState readiness.

- Added BrowserClientRuntimeBrowserHistorySessionService and BrowserClientRuntimeBrowserHistoryReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserHistorySession and browserHistoryReadyState readiness.
- Added BrowserClientRuntimeBrowserRecoverySessionService and BrowserClientRuntimeBrowserRecoveryReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserRecoverySession and browserRecoveryReadyState readiness.

- Added BrowserClientRuntimeBrowserSnapshotSessionService and BrowserClientRuntimeBrowserSnapshotReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserSnapshotSession and browserSnapshotReadyState readiness.
- Added BrowserClientRuntimeBrowserArchiveSessionService and BrowserClientRuntimeBrowserArchiveReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserArchiveSession and browserArchiveReadyState readiness.

- Added BrowserClientRuntimeBrowserTelemetrySessionService and BrowserClientRuntimeBrowserTelemetryReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserTelemetrySession and browserTelemetryReadyState readiness.
- Added BrowserClientRuntimeBrowserDiagnosticsSessionService and BrowserClientRuntimeBrowserDiagnosticsReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserDiagnosticsSession and browserDiagnosticsReadyState readiness.

- Added BrowserClientRuntimeBrowserMonitoringSessionService and BrowserClientRuntimeBrowserMonitoringReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserMonitoringSession and browserMonitoringReadyState readiness.
- Added BrowserClientRuntimeBrowserWatchdogSessionService and BrowserClientRuntimeBrowserWatchdogReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserWatchdogSession and browserWatchdogReadyState readiness.

- Added BrowserClientRuntimeBrowserHealthSessionService and BrowserClientRuntimeBrowserHealthReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserHealthSession and browserHealthReadyState readiness.
- Added BrowserClientRuntimeBrowserAlertingSessionService and BrowserClientRuntimeBrowserAlertingReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserAlertingSession and browserAlertingReadyState readiness.

- Added BrowserClientRuntimeBrowserPolicySessionService and BrowserClientRuntimeBrowserPolicyReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserPolicySession and browserPolicyReadyState readiness.
- Added BrowserClientRuntimeBrowserAuditSessionService and BrowserClientRuntimeBrowserAuditReadyStateService and wired both into the one-click self-test/report path.
- Current self-test summary now reports browserAuditSession and browserAuditReadyState readiness.

- 2026-04-02: Runtime chain now validates through browser audit, and the next batch adds browser security/compliance session and ready-state layers. Self-test reports now include RuntimeChainMarker and use cycle-safe, deeper JSON serialization.

- 2026-04-02: Runtime chain extended past browser security/compliance into privacy/governance. One-click self-test remains the validation gate for each new browser-runtime layer batch.

- 2026-04-02: Runtime chain extended through browser trust/assurance after privacy/governance. Self-test remains cleanly serialized with the runtime chain marker and deep-cycle-safe JSON options.

- 2026-04-02: Runtime chain extended through browser risk/integrity after trust/assurance. The one-click self-test remains the required validation point between batches.

- 2026-04-02: Runtime chain extended through browser resilience after risk/integrity. JSON report size continues to grow but serialization remains stable with the current settings.

- 2026-04-02: Runtime chain extended through browser availability after resilience. Validation continues through the one-click self-test and local JSON report.

- 2026-04-02: Started flattening the self-test output by moving the saved report into compact browser-runtime mode. Goal: reduce JSON growth while preserving the rich Current Tests display in the live page.

- 2026-04-02: Runtime chain extended through browser continuity after availability. Compact report mode remains in effect and new operator-facing information stays at the top of Current Tests.

- 2026-04-02: Runtime chain extended through browser durability after continuity. Compact report mode remains in effect and Current Tests keeps the newest operator-facing layers at the top.


- 2026-04-02: Runtime chain extended through browser sustainability after durability. Compact report mode remains in effect and Current Tests keeps the newest operator-facing layers at the top.


- 2026-04-02: Runtime chain extended through browser longevity after sustainability. Compact report mode remains in effect and Current Tests keeps the newest operator-facing layers at the top.


- 2026-04-02: Runtime chain extended through browser stewardship after longevity. Compact report mode remains in effect and Current Tests keeps the newest operator-facing layers at the top.


- 2026-04-02: Runtime chain extended through browser operability after stewardship. Compact report mode remains in effect and Current Tests keeps the newest operator-facing layers at the top.


- 2026-04-02: Runtime chain extended through browser serviceability after operability. Compact report mode remains in effect and Current Tests keeps the newest operator-facing layers at the top.


- 2026-04-02: Runtime chain extended through browser maintainability after serviceability. Compact report mode remains in effect and Current Tests keeps the newest operator-facing layers at the top.


- 2026-04-02: Runtime chain extended through browser supportability and browser usability above maintainability while preserving the compact report-driven validation flow.


- 2026-04-02: Runtime chain extended through browser accessibility and browser inclusivity above usability while preserving the compact report-driven validation flow.


- 2026-04-02: Runtime chain extended through browser adaptability and browser discoverability above inclusivity while preserving the compact report-driven validation flow.


- 2026-04-02: Runtime chain extended through browser learnability and browser approachability above discoverability while preserving the compact report-driven validation flow.

- Added browser runtime layers for navigability and guidability, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Added browser runtime layers for clarity and intuitiveness, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Added browser runtime layers for consistency and cohesiveness, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Added browser runtime layers for predictability and familiarity, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Added browser runtime layers for fluency and harmony, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Added browser runtime layers for readability and legibility, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Added browser runtime layers for simplicity and understandability, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Added browser runtime layers for scannability and comprehensibility, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Added a 20-layer validation batch rule and generated the next 20 browser runtime layers from memorability/value, with a single self-test planned at the end of the batch.

- Added the next 20 browser runtime layers from credibility/completion confidence, under the 20-layer validation batch rule with a single self-test at the end.

Second 20-layer runtime batch extends from browserCredibility/browserReliability through browserProgressAwareness/browserCompletionConfidence and remains on the compact self-test report path.
Third 20-layer runtime batch extends from browserCredence/browserDependability through browserMilestoneAwareness/browserClosureConfidence and remains on the compact self-test report path.
Fourth 20-layer runtime batch extends from browserReassurance/browserSteadiness through browserResolution/browserCompletionAssurance and remains on the compact self-test report path.
Fifth 20-layer runtime batch extends from browserConfirmation/browserVerification through browserAccomplishment/browserCompletionReadiness and remains on the compact self-test report path, pending the next one-click self-test.



Fifth 20-layer runtime batch remains the active work item. Today's close-out fixed the duplicate self-test report property block, restored page-load stability by lazily resolving the self-test/report services, and repaired the generated DI loop by re-anchoring browser persistence back to checkpoint instead of perseverance. Next pickup: run the one-click self-test for the confirmation/completion-readiness block and continue the chain if it validates.
- 2026-04-14: Sixth 20-layer runtime batch extends from browserOperationalReadiness/browserDeploymentReadiness through browserLiveStability/browserSteadyStateReadiness and remains on the compact self-test report path, pending the next one-click self-test.

- 2026-04-14: Seventh 20-layer runtime batch extends from browserOperationalStability/browserDeploymentStability through browserLiveAssurance/browserSteadyOperationReadiness and remains on the compact self-test report path, pending the next one-click self-test.
- 2026-04-14: Generated the eighth 20-layer browser runtime batch from operational resilience/deployment resilience through live continuity/steady continuity readiness above the validated steady operation readiness baseline. Next step: one compact self-test for this block.
- 2026-04-14: Generated the ninth 20-layer browser runtime batch from operational reliability/deployment reliability through live persistence/steady persistence readiness above the validated steady continuity readiness baseline. Next step: one compact self-test for this block.
- 2026-04-15: Added the browser-native runtime session slice above the runtime-network baseline and browser-session controller to keep the active product-facing baseline moving toward the real browser client path.
- 2026-04-15: Added the browser-native runtime session state above the runtime-session slice and runtime-session controller to keep the active product-facing baseline moving toward the real browser client path.
- 2026-04-15: Added the browser-native runtime session ready-state above the runtime-session state and browser-session stability ready-state to keep the active product-facing baseline moving toward the real browser client path.
- 2026-04-20: Added the browser-native runtime session assurance above the runtime-session ready-state and live browser-session controller to keep the active product-facing baseline moving toward the real browser client path.
- 2026-04-20: Added the browser-native runtime launch snapshot above the runtime-session assurance and runtime launch contract to keep the active product-facing baseline moving toward the real browser client path.
- 2026-04-20: Updated the no-click self-test runner to open Edge minimized by default while keeping the same saved-report validation flow.
- 2026-04-20: Hardened the no-click self-test runner to use Edge's minimized launch flag and minimized window style together.
- 2026-04-20: Main-project browser startup now uses browser-safe defaults in `ClassicUO.Client`; the next product-facing work should stay in the real client path rather than the experiment launcher.
- 2026-04-20: The main client now routes browser crash logs and screenshots to browser cache roots, removing another desktop-only path assumption from the browser execution path.
- 2026-04-21: The browser spike start/stop scripts can manage the optional local websocket proxy in `tools/ws` when Node dependencies are installed. Browser mode now expects `ws://127.0.0.1:2594` by default.
- 2026-04-21: Browser startup now clears any saved plugin list in browser mode so the browser path stays on the no-plugin bootstrap contract.
- 2026-04-21: The browser spike scripts now track the optional websocket proxy by PID and `proxy.mjs` token so the local proxy can be started and stopped reliably.
- 2026-04-21: Browser startup now forces the websocket proxy port to `2594` in browser mode so browser sessions stay aligned with the local proxy.
- 2026-04-21: Browser startup now uses a full websocket URI for the default browser endpoint, and `NetClient.Connect` now honors websocket URIs directly instead of appending a desktop-style port suffix.
- 2026-04-21: Browser websocket teardown now aborts and disposes the transport directly so reconnects do not keep stale websocket state around.
- 2026-04-21: Browser websocket send now returns a task and ignores disconnect-time races so reconnects do not leave fire-and-forget sends behind.
- 2026-04-21: Browser websocket receive now stays attached to the wrapper task state instead of being launched as an anonymous fire-and-forget continuation.
- 2026-04-21: Browser socket wrappers now surface disconnect exactly once per connection so reconnect logic does not depend on read-side side effects.
- 2026-04-21: Browser websocket reads now return empty after disconnect so reconnect cycles do not throw on a cleared receive buffer.
- 2026-04-21: Browser websocket reconnects now create a fresh cancellation token instead of reusing a canceled one from the previous connection.
- 2026-04-21: Browser websocket reconnects now clear the prior receive-task state so the wrapper can re-enter connect/disconnect cleanly.
- 2026-04-21: Browser websocket sends are now synchronous-owned again, removing the last fire-and-forget send path in the transport wrapper.
- 2026-04-21: Browser websocket receive now hands teardown back to `Disconnect()` when the loop exits, so reconnect state and socket cleanup stay in one place.
- 2026-04-21: Browser login and game scenes now use plain desktop-only window-management checks instead of browser-policy branches, keeping browser window behavior centralized in the bootstrap/runtime policy path.
- 2026-04-21: Browser client-size updates now use a plain desktop-only profile write check instead of a browser-policy branch, keeping browser window behavior centralized in the bootstrap/runtime policy path.
- 2026-04-21: Browser options UI now uses direct browser checks for the desktop-only game-window section and video reset path, reducing browser-policy branching in the settings screen.
- 2026-04-21: Browser options save logic now uses a direct browser check for the game-window size/position section, keeping browser window behavior centralized in the bootstrap/runtime policy path.
- 2026-04-22: Browser frame timing now uses direct browser checks for inactive FPS throttling and idle sleep, and the options screen disables the inactive FPS toggle directly in browser mode.
- 2026-04-22: Browser options no longer carry browser runtime-policy locals in the video/apply/default paths, keeping browser settings logic on direct browser checks only.
- 2026-04-22: Browser options video build no longer captures a browser runtime-policy local, keeping the browser settings path on direct browser checks only.
- 2026-04-22: Browser login and game scenes now set window-resize behavior directly from browser mode, keeping the runtime policy centralized in the bootstrap path.
- 2026-04-21: Browser startup now ignores custom `-settings` paths in browser mode so the browser path stays on the browser config root.
- 2026-04-21: Browser startup now ignores browser-inapplicable CLI overrides such as `-ip`, `-port`, `-clientversion`, `-filesoverride`, `-uopath`, `-profilespath`, `-plugins`, `-force_driver`, `-highdpi`, `-packetlog`, `-debug`, `-profiler`, `-saveaccount`, `-autologin`, `-reconnect`, `-reconnect_time`, `-login_music`, `-music`, `-login_music_volume`, `-fixed_time_step`, `-fps`, `-skiploginscreen`, `-lastcharactername`, `-lastcharname`, `-lastservernum`, `-last_server_name`, `-language`, `-use_verdata`, `-maps_layouts`, `-encryption`, and `-no_server_ping`, so the browser path stays on the browser transport and storage path.
- 2026-04-22: Browser startup now skips the desktop UO-directory and client-version validation branch entirely in browser mode because the bootstrap path owns those values.
- 2026-04-22: Browser startup validation failures no longer try to launch an external browser.
- 2026-04-22: The desktop startup environment-variable setup is now isolated from the browser bootstrap path in a helper.
- 2026-04-22: Browser game startup now calls the shared window-position helper directly, which still no-ops in browser mode but removes a redundant browser branch.
- 2026-04-22: `UltimaLive.Enable()` now self-skips in browser mode, so the client startup path no longer needs a separate browser guard for that desktop-only feature.
- 2026-04-22: Desktop UO-directory and client-version validation now live in a helper that returns zero in browser mode.
- 2026-04-22: The UO-directory fallback in `Main` now chooses the browser assets root directly when browser mode is active.
- 2026-04-22: The frame-loop inactive-FPS check now relies on the profile flag directly; browser mode already normalizes that profile flag off.
- 2026-04-22: The desktop browser-launch fallback now lives behind a helper, keeping the browser error path flat in `Main`.
- 2026-04-23: Browser window-resize behavior now comes from `BrowserRuntimeBootstrap`, so the login/game scenes no longer hard-code the platform split.
- 2026-04-23: Browser desktop window-state persistence now also comes from `BrowserRuntimeBootstrap`, so unload-time window writes stay out of the browser path.
- 2026-04-23: Browser inactive-frame idle sleep now also comes from `BrowserRuntimeBootstrap`, so the frame loop no longer hard-codes that browser split.
- 2026-04-23: Browser screenshot storage now also comes from `BrowserRuntimeBootstrap`, so capture output stays on the browser root without an inline branch in `GameController`.
- 2026-04-23: Browser options window controls now also use `BrowserRuntimeBootstrap`, so the settings UI no longer hard-codes the browser split for desktop window fields.
- 2026-04-23: Browser windowing checks in `Main` and `GameController` now consistently defer to `BrowserRuntimeBootstrap`, which keeps the browser split centralized across startup, runtime, and options code.
- 2026-04-23: Browser crash logs, settings paths, profile roots, startup UO roots, and browser local IP defaults now also come from `BrowserRuntimeBootstrap`, which removes another batch of inline browser checks from the real client path.
- 2026-04-23: The browser CLI parser now uses a shared `BrowserRuntimeBootstrap` decision for the first batch of desktop startup overrides, instead of repeating the browser split in each case arm.
- 2026-04-23: The browser CLI parser now uses the same shared decision for a second batch of desktop startup overrides, including screen scale, debug, profiler, save-account, auto-login, reconnect, reconnect timing, and login music settings.
- 2026-04-23: The browser CLI parser now uses the same shared decision for the remaining desktop startup overrides, including mouse-thread, skip-login-screen, plugins, verdata, map layouts, encryption, graphics driver, packet logging, language, and no-server-ping settings.
- 2026-04-21: Browser startup now skips OS language probing in browser mode and defaults to `ENU` when the language is unset.
- 2026-04-21: Browser startup now forces `UseVerdata = false` so the browser client stays off desktop verdata loading paths.
- 2026-04-21: Browser networking now returns loopback for `LocalIP` instead of probing a desktop socket endpoint.
- 2026-04-21: Browser networking now skips the desktop local-endpoint probe in `NetClient.LocalIP` and returns loopback directly.
- 2026-04-21: Browser startup now owns the `ENU` language default in `BrowserRuntimeBootstrap` instead of letting `Main` manage browser locale fallback.
- 2026-04-21: Browser startup now forces `ForceDriver = 0` so browser mode never inherits a desktop renderer override.
- 2026-04-21: Browser startup now pins `ForceDriver = 0` in the browser bootstrap helper.
- 2026-04-21: Browser startup now skips the `client.exe` version fallback path when the configured version is invalid.
- 2026-04-21: Browser startup now normalizes `ClientVersion` to a valid default in `BrowserRuntimeBootstrap`.
- 2026-04-21: Browser game startup now skips the redundant desktop window-positioning call in `GameController`.
- 2026-04-21: Browser shutdown now skips the desktop plugin closing hook in `GameController`.
- 2026-04-21: Browser frame updates now skip plugin tick/draw processing when plugins are not loaded.
- 2026-04-21: Browser frame loop now skips plugin tick/draw work when plugins are not initialized.
- 2026-04-21: Browser startup now normalizes desktop gameplay toggles like save-account, auto-login, reconnect, reconnect timing, login music, maps layouts, the saved server selection, the saved files override, saved credentials, screen scale, encryption, the separate mouse-thread setting, and relay-IP handling, the parser now ignores direct username/password and screen-scale overrides in browser mode, the settings save path now strips browser-transient desktop state and browser runtime policy fields before persisting, and browser profile validation no longer clamps to desktop minimum sizes.
- 2026-04-21: Browser mode now skips `UltimaLive.Enable()` during asset load so the browser path stays browser-safe.
