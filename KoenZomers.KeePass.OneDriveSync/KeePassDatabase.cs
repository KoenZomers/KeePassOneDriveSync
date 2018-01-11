using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeePass.DataExchange;
using KeePassLib.Serialization;
using KoenZomers.KeePass.OneDriveSync;
using KoenZomers.KeePass.OneDriveSync.Enums;

namespace KoenZomersKeePassOneDriveSync
{
    /// <summary>
    /// Methods that regard the KeePass database
    /// </summary>
    public static class KeePassDatabase
    {
        /// <summary>
        /// Allows a KeePass database to be opened directly from a cloud service
        /// </summary>
        /// <param name="updateStatus">Method to call to update the status</param> 
        public static async Task OpenDatabaseFromCloudService(Action<string> updateStatus)
        {
            // Ask which cloud service to connect to
            var cloudStorageProviderForm = new OneDriveCloudTypeForm {ExplanationText = "Choose the cloud service you wish to open the KeePass database from:"};
            cloudStorageProviderForm.ShowDialog();

            if (cloudStorageProviderForm.DialogResult != DialogResult.OK) return;

            // Create a new database configuration entry
            var databaseConfig = new Configuration
            {
                CloudStorageType = cloudStorageProviderForm.ChosenCloudStorageType
            };

            // Verify the config object is ready to be used
            if (!databaseConfig.CloudStorageType.HasValue)
            {
                UpdateStatus("Invalid cloud storage provider chosen");
                return;
            }

            string localKeePassDatabasePath = null;
            switch (databaseConfig.CloudStorageType)
            {
                case CloudStorageType.MicrosoftGraph:
                case CloudStorageType.OneDriveConsumer:
                case CloudStorageType.OneDriveForBusiness:
                    localKeePassDatabasePath = await Providers.OneDriveProvider.OpenFromOneDriveCloudProvider(databaseConfig, updateStatus);
                    break;

                case CloudStorageType.SharePoint:
                    localKeePassDatabasePath = await Providers.SharePointProvider.OpenFromSharePoint(databaseConfig, updateStatus);
                    break;
            }
            
            if(localKeePassDatabasePath == null)
            {
                return;
            }

            // Register the KeePass database sync information
            UpdateStatus("Configuring KeePass database");
            databaseConfig.LocalFileHash = Utilities.GetDatabaseFileHash(localKeePassDatabasePath);
            databaseConfig.LastSyncedAt = DateTime.Now;
            databaseConfig.LastCheckedAt = DateTime.Now;

            Configuration.PasswordDatabases[localKeePassDatabasePath] = databaseConfig;
            Configuration.Save();

            UpdateStatus("Opening KeePass database");

            // Open the KeePass database
            var databaseFile = IOConnectionInfo.FromPath(localKeePassDatabasePath);
            KoenZomersKeePassOneDriveSyncExt.Host.MainWindow.OpenDatabase(databaseFile, null, false);

            UpdateStatus("KeePass database is ready to be used");
        }

        /// <summary>
        /// Checks if a newer database exists on OneDrive compared to the locally opened version and syncs them if so
        /// </summary>
        /// <param name="localKeePassDatabasePath">Full path or relative path from the KeePass executable folder to where the KeePass database resides locally</param>
        /// <param name="updateStatus">Method to call to update the status</param>
        /// <param name="forceSync">If True, no attempts will be made to validate if the local copy differs from the remote copy and it will always sync. If False, it will try to validate if there are any changes and not sync if there are no changes.</param>
        public static async Task SyncDatabase(string localKeePassDatabasePath, Action<string> updateStatus, bool forceSync, Configuration databaseConfig)
        {
            try
            {
                // If something is already syncing, abort this attempt. This happens when the Import saves the resulting KeePass database. That by itself triggers another sync which doesn't have to go through the sync process as it just regards the temp database.
                if (KoenZomersKeePassOneDriveSyncExt.IsSomethingStillRunning)
                {
                    UpdateStatus("A synchronization is already in progress");
                    return;
                }

                // Set flag to block exiting KeePass until this is done
                KoenZomersKeePassOneDriveSyncExt.IsSomethingStillRunning = true;

                UpdateStatus("Starting database synchronization");

                // Retrieve the KeePassOneDriveSync settings
                if (databaseConfig == null)
                {
                    databaseConfig = Configuration.GetPasswordDatabaseConfiguration(localKeePassDatabasePath);
                }

                // Check if this database explicitly does not allow to be synced with OneDrive and if syncing is enabled on this database
                if (databaseConfig.DoNotSync || !databaseConfig.SyncingEnabled)
                {
                    // Set flag to allow exiting KeePass
                    KoenZomersKeePassOneDriveSyncExt.IsSomethingStillRunning = false;

                    return;
                }

                // Check if the current database is synced with OneDrive
                if (string.IsNullOrEmpty(databaseConfig.RemoteDatabasePath) && string.IsNullOrEmpty(databaseConfig.RemoteFolderId) && string.IsNullOrEmpty(databaseConfig.RemoteFileName))
                {
                    var cloudStorageType = AskIfShouldBeSynced(databaseConfig);

                    if(!cloudStorageType.HasValue)
                    {
                        // Set flag to allow exiting KeePass
                        KoenZomersKeePassOneDriveSyncExt.IsSomethingStillRunning = false;
                        return;
                    }

                    databaseConfig.CloudStorageType = cloudStorageType.Value;
                    Configuration.Save();
                }

                // Start the sync operation using one of the Cloud Storage Providers
                bool syncSuccessful = false;

                switch(databaseConfig.CloudStorageType)
                {
                    case CloudStorageType.MicrosoftGraph:
                    case CloudStorageType.OneDriveConsumer:
                    case CloudStorageType.OneDriveForBusiness:
                        syncSuccessful = await Providers.OneDriveProvider.SyncUsingOneDriveCloudProvider(databaseConfig, localKeePassDatabasePath, forceSync, updateStatus);
                        break;

                    case CloudStorageType.SharePoint:
                        syncSuccessful = await Providers.SharePointProvider.SyncUsingSharePointPlatform(databaseConfig, localKeePassDatabasePath, forceSync, updateStatus);
                        break;
                }

                if(!syncSuccessful)
                {
                    // Set flag to allow exiting KeePass
                    KoenZomersKeePassOneDriveSyncExt.IsSomethingStillRunning = false;
                    return;
                }

                // Update the KeePass UI so it shows the new entries
                KoenZomersKeePassOneDriveSyncExt.Host.MainWindow.UpdateUI(false, null, true, null, true, null, false);

                updateStatus("KeePass database has successfully been synchronized and uploaded");

                databaseConfig.LocalFileHash = Utilities.GetDatabaseFileHash(localKeePassDatabasePath);
                //databaseConfig.ETag = uploadResult.ETag;
                databaseConfig.LastSyncedAt = DateTime.Now;
                databaseConfig.LastCheckedAt = DateTime.Now;
                Configuration.Save();
            }
            catch(Exception e)
            {
                updateStatus(string.Concat("Failed to sync database: ", e.Message));
            }

            // Set flag to allow exiting KeePass
            KoenZomersKeePassOneDriveSyncExt.IsSomethingStillRunning = false;
        }

