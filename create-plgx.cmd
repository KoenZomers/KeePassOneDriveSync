ECHO Creating KeePass plugin package
set PLUGIN_NAME=KeeOneDriveSync
set SourceFolder=KoenZomers.KeePass.OneDriveSync
rd /s /q %~dp0%SourceFolder%\obj %~dp0%SourceFolder%\bin
del %~dp0%PLUGIN_NAME%.plgx
"C:\Program Files (x86)\KeePass Password Safe 2\KeePass.exe" --plgx-create %~dp0%SourceFolder% --plgx-prereq-net:4.0
ren %~dp0%SourceFolder%.plgx %PLUGIN_NAME%.plgx
ECHO KeePass Plugin package has been created
