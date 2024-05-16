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
set EXE_PATH=%~dp0bin\%CONFIGURATION%\%TARGETFRAMEWORK%\KnowledgeClip.exe

:: Escape backslashes for the reg file by replacing each \ with \\
set "EXE_PATH_ESCAPED=%EXE_PATH:\=\\%"

:: Remove any trailing backslash (if any) at the end of the path
if "%EXE_PATH_ESCAPED:~-1%"=="\\" (
    set "EXE_PATH_ESCAPED=%EXE_PATH_ESCAPED:~0,-1%"
)

:: Create the reg file content
(
echo Windows Registry Editor Version 5.00
echo.
echo [HKEY_CLASSES_ROOT\Directory\shell\KnowledgeClip]
echo @="Copy to clipboard"
echo.
echo [HKEY_CLASSES_ROOT\Directory\shell\KnowledgeClip\command]
echo @="\"%EXE_PATH_ESCAPED%" \"%%V\""
) > "%~dp0\KnowledgeClipContextMenu.reg"

endlocal
