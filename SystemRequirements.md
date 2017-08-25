# KeePass OneDrive Sync

## System Requirements

- The plugin is written for KeePass v2.36 but should work with any KeePass version in the 2.x range
- It requires to have the Microsoft .NET v4.5.2 framework installed. This means this plugin will NOT work on Windows XP as the Microsoft .NET Framework v4.5.2 is not available on that OS. On later operating systems, you may have to install or enable it manually.
- It only supports KeePass databases which are opened via File -> Open -> Open File in KeePass. Databases which you open via File -> Open -> Open URL will not be able to use this plugin and will get a message stating that they're not supported.
- It works with a personal OneDrive and OneDrive for Business on Office 365. It doesn't work with OneDrive for Business on an on premises SharePoint 2013 or SharePoint 2016 farm. Nor does it allow syncing to SharePoint Online (besides OneDrive for Business) at this time.