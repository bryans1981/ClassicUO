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

### After Implementation

- Record what changed.
- Record how it was validated.
- Record open risks or unresolved follow-up items.
- Update the roadmap if the critical path changed.

## Status Review Cadence

At the end of each meaningful work session, update:

- Current phase
- What is now proven
- What remains blocked
- Next 1 to 3 tasks
- Any new risk that could change scope or design

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

1. Define and validate a custom browser-port path from the open-source client.
2. Prove a minimal browser host, asset access layer, and websocket network path.
3. Convert remaining deep System.IO usage in asset and client loaders only after the shared browser filesystem seam is stable.
4. Package a minimal Linux-hosted proof of concept only after those browser spikes work.

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
- The browser-host spike now has a layered asset subsystem: raw asset source, processed cache, loader harness, family readers, bootstrap aggregation, runtime bootstrap service, and client-facing asset service.
- The browser-host spike UI is now organized so the active validation work stays in a `Current Tests` tab and older diagnostics stay out of the critical path.
- The shared utility layer now supports a read-only binary asset provider shape for `/uo` paths, separate from writable profile/config storage.
- The browser-port effort has now covered the major direct filesystem clusters including asset loaders, profile/state managers, data tables, world-map support, container persistence, and UltimaLive.
- The remaining direct System.IO usage in the client is now a smaller cleanup set rather than a major architectural blocker.
