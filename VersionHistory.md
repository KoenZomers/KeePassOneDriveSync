# KeePass OneDrive Sync

## Version History

Version 2.0.2.1 - October 16, 2018

- Fixed incorrect spelling of equivalent in the where to store the token dialog. Thanks to [awesomecogs](https://github.com/awesomecogs) for [reporting it](https://github.com/KoenZomers/KeePassOneDriveSync/issues/64)!

Version 2.0.2.0 - April 30, 2018

- Fixed [issue 50](https://github.com/KoenZomers/KeePassOneDriveSync/issues/50): Opening the same kbdx file in different ways results in two Local Paths
- Issue with Shared With Me folders hosted on a SharePoint site has been resolved. Thanks to [Andy Martin](https://github.com/Esvandiary) for fixing this through a PR.
- Fixed [issue 52](https://github.com/KoenZomers/KeePassOneDriveSync/issues/52): When opening the database in KeePass portable it says "Failes to sync"

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
- Added support for synchronizing with a SharePoint 2013, 2016, 2019 or SharePoint Online site when using Low Trust oAuth (ACS). More information [here](./Configuration.md#sharepoint-2013-2016-and-sharepoint-online-support).
- Fixed issue with not being able to close the database anymore if something would go wrong during a sync attempt (i.e. network not available)
- Added a OneDriveSync Offline option under the File menu. If for whatever reason you temporarily don't want OneDriveSync to communicate with where you stored your database online during opening or saving, you can enable this option. If it's in offline mode, you can still force a sync through Tools -> KeeOneDriveSync Options -> right click on database -> Sync Now. [More info](./Configuration.md#offline-mode). Thanks to Albert Krawczyk for suggesting this option.

Version 1.8.3.0 - August 18, 2017

- Fixed an issue where having unsaved changes to your KeePass database and then exiting KeePass would not get the changes uploaded to OneDrive/OneDrive for Business before KeePass would exit. It will now wait with closing KeePass until the changes are saved. Thanks to Darko Jamnik for reporting this issue.

Version 1.8.2.0 - May 9, 2017

- Improved the OneDrive platform selection dialog to aid visually impaired. Thanks to Oire for reporting it [issue 28](../../issues/28)

Version 1.8.1.0 - December 24, 2016

- Bugfix of a bug introduced in 1.8.0.0 where having a KeePass database stored in the same folder or in a folder under the folder where the KeePass executable resides gave the error "Failed to sync. Please don't switch to another database before done." when trying to save the changes of the KeePass database. Thanks to DazzaQLD for reporting it [issue 26](../../issues/26)

Version 1.8.0.0 - December 16, 2016

- If you had two or more KeePass databases open at the same time and you would cause a sync of one database by i.e. pressing ctrl+s while switching to another open database in KeePass before the sync was done, it would try to merge the first database with the other open database. A check has been added to stop the sync in this scenario so two different databases will never become merged. [issue 25](../../issues/25)

Version 1.7.3.0 - December 14, 2016

- When being offline, it would show a modal dialog with an error which you would need to dismiss. Now instead it will just display an offline message in the status bar to make it more user friendly.

Version 1.7.2.0 - December 14, 2016

- When using the File -> Open from OneDrive option and getting to saving the KeePass database locally, it will now assume the same file name as it is stored under on OneDrive as the default filename

Version 1.7.1.0 - October 17, 2016

- Fixed an issue when using a proxy with the system default settings and the system default credentials which I introduced in 1.7.0.0. Thanks everyone for reporting this!

Version 1.7.0.0 - October 14, 2016

- The plugin now also supports relative paths. If you store your KeePass database in the same folder as from where you run KeePass or from a folder below the folder from where you run KeePass, it will automatically reference the database by its path relative to the KeePass executable folder. This allows for portable usage when using KeePass on i.e. a thumb drive. If your KeePass database is located in a different folder below the KeePass executable or from a different drive, it will keep referencing it by its full path. Thanks to Devin Jenson for making this request.

Version 1.6.2.0 - October 14, 2016

- If for whatever reason it can't get an OneDrive instance, it will now prompt you to log in again. This could i.e. happen if you change your OneDrive/OneDrive for Business credentials. This invalidates all existing oAuth refresh tokens. If you change your credentials, the plugin will now detect that the refresh token is no longer valid and will simply request you to log in once with your new credentials to get a new working refresh token. Thanks to Carl Craft for bringing this issue to my attention.

Version 1.6.1.0 - October 13, 2016

- Thanks to pointers from Filip Hasa I managed to set up a Squid on Windows proxy server so I can finally test the proxy scenarios myself. Fixed several proxy issues, mainly when using the "Default Credentials" and/or "Use system proxy settings" things could go wrong. If you're still encountering proxy issues with this version, please let me know.

Version 1.6.0.0 - August 15, 2016

- Added the option to open a KeePass database directly from OneDrive Personal or OneDrive for Business under File > Open > Open from OneDrive. It will automatically set up the sync connection between your cloud hosting and local mapping.

Version 1.5.1.0 - August 13, 2016

- Attempt to fix the proxy issue when using NTLM as reported [here](https://github.com/KoenZomers/KeePassOneDriveSync/issues/10)

Version 1.5.0.0 - August 13, 2016

- Introduced an easy to use OneDrive file picker dialog to select the location where you want to store your KeePass database. The #1 issue people contacted me about was that they misunderstood the previous single open text dialog. Hopefully that's fixed with introducing this dialog. Check out the extra options under the right click in the OneDrive location picker dialog!
- Introduced the ability to store the KeePass database in a OneDrive folder that is located on another OneDrive and has been shared with you. Check out the [FAQ](./Faq.md) for instructions on how to do this. This was the #2 most requested option and allows you to share a KeePass database with others without having to give away your credentials to your OneDrive.

Version 1.4.5.0 - August 10, 2016

- Bugfix: you should no longer receive a crash when logging in to your KeePass database that is synced with a cloud service while not having an active internet connection. Thanks to Daniel Matschull for reporting this issue!

Version 1.4.4.0 - April 29, 2016

- Updated [OneDrive API](https://github.com/KoenZomers/OneDriveAPI) to version 1.5.0.0 which now supports the use of HTTP proxies throughout all functionality. This should fix the plugin bypassing the proxy on writing updates back to OneDrive\OneDrive for Business
- Fixed the resources exception when opening the about box

Version 1.4.3.0 - April 26, 2016

- Fixed an issue where if you would use different filenames for your KeePass database locally and on OneDrive or OneDrive for Business, that it wouldn't sync properly anymore. Thanks to Jörgen Nydahl for reporting this!
- Attempt to solve an issue where on Windows machines running Windows in a non English language, an error may occur. If you still run into this issue, let me know.

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
