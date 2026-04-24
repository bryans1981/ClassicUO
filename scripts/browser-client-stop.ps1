param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug"
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$pidFile = Join-Path $repoRoot "bin\$Configuration\net10.0\browser-wasm\.browser-client-server.pid"

if (-not (Test-Path $pidFile)) {
    Write-Host "No browser client server PID file found."
    exit 0
}

$pidValue = Get-Content $pidFile -ErrorAction SilentlyContinue
$process = Get-Process -Id $pidValue -ErrorAction SilentlyContinue

if ($process) {
    Stop-Process -Id $pidValue -Force
    Write-Host "Stopped browser client server process $pidValue."
} else {
    Write-Host "No running browser client server process was found."
}

Remove-Item -LiteralPath $pidFile -Force -ErrorAction SilentlyContinue
