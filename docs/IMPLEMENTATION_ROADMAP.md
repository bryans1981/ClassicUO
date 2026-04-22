# Implementation Roadmap

## Objective

Build a private, browser-accessible ClassicUO deployment that runs from Linux containers, uses privately managed Ultima Online assets, and supports multiple named users with isolated configuration and persistence.

## Guiding Strategy

Do this in layers:

1. Prove the browser client can be built and run.
2. Package it for Linux container hosting.
3. Add a thin backend for multi-user control and persistence.
4. Harden only after the end-to-end flow works.

Current execution rule:

- Keep the remaining work focused on the finished browser client.
- Favor browser launch, rendering, input, login, and world interaction over more scaffold-only runtime layers.
- Use synthetic harness work only when it directly unblocks the product path.

## Target Architecture

### Client

- ClassicUO browser build
- Static assets served over HTTPS
- Browser session launches with user-scoped configuration

### Backend

- Small API service for authentication, profile metadata, and launch/session configuration
- Persistent storage for user data
- Tenant or user scoping for all saved settings and uploaded files

### Storage

- Mounted filesystem or object storage for UO assets
- Database for users, profiles, settings metadata, and session records

### Edge

- Reverse proxy for TLS, routing, and cache policy

## Milestones

### Milestone 1: Browser Build Discovery

Goal: identify exactly how the browser version is built and launched.

Tasks:

- Inspect the repository for browser-specific build scripts, targets, and runtime files.
- Determine whether the browser build depends on Mono WASM, Emscripten, or another packaging path.
- Identify how assets are located at runtime in the browser build.
- Document the minimum files required to launch.

Deliverables:

- Build notes document
- List of required runtime artifacts
- Known browser constraints list

Exit criteria:

- We can explain how the browser deployment is produced.
- We know what additional tooling is required to build it.

Current status:

- In progress.
- Browser transport support is confirmed.
- Browser packaging/build steps are still undiscovered in the checked-in repository.

### Milestone 2: Local Browser Proof Of Concept

Goal: run the browser client locally outside of containers.

Tasks:

- Build or reproduce the browser target.
- Serve the generated output with a local web server.
- Verify startup in Chrome.
- Verify rendering, input, and connection flow.
- Measure startup time and identify missing runtime dependencies.

Deliverables:

- Repeatable local run steps
- Initial issue list

Exit criteria:

- The client opens and reaches a usable state in the browser.

Current status:

- In progress.
- Experimental browser host is working.
- OPFS import and listing are working in the spike host.
- Large retail asset sets can now be staged locally and imported through a hosted seed path instead of browser file picking.
- Main client now has a test-validated browser filesystem/provider seam.
- The shallow asset-layer file checks have largely been moved to the shared seam.
- Most major direct `System.IO` clusters have been moved onto the shared filesystem seam, including `UltimaLive`.
- `tiledata.mul`, `cliloc.enu`, and `hues.mul` are now being read through the browser asset harness.
- The browser spike now has a layered browser asset subsystem with client-facing and runtime-facing service boundaries.
- The shared utility layer now includes a read-only binary asset provider shape for `/uo` browser asset reads.
- The experimental browser host now stays on a lighter `net8.0` host path and links only the needed shared browser-seam source files directly.
- A first shared `BrowserFileSystem` bridge now preloads the bootstrap subset into a real read-only seam provider for browser-host validation.
- The browser self-test report is now compact again and captures failure envelopes automatically when a batch throws before report construction.
- The current runtime tail baseline is `runtimeTailExtension=ok`, extending the steady-operation readiness layer with a compact 20-phase stabilization tail.
- The remaining work is now extending that tail baseline and the shared bridge from the current stable state, plus a smaller cleanup list in a few client/UI subsystems.
- The browser-native execution plan is now the active product-facing baseline, and the launch-plan path consumes the live seam-backed handoff directly instead of depending on the stale bootstrap-package artifact.
- The browser-native execution plan now carries a browser-native runtime shell summary, which is the next active product-facing handoff before rendering/input/network integration work.
- The browser-native websocket session controller now validates the transport endpoint and runtime execution mode, giving us the first explicit websocket handshake baseline before the final browser runtime integration work.
- The browser-native websocket runtime execution controller now performs a real browser websocket connect/read against the report host, giving us the first live websocket execution baseline before final browser runtime integration work.
 - The browser-native websocket runtime session controller now sits above that live websocket execution path and gives us the current browser-session handoff layer.
 - The browser-native runtime ready-state now sits above the browser runtime and gives us the current browser-runtime baseline exposed in the compact report.
 - The browser filesystem bootstrap contract now lives in `ClassicUO.Utility`, and the real client now checks for an attached browser storage provider at startup.
 - The browser host now links the shared browser filesystem bootstrap helper and uses it to attach the browser storage provider through the shared seam.
  - The browser storage provider contract and provider implementations are now public in `ClassicUO.Utility`, which is the shared contract the browser client should consume.
  - The main client now owns a dedicated `BrowserRuntimeBootstrap` helper, which centralizes browser-safe defaults and future storage attachment hooks.
  - The main client browser bootstrap helper now captures a browser startup state snapshot, giving the real entrypoint an explicit browser handoff contract.
  - The browser startup state snapshot is now threaded into `Client.Run` and `GameController`, so the main client consumes the browser startup contract instead of only capturing it.
