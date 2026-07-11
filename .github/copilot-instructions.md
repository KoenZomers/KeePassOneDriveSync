# Copilot Instructions

## Project Guidelines
- In this KeePass plugin project (KoenZomersKeePassOneDriveSync), debugging (F5) starts KeePass.exe as an external program (configured in .csproj.user via StartAction=Program/StartProgram). The plugin is loaded from C:\Program Files\KeePass Password Safe 2\Plugins, not directly from bin\Debug. The post-build event copies the built DLL/PDB there; this requires write permissions on that folder (set up once via icacls), and KeePass must not be running during the build (otherwise the DLL is locked).