@ECHO OFF
SET "POWERSHELL_EXE=pwsh.exe"
WHERE pwsh.exe >NUL 2>NUL
IF ERRORLEVEL 1 SET "POWERSHELL_EXE=powershell.exe"

"%POWERSHELL_EXE%" -NoProfile -ExecutionPolicy Bypass -File "%~dp0create-plgx.ps1"
EXIT /B %ERRORLEVEL%
