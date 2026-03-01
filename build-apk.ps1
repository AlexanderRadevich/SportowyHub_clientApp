#!/usr/bin/env pwsh
# Build SportowyHub Android APK

param(
    [ValidateSet("Release", "Debug")]
    [string]$Configuration = "Release",
    [switch]$Install
)

$ErrorActionPreference = "Stop"

$projectPath = "$PSScriptRoot\SportowyHub.App\SportowyHub.csproj"
$framework = "net10.0-android"

Write-Host "Building APK ($Configuration)..." -ForegroundColor Cyan

dotnet publish $projectPath `
    -f $framework `
    -c $Configuration `
    -p:AndroidPackageFormat=apk

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed." -ForegroundColor Red
    exit 1
}

$apkDir = "$PSScriptRoot\SportowyHub.App\bin\$Configuration\$framework\publish"
$apk = Get-ChildItem -Path $apkDir -Filter "*.apk" | Sort-Object LastWriteTime -Descending | Select-Object -First 1

if (-not $apk) {
    Write-Host "APK not found in $apkDir" -ForegroundColor Red
    exit 1
}

Write-Host "`nAPK: $($apk.FullName)" -ForegroundColor Green
Write-Host "Size: $([math]::Round($apk.Length / 1MB, 2)) MB" -ForegroundColor Green

if ($Install) {
    $adb = "${env:ProgramFiles(x86)}\Android\android-sdk\platform-tools\adb.exe"
    if (-not (Test-Path $adb)) {
        Write-Host "adb not found at $adb" -ForegroundColor Red
        exit 1
    }
    Write-Host "`nInstalling on device..." -ForegroundColor Cyan
    & $adb install -r $apk.FullName
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Install failed." -ForegroundColor Red
        exit 1
    }
    Write-Host "Installed." -ForegroundColor Green
}
