# KeePass OneDrive Sync

## Troubleshooting ##

If you're receiving an error when starting KeePass after placing the PLGX or DLL files inside your KeePass folder, follow these steps:

1. Ensure you have the very latest version of the plugin as available here on GitHub
2. Ensure you are using the latest version of KeePass
3. Ensure you have placed EITHER the PLGX OR the DLL files inside your KeePass folder, not both the PLGX and the DLL files
4. If the problem still occurs, please run the following command:

C:\Program Files (x86)\KeePass Password Safe 2\KeePass.exe --saveplgxcr --debug

It should show you a dialog window with a path to a temporarily created file. Please e-mail me this file at mail@koenzomers.nl and I'll try to figure out why it failed.

If you're receiving an error after KeePass has started and you're setting up a sync connection or it is syncing with your OneDrive, please first try it again as it may be caused by a network glitch. Just hit continue in the error dialog. If it keeps happening, send me the error details and explain what you were trying to do when the error occurs.

### Proxy Issues ###

Every now and then I get e-mails from people that state that KeePass OneDrive Sync won't work through their proxy setup. This plugin takes over the proxy settings you define inside KeePass. You can find these in KeePass under Tools > Options > Advanced tab > Proxy at the bottom right. Make sure these settings are correct for your environment. It is nearly impossible for me unfortunately to fix issues with proxies as every proxy behaves different and I don't have access to a proxy to test against. If you do want me to try to figure out the issue, capture a trace using Fiddler (www.fiddler2.com) while trying to sync using your proxy and send me the results. If we're really lucky, that may give away what went wrong and allow me to fix it, but don't get your hopes up.