# KeePass OneDrive Sync

I've created a free plugin for KeePass that allows syncing of multiple password databases from multiple OneDrives to a local version. It allows you to synchronize an unlimited amount of KeePass databases with an unlimited amount of OneDrives. So i.e. you can synchronize your personal KeePass database with your personal OneDrive and your work related KeePass with a OneDrive for Business on Office 365 that is shared among your colleagues, if you wish. This way it's also easy to be able to access your same KeePass database from all of your Windows devices.

## Download ##
You need to download either the DLLs *OR* the PLGX and place it inside your KeePass\Plugins folder (typically C:\Program Files (x86)\KeePass Password Safe 2\Plugins), so not both. I've switched to using the GitHub releases functionality.

[You can find the latest stable version here](../../releases/latest)

## Documentation ##
- [Installation instructions](./Installaton%20Instructions.md)
- [Configuration options](./Configuration.md)
- [Checking for updates](./UpdateCheck.md)
- [Troubleshooting](./Troubleshooting.md)
- [Frequently Asked Questions](./Faq.md)

## System Requirements

- The plugin is written for KeePass v2.36 but should work with any KeePass version in the 2.x range
- It requires to have the Microsoft .NET v4.5.2 framework installed. This means this plugin will NOT work on Windows XP as the Microsoft .NET Framework v4.5.2 is not available on that OS. On later operating systems, you may have to install or enable it manually.
- It only supports KeePass databases which are opened via File -> Open -> Open File in KeePass. Databases which you open via File -> Open -> Open URL will not be able to use this plugin and will get a message stating that they're not supported.
- It works with a personal OneDrive and OneDrive for Business on Office 365. It doesn't work with OneDrive for Business on an on premises SharePoint 2013 or SharePoint 2016 farm. Nor does it allow syncing to SharePoint Online (besides OneDrive for Business) at this time.

## Latest Version

Version 2.0.0.0 - August 25, 2017

- Replaced DLLs in the solution with NuGet package references. This does increase the PLGX file size, but does ease keeping this plugin updated with the latest versions for developers
- Upgraded KoenZomers.OneDrive.Api to v2.0.1.0 which has support for the Microsoft Graph API
- Plugin is now compiled against the Microsoft .NET Framework v4.5.2 as v4.5 is out of support
- Added the option to use the Microsoft Graph API to store the KeePass database on OneDrive or OneDrive for Business. The API will automatically define if it's the Consumer OneDrive or OneDrive for Business based on the login you use. Using the Microsoft Graph API option is now the recommended option.
- Made preparations to support on-premises SharePoint 2013 and 2016 farms. This functionality will become available in a future version.

Version 1.8.3.0 - August 18, 2017

- Fixed an issue where having unsaved changes to your KeePass database and then exiting KeePass would not get the changes uploaded to OneDrive/OneDrive for Business before KeePass would exit. It will now wait with closing KeePass until the changes are saved. Thanks to Darko Jamnik for reporting this issue.

Version 1.8.2.0 - May 9, 2017

- Improved the OneDrive platform selection dialog to aid visually impaired. Thanks to Oire for reporting it [issue 28](../../issues/28)

Version 1.8.1.0 - December 24, 2016

- Bugfix of a bug introduced in 1.8.0.0 where having a KeePass database stored in the same folder or in a folder under the folder where the KeePass executable resides gave the error "Failed to sync. Please don't switch to another database before done." when trying to save the changes of the KeePass database. Thanks to DazzaQLD for reporting it [issue 26](../../issues/26)

Version 1.8.0.0 - December 16, 2016

- If you had two or more KeePass databases open at the same time and you would cause a sync of one database by i.e. pressing ctrl+s while switching to another open database in KeePass before the sync was done, it would try to merge the first database with the other open database. A check has been added to stop the sync in this scenario so two different databases will never become merged. [issue 25](../../issues/25)

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
koen@zomers.eu
