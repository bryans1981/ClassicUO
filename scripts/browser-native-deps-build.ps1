param(
    [switch]$Force
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$nativeRoot = Join-Path $repoRoot "bin\native-browser"
$buildPath = Join-Path $nativeRoot "FNA3D-sdl2"
$cachePath = Join-Path $env:LOCALAPPDATA "ClassicUO\emscripten-cache"
$emConfigPath = Join-Path $nativeRoot ".emscripten"
$fna3dArchive = Join-Path $buildPath "libFNA3D.a"
$mojoshaderArchive = Join-Path $buildPath "libmojoshader.a"

if (-not $Force -and (Test-Path $fna3dArchive) -and (Test-Path $mojoshaderArchive)) {
    Write-Host "Browser native dependencies already built: $buildPath"
    exit 0
}

$emscriptenSdkRoot = "C:\Program Files\dotnet\packs\Microsoft.NET.Runtime.Emscripten.3.1.56.Sdk.win-x64\10.0.7\tools"
$emscriptenRoot = Join-Path $emscriptenSdkRoot "emscripten"
$llvmRoot = Join-Path $emscriptenSdkRoot "bin"
$binaryenRoot = $emscriptenSdkRoot
$nodePath = "C:\Program Files\dotnet\packs\Microsoft.NET.Runtime.Emscripten.3.1.56.Node.win-x64\10.0.7\tools\bin\node.exe"
$pythonRoot = "C:\Program Files\dotnet\packs\Microsoft.NET.Runtime.Emscripten.3.1.56.Python.win-x64\10.0.7\tools"
$pythonPath = Join-Path $pythonRoot "python.exe"
$cmakePath = "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\CommonExtensions\Microsoft\CMake\CMake\bin\cmake.exe"
$ninjaPath = "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\CommonExtensions\Microsoft\CMake\Ninja\ninja.exe"
$emcmakePath = Join-Path $emscriptenRoot "emcmake.bat"

$requiredPaths = @($emscriptenRoot, $llvmRoot, $nodePath, $pythonPath, $cmakePath, $ninjaPath, $emcmakePath)
foreach ($path in $requiredPaths) {
    if (-not (Test-Path $path)) {
        throw "Required browser native build tool was not found: $path"
    }
}

New-Item -ItemType Directory -Force -Path $buildPath | Out-Null
New-Item -ItemType Directory -Force -Path $cachePath | Out-Null

@"
import os
LLVM_ROOT = os.path.expanduser(os.getenv('DOTNET_EMSCRIPTEN_LLVM_ROOT', ''))
NODE_JS = os.path.expanduser(os.getenv('DOTNET_EMSCRIPTEN_NODE_JS', ''))
BINARYEN_ROOT = os.path.expanduser(os.getenv('DOTNET_EMSCRIPTEN_BINARYEN_ROOT', ''))
CACHE = os.path.expanduser(os.getenv('EM_CACHE', ''))
FROZEN_CACHE = False
COMPILER_ENGINE = NODE_JS
JS_ENGINES = [NODE_JS]
"@ | Set-Content -LiteralPath $emConfigPath -Encoding ASCII

$env:EM_CONFIG = $emConfigPath
$env:DOTNET_EMSCRIPTEN_LLVM_ROOT = $llvmRoot
$env:DOTNET_EMSCRIPTEN_BINARYEN_ROOT = $binaryenRoot
$env:DOTNET_EMSCRIPTEN_NODE_JS = $nodePath
$env:EMSDK_PYTHON = $pythonPath
$env:PYTHONPATH = $pythonRoot
$env:PYTHONHOME = ""
$env:EM_CACHE = $cachePath
$env:PATH = (Split-Path -Parent $ninjaPath) + ";" + $env:PATH

Push-Location $buildPath
try {
    & $emcmakePath $cmakePath (Join-Path $repoRoot "external\FNA\lib\FNA3D") `
        -G "Ninja" `
        -DCMAKE_MAKE_PROGRAM="$ninjaPath" `
        -DBUILD_SHARED_LIBS=OFF `
        -DBUILD_SDL3=OFF `
        -DCMAKE_C_FLAGS="-sUSE_SDL=2 -sMAX_WEBGL_VERSION=2 -sFULL_ES3=1" `
        -DSDL2_INCLUDE_DIRS="$emscriptenRoot\cache\sysroot\include\SDL2" `
        -DSDL2_LIBRARIES="-sUSE_SDL=2 -sMAX_WEBGL_VERSION=2 -sFULL_ES3=1"

    if ($LASTEXITCODE -ne 0) {
        throw "FNA3D browser native CMake configure failed with exit code $LASTEXITCODE"
    }
}
finally {
    Pop-Location
}

& $ninjaPath -C $buildPath FNA3D
if ($LASTEXITCODE -ne 0) {
    throw "FNA3D browser native build failed with exit code $LASTEXITCODE"
}

if (-not (Test-Path $fna3dArchive) -or -not (Test-Path $mojoshaderArchive)) {
    throw "Browser native dependency build completed but expected archives were not produced at $buildPath"
}

Write-Host "Browser native dependencies built: $buildPath"
