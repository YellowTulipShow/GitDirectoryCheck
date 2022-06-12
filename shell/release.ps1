$ExecutePath = $PWD
Set-Location $PSScriptRoot
Set-Location ..

function PrintLineSplit([string]$path) {
    Write-Host ""
    Write-Host "======================================================================================="
    Write-Host ""
}
function ConfigFileHandle([string]$path) {
    $path = $path -replace "\\", "/"
    $fixed_path = $path -replace "_configs/", "_configs/fixed/"
    if (!(Test-Path $fixed_path)) {
        Write-Host "[Fixed Config File] Not Existent: $fixed_path";
        return ""
    }
    elseif (!(Test-Path $path)) {
        Write-Host "[Custom Config File] Not Existent: $fixed_path, Create ......";
        New-Item -ItemType File -Force -Path $path
        $content = Get-Content $fixed_path
        $content > $path
    }
    else {
        return $path;
    }
}

# 系统发布名单配置定义
PrintLineSplit
$cpath = ConfigFileHandle("./_configs/systems.config")
if ($cpath -and (Test-Path $cpath)) {
    $systems = Get-Content $cpath
    Write-Host "[Get Release Signs]: $systems";
}
else {
    Write-Host "[Systems Sign Config File] Not Existent: $cpath";
    PrintLineSplit
    return;
}

# 发布应用程序
PrintLineSplit
Write-Host "[Release Apps]:";
$cpath = ConfigFileHandle("./_configs/app.config")
if ($cpath -and (Test-Path $cpath)) {
    $list = Get-Content $cpath
    for ($i = 0; $i -lt $list.Count; $i++) {
        if ($list.Count -eq 1) {
            $item = $list;
        } else {
            $item = $list[$i]
        }
        if (!$item) {
            continue;
        }
        $name = $item -replace '^.+[/\\]'
        $name = $name -replace '\.csproj$'
        for ($j = 0; $j -lt $systems.Count; $j++) {
            if ($systems.Count -eq 1) {
                $sys = $systems;
            } else {
                $sys = $systems[$j];
            }

            PrintLineSplit

            # 发布项目
            Write-Host "dotnet publish $item -o './_release/$sys/$name' --runtime '$sys'"
            dotnet publish $item -o "./_release/$sys/$name" --runtime "$sys"
        }
    }
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
        } else {
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

PrintLineSplit

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}
