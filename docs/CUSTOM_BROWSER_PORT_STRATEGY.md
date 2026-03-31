# Custom Browser Port Strategy

## Goal

Build a private browser-hosted version of ClassicUO for personal sessions by porting the open-source client to a custom WebAssembly/browser runtime path.

## Bottom-Line Assessment

This is possible enough to justify a proof-of-concept, but it is not a packaging exercise.

It is a port.

The main reason is that the current client is built around desktop-oriented graphics, native library loading, and direct filesystem access.

## Confirmed Constraints

### 1. The current graphics/runtime stack is desktop-oriented

Relevant files:

- `src/ClassicUO.Client/GameController.cs`
- `src/ClassicUO.Client/ClassicUO.Client.csproj`
- `src/ClassicUO.Renderer/ClassicUO.Renderer.csproj`

Observed facts:

- `GameController` derives from `Microsoft.Xna.Framework.Game` and uses `GraphicsDeviceManager`, SDL window APIs, and FNA rendering flow.
- The client project publishes native shared libraries and copies platform-specific external graphics/audio dependencies.
- FNA's public README describes FNA as focused on an accurate XNA runtime for the desktop.

Implication:

- We should assume the current rendering path will not run in the browser unchanged.
- Rendering is likely the highest-risk subsystem.

### 2. The asset model assumes direct filesystem access

Relevant files:

- `src/ClassicUO.Client/Main.cs`
- `src/ClassicUO.Assets/UOFileManager.cs`

Observed facts:

- Startup requires a real `UltimaOnlineDirectory` and validates `tiledata.mul`.
- `UOFileManager` and loaders use path-based file resolution extensively.
- The main codebase now routes core file access through `FileSystemHelper`.
- The IO layer now accepts generic `Stream` input, retaining memory-mapped fast paths only for desktop `FileStream` inputs.

Implication:

- We now have the right seam for browser-backed file reads.
- The next requirement is a browser-side storage provider that can serve imported OPFS files back through that seam.

### 3. Native loading and plugin bootstrap are incompatible with browser execution

Relevant files:

- `src/ClassicUO.Utility/Platforms/Native.cs`
- `src/ClassicUO.Bootstrap/src/Program.cs`
- `src/ClassicUO.Client/PluginHost.cs`

Observed facts:

- The code uses `DllImport`, dynamic library loading, exported entrypoints, and unmanaged host bindings.
- The bootstrap project is a desktop launcher that loads the native-published client library.

Implication:

- Browser execution cannot rely on the existing bootstrap path.
- Plugin support should be disabled for the first browser milestone.

### 4. Browser-compatible networking already exists

Relevant files:

- `src/ClassicUO.Client/Network/NetClient.cs`
- `src/ClassicUO.Client/Network/Socket/WebSocketWrapper.cs`
- `tools/ws/README.md`

Observed facts:

- The client supports `ws://` and `wss://` shard endpoints.
- A websocket transport path already exists.

Implication:

- Networking is not the main blocker.
- We can reuse the websocket transport model with a gateway.

### 5. Per-user session isolation already has a useful configuration hook

Relevant files:

- `src/ClassicUO.Client/Main.cs`
- `src/ClassicUO.Client/Configuration/Settings.cs`

Observed facts:

- The client supports `-settings` to use a custom settings file.

Implication:

- This can become our browser session/profile isolation model, even if the backing storage changes.

## External Platform Findings

### .NET WebAssembly

Relevant sources:

- Microsoft Learn: WebAssembly Browser App and JSImport/JSExport interop
- Microsoft Learn: .NET on Web Workers

Observed facts:

- .NET can run in the browser via a WebAssembly Browser App model.
- JSImport/JSExport provides a supported JS/.NET bridge.
- Web Workers are a supported option for CPU-heavy work.

Implication:

- A custom browser host around the ClassicUO codebase is technically plausible.
- JS interop should be treated as a first-class design layer, not an escape hatch.

### Browser filesystem

Relevant source:

- web.dev OPFS guidance

Observed facts:

