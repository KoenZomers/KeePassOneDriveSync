using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using KeePass.DataExchange;
using KeePass.Forms;
using KeePass.Plugins;
using KeePassLib;
using KeePassLib.Serialization;
using KoenZomers.KeePass.OneDriveSync;

namespace KoenZomersKeePassOneDriveSync
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>http://keepass.info/help/v2_dev/plg_index.html</remarks>
    public class KoenZomersKeePassOneDriveSyncExt : Plugin
    {
        #region Constants

        /// <summary>
        /// Client ID to use for communication with the OneDrive API
        /// </summary>
        private const string OneDriveClientId = "000000004813F265";

        /// <summary>
        /// Client Secret to use for communication with the OneDrive API
        /// </summary>
        private const string OneDriveClientSecret = "GXpsZ6DX4AaNjRWFovMLIp1xS-FpbPgO";

        #endregion

        public static IPluginHost Host;

        /// <summary>
        /// Returns the URL where KeePass can check for updates of this plugin
        /// </summary>
        public override string UpdateUrl
        {
            get { return "http://services.zomers.eu/KeeOneDrive/VersionCheck.txt"; }
        }

        public override bool Initialize(IPluginHost pluginHost)
        {
            Host = pluginHost;
            //Configuration.Load();

            //Host.MainWindow.Shown += MainWindowOnShown;
            //Host.MainWindow.Activated += MainWindowOnShown;
            Host.MainWindow.FileOpened += OnKeePassDatabaseOpened;
            Host.MainWindow.FileSaved += MainWindowOnFileSaved;


            //Host.MainWindow. += MenuOpenFromOneDriveOnClick;
            //var menu = Host.MainWindow.ToolsMenu.DropDownItems;
            //menu.Add(new ToolStripSeparator());
            //var menuItem = new ToolStripMenuItem("KeeOneDrive Options");
            //menuItem.Click += MenuOptionsOnClick;
            //menu.Add(menuItem);

            return true;
        }

        private void MainWindowOnFileSaved(object sender, EventArgs e)
        {

        }

        private void MenuOptionsOnClick(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Triggered when a KeePass database is being opened
        /// </summary>
        private void OnKeePassDatabaseOpened(object sender, FileOpenedEventArgs fileOpenedEventArgs)
        {
            CheckForNewerDatabase(fileOpenedEventArgs.Database.IOConnectionInfo.Path);
        }

        /// <summary>
        /// Checks if a newer database exists on OneDrive compared to the locally opened version
        /// </summary>
        /// <param name="localKeePassDatabasePath">Full path to where the KeePass database resides locally</param>
        private static async void CheckForNewerDatabase(string localKeePassDatabasePath)
        {
            // Retrieve the KeePassOneDriveSync settings for the database that got opened
            var databaseConfig = Configuration.GetPasswordDatabaseConfiguration(localKeePassDatabasePath);

            // Check if this database explicitly does not allow to be synced with OneDrive
            if (databaseConfig.DoNotSync)
            {
                return;
            }

            // Check if the current database is syned with OneDrive
            if (string.IsNullOrEmpty(databaseConfig.RemoteDatabasePath))
            {
                // Current database is not being syned with OneDrive, ask if it should be synced
                var oneDriveAskToStartSyncingForm = new OneDriveAskToStartSyncing(databaseConfig);
                var result = oneDriveAskToStartSyncingForm.ShowDialog();
                
                if (result != DialogResult.OK || oneDriveAskToStartSyncingForm.NotNowRadio.Checked)
                {                    
                    return;
                }
            }

            // Connect to OneDrive
            var oneDriveApi = await Utilities.GetOneDriveApi(databaseConfig, OneDriveClientId, OneDriveClientSecret);

            if (oneDriveApi == null)
            {
                UpdateStatus("Failed to connect to OneDrive");
                return;
            }

            // Check if we have a location on OneDrive to sync with
            if (string.IsNullOrEmpty(databaseConfig.RemoteDatabasePath))
            {
                // Have the user enter a location for the database on OneDrive 
                var oneDriveRemoteLocationForm = new OneDriveRemoteLocation(databaseConfig);
                var result = oneDriveRemoteLocationForm.ShowDialog();
                if (result != DialogResult.OK || string.IsNullOrWhiteSpace(oneDriveRemoteLocationForm.OneDriveRemotePathTextBox.Text))
                {
                    return;
                }

                databaseConfig.RemoteDatabasePath = oneDriveRemoteLocationForm.OneDriveRemotePathTextBox.Text;
            }

            // Retrieve the KeePass database from OneDrive
            var oneDriveItem = await oneDriveApi.GetItem(databaseConfig.RemoteDatabasePath);

            if (oneDriveItem == null)
            {
                // KeePass database not found on OneDrive
                UpdateStatus("Database does not exist yet on OneDrive");
                return;
            }
            
            // Calculate the SHA1 hash of the local KeePass database
            var fileHash = Utilities.GetDatabaseFileHash(localKeePassDatabasePath);

            if (oneDriveItem.File.Hashes.Sha1.Equals(fileHash))
            {
                UpdateStatus("Databases are in sync");
                return;
            }

            // Download the database from OneDrive
            UpdateStatus("Downloading KeePass database from OneDrive");

            var temporaryKeePassDatabasePath = Path.GetTempFileName();
            var downloadSuccessful = await oneDriveApi.DownloadItem(oneDriveItem, temporaryKeePassDatabasePath);

            if (!downloadSuccessful)
            {
                UpdateStatus("Failed to download the KeePass database from OneDrive");
                return;
            }

            // Sync database
            UpdateStatus("KeePass database downloaded, going to sync");
            
            var connectionInfo = IOConnectionInfo.FromPath(temporaryKeePassDatabasePath);
            var formatter = Host.FileFormatPool.Find("KeePass KDBX (2.x)");

            var importSuccessful = ImportUtil.Import(Host.Database, formatter, new[] { connectionInfo }, true, Host.MainWindow, false, Host.MainWindow);

            if (!importSuccessful.GetValueOrDefault(false))
            {
                UpdateStatus("Failed to synchronize the KeePass databases");
                return;
            }

            // Upload the synced database
            UpdateStatus("Uploading the new KeePass database to OneDrive");
            var uploadResult = await oneDriveApi.UploadFile(temporaryKeePassDatabasePath, oneDriveItem);

            if (uploadResult == null)
            {
                UpdateStatus("Failed to upload the KeePass database");
                return;
            }

            UpdateStatus("KeePass database has successfully been synchronized and uploaded");
        }

        /// <summary>
        /// Displays a status message on the bottom bar
        /// </summary>
        /// <param name="message">Message to show</param>
        private static void UpdateStatus(string message)
        {
            Host.MainWindow.SetStatusEx(message);
        }
    }
}
