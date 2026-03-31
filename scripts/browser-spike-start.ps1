param(
    [string]$Url = 'http://localhost:5099'
)

$projectRoot = Split-Path -Parent $PSScriptRoot
$projectPath = Join-Path $projectRoot 'experiments\BrowserHost\BrowserHost.csproj'
$pidFile = Join-Path $projectRoot 'experiments\BrowserHost\.browserhost.pid'

if (Test-Path $pidFile) {
    $existingPid = Get-Content $pidFile -ErrorAction SilentlyContinue
    if ($existingPid -and (Get-Process -Id $existingPid -ErrorAction SilentlyContinue)) {
        Write-Host "Browser spike is already running at $Url (PID $existingPid)."
        exit 0
    }

    Remove-Item $pidFile -Force -ErrorAction SilentlyContinue
}

$proc = Start-Process dotnet -ArgumentList "run --project `"$projectPath`" --no-launch-profile --urls $Url" -WorkingDirectory $projectRoot -PassThru
$proc.Id | Set-Content $pidFile

Write-Host "Browser spike started."
Write-Host "URL: $Url"
Write-Host "PID: $($proc.Id)"
Write-Host "To stop it, run: .\\scripts\\browser-spike-stop.ps1"
