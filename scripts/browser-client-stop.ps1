param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug"
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$proxyPidFile = Join-Path $repoRoot "tools\ws\.wsproxy.pid"
$pidFile = Join-Path $repoRoot "bin\$Configuration\net10.0\browser-wasm\.browser-client-server.pid"
$browserPidFile = Join-Path $repoRoot "bin\$Configuration\net10.0\browser-wasm\.browser-client-browser.pid"
$proxyReadyFile = Join-Path $repoRoot "bin\$Configuration\net10.0\browser-wasm\AppBundle\browser-proxy-ready.json"

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

if (Test-Path $proxyPidFile) {
    $proxyPidValue = Get-Content $proxyPidFile -ErrorAction SilentlyContinue
    $proxyProcess = Get-Process -Id $proxyPidValue -ErrorAction SilentlyContinue

    if ($proxyProcess) {
        Stop-Process -Id $proxyPidValue -Force
        Write-Host "Stopped websocket proxy process $proxyPidValue."
    } else {
        Write-Host "No running websocket proxy process was found."
    }

    Remove-Item -LiteralPath $proxyPidFile -Force -ErrorAction SilentlyContinue
}

if (Test-Path $browserPidFile) {
    $browserPidValue = Get-Content $browserPidFile -ErrorAction SilentlyContinue
    $browserProcess = Get-Process -Id $browserPidValue -ErrorAction SilentlyContinue

    if ($browserProcess) {
        Stop-Process -Id $browserPidValue -Force
        Write-Host "Stopped browser client window process $browserPidValue."
    } else {
        Write-Host "No running browser client window process was found."
    }

    Remove-Item -LiteralPath $browserPidFile -Force -ErrorAction SilentlyContinue
}

if (Test-Path $proxyReadyFile) {
    Remove-Item -LiteralPath $proxyReadyFile -Force -ErrorAction SilentlyContinue
}
