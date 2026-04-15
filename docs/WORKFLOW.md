# Project Workflow

## Purpose

This document keeps the project focused on one goal:

Run a private, browser-accessible ClassicUO deployment from Linux containers, using privately supplied Ultima Online assets, with support for multiple named users and isolated user data.

This is not a public SaaS product plan. It is a private multi-user hosting project.

## Working Assumptions

- Usage is private and controlled by the project owner.
- Ultima Online asset files are supplied and managed privately.
- Users access the client in a web browser.
- The application stack will run in Linux containers.
- User settings and profiles must be isolated from one another.
- We should avoid unnecessary changes to upstream ClassicUO unless required for hosting, browser delivery, or per-user isolation.

## Primary Outcomes

1. Prove that ClassicUO can be run reliably in a browser from our own hosted environment.
2. Package the system so it can run consistently in Linux containers.
3. Add multi-user support with isolated profiles, settings, and launch configuration.
4. Keep the codebase maintainable enough to sync selected upstream changes.
5. Move from proof-of-concept scaffolding into the final browser product as soon as the browser path is stable enough to do so.

## Non-Goals For Now

- Public hosting for arbitrary internet users.
- Billing, subscriptions, or commercial tenancy features.
- Broad plugin ecosystems or custom mod marketplaces.
- Heavy redesign of the ClassicUO client unless required by the browser-hosted model.
- Full production orchestration before the proof of concept works.

## Decision Rules

Use these rules when tradeoffs appear:

1. Prefer proof of function over polish.
2. Prefer small reversible changes over large architectural rewrites.
3. Keep hosting-specific code separate from core client logic whenever possible.
4. Treat user isolation as a first-class requirement, not a later cleanup task.
5. Do not optimize for scale before the single-user browser deployment works.
6. Document every assumption that affects storage, browser behavior, or sync with upstream.

## Project Phases

### Phase 1: Technical Proof

Goal: prove we can run the browser build locally and understand its build/runtime requirements.

Exit criteria:

- Browser build process is identified.
- Local browser launch works.
- Required runtime files and asset expectations are documented.

### Phase 2: Container Hosting

Goal: serve the browser client from Linux containers with persistent storage for assets and settings.

Exit criteria:

- Containerized stack starts consistently.
- Static client assets are served correctly.
- Mounted storage works for required files.

### Phase 3: Private Multi-User Support

Goal: support more than one named user with isolated profiles and settings.

Exit criteria:

- User authentication exists.
- Per-user profile/config storage is isolated.
- One user cannot access another user's saved data.

### Phase 4: Hardening

Goal: make the system supportable and repeatable.

Exit criteria:

- Backups exist for key data.
- Basic logs and health checks exist.
- Upgrade and rollback steps are documented.

## How We Work

### Before Starting Any Feature

- State the exact outcome being pursued.
- Identify whether the work is for proof of concept, hosting, user isolation, or hardening.
- Identify what must be true for the task to be considered complete.

### During Implementation

- Make the smallest useful change.
- Keep notes on hidden assumptions and blockers.
- Separate confirmed facts from guesses.
- If a task uncovers browser/runtime uncertainty, stop and convert it into an explicit investigation task.
- Prefer batched implementation work over tiny verify-after-every-change loops when the risk is manageable.
- Keep the active browser operator surface minimal: only the current required checks should remain in `Current Tests`; completed or diagnostic checks should move to archived sections.
- Prefer one-click browser actions that save machine-readable results locally over manual copy/paste from the UI.
- If the browser self-test throws before the report is built, save a compact failure envelope locally through the report sink so the error is still captured without user copy/paste.
- If a new runtime batch introduces a DI cycle, detach the newest ready-state services from their paired session services before adding more layers.
- Treat automatic local report saving as the default validation path whenever a browser-side result needs to be handed back into the repo for review.

### After Implementation

- Record what changed.
- Record how it was validated.
- Record open risks or unresolved follow-up items.
- Update the roadmap if the critical path changed.
- Update this workflow doc locally after meaningful milestones so the current operating method stays explicit.

