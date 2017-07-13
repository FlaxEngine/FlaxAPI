@echo off

set outputDir=..\Source\Bin\Editor
set outputAssembliesDir=%outputDir%\Assemblies

if exist %outputDir% (
	echo Copy Flax assemblies
	if not exist "%outputAssembliesDir%" (
		mkdir "%outputAssembliesDir%"
	)
	start /MIN xcopy /i /s /y "FlaxEngine\bin" "%outputAssembliesDir%" /exclude:excludedFileList.txt
	start /MIN xcopy /i /s /y "FlaxEditor\bin" "%outputAssembliesDir%" /exclude:excludedFileList.txt
	echo Done!
)
