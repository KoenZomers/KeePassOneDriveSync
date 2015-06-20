using System;
using System.Windows.Forms;
using KeePass.Forms;
using KeePass.Plugins;
using KoenZomers.KeePass.OneDriveSync;

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
            await KeePass.SyncDatabase(fileSavedEventArgs.Database.IOConnectionInfo.Path, KeePass.UpdateStatus);
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
            await KeePass.SyncDatabase(fileOpenedEventArgs.Database.IOConnectionInfo.Path, KeePass.UpdateStatus);
        }
    }
}
