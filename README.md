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

- The plugin is written for KeePass v2.34 but should work with any KeePass version in the 2.x range
- It requires to have the Microsoft .NET v4.5 framework installed. This means this plugin will NOT work on Windows XP as the Microsoft .NET Framework v4.5 is not available on that OS. On later operating systems, you may have to install or enable it manually.
- It only supports KeePass databases which are opened via File -> Open -> Open File in KeePass. Databases which you open via File -> Open -> Open URL will not be able to use this plugin and will get a message stating that they're not supported.
- It works with a personal OneDrive and OneDrive for Business on Office 365. It doesn't work with OneDrive for Business on an on premises SharePoint 2013 or SharePoint 2016 farm. Nor does it allow syncing to SharePoint Online (besides OneDrive for Business) at this time.

## Latest Version

Version 1.8.0.0 - December 16, 2016

- If you had two or more KeePass databases open at the same time and you would cause a sync of one database by i.e. pressing ctrl+s while switching to another open database in KeePass before the sync was done, it would try to merge the first database with the other open database. A check has been added to stop the sync in this scenario so two different databases will never become merged. [issue 25](./issues/25)

Version 1.7.3.0 - December 14, 2016

- When being offline, it would show a modal dialog with an error which you would need to dismiss. Now instead it will just display an offline message in the status bar to make it more user friendly.

Version 1.7.2.0 - December 14, 2016

- When using the File -> Open from OneDrive option and getting to saving the KeePass database locally, it will now assume the same file name as it is stored under on OneDrive as the default filename

Version 1.7.1.0 - October 17, 2016

- Fixed an issue when using a proxy with the system default settings and the system default credentials which I introduced in 1.7.0.0. Thanks everyone for reporting this!

Version 1.7.0.0 - October 14, 2016

- The plugin now also supports relative paths. If you store your KeePass database in the same folder as from where you run KeePass or from a folder below the folder from where you run KeePass, it will automatically reference the database by its path relative to the KeePass executable folder. This allows for portable usage when using KeePass on i.e. a thumb drive. If your KeePass database is located in a different folder below the KeePass executable or from a different drive, it will keep referencing it by its full path. Thanks to Devin Jenson for making this request.

[Version History](./VersionHistory.md)

## Todo

On my todo list are still:

1. Support for OneDrive for Business for on premises SharePoint 2013 and SharePoint 2016 farms if there is desire for it. Let me know in case you're interested in that functionality and I'll consider adding it.
2. Support for storing KeePass databases within TeamSites on SharePoint

## Special Thanks

Special thanks to Oleksandr Senyuk for making [KeeSkyDrive](http://sourceforge.net/projects/keeskydrive/) as it has inspired me to create this plugin.

## Feedback

Comments\suggestions\bug reports are welcome!

Koen Zomers
mail@koenzomers.nl
