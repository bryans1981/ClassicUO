[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$SourcePath,

    [switch]$AllFiles
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$seedRoot = Join-Path $repoRoot "experiments\BrowserHost\wwwroot\local-uo"

$defaultFiles = @(
    "tiledata.mul",
    "hues.mul",
    "map0.mul",
    "art.mul",
    "artidx.mul",
    "gumpart.mul",
    "gumpidx.mul",
    "cliloc.enu"
)

function Get-RelativePath {
    param(
        [Parameter(Mandatory = $true)]
        [string]$BasePath,

        [Parameter(Mandatory = $true)]
        [string]$TargetPath
    )

    $normalizedBase = [System.IO.Path]::GetFullPath($BasePath)

    if (-not $normalizedBase.EndsWith([System.IO.Path]::DirectorySeparatorChar)) {
        $normalizedBase += [System.IO.Path]::DirectorySeparatorChar
    }

    $baseUri = [System.Uri]::new($normalizedBase)
    $targetUri = [System.Uri]::new([System.IO.Path]::GetFullPath($TargetPath))
    $relativeUri = $baseUri.MakeRelativeUri($targetUri)

    return [System.Uri]::UnescapeDataString($relativeUri.ToString()).Replace('/', [System.IO.Path]::DirectorySeparatorChar)
}

if (-not (Test-Path -LiteralPath $SourcePath)) {
    throw "Source path does not exist: $SourcePath"
}

$resolvedSource = (Resolve-Path -LiteralPath $SourcePath).Path
$sourceItem = Get-Item -LiteralPath $resolvedSource

if (Test-Path -LiteralPath $seedRoot) {
    Remove-Item -LiteralPath $seedRoot -Recurse -Force
}

New-Item -ItemType Directory -Path $seedRoot | Out-Null

$filesToCopy = @()

if ($sourceItem.PSIsContainer) {
    $candidates = Get-ChildItem -LiteralPath $resolvedSource -File -Recurse

    if ($AllFiles) {
        $filesToCopy = $candidates
    } else {
        $lookup = @{}

        foreach ($candidate in $candidates) {
            $name = $candidate.Name.ToLowerInvariant()

            if (-not $lookup.ContainsKey($name)) {
                $lookup[$name] = $candidate
            }
        }

        foreach ($fileName in $defaultFiles) {
            if ($lookup.ContainsKey($fileName)) {
                $filesToCopy += $lookup[$fileName]
            }
        }
    }
} elseif ($sourceItem.Extension -ieq ".mul" -or $sourceItem.Extension -ieq ".uop" -or $sourceItem.Extension -ieq ".idx" -or $sourceItem.Name -ieq "cliloc.enu") {
    $filesToCopy = @($sourceItem)
} else {
    throw "Source path must be a folder of UO assets or a supported asset file."
}

if ($filesToCopy.Count -eq 0) {
    throw "No matching files were found to seed."
}

$entries = @()

foreach ($file in $filesToCopy) {
    if ($sourceItem.PSIsContainer) {
        $relativePath = Get-RelativePath -BasePath $resolvedSource -TargetPath $file.FullName
    } else {
        $relativePath = $file.Name
    }

    $relativePath = $relativePath.Replace('\', '/')
    $targetPath = Join-Path $seedRoot $relativePath
    $targetDir = Split-Path -Parent $targetPath

    if (-not (Test-Path -LiteralPath $targetDir)) {
        New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
    }

    Copy-Item -LiteralPath $file.FullName -Destination $targetPath -Force

    $entries += [pscustomobject]@{
        relativePath = $relativePath
        size = [int64]$file.Length
    }
}

$entries = $entries | Sort-Object relativePath
$totalBytes = ($entries | Measure-Object -Property size -Sum).Sum
if ($null -eq $totalBytes) {
    $totalBytes = 0
}

$manifest = [pscustomobject]@{
    rootPath = "/uo"
    generatedAtUtc = [DateTime]::UtcNow.ToString("o")
    fileCount = $entries.Count
    totalBytes = [int64]$totalBytes
    entries = $entries
}

$manifestPath = Join-Path $seedRoot "manifest.json"
$manifest | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath $manifestPath -Encoding UTF8

Write-Host "Seeded $($entries.Count) file(s) into $seedRoot"
Write-Host "Manifest: $manifestPath"
