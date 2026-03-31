param(
    [string]$Url = 'http://localhost:5099'
)

try {
    $response = Invoke-WebRequest -UseBasicParsing $Url
    Write-Host "Status: $($response.StatusCode)"
    if ($response.Content -match 'ClassicUO Browser Spike') {
        Write-Host 'Content check: PASS'
    } else {
        Write-Host 'Content check: UNKNOWN'
    }
} catch {
    Write-Host "Test failed: $($_.Exception.Message)"
    exit 1
}
