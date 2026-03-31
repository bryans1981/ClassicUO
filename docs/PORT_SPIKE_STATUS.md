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

Use the new browser asset byte-source path to satisfy real loader-style reads from `/uo`, starting with the tiledata loader expectations and then expanding to additional asset files.


