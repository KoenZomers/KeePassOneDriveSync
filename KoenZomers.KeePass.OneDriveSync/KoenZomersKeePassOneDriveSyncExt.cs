using System;
using System.Windows.Forms;
using KeePass.Forms;
using KeePass.Plugins;
using KoenZomers.KeePass.OneDriveSync;
using KoenZomers.KeePass.OneDriveSync.Enums;

namespace KoenZomersKeePassOneDriveSync
{
    /// <summary>
    /// Plugin for KeePass to allow synchronization with OneDrive
    /// </summary>
    /// <remarks>KeePass SDK documentation: http://keepass.info/help/v2_dev/plg_index.html</remarks>
    public class KoenZomersKeePassOneDriveSyncExt : Plugin
    {
        #region Constants

        /// <summary>
        /// Client ID to use for communication with the OneDrive API
        /// </summary>
        internal const string OneDriveClientId = "000000004813F265";

        /// <summary>
        /// Client Secret to use for communication with the OneDrive API
        /// </summary>
        internal const string OneDriveClientSecret = "GXpsZ6DX4AaNjRWFovMLIp1xS-FpbPgO";

        #endregion

        #region Properties

        /// <summary>
        /// Reference to the KeePass instance
        /// </summary>
        public static IPluginHost Host;

        #endregion

        /// <summary>
        /// Returns the URL where KeePass can check for updates of this plugin
        /// </summary>
        public override string UpdateUrl
        {
            get { return "http://services.zomers.eu/KeeOneDrive/VersionCheck.txt"; }
        }

        /// <summary>
        /// Called when the Plugin is being loaded which happens on startup of KeePass
        /// </summary>
        /// <returns>True if the plugin loaded successfully, false if not</returns>
        public override bool Initialize(IPluginHost pluginHost)
        {
            Host = pluginHost;

            // Load the configuration
            Configuration.Load();

            // Bind to the events for loading and saving databases
            Host.MainWindow.FileOpened += OnKeePassDatabaseOpened;
            Host.MainWindow.FileSaved += MainWindowOnFileSaved;
            Host.MainWindow.FileClosed += OnKeePassDatabaseClosed;

            // Add the menu option for configuration under Tools
            var menu = Host.MainWindow.ToolsMenu.DropDownItems;
            menu.Add(new ToolStripSeparator());
            var menuItem = new ToolStripMenuItem("OneDriveSync Options");
            menuItem.Click += MenuOptionsOnClick;
            menu.Add(menuItem);

            // Indicate that the plugin started successfully
            return true;
        }

        /// <summary>
        /// Triggered when a KeePass database is being saved
        /// </summary>
        private static async void MainWindowOnFileSaved(object sender, FileSavedEventArgs fileSavedEventArgs)
        {
            var config = Configuration.GetPasswordDatabaseConfiguration(fileSavedEventArgs.Database.IOConnectionInfo.Path);
            config.KeePassDatabase = fileSavedEventArgs.Database;

            // Check if we should sync this database
            if (config.DoNotSync) return;

            // Make sure it's not a remote database on i.e. an FTP or website
            if (!fileSavedEventArgs.Database.IOConnectionInfo.IsLocalFile())
            {
                MessageBox.Show("KeePass OneDriveSync does not support synchronizing databases from remote locations and will therefore not be available for this database", "KeePass OneDriveSync", MessageBoxButtons.OK, MessageBoxIcon.Information);

                config.DoNotSync = true;
                Configuration.Save();
                return;
            }

            await KeePass.SyncDatabase(fileSavedEventArgs.Database.IOConnectionInfo.Path, KeePass.UpdateStatus);
            
            // If the OneDrive Refresh Token is stored in the KeePass database, we must trigger a save of the database here so to ensure that the actual value gets saved into the KDBX
            if (config.RefreshTokenStorage == OneDriveRefreshTokenStorage.KeePassDatabase)
            {
                fileSavedEventArgs.Database.Save(null);
            }
        }

        /// <summary>
        /// Triggered when clicking on the OneDriveSync menu item under Tools
        /// </summary>
        private static void MenuOptionsOnClick(object sender, EventArgs e)
        {
            var oneDriveConfigForm = new OneDriveConfigForm();
            oneDriveConfigForm.ShowDialog();
        }

        /// <summary>
        /// Triggered when a KeePass database is being opened
        /// </summary>
        private async static void OnKeePassDatabaseOpened(object sender, FileOpenedEventArgs fileOpenedEventArgs)
        {
            // Add the KeePass database instance to the already available configuration
            var config = Configuration.GetPasswordDatabaseConfiguration(fileOpenedEventArgs.Database.IOConnectionInfo.Path);
            config.KeePassDatabase = fileOpenedEventArgs.Database;

            // Check if we should sync this database
            if (config.DoNotSync || !fileOpenedEventArgs.Database.IOConnectionInfo.IsLocalFile()) return;

            // Check if the database configuration of the opened KeePass database is set to retrieve the OneDrive Refresh Token from the KeePass database itself
            if (config.RefreshTokenStorage == OneDriveRefreshTokenStorage.KeePassDatabase)
            {
                // Retrieve the OneDrive Refresh Token from the KeePass database that is being opened
                config.RefreshToken = Utilities.GetRefreshTokenFromKeePassDatabase(fileOpenedEventArgs.Database);
            }

            await KeePass.SyncDatabase(fileOpenedEventArgs.Database.IOConnectionInfo.Path, KeePass.UpdateStatus);

            // If the OneDrive Refresh Token is stored in the KeePass database, we must trigger a save of the database here so to ensure that the actual value gets saved into the KDBX
            if (config.RefreshTokenStorage == OneDriveRefreshTokenStorage.KeePassDatabase)
            {
                fileOpenedEventArgs.Database.Save(null);
            }
        }

        /// <summary>
        /// Triggered when a KeePass database has been closed
        /// </summary>
        private static void OnKeePassDatabaseClosed(object sender, FileClosedEventArgs fileClosedEventArgs)
        {
            // Remove the KeePass database instance from the already available configuration
            var config = Configuration.GetPasswordDatabaseConfiguration(fileClosedEventArgs.IOConnectionInfo.Path);
            config.KeePassDatabase = null;
        }
    }
}
