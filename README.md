# KeePass OneDrive Sync

I've created a free plugin for KeePass (installable and portable) that allows syncing of multiple password databases from multiple OneDrives to a local version. It allows you to synchronize an unlimited amount of KeePass databases with an unlimited amount of OneDrives. So i.e. you can synchronize your personal KeePass database with your personal OneDrive and your work related KeePass with a OneDrive for Business on Office 365 that is shared among your colleagues, if you wish. This way it's also easy to be able to access your same KeePass database from all of your devices. Note that this plugin only works on Windows based platforms, but can operate in conjunction with other KeePass plugins serving other operating systems such as KeePass2Android on Android devices.

## Download ##
Download the PLGX and place it inside your KeePass\Plugins folder. Typically this will be C:\Program Files (x86)\KeePass Password Safe 2\Plugins or if you're using KeePass Portable, put it in a subfolder called Plugins from where your keepass.exe is located. When upgrading from a previous version of this plugin, simply ensure KeePass is closed, overwrite the existing PLGX and start KeePass again.

[You can find the latest stable version here](../../releases/latest)

## Documentation ##
- [System requirements](./SystemRequirements.md)
- [Installation instructions](./Installaton%20Instructions.md)
- [Configuration options](./Configuration.md)
- [Checking for updates](./UpdateCheck.md)
- [Troubleshooting](./Troubleshooting.md)
- [Frequently Asked Questions](./Faq.md)

## Latest Version

Version 2.0.9.0 - September 26, 2019

