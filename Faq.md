# KeePass OneDrive Sync

## Download ##

### Can I share one KeePass database with friends/co-workers/family without having to share my credentials to my OneDrive? ###

Yes! Create a folder in your OneDrive Consumer where you are going to store the KeePass database. Store your KeePass database in that folder. Share that folder with the people you want to be able to access the same KeePass database. Share it by entering their e-mail addresses (sharing through a link will NOT work) and give them edit rights. The people you invited can now go into their OneDrive, go to Shared in the left bar, click on the folder you've shared with them and at the top click on 'Add to my OneDrive'. They can now manually download the KeePass database to their local machine. Have them open the KeePass database in KeePass and hit ctrl+s. Assuming they have this plugin added to their KeePass already, it will now prompt to sync the KeePass database. Have them go through the wizard and have them log in with their own OneDrive credentials. In the OneDrive location picker, they should see the folder shared by you marked with a blue arrow to indicate it's a linked folder from another OneDrive. Doubleclick on the KeePass file inside that linked folder and their version of the KeePass database from there on will sync with the shared location.

### KeePass OneDrive Sync doesn't work through my proxy! ###

 See the Proxy Issues section in [Troubleshooting](./Troubleshooting.md)

### I don't understand the OneDrive location dialog. What should I enter in the textbox? ###

I have to admit it was a rather vague dialog screen. As from version 1.5.0.0 this dialog has been replaced with an easy to use OneDrive picker which should resolve all your issues. Be sure to check out the extra options under the right click of your mouse when being in the picker.

### Can you make a version that works on non Windows machines? ###

No I cannot.

### Can you make a version that works with an older .NET Framework version than 4.5? ###

No I will not. Lots of functionality inside the plugin depends on the async development patterns introduced in the .NET 4.5 framework. I will not take the extreme efforts to use alternative approaches for this. Just install .NET 4.5 or later. If you can't because you're still on Windows XP: dude, upgrade your operating system!

### Why do I manually have to download the KeePass database first from OneDrive? ###

You don't have to anymore. With version 1.6.0.0 I've introduced the option under File -> Open -> Open from OneDrive to directly open the KeePass from OneDrive and set it up for synchronization locally with just a few steps.

### What happens to my KeePass database if multiple changes on multiple devices have been made? ###

When triggering a save (ctrl+s) or opening a KeePass database, the plugin will automatically verify against OneDrive Consumer / OneDrive for Business if the file hosted there has been updated since the last time that specific client downloaded it. If not, it will not do anything. If so, it will download a copy of that KeePass database to a temporary location on your harddrive and merge all changes from the downloaded copy with your local KeePass database. So changes in both the version stored online as well as in your local copy will be retained. Not at any time will one overwrite the other. This is ideal in situations where you're using KeePass on clients that are not always online (i.e. laptops). As soon as you get online and trigger a sync, everything will be merged again. Once the changes are merged, an updated copy of the KeePass database will automatically be uploaded to OneDrive / OneDrive for Business again so your other devices can grab the updated copy.

### I reset my OneDrive password and now my KeePass sync fails, how do I fix this? ###

It is by design that when you reset your OneDrive (Microsoft Account) password, all active refresh tokens will be invalidated. This is a security measure as the reason for changing the password could be that somebody gained access to it. In this scenario your KeePass sync will stop working. You can easily resolve this by going Tools -> OneDriveSync Options -> delete the entry with the database you're having problems with. This will not delete the KeePass file, just the configuration for the plugin for it. Now if you save your KeePass database again (ctrl+s) you will receive the wizard again to set up your sync. After going through this again all should work well again.

### Other questions ###

Feel free to e-mail me at mail@koenzomers.nl
