# KeePass OneDrive Sync

## OneDrive Refresh Token

When setting up a new synchronization connection between a local KeePass database and OneDrive, one of the questions you will get is where you want the OneDrive Refresh Token to be stored.

![](./Screenshots/WhereToStoreOneDriveToken.png)

### Why does it matter

The OneDrive Refresh Token, along with the Client ID and Client Secret which can easily be retrieved from the open source code of this plugin, gives full unrestricted access to your OneDrive.

It is built this way because we don't want to prompt you for your credentials each time KeePass needs to sync the database with OneDrive.

This means that the Refresh Token should be kept secure and safe from anyone else to avoid them getting access to your complete OneDrive without you even knowing it and bypassing Multi Factor Authentication, if you have set that up.

### Which options do I have

You can choose from the following two options:

1. Store the Refresh Token in the KeePass database itself
2. Store the Refresh Token int he KeePass configuration file on your PC

I will explain both options below. If you don't really care about the details, just go with the default option 2 which will suit most users in most scenarios.

#### 1. KeePass database

The first option is to store the OneDrive Refresh Token in the encrypted KeePass database itself. Only after unlocking the KeePass database by using whichever means you've set it to (password, certificate, windows credentials), the OneDrive Refresh Token will be accessible by the plugin to be used.

This means that the OneDrive Refresh Token is part of the KDBX file. If you copy the file over to someone else (i.e. a co-worker), you are effectively copying the OneDrive Refresh token to your OneDrive along with it. This might not be what you want to happen. If the other user opens the KeePass database and doesn't have the KeeOneDriveSync plugin installed in KeePass, nothing happens for that user and the database will function as normal. If that user does have the KeeOneDriveSync plugin installed as well, it will use the same Refresh Token to communicate with OneDrive/OneDrive for Business. The other user would also be able to go in KeePass to Tools -> Database Tools -> Database Maintenance -> Plugin Data and see and retrieve the Refresh Token.

So if you intend to share your KeePass database with others and you don't want them to sync it to the same OneDrive location, do not use this option and choose the second option.

#### 2. KeePass Configuration File

Located under %APPDATA%\KeePass (enter it like this in Windows Explorer) KeePass by default stores a configuration file as KeePass.config.xml.

If you choose this option, the OneDrive Refresh Token will be stored encrypted using a Windows encryption library in this configuration file (as of version 2.0.6.0). The encrypted value can only be unencrypted if being done by the same user logged on to Windows. So if you're using your machine with multiple users, other users will not be able to decrypt the Refresh Token, even if they do have access to the KeePass configuration file.

This option is most ideal to use if you're sharing your KeePass database with other users (i.e. co-workers) which all use their own OneDrive/OneDrive for Business credentials to access the same shared KeePass database. In this case, they will not be able to get access to your Refresh Token which gives full access to your OneDrive\OneDrive for Business.

Note that the KeePass.config.xml file will always contain the other configuration settings of your KeePass database, even if you choose to store the OneDrive Refresh Token using the first method. In that case the RefreshToken attribute in the config file will show null as it's being fetched from another location. The other information is not sensitive information, thus can perfectly be stored in this file.