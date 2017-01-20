@echo off

xcopy /s /y "FlaxEditor\bin" "..\Source\Bin\Cache\bin"
xcopy /s /y "FlaxEngine\bin" "..\Source\Bin\Cache\bin"

echo Done!
