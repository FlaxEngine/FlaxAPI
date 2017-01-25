@echo off

xcopy /s /y "FlaxEditor\bin" "..\Source\Bin\Editor\Assemblies"
xcopy /s /y "FlaxEngine\bin" "..\Source\Bin\Editor\Assemblies"

echo Done!
