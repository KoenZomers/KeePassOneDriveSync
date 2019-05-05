# KeePass OneDrive Sync

I've created a free plugin for KeePass (installable and portable) that allows syncing of multiple password databases from multiple OneDrives to a local version. It allows you to synchronize an unlimited amount of KeePass databases with an unlimited amount of OneDrives. So i.e. you can synchronize your personal KeePass database with your personal OneDrive and your work related KeePass with a OneDrive for Business on Office 365 that is shared among your colleagues, if you wish. This way it's also easy to be able to access your same KeePass database from all of your Windows devices.

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

Version 2.0.4.0 - May 5, 2019

- Fixed issue where synchronizing with a OneDrive database on someone else their OneDrive Consumer space would throw an error [issue 73](https://github.com/KoenZomers/KeePassOneDriveSync/issues/73)
- Fixed issue where opening a KeePass database from OneDrive Consumer would ask for reauthentication directly after opening the database
- The KeeOneDrive configuration now allows for multi select for deleting entries and opening their local file locations on your system
- The OneDrive file picker dialog will now show who shared the item, the file size, file creation date/time and last modification date/time when hovering over items to help with picking the right file

Version 2.0.3.0 - April 9, 2019

- Removed the option to store the OneDrive Refresh Token in the Windows Credential Manager. Several people raised an issue where storing the Refresh Token in the Windows Credential Manager kept prompting them to log in each time they would restart KeePass or reopen the database. I found out that the Refresh Tokens have become longer. Because of this they don't fit into the Windows Credential Manager store anymore. There is no way for me to enlarge the storage in the Windows Credential Manager store nor to truncate the Refresh Token, so the only option I had left was to remove it as an option. Existing databases which are using it will keep using it. New databases won't get the option anymore. If you are facing the login prompt each time you open your database, go to Tools -> OneDriveSync Options and remove the line with your database. Hit ctrl+s again and set up your sync again choosing one of the two remaining options: storing it in the KeePass database or in the KeePass config file on your local disk. [More information on both options](OneDriveRefreshToken.md).

Version 2.0.2.5 - February 18, 2019

- Enabled TLS 1.2 to be used to resolve [issue 72](https://github.com/KoenZomers/KeePassOneDriveSync/issues/72)

Version 2.0.2.4 - February 18, 2019

 - Enabled surpressing of JavaScript errors in the Internet Explorer navigation window which is used to sign in to OneDrive. The error that was shown did no harm and confused end users. This resolves [issue 76](https://github.com/KoenZomers/KeePassOneDriveSync/issues/76) .

Version 2.0.2.3 - February 18, 2019

- Updated the OneDrive API package to version 2.1.2.1 in which the issue with OneDrive for Business as addressed here has been fixed [issue 78](https://github.com/KoenZomers/KeePassOneDriveSync/issues/78)

Version 2.0.2.2 - February 17, 2019

- Fixed the instance not set to a reference of an object error reported by several people typically when still using the OneDriveConsumer Storage Provider
- I've stopped publishing the DLL files from now on to avoid confusion. I believe everyone is using the PLGX already anyway. If not and you have a good reason to use the DLLs over the PLGX, let me know.

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
