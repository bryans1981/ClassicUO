param(
    [string]$Url = 'http://localhost:5099/?autoSelfTest=1',
    [string]$ReportPath = (Join-Path (Join-Path $PSScriptRoot '..') 'docs\test-results\browser-self-test-latest.json'),
    [int]$TimeoutSeconds = 120,
    [switch]$KeepBrowserOpen
)

$edgePath = 'C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe'
$browserProfileRoot = Join-Path $env:TEMP 'ClassicUO-BrowserSelfTest'
$browserProfilePath = Join-Path $browserProfileRoot 'EdgeProfile'

if (-not (Test-Path $edgePath)) {
    throw "Microsoft Edge was not found at $edgePath."
}

if (-not (Test-Path $ReportPath)) {
    throw "Report file was not found at $ReportPath."
}

$before = (Get-Item $ReportPath).LastWriteTimeUtc
$browserProc = $null

try {
    New-Item -ItemType Directory -Force -Path $browserProfilePath | Out-Null
    $browserProc = Start-Process -FilePath $edgePath -ArgumentList "--new-window --user-data-dir=`"$browserProfilePath`" `"$Url`"" -PassThru
    Start-Sleep -Seconds 2

    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    while ((Get-Date) -lt $deadline) {
        Start-Sleep -Seconds 2

        if (-not (Test-Path $ReportPath)) {
            continue
        }

        $current = (Get-Item $ReportPath).LastWriteTimeUtc
        if ($current -le $before) {
            continue
        }

        $report = Get-Content -Raw $ReportPath | ConvertFrom-Json
        $summary = [string]$report.Summary
        $mode = [string]$report.ReportMode
        $marker = [string]$report.RuntimeChainMarker

        Write-Host "Saved report updated."
        Write-Host "Mode: $mode"
        Write-Host "Marker: $marker"
        Write-Host "Summary: $summary"
        exit 0
    }

    throw "Timed out waiting for the self-test report to update."
}
finally {
    if ($browserProc -and -not $KeepBrowserOpen) {
        try {
            Stop-Process -Id $browserProc.Id -Force -ErrorAction SilentlyContinue
        } catch {
        }
    }
}
