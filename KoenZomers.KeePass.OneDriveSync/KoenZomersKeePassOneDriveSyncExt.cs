using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using KeePass.Forms;
using KeePass.Plugins;
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
            //Host.MainWindow.FileSaved += MainWindowOnFileSaved;

            //var menu = Host.MainWindow.ToolsMenu.DropDownItems;
            //menu.Add(new ToolStripSeparator());
            //var menuItem = new ToolStripMenuItem("KeeOneDrive Options");
            //menuItem.Click += MenuOptionsOnClick;
            //menu.Add(menuItem);

            return true;
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
                MessageBox.Show("Failed to connect to OneDrive");
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
                MessageBox.Show("Database does not exist yet on OneDrive");
            }
            else
            {
                // Calculate the SHA1 hash of the local KeePass database
                var fileHash = Utilities.GetDatabaseFileHash(localKeePassDatabasePath);

                if (oneDriveItem.File.Hashes.Sha1.Equals(fileHash))
                {
                    MessageBox.Show("Databases are in sync!", "OneDrive Sync", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Sync the database
                MessageBox.Show("Going to sync");
            }

            //try
            //{

            //    downloadDatabase = Task<OneDriveIo.DownloadInformation>.Factory.StartNew(() =>
            //    {
            //        var config = Configuration.PasswordDatabases[fileOpenedEventArgs.Database.IOConnectionInfo.Path];
            //        var session = new LiveSession(OneDriveClientId, OneDriveClientSecret);
            //        session.Open(config.RefreshToken);
            //        var liveApi = new LiveApi(session);
            //        return OneDriveIo.DownloadIfUpdated(liveApi, config, Host.MainWindow.SetStatusEx);
            //    });

            //    if (downloadDatabase == null)
            //    {
            //        return;
            //    }

            //    var downloadResult = downloadDatabase.Result;

            //    if (downloadResult.FileName == null)
            //    {
            //        return;
            //    }

            //    SyncDatabase(downloadResult.FileName);
            //    var remoteUpdateTime = DateTime.Parse(downloadResult.RemoteItem.UpdatedTime);

            //    if (new FileInfo(downloadResult.FileName).LastWriteTimeUtc != remoteUpdateTime)
            //    {
            //        Task.Factory.StartNew(() =>
            //            UpdateRemoteFile(downloadResult.Api, downloadResult.FileName, downloadResult.RemoteItem));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Host.MainWindow.SetStatusEx("Error syncing database: " + ex.Message);
            //}
            //finally
            //{
            //    Monitor.Exit(lockObject);
            //}

            //Task.Factory.StartNew(Sync, Configuration.PasswordDatabases[fileOpenedEventArgs.Database.IOConnectionInfo.Path]);
        }
    }
}
