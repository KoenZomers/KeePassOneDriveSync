# KeePass OneDrive Sync

I've created a free plugin for KeePass that allows syncing of multiple password databases from multiple OneDrives to a local version. It allows you to synchronize an unlimited amount of KeePass databases with an unlimited amount of OneDrives. So i.e. you can synchronize your personal KeePass database with your personal OneDrive and your work related KeePass with a OneDrive that is shared among your colleagues, if you wish. This way it's also easy to be able to access your same KeePass database from all of your Windows devices.

- [Direct download](https://github.com/KoenZomers/KeePassOneDriveSync/raw/master/KeeOneDriveSync.plgx)
- [Installation instructions](./Installaton%20Instructions.md)
- [Configuration options](./Configuration.md)
- [Checking for updates](./UpdateCheck.md)

## System Requirements

- The plugin is written for KeePass v2.29 but should work with any KeePass version in the 2.x range
- It requires to have the Microsoft .NET v4.5 framework installed. This means this plugin will NOT work on Windows XP as the Microsoft .NET Framework v4.5 is not available on that OS. On later operating systems, you may have to install or enable it manually.

## Latest Version

Version 1.3.1.0 - July 17, 2015

- Updated the link in the OneDrive Token storage selection dialog to link to the help page regarding this topic

[Version History](./VersionHistory.md)

## Todo

On my todo list are still:

1. Add a OneDrive File browser for easier selection of a location on OneDrive 
2. Allow a KeePass database to be opened directly from OneDrive without the need to download it yourself first
3. Update help link to the OneDrive token storage to https://github.com/KoenZomers/KeePassOneDriveSync/blob/master/OneDriveRefreshToken.md

## Special Thanks

Special thanks to Oleksandr Senyuk for making [KeeSkyDrive](http://sourceforge.net/projects/keeskydrive/) as it has inspired me to create this plugin.

## Feedback

Comments\suggestions\bug reports are welcome!

Koen Zomers
mail@koenzomers.nl