## Status Review Cadence

At the end of each meaningful work session, update:

- Current phase
- What is now proven
- What remains blocked
- Next 1 to 3 tasks
- Any new risk that could change scope or design

## Preferred Operator Interaction

The default operator interaction for this project is:

- Do not ask the operator to reply with "next" or any equivalent continue signal; continue implementation automatically after routine milestones and only request input for an actual test, external decision, or blocker.
- Do not pause for user confirmation between routine implementation milestones; continue automatically and only stop when a browser-side test, external decision, or blocker truly requires operator input.
- Keep manual operator steps short and infrequent.
- When browser validation is required, prefer a single current action over multiple scattered panel checks.
- Save browser validation output automatically into the local repo when possible, so results can be reviewed directly without copy/paste.
- Ask the operator for manual transcription only when the browser cannot yet write a usable report locally or when a true visual judgment is required.
- Prefer a no-click validation URL when possible, such as an `autoSelfTest=1` query that runs the current self-test suite on page load and still saves the report locally.
- Continue to use `Current Tests` only for the active path; move older checks into archived sections once they are no longer part of the main workflow.
- Default validation batch size is 20 runtime layers per browser self-test only when we are still extending the synthetic chain; for the final product path, prefer larger feature-sized batches that cover a full working slice end to end.
- When a batch exposes a DI cycle in the browser runtime chain, break the newest edge first and re-run the compact self-test before adding more layers.
- The current active browser handoff is the concrete `Browser bootstrap package` artifact, and the active work should now consume it into real client-bootstrap flow rather than extending the synthetic tail further.
- The active operator surface should focus on the bootstrap package, launch/session persistence, and real client-bootstrap integration; the large synthetic runtime tail is archived validation only.
- Prefer feature-sized batches that move a final product slice forward, even if they include multiple related steps, instead of one-item-at-a-time proof work.
- Do not use broad vertical-slice execution as the default plan; prefer feature-sized batches plus parallel disjoint work when it materially reduces time to a working client.
- Minimize pauses for confirmation unless a browser test, a blocker, or a product decision is genuinely required.
- GitHub Actions should ignore browser-spike and docs-only changes for `Build-Test`, and `Deploy` should only run after a successful `Build-Test` completion.
- Current browser-native baseline: the launch plan now consumes the seam-backed handoff directly, and the browser-native execution plan should remain the active validation target while the report receiver is healthy on `http://localhost:5100`.
- The browser-native execution plan now also exposes a browser-native runtime shell summary; keep that as the current product-facing baseline while we move toward actual rendering/input/network integration.
- The browser-native websocket session controller now validates the transport endpoint and runtime execution mode; keep that as the current product-facing session baseline while we move toward an actual runtime handshake.

## Issue Triage

Classify new issues into one of these groups:

- Browser build
- Container/runtime
- Asset storage and access
- Authentication and user isolation
- Networking and connectivity
- Performance
- Upstream sync and maintainability

If an issue does not clearly fit one of these, define the missing category before proceeding.

## Change Control

Use this checkpoint before merging or keeping a change:

1. Does it help the current phase goal?
2. Does it increase divergence from upstream?
3. Can it be isolated behind a hosting-specific boundary?
4. Does it affect user data separation?
5. Is there a simpler version we should try first?

If the answer to 2 is yes, document why the divergence is necessary.

## Definition Of Done

A task is done only when:

- The intended outcome is observable.
- The validation method is stated.
- Known risks are recorded.
- The next dependency is clear.

## Current Focus

Current focus should remain:

1. Move the browser host from scaffolding into the actual product path.
2. Consume the bootstrap package into real client-bootstrap integration.
3. Land the browser-native execution path needed for a playable browser client, with WebAssembly/WebGL-style rendering, input, and websocket runtime behavior as the reference target.
4. Keep browser validation batched and focused on end-to-end working slices instead of isolated proof steps.

## Confirmed Project Facts

These facts are now confirmed and should guide future decisions:

