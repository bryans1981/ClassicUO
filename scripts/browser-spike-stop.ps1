$projectRoot = Split-Path -Parent $PSScriptRoot
$proxyProjectRoot = Join-Path $projectRoot 'tools\ws'
$proxyPidFile = Join-Path $projectRoot 'tools\ws\.wsproxy.pid'
$pidFile = Join-Path $projectRoot 'experiments\BrowserHost\.browserhost.pid'
$reportPidFile = Join-Path $projectRoot 'experiments\BrowserHost.ReportHost\.reporthost.pid'

if (-not (Test-Path $pidFile)) {
    Write-Host 'Browser spike is not running.'
    exit 0
}

$processId = Get-Content $pidFile -ErrorAction SilentlyContinue
if ($processId -and (Get-Process -Id $processId -ErrorAction SilentlyContinue)) {
    Stop-Process -Id $processId -Force
    Write-Host "Stopped browser spike process $processId."
} else {
    Write-Host 'No running browser spike process was found.'
}

Remove-Item $pidFile -Force -ErrorAction SilentlyContinue

if (Test-Path $reportPidFile) {
    $reportProcessId = Get-Content $reportPidFile -ErrorAction SilentlyContinue
    if ($reportProcessId -and (Get-Process -Id $reportProcessId -ErrorAction SilentlyContinue)) {
        Stop-Process -Id $reportProcessId -Force
        Write-Host "Stopped report receiver process $reportProcessId."
    } else {
        Write-Host 'No running report receiver process was found.'
    }

    Remove-Item $reportPidFile -Force -ErrorAction SilentlyContinue
}

if (Test-Path $proxyPidFile) {
    $proxyProcessId = Get-Content $proxyPidFile -ErrorAction SilentlyContinue
    if ($proxyProcessId -and (Get-Process -Id $proxyProcessId -ErrorAction SilentlyContinue)) {
        Stop-Process -Id $proxyProcessId -Force
        Write-Host "Stopped websocket proxy process $proxyProcessId."
    } else {
        Write-Host 'No running websocket proxy process was found.'
    }

    Remove-Item $proxyPidFile -Force -ErrorAction SilentlyContinue
}
