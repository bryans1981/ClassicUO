param(
    [string]$Url = 'http://localhost:5099/?autoSelfTest=1',
    [string]$ReportPath = (Join-Path (Join-Path $PSScriptRoot '..') 'docs\test-results\browser-self-test-latest.json'),
    [int]$TimeoutSeconds = 120
)

$edgePath = 'C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe'

if (-not (Test-Path $edgePath)) {
    throw "Microsoft Edge was not found at $edgePath."
}

if (-not (Test-Path $ReportPath)) {
    throw "Report file was not found at $ReportPath."
}

$before = (Get-Item $ReportPath).LastWriteTimeUtc
Start-Process -FilePath $edgePath -ArgumentList "--new-window `"$Url`"" | Out-Null

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
