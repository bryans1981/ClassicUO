# Implementation Roadmap

## Objective

Build a private, browser-accessible ClassicUO deployment that runs from Linux containers, uses privately managed Ultima Online assets, and supports multiple named users with isolated configuration and persistence.

## Guiding Strategy

Do this in layers:

1. Prove the browser client can be built and run.
2. Package it for Linux container hosting.
3. Add a thin backend for multi-user control and persistence.
4. Harden only after the end-to-end flow works.

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
- The remaining work is now extending that bridge from bootstrap preloading to on-demand shared-seam reads plus a smaller cleanup list in a few client/UI subsystems.

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