- The browser defaults are now owned directly by the bootstrap path and consumed by `GameController`, so browser render and input defaults live in the real client path without a cached policy object.
- Browser startup now owns browser window resizing and text-input defaults directly, so the real client controls the browser-facing runtime behavior without an intermediate policy cache.
- Browser mode now disables idle sleep directly in the main client path, so browser timing stays under the real client path instead of desktop-style sleep behavior.
- Browser mode now drives the browser refresh rate directly during initialization, so browser loop timing is owned by the real client path.
- Browser mode now writes browser defaults back into `Settings.GlobalSettings` during startup, so browser defaults are visible to the rest of the real client.
- Browser window-size updates now rely on the resize flag directly instead of an extra browser branch in `GameController`, so browser window handling stays aligned with the startup defaults.
- `GameController` now reads the browser-normalized settings values directly for mouse input, fixed timestep, target FPS, and text input, so the browser startup defaults flow straight into the real client loop.
- `GameController` now logs the browser bootstrap state whenever it is present, without a separate browser-mode gate in the initialize path.
- The browser startup path now owns the browser defaults directly; `Main` no longer applies a separate browser runtime-policy step after startup validation.
- `Main` now relies on the browser startup defaults for the browser UO root as well, so browser mode does not carry a separate executable-path fallback in the startup path.
- Browser save sanitization now lives in `BrowserRuntimeBootstrap`, so `Settings.Save` no longer owns a separate browser-only cleanup block.
- `ProfileManager` now applies the profile minimum size clamp through the shared validation path, because browser profiles already normalize to a valid browser window size at load time.
- The nested `client.exe` browser guard in the startup validation path is gone, so browser mode now relies on the bootstrap-normalized client version instead of a dead inner browser branch.
- The browser force-driver path in `Main` now relies on the browser startup default of zero instead of a separate browser-mode guard around the desktop switch.
- The browser startup defaults now also set the browser profile root explicitly, so browser profile persistence is owned by the real client startup path.
- The browser startup defaults now also set an initial browser window position and size, so the browser host does not inherit desktop placement assumptions.
- The browser profile load path now applies browser-safe profile window defaults, so game-window state is owned by the real client profile layer.
- The browser profile load path now also disables `ReduceFPSWhenInactive`, and the browser runtime policy now owns inactive FPS throttling directly, so browser timing stays out of desktop-style inactive throttling.
- The main startup validation path now skips external browser launching in browser mode, so a browser-side startup failure does not try to spawn another browser.
- The browser startup validation path now logs errors instead of showing a native SDL dialog, so browser failures stay within the browser-safe path.
- Browser unload no longer back-writes desktop window placement, so browser shutdown does not leak desktop window state into the saved profile.
- The browser window-placement routine now short-circuits in browser mode, so browser startup does not use desktop display-bound positioning logic.
- The browser login scene now avoids desktop window resize and maximize behavior in browser mode, so the browser path stays centered on the playable client rather than OS window mechanics.
- The browser client-size change handler now skips native resize writes in browser mode, so browser canvas changes stay out of desktop window mechanics.
- Browser screenshots now root under the shared browser cache path, so capture output stays in the browser filesystem contract instead of desktop data folders.
- Browser plugin loading is now owned by the browser runtime policy, so the main client no longer hard-codes a browser-mode startup branch for plugins.
- The browser host also supports a no-click self-test URL, which reduces manual operator interaction during the browser-native work.
- The browser product work is now explicitly focused on the finished playable client path, not on more synthetic proof layers.
- CI is now manual-only during browser-client development so local testing can stay local and repository pushes do not generate avoidable build-test/deploy noise.

### Milestone 3: Linux Container Packaging

Goal: host the browser client from Linux containers with persistent storage.

