$projectRoot = Split-Path -Parent $PSScriptRoot
$pidFile = Join-Path $projectRoot 'experiments\BrowserHost\.browserhost.pid'

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
