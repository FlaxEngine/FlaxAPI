@echo off

cd "%~dp0"
call CopyDotNetApi.bat %1
SET ERRORLEVEL = 0
EXIT 0