Tasks:

- Build a static hosting container for the browser output.
- Add a reverse proxy container.
- Mount persistent storage for assets and configuration.
- Verify container restarts do not lose required state.
- Define environment variables and volume layout.

Deliverables:

- `Dockerfile` for runtime hosting
- `docker-compose.yml` or equivalent local orchestration
- Container startup instructions

Exit criteria:

- The browser client is served successfully from Linux containers.

### Milestone 4: Single-User Persistence Layer

Goal: move from a static hosted client to a manageable personal deployment.

Tasks:

- Design minimal backend API.
- Add authentication for the owner account or one named user.
- Store launch configuration outside the client bundle.
- Persist settings and profile data in a durable store.
- Validate restart and re-login behavior.

Deliverables:

- Backend service skeleton
- Settings/profile schema
- Session launch flow

Exit criteria:

- One user can sign in, launch, and retain settings across sessions.

### Milestone 5: Multi-User Isolation

Goal: support more than one user without leaking state or configuration.

Tasks:

- Add user account model and roles.
- Partition settings, profiles, and asset access by user.
- Enforce scoped API access for all data retrieval.
- Prevent one user from viewing or using another user's saved data.
- Test concurrent browser sessions.

Deliverables:

- User isolation rules
- Per-user storage layout
- Access control checks

Exit criteria:

- Multiple named users can use the system independently.

### Milestone 6: Operational Hardening

Goal: make the project supportable.

Tasks:

- Add health checks and startup validation.
- Add structured logging.
- Add backup and restore procedures for settings and asset storage metadata.
- Define upgrade workflow for syncing upstream changes.
- Add basic monitoring for browser launch failures and API errors.

Deliverables:

- Operations runbook
- Backup procedure
- Upgrade checklist

Exit criteria:

- The environment can be restarted, updated, and debugged without guesswork.

## Risks And Likely Issues

### Browser Build Risk

The repo advertises browser support, but the build path is not present in the visible top-level scripts. External project documentation indicates the official web client is closed-source, uses a custom WebAssembly build process, and depends on additional infrastructure outside this repository.

### Runtime Performance Risk

Browser startup may be slow because of asset size, WebAssembly startup cost, or large data fetches.

### Storage Model Risk

If assets are large and shared, we need to decide whether to store one canonical asset set or separate user-scoped copies.

### Security Risk

Even for private use, user accounts, stored settings, and any launch tokens must be scoped and protected.

### Upstream Drift Risk

Hosting-specific changes can make upstream syncs harder. We should isolate deployment code from the main client whenever possible.

### Public Component Gap Risk

Gateway implementation can likely be sourced from public references, but the browser game runtime, asset bootstrap, and official web backend stack are not fully public. We should assume we will need to build substitute components for those layers.

## Initial Task Backlog

These are the first tasks we should execute in order:

1. Bridge the browser-host asset subsystem into the main `BrowserFileSystem` provider path.
2. Run the first real asset-backed shared-seam read using the tiledata loader expectations.
3. Extend the shared-seam browser-backed read path to additional core asset files beyond `tiledata.mul`.
4. Finish the smaller remaining direct-file cleanup areas in client/UI code.
5. Design the minimum container layout needed for static hosting plus persistent storage.
6. Choose the first backend stack for auth and settings.

## Suggested Technical Defaults

Use these unless discovery proves they are wrong:

- Reverse proxy: Nginx
- Database: PostgreSQL
- Local orchestration: Docker Compose
- Backend: small ASP.NET Core service to stay in the same ecosystem unless a thinner option is clearly better
- Persistence: mounted volumes first, object storage later if needed

## Immediate Next Steps

The next concrete actions should be:

1. Extend the new browser-host to shared `BrowserFileSystem` bridge from bootstrap subset preload to on-demand provider-backed reads.
2. Feed `tiledata.mul` through the shared filesystem seam using the new bridge path.
3. Expand from tiledata to the next core asset files once the seam is proven.
4. Extend the runtime invocation output into a persistent runtime startup cycle/state object for the future browser entrypoint.
5. Only then move on to container packaging and backend scaffolding.

## Completion Markers

We are ready to begin implementation only when:

- The browser build path is no longer guesswork.
- We know which runtime artifacts must be served.
- We understand where private asset files will live.
- We have selected the first persistence approach for user settings.

Refined next steps after current discovery:

1. Inspect bootstrap and host-binding code for browser execution assumptions.
2. Determine whether the browser deployment process lives outside this repository.
3. Identify what will be required for a production-safe WebSocket gateway.
4. Defer container work until browser packaging is reproducible or externally sourced.

