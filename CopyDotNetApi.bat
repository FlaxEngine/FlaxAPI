@echo off

set outputDir="..\Source\Bin\Editor\Assemblies"

if exist %outputDir% (
	echo Copy Flax assemblies
	start /MIN xcopy /s /y "FlaxEditor\bin" %outputDir%
	start /MIN xcopy /s /y "FlaxEngine\bin" %outputDir%
	echo Done!
)