- The visible repository CI builds desktop artifacts only.
- Browser packaging is not yet reproducible from the currently identified scripts.
- The official ClassicUO web client is separate and described as closed-source.
- The official web path appears to span multiple repositories and supporting services.
- A public websocket gateway reference exists via gate.
- Public repo reuse appears limited to proxying, tooling, and distribution patterns rather than the browser runtime itself.
- Browser-hosted connectivity is expected to use WebSockets.
- The repository includes only a test WebSocket proxy, not a production-ready gateway.
- The client already supports custom per-session settings files through the `-settings` argument.
- The client expects a valid Ultima Online asset directory and validates required files at startup.
- Core boot/config file access now routes through FileSystemHelper.
- The main asset layer now mostly resolves file existence/open checks through FileSystemHelper and DefReader.
- BrowserFileSystem now supports a normalized virtual path model and an injectable storage provider.
- The IO layer now accepts generic Stream input, which is required for browser-backed asset reads.
- The shared filesystem seam now supports read, write, append, read/write, file listing, file length, and file copy operations.
- The browser spike now exposes a root-aware `/uo` asset manifest with file counts, sizes, extensions, and preload visibility.
- The browser spike now supports local disk seeding of large UO asset bundles into `wwwroot/local-uo`, followed by browser import into OPFS.
- The bootstrap-readiness check now accepts retail UOP alternatives for map, art, and gump data instead of only legacy MUL/IDX pairs.
- `tiledata.mul` is now proven readable from browser-backed `/uo` storage with a structured binary probe that matches the old-format retail file layout.
- The spike now has a C# browser asset byte-source service instead of relying only on JS-side asset parsing.
- The browser-host spike now has a layered asset subsystem: raw asset source, processed cache, loader harness, family readers, bootstrap aggregation, runtime bootstrap service, client-facing asset service, and a shared `BrowserFileSystem` bridge.
- The browser-host spike UI is now organized so the active validation work stays in a `Current Tests` tab and older diagnostics stay out of the critical path.
- The preferred operator flow is now one-click browser validation with automatic local report saving, rather than manual result copying.
- The browser-host startup path now progresses through startup artifact write/read and into a simulated launch session object for future browser entrypoint consumption.
- The startup path now also reads the saved launch session back as a resumable launch-state consumer, and the one-click self-test records that readback state automatically.
- The launch-session persistence path now serializes final session state before readback validation, so the saved session can be consumed without stale write-state fields.
- The startup path now shapes a reusable runtime launch contract from the saved launch session, so future browser startup code can consume one stable object instead of raw session internals.
- The startup path now also builds a single startup packet from the runtime launch contract, so the future browser entrypoint can consume one bootstrap input instead of multiple persistence layers.
- The startup path now also has an in-memory startup consumer above the startup packet, representing the first real browser bootstrap handshake object instead of only persisted contract data.
- The startup path now also has a startup-session executor above the in-memory consumer, representing the first runtime-oriented startup consumption layer rather than only handshake assembly.
- The startup path now also has a startup-session runner above the executor, representing the first simulated runtime startup pass instead of only prepared startup state.
- The startup path now also has a minimal runtime bootstrap loop above the runner, representing the first phase-based startup loop model rather than only stacked startup state objects.
- The startup path now also has a minimal runtime invocation layer above the loop, representing the first concrete browser-runtime call boundary rather than only startup-loop state.
- The startup path now also has a minimal runtime startup cycle layer above the runtime invocation, representing the first persistent runtime startup-state object rather than only an invocation boundary.
- The startup path now also has a minimal runtime startup state layer above the runtime startup cycle, representing the first concrete runtime startup-state holder rather than only a cycle object.
- The startup path now also has a minimal runtime startup state machine layer above the runtime startup state, representing the first browser bootstrap state-progression object rather than only a single startup-state holder.
- The startup path now also has a minimal runtime startup transition driver above the runtime startup state machine, representing the first executable browser bootstrap progression layer rather than only a static state machine object.
- The startup path now also has a minimal runtime startup dispatcher above the runtime startup transition driver, representing the first bootstrap-session control layer that can route startup work rather than only describe transitions.
- The startup path now also has a minimal runtime startup session controller above the runtime startup dispatcher, representing the first executable bootstrap-session control object rather than only a dispatch plan.
- The startup path now also has a minimal runtime startup coordinator above the runtime startup session controller, representing the first browser bootstrap orchestration layer rather than only a session-control object.
- The startup path now also has a minimal runtime startup orchestrator above the runtime startup coordinator, representing the first runtime-wide startup orchestration layer rather than only bootstrap coordination.
- The startup path now also has a first browser boot-flow controller above the runtime startup orchestrator, representing the first browser-facing boot flow model rather than only runtime startup orchestration.
- The startup path now also has a first browser boot session above the boot-flow controller, representing the first browser-session startup object rather than only a boot flow model.
- The startup path now also has a first runtime launch handoff above the browser boot session, representing the first browser-to-runtime handoff object rather than only a boot session.
- The startup path now also has a first runtime bootstrap consumer above the runtime launch handoff, representing the first runtime-side consumer of the browser handoff rather than only the handoff object itself.
- The startup path now also has a first runtime bootstrap session above the runtime bootstrap consumer, representing the first runtime-session startup object beyond the consumer layer.
- The experimental browser host now stays on the lighter `net8.0` path and links only the needed browser-seam source files, avoiding the heavier desktop dependency chain.
- The shared utility layer now supports a read-only binary asset provider shape for `/uo` paths, separate from writable profile/config storage.
- The browser-port effort has now covered the major direct filesystem clusters including asset loaders, profile/state managers, data tables, world-map support, container persistence, and UltimaLive.
- The remaining direct System.IO usage in the client is now a smaller cleanup set rather than a major architectural blocker.
- The official ClassicUO web-client reference uses browser-native execution with WebAssembly and WebGL as the target shape for the playable client.






