param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug",
    [int]$Port = 5110,
    [string]$ProxyTarget = "127.0.0.1:2593",
    [string]$LoginUsername = "",
    [string]$LoginPassword = "",
    [switch]$OpenBrowser,
    [string]$BrowserPath = 'C:\Program Files (x86)\Google\Chrome\Application\chrome.exe',
    [switch]$KeepBrowserOpen
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$proxyProjectRoot = Join-Path $repoRoot 'tools\ws'
$proxyPidFile = Join-Path $repoRoot 'tools\ws\.wsproxy.pid'
$pidFile = Join-Path $repoRoot "bin\$Configuration\net10.0\browser-wasm\.browser-client-server.pid"
$browserPidFile = Join-Path $repoRoot "bin\$Configuration\net10.0\browser-wasm\.browser-client-browser.pid"
$bundlePath = Join-Path $repoRoot "bin\$Configuration\net10.0\browser-wasm\AppBundle"
$browserLoginPath = Join-Path $bundlePath "browser-login.json"
$browserLoginAssetPath = Join-Path $bundlePath "uo\browser-login.json"
$serverAlreadyRunning = $false

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

function Stop-StaleBrowserServer {
    param(
        [int]$ListenPort
    )

    $connections = Get-NetTCPConnection -LocalPort $ListenPort -State Listen -ErrorAction SilentlyContinue
    if (-not $connections) {
        return
    }

    $connectionProcessIds = $connections.OwningProcess | Sort-Object -Unique
    foreach ($connectionProcessId in $connectionProcessIds) {
        if (-not $connectionProcessId) {
            continue
        }

        $commandLine = $null
        try {
            $commandLine = (Get-CimInstance Win32_Process -Filter "ProcessId = $connectionProcessId").CommandLine
        } catch {
            $commandLine = $null
        }

        if ($commandLine -and $commandLine -notlike "*browser-client-serve.ps1*") {
            continue
        }

        Stop-Process -Id $connectionProcessId -Force -ErrorAction SilentlyContinue
        Write-Host "Stopped stale browser client server process $connectionProcessId listening on port $ListenPort."
    }
}

function Stop-ManagedBrowserWindow {
    param(
        [string]$PidFilePath
    )

    if (-not (Test-Path $PidFilePath)) {
        return
    }

    $browserPidValue = Get-Content $PidFilePath -ErrorAction SilentlyContinue
    if (-not $browserPidValue) {
        Remove-Item -LiteralPath $PidFilePath -Force -ErrorAction SilentlyContinue
        return
    }

    $browserProcess = Get-Process -Id $browserPidValue -ErrorAction SilentlyContinue
    if ($browserProcess) {
        Stop-Process -Id $browserPidValue -Force
        Write-Host "Stopped previous browser client window process $browserPidValue."
    } else {
        Write-Host "No running browser client window process was found for PID $browserPidValue."
    }

    Remove-Item -LiteralPath $PidFilePath -Force -ErrorAction SilentlyContinue
}

function Start-BrowserWindow {
    param(
        [string]$TargetUrl,
        [string]$PreferredBrowserPath,
        [switch]$PreserveBrowserOpen
    )

    $resolvedBrowserPath = $PreferredBrowserPath
    if (-not (Test-Path $resolvedBrowserPath)) {
        $fallbackEdgePath = 'C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe'
        if (Test-Path $fallbackEdgePath) {
            $resolvedBrowserPath = $fallbackEdgePath
        } else {
            throw "Chrome was not found at $resolvedBrowserPath and Edge was not found at $fallbackEdgePath."
        }
    }

    $browserProfileRoot = Join-Path $env:TEMP 'ClassicUO-BrowserClient'
    $browserProfilePath = Join-Path $browserProfileRoot 'Profile'
    New-Item -ItemType Directory -Force -Path $browserProfilePath | Out-Null

    $browserArguments = "--new-window --no-first-run --no-default-browser-check --disable-features=ChromeWhatsNewUI --disable-background-timer-throttling --disable-renderer-backgrounding --disable-backgrounding-occluded-windows --user-data-dir=`"$browserProfilePath`""
    $browserArguments += " `"$TargetUrl`""

    $browserStartProcess = @{
        FilePath = $resolvedBrowserPath
        ArgumentList = $browserArguments
        PassThru = $true
        WindowStyle = 'Normal'
    }

    $browserProc = Start-Process @browserStartProcess
    $browserProc.Id | Set-Content $browserPidFile
    Write-Host "Browser client window PID: $($browserProc.Id)"
    Write-Host "Browser client window URL: $TargetUrl"

    if (-not $PreserveBrowserOpen) {
        Write-Host "Browser client window will be closed by scripts/browser-client-stop.ps1."
    }
}

& (Join-Path $PSScriptRoot "browser-client-publish.ps1") -Configuration $Configuration

