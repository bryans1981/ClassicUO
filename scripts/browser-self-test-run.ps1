param(
    [string]$Url = 'http://localhost:5099/?autoSelfTest=1',
    [string]$ReportPath = (Join-Path (Join-Path $PSScriptRoot '..') 'docs\test-results\browser-self-test-latest.json'),
    [int]$TimeoutSeconds = 120,
    [string]$BrowserPath = 'C:\Program Files (x86)\Google\Chrome\Application\chrome.exe',
    [switch]$KeepBrowserMinimized = $true,
    [switch]$KeepBrowserOpen
)

$browserProfileRoot = Join-Path $env:TEMP 'ClassicUO-BrowserSelfTest'
$browserProfilePath = Join-Path $browserProfileRoot ('EdgeProfile-' + [DateTimeOffset]::UtcNow.ToString('yyyyMMddHHmmss'))

if (-not (Test-Path $BrowserPath)) {
    $fallbackEdgePath = 'C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe'
    if (Test-Path $fallbackEdgePath) {
        $BrowserPath = $fallbackEdgePath
    } else {
        throw "Chrome was not found at $BrowserPath and Edge was not found at $fallbackEdgePath."
    }
}

if (-not (Test-Path $ReportPath)) {
    throw "Report file was not found at $ReportPath."
}

function Wait-ForEndpoint {
    param(
        [string]$EndpointUrl,
        [int]$TimeoutSeconds = 60
    )

    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    while ((Get-Date) -lt $deadline) {
        try {
            $response = Invoke-WebRequest -Uri $EndpointUrl -UseBasicParsing -TimeoutSec 5
            if ($response.StatusCode -ge 200 -and $response.StatusCode -lt 300) {
                return
            }
        } catch {
        }

        Start-Sleep -Seconds 2
    }

    throw "Timed out waiting for $EndpointUrl."
}

$browserBaseUrl = [uri]$Url
$serviceBaseUrl = '{0}://{1}:{2}' -f $browserBaseUrl.Scheme, $browserBaseUrl.Host, $browserBaseUrl.Port
Wait-ForEndpoint -EndpointUrl $serviceBaseUrl -TimeoutSeconds $TimeoutSeconds
Wait-ForEndpoint -EndpointUrl 'http://localhost:5100/health' -TimeoutSeconds $TimeoutSeconds

$before = (Get-Item $ReportPath).LastWriteTimeUtc
$browserProc = $null

try {
    Remove-Item -LiteralPath $browserProfilePath -Recurse -Force -ErrorAction SilentlyContinue
    New-Item -ItemType Directory -Force -Path $browserProfilePath | Out-Null
    $browserArguments = "--new-window --user-data-dir=`"$browserProfilePath`""

    if ($KeepBrowserMinimized) {
        $browserArguments += " --start-minimized"
    }

    $browserArguments += " `"$Url`""

    $browserStartProcess = @{
        FilePath = $BrowserPath
        ArgumentList = $browserArguments
        PassThru = $true
    }

    if ($KeepBrowserMinimized) {
        $browserStartProcess.WindowStyle = 'Minimized'
    }

    $browserProc = Start-Process @browserStartProcess
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