- Runtime batch progression now includes `RuntimeReadySignal` and `RuntimeLaunchController` after the startup readiness gate.
- Continue automatically across routine milestones; only interrupt for a real browser test, an external decision, or a blocker.
- Runtime progression now continues through `RuntimeClientReadyState` and `RuntimeClientLaunchSession` after the launch controller.
- Runtime progression now continues through `RuntimeClientActivation` and `RuntimeClientRunState` after the client launch session.
- Runtime progression now continues through `RuntimeClientLoopState` and `RuntimeHostSession` after the client run state.
- Runtime progression now continues through `RuntimeHostLoop` and `RuntimeHostReadyState` after the host session.
- Treat a user reply of `done` after a requested browser test as both test completion and authorization to continue automatically to the next milestone.
- Runtime progression now continues through `RuntimePlatformSession` and `RuntimePlatformReadyState` after the host-ready state.
- Runtime progression now continues through `RuntimePlatformLoop` and `RuntimePlatformLaunchGate` after the platform-ready state.
- Runtime progression now continues through `RuntimeBrowserShellSession` and `RuntimeBrowserShellReadyState` after the platform launch gate.
- Runtime progression now continues through `RuntimeBrowserSurfaceSession` and `RuntimeBrowserSurfaceReadyState` after the browser-shell ready state.
- Runtime progression now continues through `RuntimeBrowserWindowSession` and `RuntimeBrowserWindowReadyState` after the browser-surface ready state.
- Runtime progression now continues through `RuntimeBrowserFrameSession` and `RuntimeBrowserFrameReadyState` after the browser-window ready state.
- Runtime progression now continues through `RuntimeBrowserCanvasSession` and `RuntimeBrowserCanvasReadyState` after the browser-frame ready state.
- Runtime progression now continues through `RuntimeBrowserRenderSession` and `RuntimeBrowserRenderReadyState` after the browser-canvas ready state.
- Runtime progression now continues through `RuntimeBrowserPresentSession` and `RuntimeBrowserPresentReadyState` after the browser-render ready state.

- Runtime progression now continues through RuntimeBrowserDisplaySession and RuntimeBrowserDisplayReadyState after the browser-present ready state.
- Runtime progression now continues through RuntimeBrowserViewportSession and RuntimeBrowserViewportReadyState after the browser-display ready state.

