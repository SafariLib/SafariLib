@echo off
setlocal
set "apikey=%~1"
set "version=%~2"
dotnet nuget push .\SafariLib.Core\bin\Release\SafariLib.Core.%version%.nupkg --api-key %apikey% --source https://api.nuget.org/v3/index.json
endlocal