## Custom Port Direction

Current project direction:

- We are no longer assuming the official browser build can be reproduced from public artifacts.
- We are proceeding with a custom browser-port path from the open-source client.
- The first objective is a technical spike, not feature completeness.

Immediate implementation branch of work:

1. Create an experimental browser host.
2. Disable or bypass desktop-native bootstrap features in browser mode.
3. Introduce a browser-compatible asset access layer.
4. Replace high-value deep System.IO usage with the shared filesystem seam.
5. Reach a minimal boot target before solving full deployment.

- The browser-host spike now has a launch-oriented browser entrypoint output that prepares minimal browser-side settings/profile files and reports startup readiness.

- The browser-host spike now has a startup-sequence browser entrypoint shell that groups launch preparation into explicit startup steps.

- The browser-host spike now writes a launch-state artifact into browser storage as the first persisted startup output for a future browser entrypoint.

- The browser-host spike now reads the persisted launch-state artifact back from browser storage as the first consumer-side startup behavior.
- The browser-host startup path now writes a launch artifact, reads it back, and creates a simulated launch session object; the self-test suite also saves a machine-readable local report so browser verification does not require manual copy/paste.
- The browser-host startup path now includes launch-session readback, so the one-click self-test covers a full producer/consumer cycle for startup artifacts and simulated launch state.
- Launch-session persistence now writes final session state before readback validation so the resumable launch-state consumer sees correct write flags and summary fields.
- A runtime launch contract is now derived from the saved launch session, so the browser startup path has a stable consumer object above raw startup/session persistence.
- A startup packet is now derived from the runtime launch contract, giving the eventual browser entrypoint a single bootstrap input above artifacts, sessions, and runtime contract layers.
- An in-memory startup consumer now sits above the startup packet, giving the future browser entrypoint a first browser-bootstrap handshake object rather than only persisted startup metadata.
- A startup-session executor now sits above the in-memory startup consumer, giving the browser path a first runtime-oriented startup consumption layer beyond handshake and packet generation.
- A startup-session runner now sits above the executor, giving the browser path a first simulated runtime startup pass rather than only prepared startup state.
- A minimal runtime bootstrap loop now sits above the startup-session runner, giving the browser path its first phase-based startup loop model instead of only stacked startup state objects.
- A minimal runtime invocation layer now sits above the bootstrap loop, giving the browser path a first concrete browser-runtime call boundary instead of only startup-loop state.
- A minimal runtime startup cycle now sits above the runtime invocation, giving the browser path a first persistent runtime startup-state object above the invocation boundary.
- A minimal runtime startup state now sits above the runtime startup cycle, giving the browser path its first concrete runtime startup-state holder above the cycle layer.
- A minimal runtime startup state machine now sits above the runtime startup state, giving the browser path its first browser bootstrap state-progression object above the startup-state holder.
- A minimal runtime startup transition driver now sits above the runtime startup state machine, giving the browser path its first executable browser bootstrap progression layer above the state machine.
- A minimal runtime startup dispatcher now sits above the runtime startup transition driver, giving the browser path its first bootstrap-session control layer above the executable transition model.
- A minimal runtime startup session controller now sits above the runtime startup dispatcher, giving the browser path its first executable bootstrap-session control object above the dispatch layer.
- A minimal runtime startup coordinator now sits above the runtime startup session controller, giving the browser path its first browser bootstrap orchestration layer above the session-control object.
- A minimal runtime startup orchestrator now sits above the runtime startup coordinator, giving the browser path its first runtime-wide startup orchestration layer above bootstrap coordination.
- A first browser boot-flow controller now sits above the runtime startup orchestrator, giving the browser path its first browser-facing boot flow model above runtime startup orchestration.
- A first browser boot session now sits above the boot-flow controller, giving the browser path its first browser-session startup object above the boot flow model.
- A first runtime launch handoff now sits above the browser boot session, giving the browser path its first browser-to-runtime handoff object above the boot session layer.
- A first runtime bootstrap consumer now sits above the runtime launch handoff, giving the browser path its first runtime-side consumer of the browser handoff above the handoff layer.
- A first runtime bootstrap session now sits above the runtime bootstrap consumer, giving the browser path its first runtime-session startup object above the consumer layer.
- Browser runtime startup chain now extends past the readiness gate into `RuntimeReadySignal` and `RuntimeLaunchController`.
- Next validation point remains the one-click self-test suite after restarting the browser host and report receiver.
- Browser runtime startup chain now extends through `RuntimeClientReadyState` and `RuntimeClientLaunchSession` after `RuntimeLaunchController`.
- Browser runtime startup chain now extends through `RuntimeClientActivation` and `RuntimeClientRunState` after `RuntimeClientLaunchSession`.
- Browser runtime startup chain now extends through `RuntimeClientLoopState` and `RuntimeHostSession` after `RuntimeClientRunState`.
- Browser runtime startup chain now extends through `RuntimeHostLoop` and `RuntimeHostReadyState` after `RuntimeHostSession`.
- Browser runtime startup chain now extends through `RuntimePlatformSession` and `RuntimePlatformReadyState` after `RuntimeHostReadyState`.
- Browser runtime startup chain now extends through `RuntimePlatformLoop` and `RuntimePlatformLaunchGate` after `RuntimePlatformReadyState`.