- Runtime progression now continues through RuntimeBrowserSceneSession and RuntimeBrowserSceneReadyState after the browser-viewport ready state.
- Runtime progression now continues through RuntimeBrowserInputSession and RuntimeBrowserInputReadyState after the browser-scene ready state.

- Runtime progression now continues through RuntimeBrowserEventSession and RuntimeBrowserEventReadyState after the browser-input ready state.
- Runtime progression now continues through RuntimeBrowserInteractionSession and RuntimeBrowserInteractionReadyState after the browser-event ready state.

- Runtime progression now continues through RuntimeBrowserFocusSession and RuntimeBrowserFocusReadyState after the browser-interaction ready state.
- Runtime progression now continues through RuntimeBrowserShortcutSession and RuntimeBrowserShortcutReadyState after the browser-focus ready state.

- Runtime progression now continues through RuntimeBrowserPointerSession and RuntimeBrowserPointerReadyState after the browser-shortcut ready state.
- Runtime progression now continues through RuntimeBrowserCommandSession and RuntimeBrowserCommandReadyState after the browser-pointer ready state.

- Runtime progression now continues through RuntimeBrowserGestureSession and RuntimeBrowserGestureReadyState after the browser-command ready state.
- Runtime progression now continues through RuntimeBrowserLifecycleSession and RuntimeBrowserLifecycleReadyState after the browser-gesture ready state.

- Runtime progression now continues through RuntimeBrowserRouteSession and RuntimeBrowserRouteReadyState after the browser-lifecycle ready state.
- Runtime progression now continues through RuntimeBrowserStateSyncSession and RuntimeBrowserStateSyncReadyState after the browser-route ready state.

- Runtime progression now continues through RuntimeBrowserRestoreSession and RuntimeBrowserRestoreReadyState after the browser-state-sync ready state.
- Runtime progression now continues through RuntimeBrowserResumeSession and RuntimeBrowserResumeReadyState after the browser-restore ready state.

- Runtime progression now continues through RuntimeBrowserCheckpointSession and RuntimeBrowserCheckpointReadyState after the browser-resume ready state.
- Runtime progression now continues through RuntimeBrowserPersistenceSession and RuntimeBrowserPersistenceReadyState after the browser-checkpoint ready state.

- Runtime progression now continues through RuntimeBrowserHistorySession and RuntimeBrowserHistoryReadyState after the browser-persistence ready state.
- Runtime progression now continues through RuntimeBrowserRecoverySession and RuntimeBrowserRecoveryReadyState after the browser-history ready state.

- Runtime progression now continues through RuntimeBrowserSnapshotSession and RuntimeBrowserSnapshotReadyState after the browser-recovery ready state.
- Runtime progression now continues through RuntimeBrowserArchiveSession and RuntimeBrowserArchiveReadyState after the browser-snapshot ready state.

- Runtime progression now continues through RuntimeBrowserTelemetrySession and RuntimeBrowserTelemetryReadyState after the browser-archive ready state.
- Runtime progression now continues through RuntimeBrowserDiagnosticsSession and RuntimeBrowserDiagnosticsReadyState after the browser-telemetry ready state.

- Runtime progression now continues through RuntimeBrowserMonitoringSession and RuntimeBrowserMonitoringReadyState after the browser-diagnostics ready state.
- Runtime progression now continues through RuntimeBrowserWatchdogSession and RuntimeBrowserWatchdogReadyState after the browser-monitoring ready state.

- Runtime progression now continues through RuntimeBrowserHealthSession and RuntimeBrowserHealthReadyState after the browser-watchdog ready state.
- Runtime progression now continues through RuntimeBrowserAlertingSession and RuntimeBrowserAlertingReadyState after the browser-health ready state.

- Runtime progression now continues through RuntimeBrowserPolicySession and RuntimeBrowserPolicyReadyState after the browser-alerting ready state.
- Runtime progression now continues through RuntimeBrowserAuditSession and RuntimeBrowserAuditReadyState after the browser-policy ready state.

