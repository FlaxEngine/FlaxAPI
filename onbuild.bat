@echo off

cd "%~dp0"
call CopyDotNetApi.bat
SET ERRORLEVEL = 0
EXIT 0