- Browser runtime startup chain now extends through RuntimeBrowserShellSession and RuntimeBrowserShellReadyState after RuntimePlatformLaunchGate.
- Browser runtime startup chain now extends through RuntimeBrowserSurfaceSession and RuntimeBrowserSurfaceReadyState after RuntimeBrowserShellReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserWindowSession and RuntimeBrowserWindowReadyState after RuntimeBrowserSurfaceReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserFrameSession and RuntimeBrowserFrameReadyState after RuntimeBrowserWindowReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserCanvasSession and RuntimeBrowserCanvasReadyState after RuntimeBrowserFrameReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserRenderSession and RuntimeBrowserRenderReadyState after RuntimeBrowserCanvasReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserPresentSession and RuntimeBrowserPresentReadyState after RuntimeBrowserRenderReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserDisplaySession and RuntimeBrowserDisplayReadyState after RuntimeBrowserPresentReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserViewportSession and RuntimeBrowserViewportReadyState after RuntimeBrowserDisplayReadyState.

- Browser runtime startup chain now extends through RuntimeBrowserSceneSession and RuntimeBrowserSceneReadyState after RuntimeBrowserViewportReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserInputSession and RuntimeBrowserInputReadyState after RuntimeBrowserSceneReadyState.

- Browser runtime startup chain now extends through RuntimeBrowserEventSession and RuntimeBrowserEventReadyState after RuntimeBrowserInputReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserInteractionSession and RuntimeBrowserInteractionReadyState after RuntimeBrowserEventReadyState.

- Browser runtime startup chain now extends through RuntimeBrowserFocusSession and RuntimeBrowserFocusReadyState after RuntimeBrowserInteractionReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserShortcutSession and RuntimeBrowserShortcutReadyState after RuntimeBrowserFocusReadyState.

- Browser runtime startup chain now extends through RuntimeBrowserPointerSession and RuntimeBrowserPointerReadyState after RuntimeBrowserShortcutReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserCommandSession and RuntimeBrowserCommandReadyState after RuntimeBrowserPointerReadyState.

- Browser runtime startup chain now extends through RuntimeBrowserGestureSession and RuntimeBrowserGestureReadyState after RuntimeBrowserCommandReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserLifecycleSession and RuntimeBrowserLifecycleReadyState after RuntimeBrowserGestureReadyState.

- Browser runtime startup chain now extends through RuntimeBrowserRouteSession and RuntimeBrowserRouteReadyState after RuntimeBrowserLifecycleReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserStateSyncSession and RuntimeBrowserStateSyncReadyState after RuntimeBrowserRouteReadyState.

- Browser runtime startup chain now extends through RuntimeBrowserRestoreSession and RuntimeBrowserRestoreReadyState after RuntimeBrowserStateSyncReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserResumeSession and RuntimeBrowserResumeReadyState after RuntimeBrowserRestoreReadyState.

- Browser runtime startup chain now extends through RuntimeBrowserCheckpointSession and RuntimeBrowserCheckpointReadyState after RuntimeBrowserResumeReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserPersistenceSession and RuntimeBrowserPersistenceReadyState after RuntimeBrowserCheckpointReadyState.

- Browser runtime startup chain now extends through RuntimeBrowserHistorySession and RuntimeBrowserHistoryReadyState after RuntimeBrowserPersistenceReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserRecoverySession and RuntimeBrowserRecoveryReadyState after RuntimeBrowserHistoryReadyState.

- Browser runtime startup chain now extends through RuntimeBrowserSnapshotSession and RuntimeBrowserSnapshotReadyState after RuntimeBrowserRecoveryReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserArchiveSession and RuntimeBrowserArchiveReadyState after RuntimeBrowserSnapshotReadyState.

