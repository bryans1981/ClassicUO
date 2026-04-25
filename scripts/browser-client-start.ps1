param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug",
    [int]$Port = 5110,
    [string]$ProxyTarget = "127.0.0.1:2593",
    [string]$LoginUsername = "",
    [string]$LoginPassword = ""
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$proxyProjectRoot = Join-Path $repoRoot 'tools\ws'
$proxyPidFile = Join-Path $repoRoot 'tools\ws\.wsproxy.pid'
$pidFile = Join-Path $repoRoot "bin\$Configuration\net10.0\browser-wasm\.browser-client-server.pid"
$bundlePath = Join-Path $repoRoot "bin\$Configuration\net10.0\browser-wasm\AppBundle"
$browserLoginPath = Join-Path $bundlePath "browser-login.json"
$browserLoginAssetPath = Join-Path $bundlePath "uo\browser-login.json"

function Test-ManagedProcess {
    param(
        [string]$PidValue,
        [string]$ExpectedCommandLineToken
    )

    if (-not $PidValue) {
        return $false
    }

    $process = Get-Process -Id $PidValue -ErrorAction SilentlyContinue
    if (-not $process) {
        return $false
    }

    try {
        $commandLine = (Get-CimInstance Win32_Process -Filter "ProcessId = $PidValue").CommandLine
    } catch {
        return $false
    }

    if (-not $commandLine) {
        return $false
    }

    return $commandLine -like "*$ExpectedCommandLineToken*"
}

& (Join-Path $PSScriptRoot "browser-client-publish.ps1") -Configuration $Configuration

if (-not [string]::IsNullOrWhiteSpace($LoginUsername) -and -not [string]::IsNullOrWhiteSpace($LoginPassword)) {
    $loginPayload = [ordered]@{
        username = $LoginUsername
        password = $LoginPassword
    } | ConvertTo-Json -Compress

    Set-Content -LiteralPath $browserLoginPath -Value $loginPayload -Encoding UTF8
    Set-Content -LiteralPath $browserLoginAssetPath -Value $loginPayload -Encoding UTF8
    Write-Host "Browser login override: $LoginUsername"
} elseif (Test-Path $browserLoginPath) {
    Remove-Item -LiteralPath $browserLoginPath -Force -ErrorAction SilentlyContinue
    Remove-Item -LiteralPath $browserLoginAssetPath -Force -ErrorAction SilentlyContinue
}

if (Test-Path $pidFile) {
    $existingPid = Get-Content $pidFile -ErrorAction SilentlyContinue
    $existingProcess = Get-Process -Id $existingPid -ErrorAction SilentlyContinue

    if ($existingProcess) {
        Write-Host "Browser client server is already running at http://localhost:$Port/ (PID $existingPid)."
        exit 0
    }

    Remove-Item -LiteralPath $pidFile -Force -ErrorAction SilentlyContinue
}

$proxyNode = Get-Command node -ErrorAction SilentlyContinue
if (Test-Path $proxyPidFile) {
    $existingProxyPid = Get-Content $proxyPidFile -ErrorAction SilentlyContinue
    if (Test-ManagedProcess -PidValue $existingProxyPid -ExpectedCommandLineToken 'proxy.mjs') {
        Write-Host "Local websocket proxy is already running at ws://127.0.0.1:2594 (PID $existingProxyPid)."
    } else {
        Remove-Item -LiteralPath $proxyPidFile -Force -ErrorAction SilentlyContinue
    }
}

if (-not (Test-Path $proxyPidFile) -and $proxyNode -and (Test-Path (Join-Path $proxyProjectRoot 'node_modules'))) {
    $proxyProc = Start-Process node -ArgumentList @('proxy.mjs', '--target', $ProxyTarget) -WorkingDirectory $proxyProjectRoot -PassThru
    $proxyProc.Id | Set-Content $proxyPidFile
    Write-Host "Local websocket proxy URL: ws://127.0.0.1:2594"
    Write-Host "Local websocket proxy target: $ProxyTarget"
    Write-Host "Local websocket proxy PID: $($proxyProc.Id)"
} elseif (-not (Test-Path (Join-Path $proxyProjectRoot 'node_modules'))) {
    Write-Host "Local websocket proxy dependencies are not installed yet. Run `npm install` in tools\ws to enable ws://127.0.0.1:2594."
}

$serveScript = Join-Path $PSScriptRoot "browser-client-serve.ps1"

$process = Start-Process powershell -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$serveScript`" -BundlePath `"$bundlePath`" -Port $Port" -WorkingDirectory $repoRoot -PassThru
$process.Id | Set-Content $pidFile

Write-Host "Browser client URL: http://localhost:$Port/"
Write-Host "Browser client server PID: $($process.Id)"
Write-Host "To stop it, run: .\scripts\browser-client-stop.ps1"
