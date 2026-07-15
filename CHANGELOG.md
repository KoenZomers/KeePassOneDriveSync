# Changelog

All notable changes to this project are documented in this file.

## [3.2.1.0] - July 15, 2026


- Fixed potential issue with uploading larger KeePass databases, likely only occurring with OneDrive Personal
- Fixed strict dependency on KeePass 2.61.1.0 to be more flexible. It will try to use any KeePass version now.

## [3.2.0.0] - July 14, 2026

- Fixed "Failed to upload the KeePass database" errors affecting databases larger than a few KB. This was caused by a bug in the [KoenZomers.OneDrive.Api](https://github.com/KoenZomers/OneDriveAPI) library where the simple upload size threshold was incorrectly set to 4KB instead of 4MB, which has now been corrected upstream

## [3.1.0.0] - July 12, 2026

- Addressed issue with JSON DLL missing

## [3.0.0.0] - July 11, 2026

- Fixed the authentication issue of the previous version
- Migrated to version 3 of the [KoenZomers.OneDrive.Api](https://github.com/KoenZomers/OneDriveAPI) library, which now authenticates using the Microsoft Authentication Library (MSAL) instead of the legacy hand-rolled OAuth implementation
- Removed the "Built-in browser" OneDrive sign-in option; only the "Any browser" option remains, which now opens your system's default browser and signs you in via MSAL's system browser flow (listening on `http://localhost`) instead of the device code flow
- Removed the consumer OneDrive (Live Connect) and legacy SharePoint REST v2.0 (OneDrive for Business O365) connectors; all OneDrive traffic now goes through the Microsoft Graph API. It still works with Consumer and Business OneDrives.
- Removed the option to choose where the OneDrive refresh token is stored (disk, Windows Credential Manager or the KeePass database itself). The (now MSAL-based) token cache is always stored DPAPI-encrypted in KeePass.config.xml, fully automatically
- Replaced Newtonsoft.Json with the native `System.Text.Json` for all JSON (de)serialization
- Various internal cleanup and dependency updates

## [3.0.2.0]

- Fixed a potential issue with larger file uploads [#49](https://github.com/KoenZomers/OneDriveAPI/pull/49). Thanks YasarF!

## [2.1.2.1] - May 22, 2020

- Bugfix causing opening of the KeePassOneDriveSync about dialog to crash KeePass. Thanks to Drew O'Hara for letting me know!

## [2.1.2.0] - May 22, 2020

- Fixed a bug where uploading a KeePass database to a document library on SharePoint having "Require documents to be checked out before they can be edited?" set to Yes would never make the KeePass database show up in SharePoint [issue 131](https://github.com/KoenZomers/KeePassOneDriveSync/issues/131)
- Updated the SharePoint connector so that it now makes use of versioning in the SharePoint document library allowing you to see in SharePoint when it was updated and allowing you to revert to previous versions of the KeePass database just like already was the case when storing the KeePass database on OneDrive for Business
- Removed OneDrive option in the cloud storage providers as there really should not be a reason anymore to use it instead of the Microsoft Graph options on the first tab
- Removed the wording Microsoft Graph on the first tab of the cloud storage providers as I noticed it is confusing for many people. Named them OneDrive & OneDrive for Business instead. Technically, they're still using the Microsoft Graph to connect to OneDrive, so it's just a visual change.
- Fixed a small issue when refreshing the SharePoint location picker or enabling/disabling showing of hidden libraries not entirely working well
- Changed that in the OneDrive and SharePoint file pickers, if an item in the folder has the same name as shown in the filename textbox, that it will get selected by default to make it clearer that it already exists in that folder
- Added tooltips for files and folders in the SharePoint file picker to show additional information on these items when hovering over them such as creation date/time, last modified date/time, file size and file version number
- Updated links pointing to the GitHub documentation to the new GitHub Wiki location

