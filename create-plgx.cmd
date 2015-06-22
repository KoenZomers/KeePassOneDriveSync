ECHO Creating KeePass plugin package
set PLUGIN_NAME=KeeOneDriveSync
set SourceFolder=KoenZomers.KeePass.OneDriveSync
REM rd /s /q %~dp0src\obj %~dp0src\bin
del %~dp0%PLUGIN_NAME%.plgx
"C:\Program Files (x86)\KeePass Password Safe 2\KeePass.exe" --plgx-create %~dp0%SourceFolder% --plgx-prereq-kp:2.09 --plgx-prereq-net:4.0
ren %~dp0%SourceFolder%.plgx %PLUGIN_NAME%.plgx
REM zip %PLUGIN_NAME%_1_1.zip %PLUGIN_NAME%.plgx INSTALL.txt
ECHO KeePass Plugin package has been created
