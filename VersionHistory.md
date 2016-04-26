# KeePass OneDrive Sync

## Version History

Version 1.4.2.0 - March 31, 2016

- Fixed the [issue](https://github.com/KoenZomers/KeePassOneDriveSync/issues/13) where not synced databases kept asking if they should be synced after restarting KeePass. Thanks to [mushak](https://github.com/mushak) for reporting this!

Version 1.4.1.0 - February 4, 2016

- Fixed broken about dialog, added missing resx file. Thanks Ruslan Aleksandrovic for reporting this!

Version 1.4.0.0 - February 3, 2016

- Added support for OneDrive for Business based on many requests from users

Version 1.3.3.1 - December 3, 2015

- Finally was able to fix the issue with the PLGX. Thanks to Dominik Reichl!

Version 1.3.3.0 - November 25, 2015

- Added support for using a HTTP/HTTPS proxy. You can switch this on under Tools -> Options -> Advanced -> Proxy.  

Version 1.3.2.0 - July 18, 2015

- Added better error handling when remote databases on FTP or websites are used. These are still not supported, but now should not let the plug in crash anymore when used.
- Limited the rights requested to your OneDrive to the minimum

Version 1.3.2.0 - July 18, 2015

- Added better error handling when remote databases on FTP or websites are used. These are still not supported, but now should not let the plug in crash anymore when used.
- Limited the rights requested to your OneDrive to the minimum

Version 1.3.1.0 - July 17, 2015

- Updated the link in the OneDrive Token storage selection dialog to link to the help page regarding this topic

Version 1.3.0.0 - July 13, 2015

- Fixed a bug where uploading could fail if you stored the KeePass database in the root of your OneDrive
- Added a red background color to the KeeOneDriveSync Configurations overview to configurations for KeePass databases that no longer exist locally
- Added two extra options for storing the OneDrive Refresh Token: Windows Credential Manager and in the KeePass datbase itself [more info](./OneDriveRefreshToken.md)
- Added version number of the plugin to be shown in the About dialog
- Added option to open the Windows File Explorer to go to the location where the KeePass database is stored locally by right clicking on a configuration item in the KeeOneDriveSync configuration overview
- Revised the delete dialogs to make it more clear that it just deleted the KeeOneDriveSync configuration for that database, not the KeePass database itself
- Fixed typo on the ask to start synchronization screen

Version 1.2.0.0 - July 10, 2015

- Fixed bug where uploading KeePass databases >= 5 KB would throw an error