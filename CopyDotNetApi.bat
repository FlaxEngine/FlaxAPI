REM Press Build->Build Solution (F6) or Build->Build FlaxEngine (Shift+F6) to invoke this script automatically form Visual Studio Enviroment
@echo off

set outputDir=..\Source\Bin\Editor
set outputAssembliesDir=%outputDir%\Assemblies

if exist %outputDir% (
	echo Copy Flax assemblies
	if not exist "%outputAssembliesDir%" (
		mkdir "%outputAssembliesDir%"
	)
	xcopy /i /s /y "FlaxEngine\bin" "%outputAssembliesDir%" /exclude:excludedFileList.txt
	xcopy /i /s /y "FlaxEditor\bin" "%outputAssembliesDir%" /exclude:excludedFileList.txt
	echo Done!
)
REM Remove "#REM" from lines below to enable automatic flax update with your custom DLLs, and change name of your current project
REM taskkill /f /im FlaxEditor.exe /t
REM "%outputDir%\..\Win64\FlaxEditor.exe" -project "%userprofile%\Documents\Flax Projects\MyProject"