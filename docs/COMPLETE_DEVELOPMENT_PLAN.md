# Complete Development Plan

## Purpose

This document is the execution plan for the browser product version of ClassicUO. It exists to keep the work pointed at the finish line: a working web application that runs the game, not a stream of side cleanup tasks.

## Scope Rule

Work is only active if it directly moves the browser client toward:

1. browser launch
2. asset loading
3. rendering
4. input
5. websocket/session flow
6. login
7. world interaction
8. browser-safe persistence
9. container hosting
10. multi-user isolation

If a task does not help one of those outcomes, record it in the deferred side-work list and move on unless it is a critical blocker.

## Current Baseline

What is already proven:

- browser-safe startup defaults are centralized in the main client
- browser filesystem/storage seam exists
- browser transport and websocket cleanup are in place
- the main client can publish a `browser-wasm` app bundle locally with the WebAssembly workload installed
- browser canvas host, frame pump, and input bridge exist in the spike path
- local no-click self-test and compact report saving work
- browser-only desktop assumptions have been reduced across startup, scenes, settings, and transport
- browser asset staging now exists in `browser-assets/uo/versions/<version>/`
- the selected asset version can be copied into the browser bundle as `/uo`
- the live browser shell now suppresses nonessential tracing unless debug mode is enabled
- browser canvas input is hardened for play: focus-on-pointer, no page scroll, no context menu
- browser login ping polling is disabled so unsupported browser network APIs do not block shard list flow
- browser asset version folders are treated as local-only drops and should not be pushed with code commits

What is not finished:

- the main client is not yet a playable browser client
- login and world flow are not yet proven end to end in the browser product path
- containerized deployment is not yet the default runtime path
- multi-user isolation is not yet implemented end to end

## Milestone Plan

### Milestone 1: Live Browser Client Viability

Goal: the browser client launches reliably and reaches a useful in-browser runtime without relying on desktop-only behavior.

Steps:

1. Keep browser startup owned by `BrowserRuntimeBootstrap`.
2. Keep desktop-only branches out of browser startup, settings, and scene load/unload paths unless they block launch.
3. Keep browser-safe asset, profile, settings, and transport defaults centralized.
4. Preserve local automated validation for the browser path.

Done when:

- browser startup is deterministic
- browser-safe storage and transport are stable
- the browser client can launch without desktop assumptions leaking in

Current status:

- mostly complete for startup, transport, and standalone local serving
- local `browser-wasm` publish now produces `bin/<Configuration>/net10.0/browser-wasm/AppBundle`
- `scripts/browser-client-start.ps1` now publishes and serves the product browser bundle at `http://localhost:5110/`
- websocket transport now avoids raw TCP socket setup in browser mode and uses browser-native `ClientWebSocket` connect behavior
- browser logging avoids unsupported console color APIs
- browser startup attaches temporary rooted in-memory storage when no host storage provider is configured, so settings/profile writes no longer block launch
- `scripts/browser-client-start.ps1 -OpenBrowser` now launches a minimized external browser window and `scripts/browser-client-stop.ps1` closes it, so the repo owns the browser lifecycle instead of the Codex in-app panel
- main-client browser startup now links native SDL2/FNA3D and creates a WebGL2/OpenGL ES 3 renderer in Chrome headless
- browser asset staging now has a repo location at `browser-assets/uo/versions/<asset-version>/` and publish-time copy/manifest support into the bundle
- browser file loading now avoids `MemoryMappedFile` in browser mode and falls back to stream reads for UO files
- the latest browser self-test report is green for the browser-spike harness
- `scripts/browser-client-start.ps1` now also launches the local WebSocket proxy when `tools/ws/node_modules` exists, which is the next local prerequisite for session flow testing
- the current remaining product work is proving the full main-client gameplay loop in browser mode with the real asset set

### Milestone 2: Real Browser Rendering

Goal: the actual client draws and updates correctly in a browser canvas path.

Steps:

1. Ensure the browser client can render a live frame in the browser path.
2. Ensure canvas resize behavior follows browser-safe settings.
3. Ensure the frame loop stays stable under browser timing.
4. Remove any remaining desktop-only render assumptions that block browser drawing.

Done when:

- the game draws reliably in browser mode
- resize and redraw behavior work without desktop window semantics
- rendering stays stable across repeated browser starts

Current status:

- active product milestone
- partially proven in the spike path
- native browser renderer is now linked through SDL2/FNA3D static archives
- Chrome headless confirms `FNA3D Driver: OpenGL`, `OpenGL ES 3.0 (WebGL 2.0)`, and `MojoShader Profile: glsles3`
- remaining browser work is now gameplay-path validation, not SDL/FNA native linking or asset staging

### Milestone 3: Browser Input and Focus

Goal: keyboard, mouse, focus, and clipboard behavior work in the browser client.

Steps:

1. Keep input capture browser-safe.
2. Verify mouse and keyboard events reach the game loop.
3. Keep focus, blur, and pointer state consistent with browser behavior.
4. Validate text entry and hotkey handling in the browser client.

Done when:

- the player can control the client in-browser
- input does not rely on desktop window mechanics
- focus changes do not break the game loop

Current status:

- partially proven in the spike path
- still active for product completion
- desktop SDL3 event filtering and text input startup are now skipped in browser builds; browser input still needs a dedicated product path after first asset-backed render