- OPFS provides origin-scoped persistent storage.
- OPFS is suitable for WebAssembly-heavy and performance-sensitive file access.
- Web Workers are the preferred place for synchronous high-speed file operations.

Implication:

- OPFS is the right default storage target for browser-side UO assets.

## Recommended Port Strategy

Do not try to compile the whole current app to browser-wasm in one step.

Instead split the work into five tracks.

### Track A: Browser host and runtime spike

Objective:

Create the smallest possible .NET WebAssembly browser host that can load selected ClassicUO libraries and run a minimal entrypoint.

Deliverables:

- Browser host project
- `main.js` or equivalent startup file
- Proof that selected code runs under `browser-wasm`

Success criteria:

- We can boot managed code in a browser and call into JS.

### Track B: Filesystem abstraction

Objective:

Remove the hard dependency on OS paths for browser mode.

Work:

- Introduce an abstraction around file enumeration, file reads, and directory existence.
- Keep the current path-based implementation for desktop.
- Add a browser implementation backed by OPFS and/or fetched blobs.

Success criteria:

- Asset validation and file reads work without a host OS directory.

### Track C: Asset bootstrap and caching

Objective:

Load required UO files into browser storage.

Work:

- Define the first minimal asset set needed to reach the login scene.
- Build a browser bootstrap flow that imports or downloads these files.
- Cache files in OPFS.

Success criteria:

- Browser session can open required asset files repeatedly without re-download.

### Track D: Rendering replacement or adaptation

Objective:

Make the game render in browser mode.

Work:

- Evaluate whether FNA can be adapted with a browser-capable backend or whether a replacement rendering path is required.
- If adaptation is not viable, isolate rendering interfaces and target WebGL/WebGPU through a browser-specific layer.

Success criteria:

- Render a minimal scene in the browser.

### Track E: Networking and session launch

Objective:

Use websocket-based connectivity from the browser to a shard.

Work:

- Reuse the current websocket transport path.
- Use `gate` as a reference or direct component.
- Provide per-session settings/config generation.

Success criteria:

- Browser client reaches login over websocket transport.

## Milestone Order

### Milestone P1: Browser Host Spike

Target outcome:

- A minimal browser-wasm host project exists and executes managed code from this solution.

### Milestone P2: Headless Asset Access Spike

Target outcome:

- Browser mode can mount or emulate enough file access to validate an asset set.

### Milestone P3: Login-Screen Spike

Target outcome:

- Browser build reaches the login screen with assets loaded and settings supplied.

### Milestone P4: Network Login Spike

Target outcome:

- Browser build connects through websocket transport.

### Milestone P5: Friends-Only Private Deployment

Target outcome:

- Private hosted environment supports named users, isolated settings, and a stable launch path.

## Immediate Engineering Decisions

These should be the defaults unless disproven quickly:

- Browser host model: .NET WebAssembly Browser App, not Blazor-first UI
- JS bridge: JSImport/JSExport
- Browser storage: OPFS
- First network path: websocket proxy/gateway
- First browser scope: no plugins, no assistant loading, no public shard management

## First Spike Tasks

1. Create a new experimental browser host project in the repo.
2. Prove `browser-wasm` publish output can load and run a minimal entrypoint.
3. Add environment detection using `OperatingSystem.IsBrowser()` or equivalent browser-specific compile/runtime checks.
4. Add a browser mode that bypasses native bootstrap and plugin loading.
5. Identify the minimum code path needed to construct configuration and load one asset file.
6. Prototype JS interop for browser storage access.
7. Prototype websocket connection setup from browser mode.

## Things We Should Explicitly Cut From The First Attempt

- Plugins and assistant support
- Full patching/content CDN system
- Production auth
- Broad browser compatibility beyond Chromium-class browsers
- Public multi-tenant features

## Decision Gates

We should stop the port if any of these prove false:

1. We cannot get a minimal managed browser host running from this codebase.
2. We cannot make asset reads performant enough from browser storage.
3. We cannot establish a realistic rendering path without rewriting too much of the client.

If Gate 3 fails, the fallback should be a browser-streamed desktop solution rather than continuing a bad port.
