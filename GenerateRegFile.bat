@echo off
setlocal

:: Check if all required parameters are provided
if "%~1"=="" (
    echo Usage: GenerateRegFile.bat Configuration TargetFramework
    exit /b 1
)

if "%~2"=="" (
    echo Usage: GenerateRegFile.bat Configuration TargetFramework
    exit /b 1
)

:: Get parameters
set CONFIGURATION=%~1
set TARGETFRAMEWORK=%~2

:: Get the absolute path to the executable
set EXE_PATH=%~dp0src\bin\%CONFIGURATION%\%TARGETFRAMEWORK%\KnowledgeClip.exe

:: Escape backslashes for the reg file by replacing each \ with \\
set "EXE_PATH_ESCAPED=%EXE_PATH:\=\\%"

:: Create the reg file content
(
echo Windows Registry Editor Version 5.00
echo.
echo [HKEY_CLASSES_ROOT\Directory\shell\KnowledgeClip]
echo @="Copy to clipboard"
echo.
echo [HKEY_CLASSES_ROOT\Directory\shell\KnowledgeClip\command]
echo @="\"%EXE_PATH_ESCAPED%\" \"%%V\""
) > "%~dp0\KnowledgeClipContextMenu.reg"

endlocal
