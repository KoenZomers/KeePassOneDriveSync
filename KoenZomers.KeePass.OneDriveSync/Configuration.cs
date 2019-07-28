using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using KoenZomersKeePassOneDriveSync;
using Newtonsoft.Json;

namespace KoenZomers.KeePass.OneDriveSync
{
    /// <summary>
    /// Plugin configuration class. Contains functions to serialize/deserialize to/from JSON.
    /// </summary>
    [DataContract]
    public class Configuration : ICloneable
    {
        #region Constants

        /// <summary>
        /// Name under which to store these settings in the KeePass configuration store
        /// </summary>
        private const string ConfigurationKey = "KeeOneDrive";

        #endregion

        #region Non serializable Properties

        /// <summary>
        /// Dictionary with configuration settings for all password databases. Key is the local database path, value is the configuration belonging to it.
        /// </summary>
        private static IDictionary<string, Configuration> PasswordDatabases = new Dictionary<string, Configuration>();

        /// <summary>
        /// Boolean indicating if the syncing of this database is allowed
        /// </summary>
        public bool SyncingEnabled = true;

        /// <summary>
        /// The KeePass database to which these settings belong
        /// </summary>
        public KeePassLib.PwDatabase KeePassDatabase { get; set; }

        /// <summary>
        /// Boolean indicating if this database is currently synchronizing
        /// </summary>
        public bool IsCurrentlySyncing { get; set; }

        #endregion

        #region Serializable Properties

        /// <summary>
        /// Gets or sets refresh token that can be used to get an Access Token for OneDrive access. Will be set to ClientId;ClientSecret in the case of a SharePoint configuration.
        /// </summary>
        [DataMember]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the location where the refresh token will be stored
        /// </summary>
        [DataMember]
        public Enums.OneDriveRefreshTokenStorage? RefreshTokenStorage { get; set; } 

        /// <summary>
        /// Gets or sets the name of the OneDrive the KeePass database is synchronized with. Will be set to the SharePoint site title in the scenario of a SharePoint configuration.
        /// </summary>
        [DataMember]
        public string OneDriveName { get; set; }

