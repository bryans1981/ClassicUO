param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug",
    [int]$Port = 5110
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$pidFile = Join-Path $repoRoot "bin\$Configuration\net10.0\browser-wasm\.browser-client-server.pid"
$bundlePath = Join-Path $repoRoot "bin\$Configuration\net10.0\browser-wasm\AppBundle"

& (Join-Path $PSScriptRoot "browser-client-publish.ps1") -Configuration $Configuration

if (Test-Path $pidFile) {
    $existingPid = Get-Content $pidFile -ErrorAction SilentlyContinue
    $existingProcess = Get-Process -Id $existingPid -ErrorAction SilentlyContinue

    if ($existingProcess) {
        Write-Host "Browser client server is already running at http://localhost:$Port/ (PID $existingPid)."
        exit 0
    }

    Remove-Item -LiteralPath $pidFile -Force -ErrorAction SilentlyContinue
}

$serveScript = Join-Path $PSScriptRoot "browser-client-serve.ps1"
$process = Start-Process powershell -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$serveScript`" -BundlePath `"$bundlePath`" -Port $Port" -WorkingDirectory $repoRoot -PassThru
$process.Id | Set-Content $pidFile

Write-Host "Browser client URL: http://localhost:$Port/"
Write-Host "Browser client server PID: $($process.Id)"
Write-Host "To stop it, run: .\scripts\browser-client-stop.ps1"
