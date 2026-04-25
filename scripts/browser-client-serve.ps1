param(
    [string]$BundlePath,
    [int]$Port = 5110
)

$ErrorActionPreference = "Stop"

if (-not $BundlePath) {
    throw "BundlePath is required."
}

$resolvedBundle = (Resolve-Path -LiteralPath $BundlePath).Path
$buildStampFile = Join-Path $resolvedBundle 'browser-build-stamp.txt'
$buildStamp = if (Test-Path -LiteralPath $buildStampFile) {
    (Get-Content -LiteralPath $buildStampFile -ErrorAction SilentlyContinue | Select-Object -First 1).Trim()
} else {
    ''
}
$buildPrefix = if ($buildStamp) { "build/$buildStamp/" } else { '' }
$logPath = Join-Path $env:TEMP 'classicuo-browser-serve.log'
$null = Remove-Item -LiteralPath $logPath -Force -ErrorAction SilentlyContinue
$listener = [System.Net.HttpListener]::new()
$listener.Prefixes.Add("http://localhost:$Port/")
$listener.Start()

Write-Host "Serving ClassicUO browser bundle from $resolvedBundle"
Write-Host "URL: http://localhost:$Port/"
if ($buildPrefix) {
    Write-Host "Build prefix: /$buildPrefix"
}

function Get-ContentType {
    param([string]$Path)

    switch ([System.IO.Path]::GetExtension($Path).ToLowerInvariant()) {
        ".html" { "text/html; charset=utf-8"; break }
        ".js" { "text/javascript; charset=utf-8"; break }
        ".json" { "application/json; charset=utf-8"; break }
        ".wasm" { "application/wasm"; break }
        ".map" { "application/json; charset=utf-8"; break }
        ".css" { "text/css; charset=utf-8"; break }
        default { "application/octet-stream"; break }
    }
}

try {
    while ($listener.IsListening) {
        $context = $listener.GetContext()
        $requestPath = [Uri]::UnescapeDataString($context.Request.Url.AbsolutePath.TrimStart("/"))

        if ([string]::IsNullOrWhiteSpace($requestPath)) {
            if ($buildPrefix) {
                $context.Response.StatusCode = 302
                $context.Response.RedirectLocation = "/$buildPrefix" + "index.html"
                $context.Response.Close()
                continue
            }

            $requestPath = "index.html"
        }

        if ($buildPrefix -and $requestPath.StartsWith($buildPrefix, [System.StringComparison]::OrdinalIgnoreCase)) {
            $requestPath = $requestPath.Substring($buildPrefix.Length)
        }

        $localPath = [System.IO.Path]::GetFullPath((Join-Path $resolvedBundle $requestPath))

        if (-not $localPath.StartsWith($resolvedBundle, [System.StringComparison]::OrdinalIgnoreCase) -or -not (Test-Path -LiteralPath $localPath -PathType Leaf)) {
            Add-Content -LiteralPath $logPath -Value ("404 {0}" -f $requestPath)
            $context.Response.StatusCode = 404
            $context.Response.Close()
            continue
        }

        $bytes = [System.IO.File]::ReadAllBytes($localPath)
        $context.Response.ContentType = Get-ContentType -Path $localPath
        $context.Response.Headers['Cache-Control'] = 'no-store, no-cache, must-revalidate, max-age=0'
        $context.Response.Headers['Pragma'] = 'no-cache'
        $context.Response.Headers['Expires'] = '0'
        $context.Response.ContentLength64 = $bytes.Length
        $context.Response.OutputStream.Write($bytes, 0, $bytes.Length)
        $context.Response.Close()
    }
}
finally {
    $listener.Stop()
    $listener.Close()
}
