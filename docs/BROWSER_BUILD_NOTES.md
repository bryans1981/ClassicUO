# Browser Build Notes

## Status

In progress.

This document records only findings confirmed from the current repository contents.

## Confirmed Findings

### 0. The main client now has a local browser publish and serve path

Files added or updated:

- `scripts/browser-client-publish.ps1`
- `scripts/browser-client-start.ps1`
- `scripts/browser-client-stop.ps1`
- `scripts/browser-client-serve.ps1`
- `scripts/browser-client-index.html`

Confirmed behavior:

- `scripts/browser-client-publish.ps1 -Configuration Debug` produces `bin/Debug/net10.0/browser-wasm/AppBundle`.
- `scripts/browser-client-start.ps1 -Configuration Debug -Port 5110` publishes the bundle and serves it at `http://localhost:5110/`.
- `scripts/browser-client-start.ps1` now also starts the local WebSocket proxy at `ws://127.0.0.1:2594` when `tools/ws/node_modules` is present.
- `scripts/browser-client-start.ps1 -OpenBrowser` can launch a separate minimized browser window for the client, and `scripts/browser-client-stop.ps1` now closes that browser window along with the server and proxy.
- The generated static shell loads `_framework/dotnet.js` and starts the browser runtime.

Latest local Chrome headless result:

- Browser asset delivery is now working through the staged repo folder and bundle copy path.
- The active asset set is selected through `browser-assets/uo/active-version.txt` and copied into the bundle as `/uo`.
- Browser file requests for UO data are returning 200 responses.
- The browser runtime now avoids `MemoryMappedFile` usage in browser mode and falls back to stream reads for UO files.
- The latest browser self-test report in `docs/test-results/browser-self-test-latest.json` is green for the browser-spike harness.
- The remaining work is now proving the full main-client gameplay path in browser mode, not just asset visibility or browser-safe file loading.
- Test account for repeated login validation: `bryanstest` (password provided separately during test runs).

Implication:

- The current critical path has moved from asset staging and file-loading compatibility into full browser-client gameplay validation.
- The next product milestone is proving the browser client can reach the actual game loop and scene flow with the real asset set.

### 1. Browser support is claimed, but the browser packaging path is not exposed here

The top-level README states that ClassicUO supports the browser. However, the repository content inspected so far does not include an obvious browser build script, browser-specific GitHub Actions job, or checked-in web artifact set such as `index.html`, `.wasm`, or web packaging templates.

Implication:

- The browser runtime path likely exists outside the visible top-level scripts in this checkout, is handled by a different repository, or depends on an undocumented process.

### 2. The checked-in CI workflows build desktop release artifacts, not web packages

Files inspected:

- `.github/workflows/build-test.yml`
- `.github/workflows/deploy.yml`

Confirmed behavior:

- CI restores, builds, and tests the .NET solution.
- Deploy workflow publishes `win-x64`, `linux-x64`, and `osx-x64` targets.
- The deploy workflow creates zip packages and release artifacts for those desktop targets.
- No browser or wasm target is visible in the current workflow definitions.

Implication:

- The current visible CI pipeline is not enough to reproduce a browser deployment by itself.

### 3. Browser-compatible transport support exists in the client

Files inspected:

- `src/ClassicUO.Client/Network/NetClient.cs`
- `src/ClassicUO.Client/Network/Socket/WebSocketWrapper.cs`
- `tools/ws/README.md`
- `tools/ws/proxy.mjs`

Confirmed behavior:

- The client switches to WebSocket mode when the configured shard address starts with `ws://` or `wss://`.
- A dedicated `WebSocketWrapper` implements shard connectivity over `ClientWebSocket`.
- The repository includes a simple Node-based TCP-to-WebSocket proxy for testing.

Implication:

- A browser-hosted deployment is expected to use WebSockets for shard connectivity.
- For our project, this means we will likely need either:
  - a shard that supports WebSockets directly, or
  - a production-safe WebSocket proxy or gateway in front of a TCP shard server.

### 4. Per-user settings isolation already has a useful hook

Files inspected:

- `src/ClassicUO.Client/Main.cs`
- `src/ClassicUO.Client/Configuration/Settings.cs`