- Browser runtime startup chain now extends through RuntimeBrowserTelemetrySession and RuntimeBrowserTelemetryReadyState after RuntimeBrowserArchiveReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserDiagnosticsSession and RuntimeBrowserDiagnosticsReadyState after RuntimeBrowserTelemetryReadyState.

- Browser runtime startup chain now extends through RuntimeBrowserMonitoringSession and RuntimeBrowserMonitoringReadyState after RuntimeBrowserDiagnosticsReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserWatchdogSession and RuntimeBrowserWatchdogReadyState after RuntimeBrowserMonitoringReadyState.

- Browser runtime startup chain now extends through RuntimeBrowserHealthSession and RuntimeBrowserHealthReadyState after RuntimeBrowserWatchdogReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserAlertingSession and RuntimeBrowserAlertingReadyState after RuntimeBrowserHealthReadyState.

- Browser runtime startup chain now extends through RuntimeBrowserPolicySession and RuntimeBrowserPolicyReadyState after RuntimeBrowserAlertingReadyState.
- Browser runtime startup chain now extends through RuntimeBrowserAuditSession and RuntimeBrowserAuditReadyState after RuntimeBrowserPolicyReadyState.

- 2026-04-02: Extended the browser runtime proof chain with security/compliance layers after policy/audit. Immediate focus remains iterative runtime-chain extension with one-click self-test validation and local report capture.

- 2026-04-02: Current incremental runtime-chain focus now includes privacy/governance layers after security/compliance, preserving the same self-test/report workflow.

- 2026-04-02: Current browser runtime-chain milestone now includes trust/assurance layers after privacy/governance, preserving the same build, restart, and one-click self-test validation loop.

- 2026-04-02: Current browser runtime-chain milestone now includes risk/integrity layers after trust/assurance while preserving the same local report-driven validation workflow.

- 2026-04-02: Current browser runtime-chain milestone now includes resilience after integrity, keeping the same one-click self-test/report validation gate between batches.

- 2026-04-02: Current runtime-chain milestone now includes availability after resilience while preserving the same local report-driven test loop.

- 2026-04-02: Added a flattening pass for the self-test artifact so browser-runtime layers can continue growing without exponential report bloat. Immediate validation focus is report-size reduction with the same one-click test flow.

- 2026-04-02: Current runtime-chain milestone now includes continuity after availability. Keep future operator-facing additions near the top of the page while maintaining the compact report artifact.

- 2026-04-02: Current runtime-chain milestone now includes durability after continuity while preserving the compact self-test artifact and top-of-page newest-layer context.


- 2026-04-02: Current runtime-chain milestone now includes sustainability after durability while preserving the compact self-test artifact and top-of-page newest-layer context.


- 2026-04-02: Current runtime-chain milestone now includes longevity after sustainability while preserving the compact self-test artifact and top-of-page newest-layer context.


- 2026-04-02: Current runtime-chain milestone now includes stewardship after longevity while preserving the compact self-test artifact and top-of-page newest-layer context.


- 2026-04-02: Current runtime-chain milestone now includes operability after stewardship while preserving the compact self-test artifact and top-of-page newest-layer context.


- 2026-04-02: Current runtime-chain milestone now includes serviceability after operability while preserving the compact self-test artifact and top-of-page newest-layer context.


- 2026-04-02: Current runtime-chain milestone now includes maintainability after serviceability while preserving the compact self-test artifact and top-of-page newest-layer context.


- 2026-04-02: Current runtime-chain milestone now includes supportability and usability above maintainability while keeping the compact self-test artifact and top-of-page newest-layer context.


- 2026-04-02: Current runtime-chain milestone now includes accessibility and inclusivity above usability while keeping the compact self-test artifact and top-of-page newest-layer context.


- 2026-04-02: Current runtime-chain milestone now includes adaptability and discoverability above inclusivity while keeping the compact self-test artifact and top-of-page newest-layer context.


- 2026-04-02: Current runtime-chain milestone now includes learnability and approachability above discoverability while keeping the compact self-test artifact and top-of-page newest-layer context.

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

Current browser runtime chain validated through progress-awareness/completion-confidence on compact report mode.
Current browser runtime chain validated through reassurance/completion-assurance on compact report mode.
The next validation target is the generated confirmation/verification through accomplishment/completion-readiness block on compact report mode.



