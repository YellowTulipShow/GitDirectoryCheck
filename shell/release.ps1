param([string]$runMethod)
if ($runMethod -ne "toolScript") {
    $ExecutePath = $PWD
    Set-Location $PSScriptRoot
    Set-Location ..

    . ./shell/global_tools.ps1
}

. ./shell/release_command.ps1 "toolScrtp"

if ($runMethod -ne "toolScript") {
    Set-Location $ExecutePath
    if ($PSScriptRoot -eq $ExecutePath) {
        timeout.exe /T -1
    }
}
