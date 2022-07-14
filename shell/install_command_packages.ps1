param([string]$runMethod)
if ($runMethod -ne "toolScript") {
    $ExecutePath = $PWD
    Set-Location $PSScriptRoot
    Set-Location ..

    . ./shell/global_tools.ps1
}

# 查找所有项目名单
PrintLineSplit
Write-Host "[Execute] Install CommandPackages:"

$cpath = "./_release/command_packages"
if (!(Test-Path $cpath)) {
    Write-Host "[Path Null] Not Existent: $cpath !!!";
    PrintLineSplit
    return
}
Set-Location $cpath
$list = Get-ChildItem -Recurse -Include *.nupkg -Name
for ($i = 0; $i -lt $list.Count; $i++) {
    if ($list.Count -eq 1) {
        $item = $list;
    }
    else {
        $item = $list[$i]
    }
    if (!$item) {
        continue;
    }
    PrintLineSplit
    $item = $item -replace "(\.\d)+\.nupkg$"

    # 安装打包命令项目
    Write-Host "dotnet tool uninstall -g $item"
    dotnet tool uninstall -g $item
    Write-Host "dotnet tool install -g --add-source ./ $item"
    dotnet tool install -g --add-source ./ $item
}

PrintLineSplit

if ($runMethod -ne "toolScript") {
    Set-Location $ExecutePath
    if ($PSScriptRoot -eq $ExecutePath) {
        timeout.exe /T -1
    }
}
