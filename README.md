# KeePass OneDrive Sync

> Notice: Through the years I have enjoyed building and maintaining this code. I have been surprised to see how many people started using this plugin and how often around the globe I met people that asked me if I was the person behind the plugin. Time however is scarse and as much as I would like to, I'm having too many projects to work on in my spare time, so I need to get rid of some, this unfortunately being one. I will therefore no longer maintain this code. Feel free to fork off of it and make your own version out of it. I'll be happy to link to it.

![](https://github.com/KoenZomers/KeePassOneDriveSync/workflows/.NET%20Core/badge.svg) ![](https://img.shields.io/github/downloads/koenzomers/KeePassOneDriveSync/total.svg) ![](https://img.shields.io/github/issues/koenzomers/KeePassOneDriveSync.svg) [![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)

A free plugin for KeePass (installable and portable) that allows syncing of multiple password databases from multiple OneDrives to a local version. It allows you to synchronize an unlimited amount of KeePass databases with an unlimited amount of OneDrives. So i.e. you can synchronize your personal KeePass database with your personal OneDrive and your work related KeePass with a OneDrive for Business on Office 365 that is shared among your colleagues, if you wish. This way it's also easy to be able to access your same KeePass database from all of your devices. Note that this plugin is only supported on Windows based platforms, but [has been reported to work on \*nix as well using Mono](https://github.com/KoenZomers/KeePassOneDriveSync/wiki/System-Requirements#nix-through-mono). It can also operate in conjunction with other KeePass plugins serving other operating systems such as KeePass2Android on Android devices.

## Download ##
Download the PLGX and place it inside your KeePass\Plugins folder. Typically this will be `C:\Program Files (x86)\KeePass Password Safe 2\Plugins` or if you're using KeePass Portable, put it in a subfolder called Plugins from where your keepass.exe is located. When upgrading from a previous version of this plugin, simply ensure KeePass is closed, overwrite the existing PLGX and start KeePass again.

[You can find the latest stable version here](../../releases/latest)

## Documentation ##
- [System requirements](https://github.com/KoenZomers/KeePassOneDriveSync/wiki/System-Requirements)
- [Installation instructions](https://github.com/KoenZomers/KeePassOneDriveSync/wiki/Installation-instructions)
- [Configuration options](https://github.com/KoenZomers/KeePassOneDriveSync/wiki/Configuration-options)
- [Checking for updates](https://github.com/KoenZomers/KeePassOneDriveSync/wiki/Update-check)
- [Troubleshooting](https://github.com/KoenZomers/KeePassOneDriveSync/wiki/Troubleshooting)
- [Frequently Asked Questions](https://github.com/KoenZomers/KeePassOneDriveSync/wiki/FAQ)

## Latest Version

Version 2.1.2.2 - November 24, 2021

- Fixed the issue where the plugin cannot check for an update [#151](https://github.com/KoenZomers/KeePassOneDriveSync/issues/151)

Version 2.1.2.1 - May 22, 2020

- Bugfix causing opening of the KeePassOneDriveSync about dialog to crash KeePass. Thanks to Drew O'Hara for letting me know!

Version 2.1.2.0 - May 22, 2020

- Fixed a bug where uploading a KeePass database to a document library on SharePoint having "Require documents to be checked out before they can be edited?" set to Yes would never make the KeePass database show up in SharePoint [issue 131](https://github.com/KoenZomers/KeePassOneDriveSync/issues/131)
- Updated the SharePoint connector so that it now makes use of versioning in the SharePoint document library allowing you to see in SharePoint when it was updated and allowing you to revert to previous versions of the KeePass database just like already was the case when storing the KeePass database on OneDrive for Business
- Removed OneDrive option in the cloud storage providers as there really should not be a reason anymore to use it instead of the Microsoft Graph options on the first tab
- Removed the wording Microsoft Graph on the first tab of the cloud storage providers as I noticed it is confusing for many people. Named them OneDrive & OneDrive for Business instead. Technically, they're still using the Microsoft Graph to connect to OneDrive, so it's just a visual change.
- Fixed a small issue when refreshing the SharePoint location picker or enabling/disabling showing of hidden libraries not entirely working well
- Changed that in the OneDrive and SharePoint file pickers, if an item in the folder has the same name as shown in the filename textbox, that it will get selected by default to make it clearer that it already exists in that folder
- Added tooltips for files and folders in the SharePoint file picker to show additional information on these items when hovering over them such as creation date/time, last modified date/time, file size and file version number
- Updated links pointing to the GitHub documentation to the new GitHub Wiki location

[Full version history](https://github.com/KoenZomers/KeePassOneDriveSync/releases)

## Special Thanks

Special thanks to Oleksandr Senyuk for making [KeeSkyDrive](http://sourceforge.net/projects/keeskydrive/) as it has inspired me to create this plugin.
