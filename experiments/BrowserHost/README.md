# BrowserHost Spike

This is the first browser-hosting spike for the custom ClassicUO port.

## What it does right now

- Starts a WebAssembly app in the browser
- Confirms the app is running under the browser .NET runtime
- Checks whether browser storage features such as OPFS are available
- Confirms JavaScript interop is working
- Provides an OPFS smoke test that creates, writes, reads, and lists a test file
- Imports selected files into browser OPFS and lists the stored files
- Can bulk-import a locally staged asset bundle from `wwwroot/local-uo` into OPFS

It does **not** run ClassicUO yet.

## Fast Start

From the repository root:

```powershell
.\scripts\browser-spike-start.ps1
```

Open this URL in Chrome:

```text
http://localhost:5099
```

To verify the local server is responding:

```powershell
.\scripts\browser-spike-test.ps1
```

To stop it:

```powershell
.\scripts\browser-spike-stop.ps1
```

## Manual Commands

If you do not want to use the helper scripts:

```powershell
dotnet restore .\experiments\BrowserHost\BrowserHost.csproj
dotnet build .\experiments\BrowserHost\BrowserHost.csproj -c Debug
dotnet run --project .\experiments\BrowserHost\BrowserHost.csproj --no-launch-profile --urls http://localhost:5099
```

Use `Ctrl+C` in that terminal window to stop it.

## Local Asset Seeding

For large UO files, use the local seeding script instead of browser file uploads:

```powershell
.\scripts\browser-spike-seed-assets.ps1 -SourcePath "D:\Path\To\Your\UO\Assets"
```

That stages a local bundle under `experiments\BrowserHost\wwwroot\local-uo` and creates a manifest the browser spike can import with the `Import local seed to /uo` button.

By default it stages the current minimal bundle:

- `tiledata.mul`
- `hues.mul`
- `map0.mul`
- `art.mul`
- `artidx.mul`
- `gumpart.mul`
- `gumpidx.mul`
- `cliloc.enu`

To stage every file under the source folder instead:

```powershell
.\scripts\browser-spike-seed-assets.ps1 -SourcePath "D:\Path\To\Your\UO\Assets" -AllFiles
```

## What to check in the browser

1. The page title should be `ClassicUO Browser Spike`.
2. The page should show `Probe completed successfully.`
3. `.NET browser runtime` should say `yes`.
4. `OPFS API` should ideally say `yes` in Chrome.
5. The `Run OPFS smoke test` button should succeed and show a `probe.txt` file.
6. The `Import files to OPFS` button should store selected files and list them under `Stored browser files`.
7. If a local seed bundle exists, `Import local seed to /uo` should import it without using the browser file picker.

## Why this exists

This spike proves the first required layers for a custom browser port:

- browser host
- JS interop
- browser capability probing
- browser storage smoke testing
- browser asset import into OPFS

Once this is stable, the next step is to wire browser storage into the main filesystem abstraction.
