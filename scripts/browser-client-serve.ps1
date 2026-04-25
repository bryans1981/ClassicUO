param(
    [string]$BundlePath,
    [int]$Port = 5110
)

$ErrorActionPreference = "Stop"

if (-not $BundlePath) {
    throw "BundlePath is required."
}

$resolvedBundle = (Resolve-Path -LiteralPath $BundlePath).Path
$logPath = Join-Path $env:TEMP 'classicuo-browser-serve.log'
$null = Remove-Item -LiteralPath $logPath -Force -ErrorAction SilentlyContinue
$listener = [System.Net.HttpListener]::new()
$listener.Prefixes.Add("http://localhost:$Port/")
$listener.Start()
Add-Content -LiteralPath $logPath -Value "SERVER STARTED $resolvedBundle"

function Add-Content {
    param(
        [Parameter(Mandatory = $true)]
        [string]$LiteralPath,

        [Parameter(Mandatory = $true)]
        [string]$Value
    )

    try {
        $directory = Split-Path -Parent $LiteralPath
        if ($directory -and -not (Test-Path -LiteralPath $directory)) {
            New-Item -ItemType Directory -Force -Path $directory | Out-Null
        }

        $bytes = [System.Text.Encoding]::UTF8.GetBytes($Value + [Environment]::NewLine)
        $stream = [System.IO.File]::Open($LiteralPath, [System.IO.FileMode]::Append, [System.IO.FileAccess]::Write, [System.IO.FileShare]::ReadWrite)
        try {
            $stream.Write($bytes, 0, $bytes.Length)
        } finally {
            $stream.Dispose()
        }
    } catch {
        # Logging must never take the server down.
    }
}

Write-Host "Serving ClassicUO browser bundle from $resolvedBundle"
Write-Host "URL: http://localhost:$Port/"
if ($buildPrefix) {
    Write-Host "Build prefix: /$buildPrefix"
}

function Get-BuildPrefix {
    param([string]$BundleRoot)

    $buildStampFile = Join-Path $BundleRoot 'browser-build-stamp.txt'
    if (-not (Test-Path -LiteralPath $buildStampFile)) {
        return ''
    }

    $buildStamp = (Get-Content -LiteralPath $buildStampFile -ErrorAction SilentlyContinue | Select-Object -First 1).Trim()
    if ([string]::IsNullOrWhiteSpace($buildStamp)) {
        return ''
    }

    return "build/$buildStamp/"
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

$buildPrefix = Get-BuildPrefix -BundleRoot $resolvedBundle

try {
    while ($listener.IsListening) {
        $context = $listener.GetContext()
        $requestPath = [Uri]::UnescapeDataString($context.Request.Url.AbsolutePath.TrimStart("/"))
        $buildPrefix = Get-BuildPrefix -BundleRoot $resolvedBundle
        if ($context.Request.HttpMethod -eq 'POST' -and $requestPath.EndsWith('__browser-status', [System.StringComparison]::OrdinalIgnoreCase)) {
            $reader = New-Object System.IO.StreamReader($context.Request.InputStream, $context.Request.ContentEncoding)
            $body = $reader.ReadToEnd()
            $reader.Dispose()
            Add-Content -LiteralPath $logPath -Value ("STATUS {0}" -f $body)
            $context.Response.StatusCode = 204
            $context.Response.Close()
            continue
        }

        Add-Content -LiteralPath $logPath -Value ("REQ {0}" -f $requestPath)

        if ([string]::IsNullOrWhiteSpace($requestPath)) {
            if ($buildPrefix) {
                $context.Response.StatusCode = 302
                $context.Response.RedirectLocation = "/$buildPrefix" + "index.html"
                $context.Response.Close()
                continue
            }

            $requestPath = "index.html"
        }

        if ($requestPath -match '^build/[^/]+/?$') {
            if ($buildPrefix) {
                $requestPath = "$buildPrefixindex.html"
            } else {
                $requestPath = "index.html"
            }
        }
        elseif ($requestPath -match '^build/[^/]+/(.+)$') {
            if ($buildPrefix) {
                $requestPath = "$buildPrefix$($Matches[1])"
            } else {
                $requestPath = $Matches[1]
            }
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
catch {
    Add-Content -LiteralPath $logPath -Value ("SERVER EXCEPTION {0}" -f $_.Exception.ToString())
    throw
}
finally {
    Add-Content -LiteralPath $logPath -Value "SERVER STOPPED"
    $listener.Stop()
    $listener.Close()
}