if (-not [string]::IsNullOrWhiteSpace($LoginUsername) -and -not [string]::IsNullOrWhiteSpace($LoginPassword)) {
    $browserLocalIp = $null
    try {
        $browserLocalIp = Get-NetIPConfiguration |
            Where-Object { $_.IPv4DefaultGateway -and $_.IPv4Address } |
            Sort-Object InterfaceMetric |
            ForEach-Object { $_.IPv4Address.IPAddress } |
            Where-Object { $_ -and $_ -notlike '127.*' -and $_ -notlike '169.254.*' -and $_ -ne '0.0.0.0' } |
            Select-Object -First 1
    } catch {
        $browserLocalIp = $null
    }

    $browserLocalIpValue = if ($browserLocalIp) { $browserLocalIp } else { '127.0.0.1' }

    $loginPayload = [ordered]@{
        username = $LoginUsername
        password = $LoginPassword
        localIp = $browserLocalIpValue
    } | ConvertTo-Json -Compress

    Set-Content -LiteralPath $browserLoginPath -Value $loginPayload -Encoding UTF8
    Set-Content -LiteralPath $browserLoginAssetPath -Value $loginPayload -Encoding UTF8
    Write-Host "Browser login override: $LoginUsername"
    Write-Host "Browser local IP override: $browserLocalIpValue"
}

if (Test-Path $pidFile) {
    $existingPid = Get-Content $pidFile -ErrorAction SilentlyContinue
    $existingProcess = Get-Process -Id $existingPid -ErrorAction SilentlyContinue

    if ($existingProcess) {
        Write-Host "Browser client server is already running at http://localhost:$Port/ (PID $existingPid)."
        $serverAlreadyRunning = $true
    } else {
        Remove-Item -LiteralPath $pidFile -Force -ErrorAction SilentlyContinue
    }
}

$proxyNode = Get-Command node -ErrorAction SilentlyContinue
$proxyStdoutLog = Join-Path $env:TEMP 'classicuo-browser-proxy.stdout.log'
$proxyStderrLog = Join-Path $env:TEMP 'classicuo-browser-proxy.stderr.log'
if (Test-Path $proxyPidFile) {
    $existingProxyPid = Get-Content $proxyPidFile -ErrorAction SilentlyContinue
    if (Test-ManagedProcess -PidValue $existingProxyPid -ExpectedCommandLineToken 'proxy.mjs') {
        Write-Host "Local websocket proxy is already running at ws://127.0.0.1:2594 (PID $existingProxyPid)."
    } else {
        Remove-Item -LiteralPath $proxyPidFile -Force -ErrorAction SilentlyContinue
    }
}

if (-not (Test-Path $proxyPidFile) -and $proxyNode -and (Test-Path (Join-Path $proxyProjectRoot 'node_modules'))) {
    Remove-Item -LiteralPath $proxyStdoutLog, $proxyStderrLog -Force -ErrorAction SilentlyContinue
    $proxyProc = Start-Process node -ArgumentList @('proxy.mjs', '--target', $ProxyTarget) -WorkingDirectory $proxyProjectRoot -PassThru -RedirectStandardOutput $proxyStdoutLog -RedirectStandardError $proxyStderrLog
    $proxyProc.Id | Set-Content $proxyPidFile
    Write-Host "Local websocket proxy URL: ws://127.0.0.1:2594"
    Write-Host "Local websocket proxy target: $ProxyTarget"
    Write-Host "Local websocket proxy PID: $($proxyProc.Id)"
    Write-Host "Local websocket proxy stdout: $proxyStdoutLog"
    Write-Host "Local websocket proxy stderr: $proxyStderrLog"
} elseif (-not (Test-Path (Join-Path $proxyProjectRoot 'node_modules'))) {
    Write-Host "Local websocket proxy dependencies are not installed yet. Run `npm install` in tools\ws to enable ws://127.0.0.1:2594."
}

$null = Stop-ManagedBrowserWindow -PidFilePath $browserPidFile

$serveScript = Join-Path $PSScriptRoot "browser-client-serve.ps1"

if (-not $serverAlreadyRunning) {
    Stop-StaleBrowserServer -ListenPort $Port

    $process = Start-Process powershell -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$serveScript`" -BundlePath `"$bundlePath`" -Port $Port" -WorkingDirectory $repoRoot -PassThru
    $process.Id | Set-Content $pidFile

    Write-Host "Browser client URL: http://localhost:$Port/"
    Write-Host "Browser client server PID: $($process.Id)"
} else {
    Write-Host "Browser client URL: http://localhost:$Port/"
}
if ($OpenBrowser) {
    Start-BrowserWindow -TargetUrl "http://localhost:$Port/" -PreferredBrowserPath $BrowserPath -PreserveBrowserOpen:$KeepBrowserOpen
}
Write-Host "To stop it, run: .\scripts\browser-client-stop.ps1"
