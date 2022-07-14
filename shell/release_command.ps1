param([string]$runMethod)
if ($runMethod -ne "toolScript") {
    $ExecutePath = $PWD
    Set-Location $PSScriptRoot
    Set-Location ..

    . ./shell/global_tools.ps1
}


# 打包命令项目
PrintLineSplit
Write-Host "[Release Command]:";
$cpath = ConfigFileHandle("./_configs/command.config")
if ($cpath -and (Test-Path $cpath)) {
    $save_path = "./_release/command_packages"
    if ($save_path -and (Test-Path $save_path)) {
        Remove-Item -Recurse -Force $save_path
    }

    $list = Get-Content $cpath
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

        # 打包项目
        Write-Output "dotnet build $item"
        dotnet build $item
        Write-Output "dotnet pack $item --output '$save_path'"
        dotnet pack $item --output "$save_path"
    }
}

if ($runMethod -ne "toolScript") {
    Set-Location $ExecutePath
    if ($PSScriptRoot -eq $ExecutePath) {
        timeout.exe /T -1
    }
}