        /// <summary>
        /// Asks if the database should be synced and if yes with which storage provider
        /// </summary>
        /// <param name="databaseConfig">Database to sync for</param>
        /// <returns>Storage type to sync with or NULL if it should not be synced</returns>
        private static CloudStorageType? AskIfShouldBeSynced(Configuration databaseConfig)
        {
            // Current database is not being syned with OneDrive, ask if it should be synced
            var oneDriveAskToStartSyncingForm = new OneDriveAskToStartSyncingDialog(databaseConfig);
            if (oneDriveAskToStartSyncingForm.ShowDialog() != DialogResult.OK)
            {
                // Dialog has been canceled. Return NULL to indicate no choice has been made.
                return null;
            }

            // Ask which cloud service to connect to
            var oneDriveCloudTypeForm = new OneDriveCloudTypeForm { ExplanationText = "Choose the cloud service you wish to store the KeePass database on:" };
            if (oneDriveCloudTypeForm.ShowDialog() != DialogResult.OK)
            {
                // Dialog has been canceled. Return NULL to indicate no choice has been made.
                return null;
            }

            return oneDriveCloudTypeForm.ChosenCloudStorageType;
        }

        /// <summary>
        /// Merges the KeePass database at the provided path with the current database
        /// </summary>
        /// <param name="databaseConfig">Configuration of the database to merge the KeePass database with</param>
        /// <param name="tempDatabasePath">Path to the KeePass database to merge with the currently open database</param>
        /// <returns>True if successful, false if failed to merge</returns>
        public static bool MergeDatabases(Configuration databaseConfig, string tempDatabasePath)
        {
            var connectionInfo = IOConnectionInfo.FromPath(tempDatabasePath);
            var formatter = KoenZomersKeePassOneDriveSyncExt.Host.FileFormatPool.Find("KeePass KDBX (2.x)");

            // Temporarily switch off syncing for this database to avoid the import operation, which causes a save, to create and endless loop
            databaseConfig.SyncingEnabled = false;

            // Import the current database with the downloaded database. Import causes a one way sync, syncing would try to update both ends which won't work for OneDrive.
            var importSuccessful = ImportUtil.Import(KoenZomersKeePassOneDriveSyncExt.Host.Database, formatter, new[] { connectionInfo }, true, KoenZomersKeePassOneDriveSyncExt.Host.MainWindow, false, KoenZomersKeePassOneDriveSyncExt.Host.MainWindow);

            // Enable syncing of this database again
            databaseConfig.SyncingEnabled = true;

            // Remove the temporary database from the Most Recently Used (MRU) list in KeePass as its added automatically on the import action
            KoenZomersKeePassOneDriveSyncExt.Host.MainWindow.FileMruList.RemoveItem(tempDatabasePath);

            return importSuccessful.GetValueOrDefault(false);
        }

        /// <summary>
        /// Displays a status message on the bottom bar
        /// </summary>
        /// <param name="message">Message to show</param>
        public static void UpdateStatus(string message)
        {
            KoenZomersKeePassOneDriveSyncExt.Host.MainWindow.SetStatusEx(message);
        }
    }
}
