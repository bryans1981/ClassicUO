param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug",
    [string]$AssetVersion
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$clientProject = Join-Path $repoRoot "src\ClassicUO.Client\ClassicUO.Client.csproj"
$indexTemplate = Join-Path $repoRoot "scripts\browser-client-index.html"
$assetRoot = Join-Path $repoRoot "browser-assets\uo"
$assetVersionsRoot = Join-Path $assetRoot "versions"
$activeVersionFile = Join-Path $assetRoot "active-version.txt"
$wasmCachePath = Join-Path $env:LOCALAPPDATA "ClassicUO\emscripten-cache"

if (-not $AssetVersion) {
    if (Test-Path -LiteralPath $activeVersionFile) {
        $activeVersionText = Get-Content -LiteralPath $activeVersionFile -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($activeVersionText) {
            $AssetVersion = $activeVersionText.Trim()
        }
    }
}

if ([string]::IsNullOrWhiteSpace($AssetVersion)) {
    $AssetVersion = "default"
}

$sourceAssetPath = Join-Path $assetVersionsRoot $AssetVersion

if (-not (Test-Path -LiteralPath $sourceAssetPath)) {
    throw "Browser asset version '$AssetVersion' was not found at $sourceAssetPath"
}

& (Join-Path $PSScriptRoot "browser-native-deps-build.ps1")

dotnet publish $clientProject `
    -c $Configuration `
    -r browser-wasm `
    -p:SelfContained=true `
    -p:PublishAot=false `
    -p:WasmCachePath="$wasmCachePath" `
    -p:BrowserWasmBuild=true

if ($LASTEXITCODE -ne 0) {
    throw "Browser client publish failed with exit code $LASTEXITCODE"
}

$bundlePath = Join-Path $repoRoot "bin\$Configuration\net10.0\browser-wasm\AppBundle"
$buildStamp = Get-Date -Format "yyyyMMddHHmmss"
$buildStampFile = Join-Path $bundlePath "browser-build-stamp.txt"

if (-not (Test-Path $bundlePath)) {
    throw "Browser app bundle was not produced at $bundlePath"
}

$bundleAssetRoot = Join-Path $bundlePath "uo"
$bundleManifestPath = Join-Path $bundlePath "uo-manifest.json"

if (Test-Path -LiteralPath $bundleAssetRoot) {
    Remove-Item -LiteralPath $bundleAssetRoot -Recurse -Force
}

New-Item -ItemType Directory -Path $bundleAssetRoot -Force | Out-Null

Get-ChildItem -LiteralPath $sourceAssetPath -Recurse -File | ForEach-Object {
    $relativePath = $_.FullName.Substring($sourceAssetPath.Length).TrimStart('\')
    $destinationPath = Join-Path $bundleAssetRoot $relativePath
    $destinationDirectory = Split-Path -Parent $destinationPath

    if (-not (Test-Path -LiteralPath $destinationDirectory)) {
        New-Item -ItemType Directory -Path $destinationDirectory -Force | Out-Null
    }

    Copy-Item -LiteralPath $_.FullName -Destination $destinationPath -Force
}

$manifest = [ordered]@{
    version = $AssetVersion
    root = "/uo"
    files = @(Get-ChildItem -LiteralPath $sourceAssetPath -Recurse -File | ForEach-Object {
        $relativePath = $_.FullName.Substring($sourceAssetPath.Length).TrimStart('\').Replace('\', '/')
        "/uo/$relativePath"
    })
}

$manifest | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath $bundleManifestPath -Encoding utf8
Set-Content -LiteralPath $buildStampFile -Value $buildStamp -Encoding utf8

$bootJsPath = Join-Path $bundlePath "_framework\dotnet.boot.js"
if (Test-Path -LiteralPath $bootJsPath) {
    $bootJsLines = Get-Content -LiteralPath $bootJsPath
    $filteredBootJsLines = New-Object System.Collections.Generic.List[string]
    $inSymbolBlock = $false

    foreach ($line in $bootJsLines) {
        if (-not $inSymbolBlock -and $line -match '^\s*"wasmSymbols": \[$') {
            $filteredBootJsLines.Add('    "wasmSymbols": [],')
            $inSymbolBlock = $true
            continue
        }

        if ($inSymbolBlock) {
            if ($line -match '^\s*"coreAssembly": \[$') {
                $filteredBootJsLines.Add($line)
                $inSymbolBlock = $false
            }

            continue
        }

        $filteredBootJsLines.Add($line)
    }

    Set-Content -LiteralPath $bootJsPath -Value $filteredBootJsLines -Encoding utf8
}

$indexPath = Join-Path $bundlePath "index.html"
Copy-Item -LiteralPath $indexTemplate -Destination $indexPath -Force

Write-Host "Browser app bundle: $bundlePath"
Write-Host "Browser app index: $indexPath"
Write-Host "Browser build stamp: $buildStamp"
Write-Host "Browser asset version: $AssetVersion"
Write-Host "Browser asset source: $sourceAssetPath"
Write-Host "Browser asset manifest: $bundleManifestPath"
