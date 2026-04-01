param(
    [string]$Url = 'http://localhost:5099',
    [string]$ReportUrl = 'http://localhost:5100'
)

$projectRoot = Split-Path -Parent $PSScriptRoot
$projectPath = Join-Path $projectRoot 'experiments\BrowserHost\BrowserHost.csproj'
$reportProjectPath = Join-Path $projectRoot 'experiments\BrowserHost.ReportHost\BrowserHost.ReportHost.csproj'
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
Write-Host "To stop it, run: .\\scripts\\browser-spike-stop.ps1"
