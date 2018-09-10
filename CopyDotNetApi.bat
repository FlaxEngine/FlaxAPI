REM Press Build->Build Solution (F6) or Build->Build FlaxEngine (Shift+F6) to invoke this script automatically form Visual Studio Enviroment
@echo off

set outputDir=..\Source\Editor
set outputAssembliesDir=%outputDir%\Assemblies

if exist %outputDir% (
	echo Copy Flax assemblies
	if not exist "%outputAssembliesDir%" (
		mkdir "%outputAssembliesDir%"
	)
	
	if [%~1]==[] (
		xcopy /i /s /y "FlaxEngine\bin" "%outputAssembliesDir%" /exclude:excludedFileList.txt
		xcopy /i /s /y "FlaxEditor\bin" "%outputAssembliesDir%" /exclude:excludedFileList.txt
	)
	if [%~1]==[-Engine] (
		xcopy /i /s /y "FlaxEngine\bin" "%outputAssembliesDir%" /exclude:excludedFileList.txt
	)
	if [%~1]==[-Editor] (
		xcopy /i /s /y "FlaxEditor\bin" "%outputAssembliesDir%" /exclude:excludedFileList.txt
	)
	REM TODO: Warn the user if he passed an invalid argument
	echo Done!
)

REM Remove "REM" from lines below to enable automatic flax update with your custom DLLs, and change name of your current project
rem if [%~1]==[-Editor] (
rem start /wait taskkill /f /im FlaxEditor.exe /t
rem rem start "Flax Editor - Development Mode" "%outputDir%\..\Win64\FlaxEditor.exe" -project "%userprofile%\Documents\Flax Projects\MyProject"

rem powershell start-process "%outputDir%\..\Win64\FlaxEditor.exe" -ArgumentList '-project """%userprofile%\Documents\Flax Projects\MyProject"""'
rem )