Confirmed behavior:

- The client accepts a `-settings` command-line option.
- That option can point to a custom settings file path.
- The settings file contains shard address, credentials, asset directory, and profile-related values.

Implication:

- This is a strong starting point for multi-user or multi-session isolation.
- A launcher or backend-controlled boot flow could generate separate settings files per user or session instead of sharing one global `settings.json`.

### 5. The client still expects a real Ultima Online asset directory

Files inspected:

- `src/ClassicUO.Client/Main.cs`

Confirmed behavior:

- Startup validates that `UltimaOnlineDirectory` exists.
- Startup checks for `tiledata.mul` in that directory.
- If the directory or client version is invalid, the client errors and exits the main run path.

Implication:

- Our hosted solution must provide a valid asset directory model.
- We cannot treat asset access as optional.

### 6. Browser asset staging is now available in the repo

Files inspected and updated:

- `browser-assets/uo/versions/<version>/`
- `browser-assets/uo/active-version.txt`
- `scripts/browser-client-publish.ps1`

Confirmed behavior:

- multiple asset versions can be stored under `browser-assets/uo/versions/<version>/`
- the active version can be switched without changing code
- the publish step copies the selected version into the browser bundle as `/uo`
- the browser shell now hides noisy tracing unless debug mode is enabled
- browser input is being hardened for actual play with canvas focus, no page scroll, and no context menu
- browser login ping polling is disabled so the shard list does not depend on unsupported browser-only network APIs
- any asset drops under `browser-assets/uo/versions/` should stay local and be excluded from pushes

Implication:

- asset drops are no longer a blocking manual step in the runtime path
- the remaining blocker is browser startup after the asset mount is already in place

## Current Blockers

### Blocker 1: Browser-native bootstrap and runtime startup

Asset delivery is now in place, but the browser runtime still aborts during startup with `mono_download_assets: undefined` before managed game startup is fully visible.

Minimum next proof:

- browser startup reaches managed game logging again
- `Main` or the browser bootstrap path confirms the client is alive after the native runtime loads
- the browser shell stops aborting before game initialization

### Blocker 2: Browser durable storage

The temporary rooted in-memory storage provider unblocks startup, but it is not final persistence.

We still need:

- durable browser-safe settings/profile storage
- and a repeatable hosted asset layout for future versions

## Working Hypotheses

These are not yet confirmed:

- The browser deployment may be maintained outside this repository or in a separate deploy repo.
- The browser version may rely on a custom runtime/bootstrap arrangement not represented in the standard desktop workflows.
- Asset access in the browser requires a real virtual filesystem source; simply serving static files from the bundle does not automatically make them visible to synchronous `FileSystemHelper` calls.

## What This Means For Our Project

The shortest path forward is not yet containerization. The critical path is discovery:

1. Identify the real browser build source and packaging process.
2. Identify how browser runtime asset access works.
3. Determine whether we can reproduce that pipeline locally in our private repo.

Until those three points are confirmed, any container work would be premature.

## Next Investigation Targets

1. Inspect the browser bootstrap and native runtime startup path around `mono_download_assets`.
2. Confirm whether any browser runtime configuration still expects a symbols or asset-download hook.
3. Once startup is stable again, resume validation at managed client startup and login flow.

## Confirmed External Findings

### 6. The official web client is separate and not fully open-source

External sources:

- https://www.classicuo.org/about
- https://www.classicuo.org/shard-owners/faq
- https://github.com/ClassicUO

Confirmed behavior:

- The ClassicUO team describes the web client as the closed-source web-based version of ClassicUO.
- They state it is powered by WebAssembly and additional React-based web functionality.
- They explicitly describe it as using a custom build process with many C#/JS runtime workarounds.
- They also state the web client depends on additional surrounding systems such as runtime interop, backend APIs, CDNs, authentication, assets, and shard management.

Implication:

- The browser deployment we want is not equivalent to simply publishing the current open-source repo to the web.
- Reproducing the official web deployment from this repository alone is unlikely.
- A private browser-hosted solution will probably require either:
  - substantial custom engineering on top of the open-source client, or
  - reuse of separate public repositories where available plus our own missing infrastructure.