### Milestone 4: Browser Networking and Session Flow

Goal: the browser client can connect, authenticate, and maintain a usable session.

Steps:

1. Keep websocket transport stable.
2. Keep reconnect and disconnect handling clean.
3. Validate the login handshake.
4. Validate session recovery and retry behavior.
5. Remove any remaining desktop transport assumptions that block the browser route.

Done when:

- the browser client reaches the login/session flow
- reconnect behavior is reliable
- session state survives routine browser failures

Current status:

- transport foundations are proven
- product login/session flow still needs to be completed

### Milestone 5: Asset Loading and Runtime Data

Goal: the browser client can load all required game data through the browser-safe storage contract.

Steps:

1. Keep shared filesystem/browser storage plumbing as the single source of truth.
2. Validate the full asset set needed for launch and gameplay.
3. Make sure runtime data reads are browser-safe and repeatable.
4. Record any remaining asset access gaps as blockers or deferred work.

Done when:

- the browser client can load all required assets for launch and gameplay
- no desktop-only asset path is required during normal use

Current status:

- foundational seam exists
- active product blocker has moved from asset visibility to post-login session flow
- browser asset staging folder exists for versioned asset drops
- the selected asset version is already being copied into the browser bundle as `/uo`
- managed browser startup telemetry now reports through the host log via the browser shell console bridge
- the latest live boot reaches `login-scene-connect` and `browser-set-scene` after the staged credentials are applied
- next task: confirm the first live session response after login, then continue toward the world loop and rendering path

### Milestone 6: First Playable World Loop

Goal: the browser client reaches the world and supports basic play.

Steps:

1. Reach login successfully.
2. Load into the world.
3. Verify basic movement and camera/UI interaction.
4. Verify reconnect after refresh or disconnect.
5. Verify save/load behavior for a single user.

Done when:

- a user can log in and play in-browser
- the world loop is stable enough for repeated testing

Current status:

- not complete
- this is the first major finish-line milestone

### Milestone 7: Single-User Persistence

Goal: the browser client keeps settings and profile data across sessions.

Steps:

1. Persist browser-safe configuration in the correct storage root.
2. Persist profile data without desktop leakage.
3. Verify restart and re-login behavior.
4. Make sure saved state is scoped to the browser deployment model.

Done when:

- a single user can return to the same setup reliably
- browser session data survives restarts as expected

Current status:

- browser-safe persistence foundations exist
- end-to-end product persistence still needs completion

### Milestone 8: Multi-User Isolation

Goal: more than one named user can use the system without state leakage.

Steps:

1. Add user identity and profile scoping.
2. Partition storage by user.
3. Ensure one user cannot view another user’s data.
4. Validate concurrent sessions and isolation boundaries.

Done when:

- multiple users can use the browser client independently
- saved state is cleanly partitioned

Current status:

- not started as product work
- remains a later milestone after single-user viability

### Milestone 9: Container Hosting

Goal: the browser client runs reliably from Linux containers.

Steps:

1. Package the browser runtime for container hosting.
2. Serve the client statically.
3. Mount persistent storage for assets and state.
4. Verify restart behavior in containers.

Done when:

- the browser client runs from the intended containerized deployment model

Current status:

- not yet the primary active milestone

### Milestone 10: Hardening and Operational Readiness

Goal: make the system supportable after the product works.

Steps:

1. Add backups for key data.
2. Add logs and health checks.
3. Document upgrade and rollback steps.
4. Clean up non-blocking browser and maintainability items.

Done when:

- the browser client is supportable in routine operation
- non-blocking cleanup backlog is reduced

Current status:

- explicitly deferred until after the browser product is viable

## Handoff Notes

- The browser runtime was exiting immediately after the early browser startup status beacon near `browser-initialize-before-browser-bootstrap-branch`.
- That early JS-backed status beacon has been replaced with plain trace logging for the next validation pass.
- The browser startup path in `src/ClassicUO.Client/GameController.cs` remains the active debug target until the next live run proves otherwise.
- Close the Edge window after each browser test run so the machine does not accumulate stale sessions.
- Keep browser usage to one or two open windows or tabs at most during testing.
- The repo-owned browser launcher closes any previously managed browser window before starting a new test session.
- `browser-startup` now uses trace logging only, so browser bootstrap should not stall on the status bridge.
- If a publish build fails with linker OOM, recover the build path first, then resume from the current runtime cut point.

## Deferred Side Work

These items should be recorded, not treated as active blockers, unless they directly block the live browser app:

- cleanup passes that only flatten branches
- helper extraction done purely for readability
- non-blocking browser-specific UI simplifications
- maintainability cleanup that does not affect launch, rendering, input, session flow, or persistence
- synthetic validation layers that no longer unblock the product path

## Execution Rule

When choosing the next task:

1. Ask whether it directly advances a live browser application milestone.
2. If yes, do it in the largest safe batch that keeps the work focused.
3. If no, record it in deferred side work and continue.
4. Stop early only for a browser test, a blocker, or a product decision.

## Progress Summary

The project is past browser bootstrap, local serving, transport discovery, native renderer linking, browser asset staging, and managed startup telemetry. The active critical path is now browser-native asset hydration/runtime startup completion, then managed login flow, input, session flow, gameplay, and persistence.
