# KeePass OneDrive Sync

## Version History

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

- Added the option to open a KeePass database directly from OneDrive Consumer or OneDrive for Business under File > Open > Open from OneDrive. It will automatically set up the sync connection between your cloud hosting and local mapping.

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