- Fixed the issue reported in [issue 112](https://github.com/KoenZomers/KeePassOneDriveSync/issues/112) where opening a KeePass database from OneDrive would require you to log on twice. Thanks to [keab](https://github.com/keab) for reporting this!

Version 2.0.8.0 - July 28, 2019

- Merged [Pull Request 102](https://github.com/KoenZomers/KeePassOneDriveSync/pull/102) which should solve some issues around relative paths when using KeePass portable. Thanks to [jfurtner](https://github.com/jfurtner) for his contribution!
- Added & changed shortcut keys to the right click menu option in the KeePass OneDriveSync config screen
  - Enter no longer opens the sync details screen as it was interfearing with some functions, use F1 instead
  - F1: Opens the sync details screen. Only works when one KeePass database is selected.
  - F2: Allows renaming of the storage name for the KeePass database(s) you have selected
  - F4: Starts syncing the selected entries, if those databases are currently open in KeePass
  - F5: Refreshes the list with configuration entries
  - F7: Open the local file locations of the KeePass database(s) you have selected
  - F8: Open the selected KeePass database(s) in KeePass (new feature)
  - DEL: Remove the KeePass OneDriveSync configuration entries for the selected KeePass database(s). It will not remove the KeePass database itself, just the KeePass OneDriveSync configuration for it.
  - CTRL+A: Select all KeePass databases
  - CTRL+SHIFT+A: Select all KeePass databases that no longer exist locally (red colored background)
  - CTRL+Click: Select another KeePass database
  - SHIFT+Click: Select all KeePass databases between the currently selected one and the one you're clicking on
  - Use the right click menu to select all KeePass databases that haven't synced in either the last 24 hours, last week, last 2 weeks or last month

Version 2.0.7.3 - July 24, 2019

- Various fixes around using "Shared with me" with Graph API
  - Fixed [issue 55](https://github.com/KoenZomers/KeePassOneDriveSync/issues/55)
  - Added decimal seperator to the file size in the tooltip balloons when hovering over files
  - Enabled the Up button to go back to the Shared with me overview if going into a shared folder
  - Fixed an issue with uploading a new KeePass database to a subfolder of a shared location on SharePoint where it would store it in the shared location instead of the subfolder

These fixes make it a lot easier now to store your KeePass database on a SharePoint Online (Team)site. See [this article](https://github.com/KoenZomers/KeePassOneDriveSync/blob/master/Configuration.md#sharepoint-online-teamsites) for instructions on how to do so.

Version 2.0.7.2 - July 19, 2019

- Added column to the overview with all your KeePassOneDriveSync configurations which shows the date and time at which the sync last checked if there were updates and if so, synced them. This allows you to easily see at one glance if your local KeePass databases are up to date.

Version 2.0.7.1 - July 7, 2019

- Resolved an issue with using the Open from OneDrive option as reported in [issue 100](https://github.com/KoenZomers/KeePassOneDriveSync/issues/100)

Version 2.0.7.0 - June 30, 2019

- The ability to synchronize multiple databases at once in version 2.0.5.0 didn't work yet for KeePass databases stored on SharePoint. Fixed that in this version, syncing multiple databases at once works for OneDrive Consumer, OneDrive for Business and SharePoint hosted databases now.
- When merging the local KeePass database with the one stored online fails, after the standard error dialog, a new dialog will now show offering you the option to overwrite the online KeePass database with your local version. Typically when this merging fails, it is because you changed the master key of the KeePass database locally. Before you had to manually upload the KeePass database again first before you could sync it again. By answering Yes to this new dialog, this manual step is no longer required and the plugin will do this for you automatically. Requested in [issue 74](https://github.com/KoenZomers/KeePassOneDriveSync/issues/74).

Version 2.0.6.0 - June 2, 2019

- The option to store the Refresh Token on disk will now use a Windows encryption library to store the Refresh Token encrypted to the KeePass config file instead of plain text as it was before. Only if being logged on with the same Windows user, the Refresh Token can be decrypted from the KeePass config file on disk. So even on machines having multiple users, this will be safe and keep other users on the same machine from getting their hands on the Refresh Token. Huge thanks to [Kjetil Limkjær](https://github.com/klimkjar) for adding this great functionality through his [Pull Request](https://github.com/KoenZomers/KeePassOneDriveSync/pull/89). Discussed in [issue 84](https://github.com/KoenZomers/KeePassOneDriveSync/issues/84). If you were using the store Refresh Token on disk option already, with this version it will automatically become encrypted if you open your KeePass database.

Version 2.0.5.1 - June 2, 2019

- Unblocked the KeePassOneDriveSync config dialog for allowing syncing of multiple databases by multi selecting them. The database needs to be open before it can be synced, but it doesn't have to be the active one.
- Unblocked the KeePassOneDriveSync config details dialog for allowing syncing of a database even if it's not the currently active one. The database needs to be open before it can be synced, but it doesn't have to be the active one.
- Added that when you press CTRL+A in the KeePassOneDriveSync config dialog, that it selects all entries. Handy for bulk operations.
- Allowed multi select for renaming multiple databases at once in the KeePassOneDriveSync config dialog using F2 or right click -> Rename

Version 2.0.5.0 - June 2, 2019

- Added the ability to synchronize multiple KeePass databases at the same time. So you no longer get an error message if you're trying to sync a database which is not the currently active one. This is particularly useful when you use scripts or plugins to open multiple databases at once. They all should automatically sync now when they open.
- Added a status message in the status bar or KeePass indicating that the sync is not happening when KeePassOneDriveSync has been set to offline mode so to visualize why a sync is not happening at that time

Version 2.0.4.2 - May 22, 2019

- Still was receiving reports on issues uploading larger than 5 MB databases. Did an additional fix hoping for it to have caught all scenarios this time. Discussed in [issue 88](https://github.com/KoenZomers/KeePassOneDriveSync/issues/88).
- Added option to rename the storage entry on the KeePassOneDriveSync entry using F2 or right click and then choosing Rename. Requested through [issue 91](https://github.com/KoenZomers/KeePassOneDriveSync/issues/91).
- In the dialog where it asks if you want to sync the database, it will now also show for which database it asks the question. Just in case you would use triggers to open multiple databases at once. Requested through [issue 93](https://github.com/KoenZomers/KeePassOneDriveSync/issues/93).

Version 2.0.4.1 - May 17, 2019

- Fixed an issue introduced in 2.0.4.0 where KeePass databases bigger than 5 MB would no longer upload and return an error [issue 88](https://github.com/KoenZomers/KeePassOneDriveSync/issues/88)

Version 2.0.4.0 - May 5, 2019

- Fixed issue where synchronizing with a OneDrive database on someone else their OneDrive Consumer space would throw an error [issue 73](https://github.com/KoenZomers/KeePassOneDriveSync/issues/73)
- Fixed issue where opening a KeePass database from OneDrive Consumer would ask for reauthentication directly after opening the database
- The KeeOneDrive configuration now allows for multi select for deleting entries and opening their local file locations on your system
- The OneDrive file picker dialog will now show who shared the item, the file size, file creation date/time and last modification date/time when hovering over items to help with picking the right file

Version 2.0.3.0 - April 9, 2019

- Removed the option to store the OneDrive Refresh Token in the Windows Credential Manager. Several people raised an issue where storing the Refresh Token in the Windows Credential Manager kept prompting them to log in each time they would restart KeePass or reopen the database. I found out that the Refresh Tokens have become longer. Because of this they don't fit into the Windows Credential Manager store anymore. There is no way for me to enlarge the storage in the Windows Credential Manager store nor to truncate the Refresh Token, so the only option I had left was to remove it as an option. Existing databases which are using it will keep using it. New databases won't get the option anymore. If you are facing the login prompt each time you open your database, go to Tools -> OneDriveSync Options and remove the line with your database. Hit ctrl+s again and set up your sync again choosing one of the two remaining options: storing it in the KeePass database or in the KeePass config file on your local disk. [More information on both options](OneDriveRefreshToken.md).

[Version History](./VersionHistory.md)

## TODO

1. Add support for High Trust oAuth towards SharePoint 2013/2016/2019 on premises
2. Add an easier way for SharePoint Online TeamSites to authenticate instead of using ACS oAuth tokens

## Special Thanks

Special thanks to Oleksandr Senyuk for making [KeeSkyDrive](http://sourceforge.net/projects/keeskydrive/) as it has inspired me to create this plugin.

## Feedback

Comments\suggestions\bug reports are welcome!

Koen Zomers
koen@zomers.eu