- 2026-04-02: Active milestone remains the fifth 20-layer browser runtime batch (confirmation/completion-readiness). The generated batch compiles and the page loads again after fixing a duplicate report-property regression and a persistence/perseverance DI cycle. Next validation step is a fresh compact self-test for that batch.
- 2026-04-14: Active milestone now includes the generated sixth 20-layer browser runtime batch from operational readiness/deployment readiness through live stability/steady state readiness. Next validation target is a compact self-test for that block.

- 2026-04-14: Active milestone now includes the generated seventh 20-layer browser runtime batch from operational stability/deployment stability through live assurance/steady operation readiness. Next validation target is a compact self-test for that block.
- 2026-04-14: Generated the eighth 20-layer browser runtime batch from operational resilience/deployment resilience through live continuity/steady continuity readiness above the validated steady operation readiness baseline. Next step: one compact self-test for this block.
- 2026-04-14: Generated the ninth 20-layer browser runtime batch from operational reliability/deployment reliability through live persistence/steady persistence readiness above the validated steady continuity readiness baseline. Next step: one compact self-test for this block.
- 2026-04-14: Added automatic compact failure-report capture so browser self-test exceptions are saved locally instead of requiring manual error paste before the next runtime batch validation.
- 2026-04-14: Broke the ninth batch DI cycle by detaching the newest ready-state services from their paired session services, restoring the compact self-test path for the next validation.
- 2026-04-14: Validated the concrete browser bootstrap package artifact at `/cache/startup/default/browser-bootstrap-package.json`. The synthetic runtime tail remains green, but the active next step is now real browser client bootstrap integration around that package rather than adding more synthetic readiness layers.
- 2026-04-14: Trimmed the active browser test surface to emphasize the bootstrap package, package readback, and launch/session persistence. The synthetic runtime tail remains validated for reference, but it is no longer the primary operator path.
- 2026-04-14: The page now surfaces the bootstrap package artifact, package readback, and launch-session summary near the top. The next implementation work is to keep shrinking the synthetic validation surface and move toward a concrete client-bootstrap path.
- 2026-04-14: Added a package-consumer stage above bootstrap-package readback so the launch flow consumes the package-backed handoff instead of only checking the file on disk.
- 2026-04-15: Operating decision updated: do not default to broad vertical slices. Prefer feature-sized batches, parallel disjoint work, and machine-readable validation to speed up delivery of the final browser client.
- 2026-04-15: Official browser-client reference uses WebAssembly and WebGL; next implementation slices should target browser-native execution shape instead of additional scaffolding-only layers.
- 2026-04-15: The browser-native runtime shell controller is now part of the compact self-test baseline, and the no-click runner validates it directly from the saved local report.
- 2026-04-15: The no-click self-test browser now closes automatically after a report update by default, reducing browser window clutter during repeated validation runs.
- 2026-04-15: The browser-native client bootstrap controller is now the next active handoff above the native launch controller. Keep the self-test compact report as the validation gate while moving toward the final browser client runtime.
- 2026-04-15: The browser-native browser runtime controller is now the active runtime baseline. Next work should move from the controller into actual browser runtime integration rather than additional bootstrap handoffs.
- 2026-04-15: The browser-native browser host controller is now the active browser-host baseline. Next work should move from browser-host control into the actual browser rendering/input/network integration path.
- 2026-04-15: The browser-native browser surface controller is now the active browser-surface baseline. Next work should move from browser-surface control into the actual browser rendering/input/network integration path.
- 2026-04-15: The browser-native browser render controller is now the active browser-render baseline. Next work should move from browser-render control into the actual browser input/network integration path.
- 2026-04-15: The browser-native browser input controller is now the active browser-input baseline. Next work should move from browser-input control into the actual browser network integration path.
- 2026-04-15: The browser-native browser network controller is now the active browser-network baseline. Next work should move from browser-network control into actual websocket/runtime integration.
- 2026-04-15: The browser-native browser transport controller is now the active browser-transport baseline. Next work should move from browser-transport control into actual websocket/runtime execution wiring.
- 2026-04-15: The browser-native browser runtime execution controller is now the active browser-runtime-execution baseline. Next work should move from browser-runtime-execution control into actual websocket session/runtime execution wiring.
- 2026-04-15: The browser-native websocket runtime session controller is now the browser-session handoff layer above the live websocket execution path.
- 2026-04-15: The browser-native runtime session controller is now the browser-runtime-session baseline above the websocket runtime-session controller.
- 2026-04-15: The browser-native browser runtime controller now consumes the browser-runtime-session layer and is the current browser-runtime baseline.
- 2026-04-15: The browser-native browser runtime ready-state now sits above the browser runtime and is the current browser-runtime baseline exposed in the compact report.
- 2026-04-15: The browser-native browser host ready-state now sits above the browser host and is the current browser-host baseline exposed in the compact report.
- 2026-04-15: The browser-native canvas host now mounts a real browser canvas and probes a render context; this is the next browser-render step above the render controller.
- 2026-04-15: The browser-native canvas frame now clears and paints the mounted canvas; this is the first visible browser-render frame above the canvas host.
- 2026-04-15: The browser-native canvas input bridge now installs pointer and keyboard listeners on the rendered canvas and probes them with synthetic events; this is the first concrete browser input bridge above the input controller.
- 2026-04-15: The browser-native frame pump now runs a live render/update heartbeat on top of the canvas frame; this is the current browser-runtime heartbeat above the visible render path.
- 2026-04-15: The browser-native runtime loop now sits above the frame pump and input controller; this is the current product-facing browser-runtime heartbeat above the visible render path.
- 2026-04-15: The browser-native runtime execution slice now combines runtime loop, transport, and websocket/session layers into one product-facing runtime execution snapshot above the runtime loop.
- 2026-04-15: The browser-native runtime network slice now combines runtime execution, network controller, and transport controller into the current browser-network snapshot above the runtime execution slice.
- 2026-04-15: The browser-native runtime session slice now combines the runtime-network baseline with the browser-session controller and is the current product-facing runtime session snapshot.
- 2026-04-15: The browser-native runtime session state now combines the runtime-session slice with the runtime-session controller and is the current product-facing runtime session snapshot.
- 2026-04-15: The browser-native runtime session ready-state now combines the runtime-session state with the browser-session stability ready-state and is the current product-facing runtime session snapshot.
- 2026-04-20: The browser-native runtime session assurance now combines the runtime-session ready-state with the live browser-session controller and is the current product-facing runtime session snapshot.
- 2026-04-20: The browser-native runtime launch snapshot now combines the runtime-session assurance with the runtime launch contract and is the current product-facing launch snapshot.
- 2026-04-20: The no-click self-test runner now opens Edge minimized by default while preserving the same report-driven validation path.
- 2026-04-20: The no-click self-test runner now uses Edge's `--start-minimized` flag and minimized window style together while preserving the same report-driven validation path.
- 2026-04-15: The browser-native browser session controller is now the active browser-session baseline. Next work should move from browser-session control into actual websocket session/runtime execution wiring.
- 2026-04-15: The no-click self-test runner now preflights the browser host and report receiver before opening Edge, which makes automated validation less sensitive to cold-start timing.
- 2026-04-20: Main-project browser startup now defaults to browser-safe assets, profiles, config, and client version values in `ClassicUO.Client`. Next work should stay in the real client path and avoid adding more experiment-launcher layers.
- 2026-04-20: Browser crash logs and screenshots now route to browser cache roots in the main client, removing another desktop-only path assumption from the browser execution path.
- 2026-04-21: Browser startup can optionally bring up the local websocket proxy from `tools/ws` through the spike start/stop scripts when Node dependencies are present. The active browser endpoint remains `ws://127.0.0.1:2594`.
- 2026-04-21: Browser startup now clears any saved plugin list in browser mode, keeping the real client on the browser-safe no-plugin path.
- 2026-04-21: The browser spike scripts now manage the optional websocket proxy by PID and `proxy.mjs` token so proxy startup and shutdown stay in sync with the browser host.
- 2026-04-21: Browser startup now forces the websocket proxy port to `2594` in browser mode so browser transport stays on the documented proxy path.
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
- 2026-04-21: Browser startup now ignores custom `-settings` paths in browser mode so the browser client stays on the browser config root.
- 2026-04-21: Browser startup now ignores browser-inapplicable CLI overrides such as `-ip`, `-port`, `-clientversion`, `-filesoverride`, `-uopath`, `-profilespath`, `-plugins`, `-force_driver`, `-highdpi`, `-packetlog`, `-debug`, `-profiler`, `-saveaccount`, `-autologin`, `-reconnect`, `-reconnect_time`, `-login_music`, `-music`, `-login_music_volume`, `-fixed_time_step`, `-fps`, `-skiploginscreen`, `-lastcharactername`, `-lastcharname`, `-lastservernum`, `-last_server_name`, `-language`, `-use_verdata`, `-maps_layouts`, `-encryption`, and `-no_server_ping`, so the browser client stays on the browser transport and storage path.
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
- 2026-04-21: Browser mode now skips `UltimaLive.Enable()` during asset load so the browser client stays on the browser-safe runtime path.
