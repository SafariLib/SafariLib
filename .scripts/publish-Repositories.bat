@echo off
setlocal
set "apikey=%~1"
set "version=%~2"
dotnet nuget push .\SafariLib.Repositories\bin\Release\SafariLib.Repositories.%version%.nupkg --api-key %apikey% --source https://api.nuget.org/v3/index.json
endlocal