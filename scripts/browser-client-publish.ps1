param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug"
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$clientProject = Join-Path $repoRoot "src\ClassicUO.Client\ClassicUO.Client.csproj"
$indexTemplate = Join-Path $repoRoot "scripts\browser-client-index.html"

dotnet publish $clientProject `
    -c $Configuration `
    -p:RuntimeIdentifier=browser-wasm `
    -p:SelfContained=true `
    -p:PublishAot=false

$bundlePath = Join-Path $repoRoot "bin\$Configuration\net10.0\browser-wasm\AppBundle"

if (-not (Test-Path $bundlePath)) {
    throw "Browser app bundle was not produced at $bundlePath"
}

$indexPath = Join-Path $bundlePath "index.html"
Copy-Item -LiteralPath $indexTemplate -Destination $indexPath -Force

Write-Host "Browser app bundle: $bundlePath"
Write-Host "Browser app index: $indexPath"
