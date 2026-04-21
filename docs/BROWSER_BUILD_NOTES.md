# Browser Build Notes

## Status

In progress.

This document records only findings confirmed from the current repository contents.

## Confirmed Findings

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

## Current Blockers

### Blocker 1: Missing browser build pipeline

We do not yet know:

- what command produces the browser build
- what runtime packager is used
- what output files should be served
- whether the build path depends on another repo or unpublished assets

### Blocker 2: Unknown browser runtime bootstrap

We do not yet know:

- how the browser-hosted app starts
- how the unmanaged host/bootstrap layer is adapted for the browser target
- how the asset directory expectation is represented in a browser environment

## Working Hypotheses

These are not yet confirmed:

- The browser deployment may be maintained outside this repository or in a separate deploy repo.
- The browser version may rely on a custom runtime/bootstrap arrangement not represented in the standard desktop workflows.
- Asset access in the browser may require a virtual filesystem, preloading step, or HTTP-backed data layer.

## What This Means For Our Project

The shortest path forward is not yet containerization. The critical path is discovery:

1. Identify the real browser build source and packaging process.
2. Identify how browser runtime asset access works.
3. Determine whether we can reproduce that pipeline locally in our private repo.

Until those three points are confirmed, any container work would be premature.

## Next Investigation Targets

1. Inspect any remaining code paths related to bootstrap or host bindings that might hint at browser execution.
2. Search upstream release/distribution references for a separate deployment repository or missing browser build process.
3. If needed, inspect the public ClassicUO browser deployment behavior from the outside to infer expected runtime files and network calls.

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
