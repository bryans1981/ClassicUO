param(
    [string]$BundlePath,
    [int]$Port = 5110
)

$ErrorActionPreference = "Stop"

if (-not $BundlePath) {
    throw "BundlePath is required."
}

$resolvedBundle = (Resolve-Path -LiteralPath $BundlePath).Path
$listener = [System.Net.HttpListener]::new()
$listener.Prefixes.Add("http://localhost:$Port/")
$listener.Start()

Write-Host "Serving ClassicUO browser bundle from $resolvedBundle"
Write-Host "URL: http://localhost:$Port/"

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
            $requestPath = "index.html"
        }

        $localPath = [System.IO.Path]::GetFullPath((Join-Path $resolvedBundle $requestPath))

        if (-not $localPath.StartsWith($resolvedBundle, [System.StringComparison]::OrdinalIgnoreCase) -or -not (Test-Path -LiteralPath $localPath -PathType Leaf)) {
            $context.Response.StatusCode = 404
            $context.Response.Close()
            continue
        }

        $bytes = [System.IO.File]::ReadAllBytes($localPath)
        $context.Response.ContentType = Get-ContentType -Path $localPath
        $context.Response.ContentLength64 = $bytes.Length
        $context.Response.OutputStream.Write($bytes, 0, $bytes.Length)
        $context.Response.Close()
    }
}
finally {
    $listener.Stop()
    $listener.Close()
}
