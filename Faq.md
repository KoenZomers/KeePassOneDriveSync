# KeePass OneDrive Sync

## Download ##

### Can I share one KeePass database with friends/co-workers/family without having to share my credentials to my OneDrive? ###

Yes! You can simply share the KeePass database or its entire folder on your OneDrive Personal or OneDrive for Business site. Be sure to share it with the people you want to share the KeePass database with by entering their e-mail addresses. Sharing the database through a shareable link will not work for this plugin.

Now simply have the other person do a File -> Open -> Open from OneDrive (ctrl+alt+o), have them log in to their own OneDrive Personal / OneDrive for Business site preferably using the Graph API, click on the "Shared with me" tab in the OneDrive file picker dialog and select the database that was shared. Note that it can take a few minutes between creating a new shared item and it actually showing up in the dialog. You can hit F5 to refresh the list to see when it shows up if it doesn't right-away.

Another option to share a KeePass database is to upload it to a SharePoint site on a SharePoint farm that supports ACS / Low Trust. You can then just share the Client ID / Client Secret which gives access to the database location to give others access to it as well.

### KeePass OneDrive Sync doesn't work through my proxy! ###

 See the Proxy Issues section in [Troubleshooting](./Troubleshooting.md)

### I don't understand the OneDrive location dialog. What should I enter in the textbox? ###

I have to admit it was a rather vague dialog screen. As from version 1.5.0.0 this dialog has been replaced with an easy to use OneDrive picker which should resolve all your issues. Be sure to check out the extra options under the right click of your mouse when being in the picker.

### Can you make a version that works on non Windows machines? ###

No I cannot.

### Can you make a version that works with an older .NET Framework version than 4.5.2? ###

No I will not. Lots of functionality inside the plugin depends on the async development patterns introduced in the .NET 4.5 framework. I will not take the extreme efforts to use alternative approaches for this. Just install .NET 4.5 or later. If you can't because you're still on Windows XP: dude, upgrade your operating system!

### Why do I manually have to download the KeePass database first from OneDrive? ###

You don't have to anymore. With version 1.6.0.0 I've introduced the option under File -> Open -> Open from OneDrive to directly open the KeePass from OneDrive and set it up for synchronization locally with just a few steps.

### What happens to my KeePass database if multiple changes on multiple devices have been made? ###

When triggering a save (ctrl+s) or opening a KeePass database, the plugin will automatically verify against OneDrive Personal / OneDrive for Business if the file hosted there has been updated since the last time that specific client downloaded it. If not, it will not do anything. If so, it will download a copy of that KeePass database to a temporary location on your harddrive and merge all changes from the downloaded copy with your local KeePass database. So changes in both the version stored online as well as in your local copy will be retained. Not at any time will one overwrite the other. This is ideal in situations where you're using KeePass on clients that are not always online (i.e. laptops). As soon as you get online and trigger a sync, everything will be merged again. Once the changes are merged, an updated copy of the KeePass database will automatically be uploaded to OneDrive / OneDrive for Business again so your other devices can grab the updated copy.

### With Microsoft Graph API support having been added in v2.0, should I switch my current OneDrive Personal / OneDrive for Business syncs to use that instead? ###

You don't need to. The OneDrive API will stay supported for the foreseeable future. You can though. There's no real reason to do so.

### I want to switch from using the OneDrive API to using the Graph API, how do I do this? ###

Just go into the KeePass -> Tools -> OneDriveSync Options and delete the line(s) of the KeePass databases you wish to reconnect to a cloud storage provider. Once you open the database again and save it (CTRL+S), the wizard will pop up again allowing you to set up the syncrhonization. Just choose Graph API and follow the steps.

### I reset my OneDrive password and now my KeePass sync fails, how do I fix this? ###

It is by design that when you reset your OneDrive (Microsoft Account) password, all active refresh tokens will be invalidated. This is a security measure as the reason for changing the password could be that somebody gained access to it. In this scenario your KeePass sync will stop working. You can easily resolve this by going Tools -> OneDriveSync Options -> delete the entry with the database you're having problems with. This will not delete the KeePass file, just the configuration for the plugin for it. Now if you save your KeePass database again (ctrl+s) you will receive the wizard again to set up your sync. After going through this again all should work well again.

### Other questions ###

Feel free to e-mail me at koen@zomers.eu
