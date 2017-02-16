@echo off

start /MIN xcopy /s /y "FlaxEditor\bin" "..\Source\Bin\Editor\Assemblies"
start /MIN xcopy /s /y "FlaxEngine\bin" "..\Source\Bin\Editor\Assemblies"

echo Done!
