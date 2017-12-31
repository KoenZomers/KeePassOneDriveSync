# KeePass OneDrive Sync

## Troubleshooting ##

### Known issues ###

The following issues are known issues:

- Removing custom icons and then syncing the database again will return the removed icons. This is a defect in KeePass itself and not something I can fix.
- Removing history items of an entry in KeePass and then syncing the database again will return the removed history entries. This is also a defect in KeePass itself and not something I can fix.

If the issue you're facing is not in the list above, perform the following steps:

If you're receiving an error when starting KeePass after placing the PLGX or DLL files inside your KeePass folder, follow these steps:

1. Ensure you have the very latest version of the plugin as available here on GitHub
2. Ensure you are using the latest version of KeePass
3. Ensure you have placed EITHER the PLGX OR the DLL files inside your KeePass its Plugins subfolder, not both the PLGX and the DLL files
4. If the problem still occurs, please run the following command:

C:\Program Files (x86)\KeePass Password Safe 2\KeePass.exe --debug

It should show you a dialog window with a path to a temporarily created file. Please e-mail me this file at koen@zomers.eu and I'll try to figure out why it failed.

If you're receiving an error after KeePass has started and you're setting up a sync connection or it is syncing with your OneDrive, please first try it again as it may be caused by a network glitch. Just hit continue in the error dialog. If it keeps happening, send me the error details and explain what you were trying to do when the error occurs.

Never mess with the tokens stored in the Windows Credential Manager for KeePass yourself. Just delete the entry in KeePass -> Tools -> OneDriveSync Options if you wish to change or update the credentials.

### Proxy Issues ###

Every now and then I get e-mails from people that state that KeePass OneDrive Sync won't work through their proxy setup. This plugin takes over the proxy settings you define inside KeePass. You can find these in KeePass under Tools > Options > Advanced tab > Proxy at the bottom right. Make sure these settings are correct for your environment. If taking the default system proxy and credentials don't work, try if it does work if you explicitly configure them in KeePass.

### Exceptions when opening KeePass OneDrive Sync Properties

If you get an exception thrown by the plugin when you try to open the KeePass OneDriveSync properties dialog, your config may have gotten corrupted. Close KeePass. Open the following file in notepad:

C:\Users\\\<your username>\AppData\Roaming\KeePass\KeePass.config.xml

Look for a section \<Custom>\\\<Item>\\\<Key>KeeOneDrive\</Key>. Remove this whole \<Item>...\</Item> section from the file and save it. Restart KeePass and reconfigure your KeePass OneDriveSync connections.