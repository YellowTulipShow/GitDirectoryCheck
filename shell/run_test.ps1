$ExecutePath = $PWD
Set-Location $PSScriptRoot
Set-Location ..

dotnet run --project ./src/GitCheckCommand/GitCheckCommand.csproj

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}
