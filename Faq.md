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

### Other questions ###

Feel free to e-mail me at mail@koenzomers.nl