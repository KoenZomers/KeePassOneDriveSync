# KeePass OneDrive Sync

## Download ##

### Can I share one KeePass database with friends/co-workers/family without having to share my credentials to my OneDrive? ###

Yes! You can simply share the KeePass database or its entire folder on your OneDrive Personal or OneDrive for Business site. Be sure to share it with the people you want to share the KeePass database with by entering their e-mail addresses. Sharing the database through a shareable link will not work for this plugin.

Now simply have the other person do a File -> Open -> Open from OneDrive (ctrl+alt+o), have them log in to their own OneDrive Personal / OneDrive for Business site preferably using the Graph API, click on the "Shared with me" tab in the OneDrive file picker dialog and select the database that was shared. Note that it can take a few minutes between creating a new shared item and it actually showing up in the dialog. You can hit F5 to refresh the list to see when it shows up if it doesn't right-away.

Another option to share a KeePass database is to upload it to a SharePoint site on a SharePoint farm that supports ACS / Low Trust. You can then just share the Client ID / Client Secret which gives access to the database location to give others access to it as well.

### KeePass OneDrive Sync doesn't work through my proxy! ###

 See the Proxy Issues section in [Troubleshooting](./Troubleshooting.md)

### Can you make a version that works on non Windows machines? ###

No I cannot.

### Can you make a version that works with an older .NET Framework version than 4.5.2? ###

No I will not. Lots of functionality inside the plugin depends on the async development patterns introduced in the .NET 4.5 framework. I will not take the extreme efforts to use alternative approaches for this. Just install .NET 4.5 or later. If you can't because you're still on Windows XP: dude, upgrade your operating system!

### What happens to my KeePass database if multiple changes on multiple devices have been made? ###

When triggering a save (ctrl+s) or opening a KeePass database, the plugin will automatically verify against OneDrive Personal / OneDrive for Business if the file hosted there has been updated since the last time that specific client downloaded it. If not, it will not do anything. If so, it will download a copy of that KeePass database to a temporary location on your harddrive and merge all changes from the downloaded copy with your local KeePass database. So changes in both the version stored online as well as in your local copy will be retained. Not at any time will one overwrite the other. This is ideal in situations where you're using KeePass on clients that are not always online (i.e. laptops). As soon as you get online and trigger a sync, everything will be merged again. Once the changes are merged, an updated copy of the KeePass database will automatically be uploaded to OneDrive / OneDrive for Business again so your other devices can grab the updated copy.

### With Microsoft Graph API support having been added in v2.0, should I switch my current OneDrive Personal / OneDrive for Business syncs to use that instead? ###

You don't need to. The OneDrive API will stay supported for the foreseeable future. You can though. There's no real reason to do so.

### I want to switch from using the OneDrive API to using the Graph API, how do I do this? ###

Just go into the KeePass -> Tools -> OneDriveSync Options and delete the line(s) of the KeePass databases you wish to reconnect to a cloud storage provider. Once you open the database again and save it (CTRL+S), the wizard will pop up again allowing you to set up the syncrhonization. Just choose Graph API and follow the steps.

### I reset my OneDrive password and now my KeePass sync fails, how do I fix this? ###

It is by design that when you reset your OneDrive (Microsoft Account) password, all active refresh tokens will be invalidated. This is a security measure as the reason for changing the password could be that somebody gained access to it. In this scenario your KeePass sync will stop working. You can easily resolve this by going Tools -> OneDriveSync Options -> delete the entry with the database you're having problems with. This will not delete the KeePass file, just the configuration for the plugin for it. Now if you save your KeePass database again (ctrl+s) you will receive the wizard again to set up your sync. After going through this again all should work well again.

### KeePass doesn't detect the plugin ###

If you have downloaded the PLGX and placed it inside the KeePass/Plugins folder (typically C:\Program Files (x86)\KeePass Password Safe 2\Plugins) and it doesn't show its functionality, ensure that the PLGX file is not blocked. By default it will be. go to the Plugins folder, right click the KeeOneDriveSync.plgx file and go to its properties. If it shows an option to Unblock it at the bottom right of the General tab, check the box and hit OK. Restart KeePass. It should now properly load the plugin.

### Is there any (KeePass) data that flows through any of your environments? ###

No. There is no data that flows in any way to or through any service I host or own for this plugin. All communication goes directly between the KeePass client and the cloud provider where the data is hosted, such as Microsoft OneDrive for Business. The traffic between KeePass and Microsoft is encrypted through HTTPS encryption. The refresh token which could give access to the storage provider, such as OneDrive for Business, is stored to prevent having to authenticate over and over again on each synchronization. This token is stored either in the KeePass database, thus encrypted and secured in the same ways as everything else in your KeePass database is, or on your local file system in the user profile folder:

C:\Users<username>\AppData\Roaming\KeePass

The token in this config file is encrypted using built-in Windows encryption and only can be decrypted if you are logged on to Windows with the same user as under which this data is stored.

Communication with the storage providers happens via my [OneDriveAPI](https://github.com/KoenZomers/OneDriveAPI) open source API, as you can see in the [package reference](https://github.com/KoenZomers/KeePassOneDriveSync/blob/master/KoenZomers.KeePass.OneDriveSync/packages.config). If you want to see exactly where it specifies which services to communicate with, see here:

- [OneDrive for Business](https://github.com/KoenZomers/OneDriveAPI/blob/master/Api/OneDriveForBusinessO365Api.cs)
- [OneDrive Consumer](https://github.com/KoenZomers/OneDriveAPI/blob/master/Api/OneDriveConsumerApi.cs)
- [Microsoft Graph API](https://github.com/KoenZomers/OneDriveAPI/blob/master/Api/OneDriveGraphApi.cs)

You will find the URLs of the services it communicates with at the top of each file. You can see that these are all Microsoft owned and managed services and all communicate through HTTPS.

I recommend you to read up on the oAuth flow which will show you that all communication will always go between the client and the oAuth server directly, without having any third parties in between:

https://docs.microsoft.com/en-us/onedrive/developer/rest-api/getting-started/graph-oauth?view=odsp-graph-online

### Other questions ###

Feel free to e-mail me at koen@zomers.eu or [open a GitHub Issue](https://github.com/KoenZomers/KeePassOneDriveSync/issues/new)
