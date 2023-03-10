if [%1] == [] goto end

if [%2] == [] goto end

if [%3] == [] goto end

if [%4] == [] goto end

set build=%1
set web=%~1\home\cadmin\circulo\web
set runtime=%2
set git=%3
set user=%4

del /S /Q "%build%"

dotnet publish api/API.csproj -r %runtime% -c Debug --framework net7.0 --output "%web%\api" --no-self-contained

echo f | xcopy "%git%\scripts\init.sql" "%build%\home\cadmin\circulo\scripts\init.sql"
echo f | xcopy "%git%\cert\server-cert.pem" "%build%\home\cadmin\circulo\web\client\server-cert.pem"
echo f | xcopy "%git%\cert\server-key.pem" "%build%\home\cadmin\circulo\web\client\server-key.pem"
echo f | xcopy "%git%\nginx.conf" "%build%\home\cadmin\circulo\nginx.conf"

cd "%git%\client" && echo d | xcopy /Y /F /E "%git%\client" "%web%\client" /EXCLUDE:%git%\exclude.txt && cd %git%

:end