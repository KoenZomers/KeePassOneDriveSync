# KeePass OneDrive Sync

I've created a free plugin for KeePass that allows syncing of multiple password databases from multiple OneDrives to a local version. It allows you to synchronize an unlimited amount of KeePass databases with an unlimited amount of OneDrives. So i.e. you can synchronize your personal KeePass database with your personal OneDrive and your work related KeePass with a OneDrive for Business on Office 365 that is shared among your colleagues, if you wish. This way it's also easy to be able to access your same KeePass database from all of your Windows devices.

## Download ##
You need to download either the DLLs *OR* the PLGX and place it inside your KeePass folder, so not both:
- [Direct download PLGX](https://github.com/KoenZomers/KeePassOneDriveSync/raw/master/KeeOneDriveSync.plgx)
- OR [Direct download DLLs](https://github.com/KoenZomers/KeePassOneDriveSync/raw/master/KeeOneDriveSync.zip)

## Documentation ##
- [Installation instructions](./Installaton%20Instructions.md)
- [Configuration options](./Configuration.md)
- [Checking for updates](./UpdateCheck.md)
- [Troubleshooting](./Troubleshooting.md)

## System Requirements

- The plugin is written for KeePass v2.31 but should work with any KeePass version in the 2.x range
- It requires to have the Microsoft .NET v4.5 framework installed. This means this plugin will NOT work on Windows XP as the Microsoft .NET Framework v4.5 is not available on that OS. On later operating systems, you may have to install or enable it manually.
- It only supports KeePass databases which are opened via File -> Open -> Open File in KeePass. Databases which you open via File -> Open -> Open URL will not be able to use this plugin and will get a message stating that they're not supported.
- It works with a personal OneDrive and OneDrive for Business on Office 365. It doesn't work with OneDrive for Business on an on premises SharePoint 2013 or SharePoint 2016 farm.

## Latest Version

Version 1.4.2.0 - March 31, 2016

- Fixed the [issue](https://github.com/KoenZomers/KeePassOneDriveSync/issues/13) where not synced databases kept asking if they should be synced after restarting KeePass. Thanks to [mushak](https://github.com/mushak) for reporting this!

[Version History](./VersionHistory.md)

## Todo

On my todo list are still:

1. Add a OneDrive File browser for easier selection of a location on OneDrive 
2. Allow a KeePass database to be opened directly from OneDrive without the need to download it yourself first
3. Proxy issues are extremely difficult for me to troubleshoot as I don't have access to a proxy which requires authentication myself. If you can help me with test access to one, let me know.
4. Support for OneDrive for Business for on premises SharePoint 2013 and SharePoint 2016 farms if there is desire for it. Let me know in case you're interested in that functionality and I'll consider adding it.

## Special Thanks

Special thanks to Oleksandr Senyuk for making [KeeSkyDrive](http://sourceforge.net/projects/keeskydrive/) as it has inspired me to create this plugin.

## Feedback

Comments\suggestions\bug reports are welcome!

Koen Zomers
mail@koenzomers.nl
