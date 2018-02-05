@echo off

set /a failed=0

call :ping google 
call :ping yahoo 
call :ping facebook 
call :ping instagram
rem microsoft does not accept icmp requests
rem call :ping microsoft

echo.

echo %failed% host(s) failed.
goto :eof

:ping
echo Pinging %1
ping -n 1 "%1.com" > NUL
if errorlevel 1 (set /a failed=1+failed)