### 7. The ClassicUO organization exposes related repositories outside this repo

External source:

- https://github.com/ClassicUO

Confirmed behavior:

- The organization lists separate repositories including `classicuo-web`, `deploy`, and `gate`.
- `gate` is described as a Cloudflare Worker WebSocket <-> TCP proxy.

Implication:

- The missing browser deployment path likely spans multiple repositories.
- The in-repo `tools/ws` proxy is only a test aid, not the whole production network path.

### 8. The official web architecture uses browser-side persisted content and a game proxy

External sources:

- https://www.classicuo.org/about
- https://www.classicuo.org/shard-owners/faq

Confirmed behavior:

- Content is delivered in chunks and persisted in browser storage using Origin Private File System APIs.
- Browser networking uses a WebSocket proxy because direct TCP from browsers is not possible.
- The official system uses additional infrastructure for shard management and authentication.

Implication:

- Our project should assume that browser asset delivery and network proxying are first-class architecture concerns, not minor packaging details.
- Multi-user hosting will need explicit decisions for browser-side asset persistence, proxy design, and auth boundaries.

### 9. `gate` is a real reusable websocket gateway

External sources:

- https://github.com/ClassicUO/gate
- https://raw.githubusercontent.com/ClassicUO/gate/main/README.md
- https://raw.githubusercontent.com/ClassicUO/gate/main/src/index.ts
- https://raw.githubusercontent.com/ClassicUO/gate/main/src/router.ts

Confirmed behavior:

- `gate` is a public Cloudflare Worker project.
- It upgrades incoming websocket requests and pipes them to a TCP connection using Cloudflare sockets.
- It targets a configured shard address and supports masking relay behavior.
- It is intended to let websocket-enabled ClassicUO clients reach shards that do not natively support WebSockets.

Implication:

- We have a publicly visible reference implementation for the network gateway layer.
- We could reuse its design directly or build an equivalent Linux-hosted websocket-to-TCP proxy if we do not want a Cloudflare dependency.

### 10. `classicuo-web` does not expose the browser game runtime

External sources:

- https://github.com/ClassicUO/classicuo-web
- https://raw.githubusercontent.com/ClassicUO/classicuo-web/main/README.md

Confirmed behavior:

- The public `classicuo-web` repository identifies itself as documentation and issue tracker.
- It exposes package workspaces such as `diff-tool` and modding-related packages.
- It does not present itself as the source repository for the full browser-hosted game client runtime.

Implication:

- The actual browser game runtime build remains unavailable from the public repositories inspected so far.
- Publicly reusable pieces appear to be ecosystem tooling, not the full game client delivery path.

### 11. `deploy` is mainly for packaged desktop client distribution

External sources:

- https://github.com/ClassicUO/deploy
- https://raw.githubusercontent.com/ClassicUO/deploy/main/make_client_release.sh
- https://raw.githubusercontent.com/ClassicUO/deploy/main/client/linux-x64_manifest.xml

Confirmed behavior:

- `deploy` downloads desktop release zips from ClassicUO GitHub releases.
- It expands them and generates manifests for `linux-x64`, `win-x64`, and `osx-x64`.
- The published manifest structure is file-centric and oriented toward desktop update distribution.
- No browser runtime package is exposed in the inspected deploy assets.

Implication:

- `deploy` is useful as a reference for artifact/manifests and update distribution patterns.
- It does not solve the missing browser-runtime build problem for us.

## Updated Reusability Assessment

Publicly reusable now:

- WebSocket proxy architecture via `gate`
- Some patching and diff tooling via `classicuo-web/packages/diff-tool`
- Desktop artifact manifest patterns via `deploy`

Still missing or non-public:

- Browser game runtime build pipeline
- Browser runtime artifacts and bootstrap
- Full asset delivery implementation used by the official web client
- Auth/session/backend stack used by the official service

The local browser spike now also has an optional test websocket proxy in `tools/ws`, managed by `scripts/browser-spike-start.ps1` and `scripts/browser-spike-stop.ps1` when Node dependencies are installed.
