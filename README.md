# KeePass OneDrive Sync

I've created a free plugin for KeePass that allows syncing of multiple password databases from multiple OneDrives to a local version. It allows you to synchronize an unlimited amount of KeePass databases with an unlimited amount of OneDrives. So i.e. you can synchronize your personal KeePass database with your personal OneDrive and your work related KeePass with a OneDrive for Business on Office 365 that is shared among your colleagues, if you wish. This way it's also easy to be able to access your same KeePass database from all of your Windows devices.

## Download ##
You need to download either the DLLs *OR* the PLGX and place it inside your KeePass\Plugins folder (typically C:\Program Files (x86)\KeePass Password Safe 2\Plugins), so not both. I've switched to using the GitHub releases functionality.

[You can find the latest stable version here](../../releases/latest)

## Documentation ##
- [System requirements](./SystemRequirements.md)
- [Installation instructions](./Installaton%20Instructions.md)
- [Configuration options](./Configuration.md)
- [Checking for updates](./UpdateCheck.md)
- [Troubleshooting](./Troubleshooting.md)
- [Frequently Asked Questions](./Faq.md)

## Latest Version

** NOTICE **
I've received several messages of people upgrading to 2.0.1.1 reporting that OneDrive stopped syncing for them after upgrading. I'm trying to fix this in the code. In the meantime you can easily fix this yourself by deleting your existing KeePass sync config through Tools > OneRiveSync Options and hitting CTRL+S again to set up your sync again. After this it should work again. If not, let me know.

Version 2.0.1.2 - February 5, 2018

- Generated a new client secret for the OneDrive for Business API as it expired at February 2, 2018 and caused all syncs using this provider to stop working. The new client secret has and endless lifetime so it should not occur again. [Issue #51](../../issues/51)

Version 2.0.1.1 - January 12, 2018

- Upgraded KoenZomers.OneDrive.Api to v2.1.0.1 in which several issues with uploading to shared drives have been fixed
- Bugfixes in uploading to shared drives
- Syncing with OneDrive, especially if the KeePass database resides in a folder on OneDrive with many files, should be noticeably faster now
- Fixed an issue where it would always resync instead of properly detecting that there were no changes since last sync

Version 2.0.1.0 - January 11, 2018

- Upgraded KoenZomers.OneDrive.Api to v2.1.0.0 which has support for using items shared from other OneDrive drives
- Added a tab "Shared with me" to the OneDrive location picker dialog which allows you to open a KeePass database from or save it to a location on another OneDrive Personal or OneDrive for Business site which has been shared with you. Please note that due to a bug in the OneDrive for Business v2 File API it only works with OneDrive for Business sites if you use the Microsoft Graph API which is recommended anyway. This feature is not available if you use the OneDrive for Business option in the storage type selection box. [Request #43](../../issues/43)

Version 2.0.0.1 - January 2, 2018

- Bugfix in the About dialog not working. Thanks to everyone that reported this!
- Added clickable link in the configuration details screen to open Windows Explorer directly to where the database resides

Version 2.0.0.0 - December 31, 2017

To celebrate the start of 2018, I've spent countless hours on adding some highly requested features in this release. Happy New Year!

Note: this is a mayor release with many changes to the code. I've tested everything carefully, but if you nevertheless run into problems, please [revert to the previous version](../../releases/tag/1.8.3.0) and [share with me the](../../issues/new) the issue you are facing.

- Replaced DLLs in the solution with NuGet package references. This does increase the PLGX file size, but does ease keeping this plugin updated with the latest versions for developers
- Upgraded KoenZomers.OneDrive.Api to v2.0.3.1 which has support for the Microsoft Graph API
- Plugin is now compiled against the Microsoft .NET Framework v4.5.2 as v4.5 is out of support
- Added the option to use the Microsoft Graph API to store the KeePass database on OneDrive or OneDrive for Business. The API will automatically define if it's the Consumer OneDrive or OneDrive for Business based on the login you use. Using the Microsoft Graph API option is now the recommended option.
- Added support for synchronizing with a SharePoint 2013, 2016 or SharePoint Online site when using Low Trust oAuth (ACS). More information [here](./Configuration.md#sharepoint-2013-2016-and-sharepoint-online-support).
- Fixed issue with not being able to close the database anymore if something would go wrong during a sync attempt (i.e. network not available)
- Added a OneDriveSync Offline option under the File menu. If for whatever reason you temporarily don't want OneDriveSync to communicate with where you stored your database online during opening or saving, you can enable this option. If it's in offline mode, you can still force a sync through Tools -> KeeOneDriveSync Options -> right click on database -> Sync Now. [More info](./Configuration.md#offline-mode). Thanks to Albert Krawczyk for suggesting this option.

[Version History](./VersionHistory.md)

## TODO

1. Add support for High Trust oAuth towards SharePoint 2013 and 2016 on premises
2. Add an easier way for SharePoint Online TeamSites to authenticate instead of using ACS oAuth tokens

## Special Thanks

Special thanks to Oleksandr Senyuk for making [KeeSkyDrive](http://sourceforge.net/projects/keeskydrive/) as it has inspired me to create this plugin.

## Feedback

Comments\suggestions\bug reports are welcome!

Koen Zomers
koen@zomers.eu
