param(
    [string]$Url = 'http://localhost:5099',
    [string]$ReportUrl = 'http://localhost:5100'
)

$projectRoot = Split-Path -Parent $PSScriptRoot
$projectPath = Join-Path $projectRoot 'experiments\BrowserHost\BrowserHost.csproj'
$reportProjectPath = Join-Path $projectRoot 'experiments\BrowserHost.ReportHost\BrowserHost.ReportHost.csproj'
$proxyProjectRoot = Join-Path $projectRoot 'tools\ws'
$proxyPidFile = Join-Path $projectRoot 'tools\ws\.wsproxy.pid'
$pidFile = Join-Path $projectRoot 'experiments\BrowserHost\.browserhost.pid'
$reportPidFile = Join-Path $projectRoot 'experiments\BrowserHost.ReportHost\.reporthost.pid'

function Test-ManagedProcess {
    param(
        [string]$PidValue,
        [string]$ExpectedProjectPath
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

    return $commandLine -like "*$ExpectedProjectPath*"
}

if (Test-Path $pidFile) {
    $existingPid = Get-Content $pidFile -ErrorAction SilentlyContinue
    if (Test-ManagedProcess -PidValue $existingPid -ExpectedProjectPath $projectPath) {
        Write-Host "Browser spike is already running at $Url (PID $existingPid)."
        exit 0
    }

    Remove-Item $pidFile -Force -ErrorAction SilentlyContinue
}

if (Test-Path $reportPidFile) {
    $existingReportPid = Get-Content $reportPidFile -ErrorAction SilentlyContinue
    if (Test-ManagedProcess -PidValue $existingReportPid -ExpectedProjectPath $reportProjectPath) {
        Write-Host "Local report receiver is already running at $ReportUrl (PID $existingReportPid)."
    } else {
        Remove-Item $reportPidFile -Force -ErrorAction SilentlyContinue
    }
}

$proxyProc = $null
if (Test-Path $proxyPidFile) {
    $existingProxyPid = Get-Content $proxyPidFile -ErrorAction SilentlyContinue
    if (Test-ManagedProcess -PidValue $existingProxyPid -ExpectedProjectPath $proxyProjectRoot) {
        Write-Host "Local websocket proxy is already running at ws://127.0.0.1:2594 (PID $existingProxyPid)."
    } else {
        Remove-Item $proxyPidFile -Force -ErrorAction SilentlyContinue
    }
}

$proxyNode = Get-Command node -ErrorAction SilentlyContinue
if (-not (Test-Path $proxyPidFile) -and $proxyNode -and (Test-Path (Join-Path $proxyProjectRoot 'node_modules')))
{
    $proxyProc = Start-Process node -ArgumentList "proxy.mjs" -WorkingDirectory $proxyProjectRoot -PassThru
    $proxyProc.Id | Set-Content $proxyPidFile
    Write-Host "Local websocket proxy URL: ws://127.0.0.1:2594"
    Write-Host "Local websocket proxy PID: $($proxyProc.Id)"
}
elseif (-not (Test-Path (Join-Path $proxyProjectRoot 'node_modules')))
{
    Write-Host "Local websocket proxy dependencies are not installed yet. Run `npm install` in tools\ws to enable ws://127.0.0.1:2594."
}

$reportProc = $null
if (-not (Test-Path $reportPidFile)) {
    $reportProc = Start-Process dotnet -ArgumentList "run --project `"$reportProjectPath`" --no-launch-profile --urls $ReportUrl" -WorkingDirectory $projectRoot -PassThru
    $reportProc.Id | Set-Content $reportPidFile
}

$proc = Start-Process dotnet -ArgumentList "run --project `"$projectPath`" --no-launch-profile --urls $Url" -WorkingDirectory $projectRoot -PassThru
$proc.Id | Set-Content $pidFile

Write-Host "Browser spike started."
Write-Host "URL: $Url"
Write-Host "PID: $($proc.Id)"
if ($reportProc) {
    Write-Host "Report receiver URL: $ReportUrl"
    Write-Host "Report receiver PID: $($reportProc.Id)"
}
if ($proxyProc) {
    Write-Host "Local websocket proxy URL: ws://127.0.0.1:2594"
    Write-Host "Local websocket proxy PID: $($proxyProc.Id)"
}
Write-Host "To stop it, run: .\\scripts\\browser-spike-stop.ps1"
