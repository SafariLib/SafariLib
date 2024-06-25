@echo off
setlocal
set "apikey=%~1"
set "version=%~2"
dotnet nuget push .\SafariLib.Jwt\bin\Release\SafariLib.Jwt.%version%.nupkg --api-key %apikey% --source https://api.nuget.org/v3/index.json
endlocal