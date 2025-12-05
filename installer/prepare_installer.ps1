# Copia los archivos necesarios a installer/package desde el bin\Release
param(
    [string]$projectDir = "..\DotNetOCR",
    [string]$configuration = "Release"
)

$bin = Join-Path -Path $projectDir -ChildPath "bin\$configuration"
$dest = Join-Path -Path (Split-Path -Parent $MyInvocation.MyCommand.Path) -ChildPath "package"

if (Test-Path $dest) { Remove-Item -Recurse -Force $dest }
New-Item -ItemType Directory -Path $dest | Out-Null

# Copiar exe, dll, config
Get-ChildItem -Path $bin -Include *.exe,*.dll,*.config,*.pdb -File -Recurse | ForEach-Object {
    $target = Join-Path $dest $_.Name
    Copy-Item -Path $_.FullName -Destination $target
}

# Copiar carpeta tessdata
$tess = Join-Path $bin 'tessdata'
if (Test-Path $tess) {
    Copy-Item -Path $tess -Destination (Join-Path $dest 'tessdata') -Recurse
}

Write-Host "Files copied to $dest"