        /// <summary>
        /// Gets or sets database file path on OneDrive relative to the user. Will be set to the site URL in the case of a SharePoint configuration.
        /// </summary>
        [DataMember]
        public string RemoteDatabasePath { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the folder on OneDrive in which the file is stored. Will be set to the Document Library server relative URL in the case of a SharePoint configuration.
        /// </summary>
        [DataMember]
        public string RemoteFolderId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the drive on OneDrive in which the file is stored. If NULL then the drive of the current user will be used.
        /// </summary>
        [DataMember]
        public string RemoteDriveId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the file on OneDrive
        /// </summary>
        [DataMember]
        public string RemoteItemId { get; set; }

        /// <summary>
        /// Gets or sets the filename under which the database is stored on OneDrive
        /// </summary>
        [DataMember]
        public string RemoteFileName { get; set; }

        /// <summary>
        /// Gets or sets a boolean to indicate if the database should be synced with OneDrive
        /// </summary>
        [DataMember]
        public bool DoNotSync { get; set; }

        /// <summary>
        /// The SHA1 hash of the local KeePass database
        /// </summary>
        [DataMember]
        public string LocalFileHash { get; set; }

        /// <summary>
        /// The ETag of the KeePass database on OneDrive
        /// </summary>
        [DataMember]
        public string ETag { get; set; }

        /// <summary>
        /// Date and time at which the database last synced with OneDrive
        /// </summary>
        [DataMember]
        public DateTime? LastSyncedAt { get; set; }

        /// <summary>
        /// Date and time at which the database has last been compared with its equivallent on OneDrive
        /// </summary>
        [DataMember]
        public DateTime? LastCheckedAt { get; set; }

        /// <summary>
        /// Type of cloud storage used for storing the database
        /// </summary>
        [DataMember]
        public Enums.CloudStorageType? CloudStorageType { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the KeePass database configuration for KeePassOneDriveSync for the KeePass database of which the local path is provided
        /// </summary>
        /// <param name="localPasswordDatabasePath">Full path to where the KeePass database resides locally</param>
        /// <returns>KeePassOneDriveSync settings for the provided database</returns>
        public static Configuration GetPasswordDatabaseConfiguration(string localPasswordDatabasePath)
        {
            localPasswordDatabasePath = NormalizePath(localPasswordDatabasePath);

            if (!PasswordDatabases.ContainsKey(localPasswordDatabasePath))
            {
                PasswordDatabases.Add(new KeyValuePair<string, Configuration>(localPasswordDatabasePath, new Configuration()));
            }
            return PasswordDatabases[localPasswordDatabasePath];
        }

        /// <summary>
        /// Loads the configuration stored in KeePass
        /// </summary>
        public static void Load()
        {
            // Retrieve the stored configuration from KeePass
            var value = KoenZomersKeePassOneDriveSyncExt.Host.CustomConfig.GetString(ConfigurationKey, null);

            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            try
            {
                // Convert the retrieved JSON to a typed entity
                PasswordDatabases = JsonConvert.DeserializeObject<Dictionary<string, Configuration>>(value);
            }
            catch (JsonSerializationException)
            {                
                MessageBox.Show("Unable to parse the plugin configuration for the KeePass OneDriveSync plugin. If this happens again, please let me know. Sorry for the inconvinience. Koen Zomers <koen@zomers.eu>", "KeePass OneDriveSync Plugin", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                // Reset the configuration so at least it works again next time, all be it that all configuration will be lost
                PasswordDatabases = new Dictionary<string, Configuration>();
                Save();
            }

            // Get all database configurations which have their OneDrive Refresh Token stored in the Windows Credential Manager and retrieve them
            var windowsCredentialManagerDatabaseConfigs = PasswordDatabases.Where(pwdDb => pwdDb.Value.RefreshTokenStorage == Enums.OneDriveRefreshTokenStorage.WindowsCredentialManager);
            foreach (var windowsCredentialManagerDatabaseConfig in windowsCredentialManagerDatabaseConfigs)
            {
                windowsCredentialManagerDatabaseConfig.Value.RefreshToken = Utilities.GetRefreshTokenFromWindowsCredentialManager(windowsCredentialManagerDatabaseConfig.Key);
            }

            // Decrypt all database configurations which have their OneDrive Refresh Token stored encrypted in the config file
            var encryptedDatabaseConfigs = PasswordDatabases.Where(pwdDb => pwdDb.Value.RefreshTokenStorage == Enums.OneDriveRefreshTokenStorage.DiskEncrypted);
            foreach (var encryptedDatabaseConfig in encryptedDatabaseConfigs)
            {
                encryptedDatabaseConfig.Value.RefreshToken = Utilities.Unprotect(encryptedDatabaseConfig.Value.RefreshToken);
            }
        }

        /// <summary>
        /// Saves the current configuration
        /// </summary>
        public static void Save()
        {
            // Create a new dictionary with the information to store in KeePass.config.xml
            var passwordDatabasesForStoring = new Dictionary<string, Configuration>();

            // Loop through the entries to store
            foreach (var passwordDatabase in PasswordDatabases)
            {
                switch (passwordDatabase.Value.RefreshTokenStorage)
                {
                    case Enums.OneDriveRefreshTokenStorage.Disk:
                    case Enums.OneDriveRefreshTokenStorage.DiskEncrypted:
                        // Enforce encryption of tokens previously stored in plain text
                        passwordDatabase.Value.RefreshTokenStorage = Enums.OneDriveRefreshTokenStorage.DiskEncrypted;

                        // Refresh token will be stored encrypted on disk, we create a copy of the configuration and encrypt the refresh token
                        var diskConfiguration = (Configuration)passwordDatabase.Value.Clone();
                        diskConfiguration.RefreshToken = Utilities.Protect(diskConfiguration.RefreshToken);
                        passwordDatabasesForStoring.Add(passwordDatabase.Key, diskConfiguration);
                        break;

                    case Enums.OneDriveRefreshTokenStorage.KeePassDatabase:
                    case Enums.OneDriveRefreshTokenStorage.WindowsCredentialManager:
                        // Refresh token will not be stored on disk, we create a copy of the configuration and remove the refresh token from it so it will not be stored on disk
                        var tempConfiguration = (Configuration) passwordDatabase.Value.Clone();
                        tempConfiguration.RefreshToken = null;
                        passwordDatabasesForStoring.Add(passwordDatabase.Key, tempConfiguration);

                        if (passwordDatabase.Value.RefreshTokenStorage == Enums.OneDriveRefreshTokenStorage.KeePassDatabase && passwordDatabase.Value.KeePassDatabase != null && !string.IsNullOrEmpty(passwordDatabase.Value.RefreshToken))
                        {
                            Utilities.SaveRefreshTokenInKeePassDatabase(passwordDatabase.Value.KeePassDatabase, passwordDatabase.Value.RefreshToken);
                        }
                        if (passwordDatabase.Value.RefreshTokenStorage == Enums.OneDriveRefreshTokenStorage.WindowsCredentialManager)
                        {
                            Utilities.SaveRefreshTokenInWindowsCredentialManager(passwordDatabase.Key, passwordDatabase.Value.RefreshToken);
                        }
                        break;
                    default:
                        // Hit when user selected the do not sync database option. In that case just store the config on disk as it doesn't contain any tokens.
                        passwordDatabasesForStoring.Add(passwordDatabase.Key, passwordDatabase.Value);
                        break;
                }
            }

            // Serialize the configuration to JSON
            var json = JsonConvert.SerializeObject(passwordDatabasesForStoring);

            // Store the configuration in KeePass.config.xml
            KoenZomersKeePassOneDriveSyncExt.Host.CustomConfig.SetString(ConfigurationKey, json);
        }

        /// <summary>
        /// Returns a copy of the current instance
        /// </summary>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Deletes the complete configuration of the KeePass database on the provided local path
        /// </summary>
        /// <param name="localPasswordDatabasePath">Full local path to a KeePass database of which to delete the configuration</param>
        public static void DeleteConfig(string localPasswordDatabasePath)
        {
            localPasswordDatabasePath = NormalizePath(localPasswordDatabasePath);

            // Verify if we have configuration available of a KeePass database stored on the provided full local path
            if (!PasswordDatabases.ContainsKey(localPasswordDatabasePath)) return;
            
            // Retrieve the configuration we have available about the KeePass database
            var config = PasswordDatabases[localPasswordDatabasePath];
            
            // Take cleanup actions based on where the OneDrive Refresh Token is stored
            switch (config.RefreshTokenStorage)
            {
                case Enums.OneDriveRefreshTokenStorage.Disk:
                case Enums.OneDriveRefreshTokenStorage.DiskEncrypted:
                    // No action required as it will be removed as part of removing the complete configuration
                    break;

                case Enums.OneDriveRefreshTokenStorage.KeePassDatabase:
                    // There's no way to remove it from the KeePass database without having the database open, so we'll have to leave it there
                    break;

                case Enums.OneDriveRefreshTokenStorage.WindowsCredentialManager:
                    Utilities.DeleteRefreshTokenFromWindowsCredentialManager(localPasswordDatabasePath);
                    break;
            }

            // Remove the configuration we have available
            PasswordDatabases.Remove(localPasswordDatabasePath);

            // Initiate a save to write the results
            Save();
        }

        /// <summary>
        /// Get the full PasswordDatabases object list, used by the configuration form
        /// </summary>
        /// <returns></returns>
        internal static IDictionary<string, Configuration> GetAllConfigurations()
        {
            return PasswordDatabases;
        }

        /// <summary>
        /// Boolean to indicate if some process is still running that we need to wait for before we shut down KeePass
        /// </summary>
        internal static bool IsSomethingStillRunning
        {
            get
            {
                return PasswordDatabases.Any(db => db.Value.IsCurrentlySyncing);
            }
        }

        /// <summary>
        /// Normalize database path - if located under the KeePass folder (portable install) then use a relative path, otherwise use the original (absolute)
        /// </summary>
        /// <param name="localPasswordDatabasePath">Full path to a KeePass database</param>
        /// <returns>If the path resides under the folder where KeePass runs from, it will return a relative path, otherwise the full path will be returned</returns>
        private static string NormalizePath(string localPasswordDatabasePath)
        {
            if (localPasswordDatabasePath.StartsWith(AppDomain.CurrentDomain.BaseDirectory))
            {
                localPasswordDatabasePath = localPasswordDatabasePath.Remove(0, AppDomain.CurrentDomain.BaseDirectory.Length);
            }

            return localPasswordDatabasePath;
        }

        #endregion
    }
}