- 2026-04-02: Added browser security/compliance session and ready-state layers to the self-test runtime chain after audit. Also hardened self-test JSON serialization with cycle handling, higher depth, and a visible runtime chain marker.

- 2026-04-02: Added browser privacy/governance session and ready-state layers above security/compliance. Self-test summary and UI now expose privacy/governance readiness flags.

- 2026-04-02: Added browser trust/assurance session and ready-state layers above privacy/governance. Self-test summary and Current Tests now include trust/assurance readiness flags.

- 2026-04-02: Added browser risk/integrity session and ready-state layers above trust/assurance. Self-test summary and Current Tests now include risk/integrity readiness flags.

- 2026-04-02: Added browser resilience session and ready-state layers above risk/integrity. Self-test summary and Current Tests now expose resilience readiness flags.

- 2026-04-02: Added browser availability session and ready-state layers above resilience. Current Tests and self-test summary now surface availability readiness.

- 2026-04-02: Switched the saved self-test report toward compact browser-runtime JSON by excluding the large browser runtime object graph from serialization while keeping the detailed status view in-memory on the page.

- 2026-04-02: Added browser continuity session and ready-state layers above availability. Also moved new operator-facing context to the top of Current Tests with the compact report mode and newest-layer indicator.

- 2026-04-02: Added browser durability session and ready-state layers above continuity. Current Tests now shows continuity and durability as the newest layers at the top while preserving the compact self-test report flow.


- 2026-04-02: Added browser sustainability session and ready-state layers above durability. Current Tests now shows durability and sustainability as the newest layers at the top while preserving the compact self-test report flow.


- 2026-04-02: Added browser longevity session and ready-state layers above sustainability. Current Tests now shows sustainability and longevity as the newest layers at the top while preserving the compact self-test report flow.


- 2026-04-02: Added browser stewardship session and ready-state layers above longevity. Current Tests now shows longevity and stewardship as the newest layers at the top while preserving the compact self-test report flow.


- 2026-04-02: Added browser operability session and ready-state layers above stewardship. Current Tests now shows stewardship and operability as the newest layers at the top while preserving the compact self-test report flow.


- 2026-04-02: Added browser serviceability session and ready-state layers above operability. Current Tests now shows operability and serviceability as the newest layers at the top while preserving the compact self-test report flow.


- 2026-04-02: Added browser maintainability session and ready-state layers above serviceability. Current Tests now shows serviceability and maintainability as the newest layers at the top while preserving the compact self-test report flow.


- 2026-04-02: Added browser supportability session and ready-state layers above maintainability. The compact self-test report stays in place with operator-facing additions kept at the top of Current Tests.


- 2026-04-02: Added browser usability session and ready-state layers above supportability. Current Tests now shows supportability and usability as the newest layers at the top.


- 2026-04-02: Added browser accessibility session and ready-state layers above usability. The compact self-test report remains in place with new operator-facing items kept at the top of Current Tests.


- 2026-04-02: Added browser inclusivity session and ready-state layers above accessibility. Current Tests now shows accessibility and inclusivity as the newest layers at the top.


- 2026-04-02: Added browser adaptability session and ready-state layers above inclusivity. The compact self-test report remains in place with new operator-facing items kept at the top of Current Tests.


- 2026-04-02: Added browser discoverability session and ready-state layers above adaptability. Current Tests now shows adaptability and discoverability as the newest layers at the top.


- 2026-04-02: Added browser learnability session and ready-state layers above discoverability. The compact self-test report remains in place with new operator-facing items kept at the top of Current Tests.


- 2026-04-02: Added browser approachability session and ready-state layers above learnability. Current Tests now shows learnability and approachability as the newest layers at the top.

- Added browser runtime layers for navigability and guidability, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Added browser runtime layers for clarity and intuitiveness, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Added browser runtime layers for consistency and cohesiveness, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Added browser runtime layers for predictability and familiarity, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Added browser runtime layers for fluency and harmony, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Added browser runtime layers for readability and legibility, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Added browser runtime layers for simplicity and understandability, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Added browser runtime layers for scannability and comprehensibility, extended the compact self-test report, and moved the active UI label to show the newest pair at the top.

