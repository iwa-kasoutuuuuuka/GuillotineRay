# Guillotine Ray 最終配布用パッケージ作成スクリプト

$version = "1.0.1"
$distDir = "dist"
$binDir = "bin"
$zipName = "GuillotineRay_v$version.zip"

Write-Host "--- Packaging Guillotine Ray v$version ---" -ForegroundColor Cyan

# 1. ビルド
Write-Host "Building project..."
dotnet build src/GuillotineRay.csproj -o $binDir -c Release

# 2. 出力ディレクトリ準備
if (Test-Path $distDir) { Remove-Item -Recurse -Force $distDir }
New-Item -ItemType Directory -Path $distDir

# 3. 必要なファイルをコピー
Write-Host "Copying files..."
Copy-Item "$binDir\*" "$distDir\" -Exclude "*.pdb", "*.dll.config"
Copy-Item "LICENSE" "$distDir\"
Copy-Item "docs\manual_ja.md" "$distDir\"
Copy-Item "templates" "$distDir\" -Recurse

# 4. ZIP圧縮
Write-Host "Creating ZIP: $zipName"
if (Test-Path $zipName) { Remove-Item $zipName }
Compress-Archive -Path "$distDir\*" -DestinationPath $zipName -Force

Write-Host "--- Done! Created $zipName ---" -ForegroundColor Green
