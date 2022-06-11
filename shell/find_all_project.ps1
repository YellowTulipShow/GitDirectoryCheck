$ExecutePath = $PWD
Set-Location $PSScriptRoot
Set-Location ..

function PrintLineSplit([string]$path) {
    Write-Host ""
    Write-Host "======================================================================================="
    Write-Host ""
}

# 查找所有项目名单
PrintLineSplit
Write-Host "[Execute] Find All *.csproj Files"
PrintLineSplit
$cpath = "./_configs/fixed/projects.config"
if (!(Test-Path $cpath)) {
    Write-Host "[Config File] Not Existent: $cpath, Create ......";
    New-Item -ItemType File -Force -Path $cpath
}
$list = Get-ChildItem -Recurse -Include *.csproj -Name
if ($list.getType().Name -eq "String" -or $list.Count -eq 1) {
    $list = $list -replace "\\", "/"
    Write-Host "project: $list"
}
else {
    for ($i = 0; $i -lt $list.Count; $i++) {
        $list[$i] = $list[$i] -replace "\\", "/"
        $item = $list[$i];
        Write-Host "project: $item"
    }
}
$list > $cpath
PrintLineSplit

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}