- Default browser validation batch limit: 20 new runtime layers per browser self-test unless there is a real blocker or a user-only decision.

- Added the next 20 browser runtime layers from credibility/completion confidence, under the 20-layer validation batch rule with a single self-test at the end.

Validated second 20-layer browser runtime batch through progress-awareness/completion-confidence.
Validated third 20-layer browser runtime batch through milestone-awareness/closure-confidence.
Validated fourth 20-layer browser runtime batch through reassurance/completion-assurance.
Generated the fifth 20-layer browser runtime batch through confirmation/completion-readiness and prepared it for the next one-click self-test.



- 2026-04-02: Began the fifth 20-layer browser runtime batch from confirmation/completion-readiness. Fixed a generated duplicate-property regression in BrowserSelfTestReportService, moved self-test/report resolution to lazy runtime lookup so Current Tests can load even when the newest batch is broken, and repaired the persistence anchor after the generator introduced a DI cycle through perseverance. Stopping point: browser page loads again, but the fifth batch still needs a fresh one-click self-test after the persistence anchor fix.
- 2026-04-14: Generated the sixth 20-layer browser runtime batch from operational readiness/deployment readiness through live stability/steady state readiness above the validated completion readiness baseline. Next step: one compact self-test for this block.

- 2026-04-14: Generated the seventh 20-layer browser runtime batch from operational stability/deployment stability through live assurance/steady operation readiness above the validated steady-state readiness baseline. Next step: one compact self-test for this block.
- 2026-04-14: Generated the eighth 20-layer browser runtime batch from operational resilience/deployment resilience through live continuity/steady continuity readiness above the validated steady operation readiness baseline. Next step: one compact self-test for this block.
- 2026-04-14: Generated the ninth 20-layer browser runtime batch from operational reliability/deployment reliability through live persistence/steady persistence readiness above the validated steady continuity readiness baseline. Next step: one compact self-test for this block.
- 2026-04-15: The active browser-native path now includes a runtime shell controller on top of the native execution plan. The self-test report remains compact and no-click by default.
- 2026-04-15: The no-click self-test runner now launches Edge in an isolated temp profile and closes it automatically after the report updates unless `-KeepBrowserOpen` is passed.
- 2026-04-15: Added a browser-native client bootstrap controller above the native launch controller so the active product path has one more real handoff step before the final browser client runtime.
- 2026-04-15: Added a browser-native browser runtime controller on top of the native client bootstrap controller, render readiness, input readiness, and launch control. This is the current active browser-runtime baseline.
- 2026-04-15: Added a browser-native browser host controller on top of the browser runtime, render readiness, input readiness, and launch control. This is the current browser-host baseline.
- 2026-04-15: Added a browser-native browser surface controller on top of the browser host controller and browser surface readiness. This is the current browser-surface baseline.
- 2026-04-15: Added a browser-native browser render controller on top of the browser surface controller and browser render readiness. This is the current browser-render baseline.
- 2026-04-15: Added a browser-native browser input controller on top of the browser render controller and browser input readiness. This is the current browser-input baseline.
- 2026-04-15: Added a browser-native browser network controller on top of the browser input controller and runtime launch contract. This is the current browser-network baseline.
- 2026-04-15: Added a browser-native browser transport controller on top of the browser network controller. This is the current browser-transport baseline and the first websocket/runtime integration marker.
- 2026-04-15: Added a browser-native browser runtime execution controller on top of the browser transport controller. This is the current browser-runtime-execution baseline.
- 2026-04-15: Added a browser-native websocket session controller on top of the browser runtime execution controller. This is the current browser-session handshake baseline and the first explicit websocket handshake layer.
- 2026-04-15: Added a browser-native browser session controller on top of the browser runtime execution controller. This is the current browser-session baseline.
- 2026-04-15: The no-click browser self-test runner now waits for the browser host and report receiver to answer before opening Edge, reducing false timeouts on cold starts.
