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
- [Frequently Asked Questions](./Faq.md)

## System Requirements

- The plugin is written for KeePass v2.31 but should work with any KeePass version in the 2.x range
- It requires to have the Microsoft .NET v4.5 framework installed. This means this plugin will NOT work on Windows XP as the Microsoft .NET Framework v4.5 is not available on that OS. On later operating systems, you may have to install or enable it manually.
- It only supports KeePass databases which are opened via File -> Open -> Open File in KeePass. Databases which you open via File -> Open -> Open URL will not be able to use this plugin and will get a message stating that they're not supported.
- It works with a personal OneDrive and OneDrive for Business on Office 365. It doesn't work with OneDrive for Business on an on premises SharePoint 2013 or SharePoint 2016 farm.

## Latest Version

Version 1.5.0.0 - August 13, 2016

- Introduced an easy to use OneDrive file picker dialog to select the location where you want to store your KeePass database. The #1 issue people contacted me about was that they misunderstood the previous single open text dialog. Hopefully that's fixed with introducing this dialog. Check out the extra options under the right click in the OneDrive location picker dialog!
- Introduced the ability to store the KeePass database in a OneDrive folder that is located on another OneDrive and has been shared with you. Check out the [FAQ](./Faq.md) for instructions on how to do this.

[Version History](./VersionHistory.md)

## Todo

On my todo list are still:

1. Allow a KeePass database to be opened directly from OneDrive without the need to download it yourself first
2. Support for OneDrive for Business for on premises SharePoint 2013 and SharePoint 2016 farms if there is desire for it. Let me know in case you're interested in that functionality and I'll consider adding it.
3. Support for storing KeePass databases within TeamSites on SharePoint

## Special Thanks

Special thanks to Oleksandr Senyuk for making [KeeSkyDrive](http://sourceforge.net/projects/keeskydrive/) as it has inspired me to create this plugin.

## Feedback

Comments\suggestions\bug reports are welcome!

Koen Zomers
mail@koenzomers.nl
