# KeePass OneDrive Sync

A free plugin for KeePass (installable and portable) that allows syncing of multiple password databases from multiple OneDrives to a local version. It allows you to synchronize an unlimited amount of KeePass databases with an unlimited amount of OneDrives. So i.e. you can synchronize your personal KeePass database with your personal OneDrive and your work related KeePass with a OneDrive for Business on Office 365 that is shared among your colleagues, if you wish. This way it's also easy to be able to access your same KeePass database from all of your devices. Note that this plugin only works on Windows based platforms, but can operate in conjunction with other KeePass plugins serving other operating systems such as KeePass2Android on Android devices.

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

Version 2.1.1.2 - March 13, 2020

- Fixed a bug where corrupted configuration entries could not be deleted [issue 128](https://github.com/KoenZomers/KeePassOneDriveSync/issues/128)

[Full version history](https://github.com/KoenZomers/KeePassOneDriveSync/releases)

## TODO

1. Add support for High Trust oAuth towards SharePoint 2013/2016/2019 on premises
2. Add an easier way for SharePoint Online TeamSites to authenticate instead of using ACS oAuth tokens

## Special Thanks

Special thanks to Oleksandr Senyuk for making [KeeSkyDrive](http://sourceforge.net/projects/keeskydrive/) as it has inspired me to create this plugin.

## Feedback

[Comments\suggestions\bug reports](https://github.com/KoenZomers/KeePassOneDriveSync/issues/new/choose) are welcome!

Koen Zomers
koen@zomers.eu
