using System;
using System.Windows.Forms;
using KeePass.Forms;
using KeePass.Plugins;
using KoenZomers.KeePass.OneDriveSync;
using KoenZomers.KeePass.OneDriveSync.Enums;
using System.Threading.Tasks;
using System.Linq;

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
        /// Client ID to use for communication with the OneDrive Consumer API
        /// </summary>
        internal const string OneDriveConsumerClientId = "000000004813F265";

        /// <summary>
        /// Client Secret to use for communication with the OneDrive Consumer API
        /// </summary>
        internal const string OneDriveConsumerClientSecret = "GXpsZ6DX4AaNjRWFovMLIp1xS-FpbPgO";

        /// <summary>
        /// Client ID to use for communication with the OneDrive for Business API
        /// </summary>
        internal const string OneDriveForBusinessClientId = "c1b42b56-2aa9-4f01-ad76-11644ae1a859";

        /// <summary>
        /// Client Secret to use for communication with the OneDrive for Business API
        /// </summary>
        internal const string OneDriveForBusinessClientSecret = "ezb/qt5Iat0aE8S+ZWBtxhDtQLISEaPCkZ5agQv9MgM=";

        /// <summary>
        /// Application ID to use for communication with the Microsoft Graph API
        /// </summary>
        internal const string GraphApiApplicationId = "7bcec80a-2ffe-4713-b9ea-0150361c8209";

        #endregion

        #region Properties

        /// <summary>
        /// Reference to the KeePass instance
        /// </summary>
        public static IPluginHost Host;

        #endregion

        #region Fields

        /// <summary>
        /// Separator line in the tools menu
        /// </summary>
        private ToolStripSeparator _toolsMenuSeparator;

        /// <summary>
        /// Config option item in the Tools menu
        /// </summary>
        private ToolStripMenuItem _toolsMenuConfigMenuItem;

        /// <summary>
        /// Open from OneDrive option in File > Open menu
        /// </summary>
        private ToolStripMenuItem _fileOpenMenuItem;

        /// <summary>
        /// Offline OneDrive option in File menu
        /// </summary>
        private ToolStripMenuItem _fileOfflineMenuItem;


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

            // Enable TLS 1.2
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;

            // Load the configuration
            Configuration.Load();                       

            // Bind to the events for loading and saving databases
            Host.MainWindow.FileOpened += OnKeePassDatabaseOpened;
            Host.MainWindow.FileSaved += MainWindowOnFileSaved;
            Host.MainWindow.FileClosed += OnKeePassDatabaseClosed;
            Host.MainWindow.FileClosingPost += OnKeePassDatabaseClosing;

            // Add the menu option for configuration under Tools
            var toolsmenu = Host.MainWindow.ToolsMenu.DropDownItems;

            _toolsMenuSeparator = new ToolStripSeparator();
            _toolsMenuConfigMenuItem = new ToolStripMenuItem("OneDriveSync Options", Resources.OneDriveIcon);
            _toolsMenuConfigMenuItem.Click += MenuOptionsOnClick;
            toolsmenu.Add(_toolsMenuConfigMenuItem);

            // Add the menu option for configuration under File > Open
            var filemenu = Host.MainWindow.MainMenu.Items["m_menuFile"] as ToolStripMenuItem;
            if (filemenu != null)
            {
                var openmenu = filemenu.DropDownItems["m_menuFileOpen"] as ToolStripMenuItem;
                if (openmenu != null)
                {
                    _fileOpenMenuItem = new ToolStripMenuItem("Open from OneDrive...", Resources.OneDriveIcon)
                    {
                        ShortcutKeys = Keys.Control | Keys.Alt | Keys.O
                    };
                    _fileOpenMenuItem.Click += MenuFileOpenFromOneDriveOnClick;
                    openmenu.DropDownItems.Add(_fileOpenMenuItem);
                }

                // Add the menu option for Offline use under File
                var fileLockIndex = filemenu.DropDownItems.IndexOfKey("m_menuFileLock");
                _fileOfflineMenuItem = new ToolStripMenuItem("OneDriveSync is Online", Resources.OneDriveIcon)
                {
                    Checked = true
                };
                _fileOfflineMenuItem.Click += MenuFileOneDriveSyncOfflineOnClick;
                filemenu.DropDownItems.Insert(fileLockIndex != -1 ? fileLockIndex : filemenu.DropDownItems.Count - 1, _toolsMenuSeparator);
                filemenu.DropDownItems.Insert(fileLockIndex != -1 ? fileLockIndex : filemenu.DropDownItems.Count - 1, _fileOfflineMenuItem);
            }

            // Indicate that the plugin started successfully
            return true;
        }

        /// <summary>
        /// Triggered when the Plugin is being terminated
        /// </summary>
        public override void Terminate()
        {
            // Remove custom items from the menu as per recommendations on http://keepass.info/help/v2_dev/plg_index.html
            var optionsmenu = Host.MainWindow.ToolsMenu.DropDownItems;

            _toolsMenuConfigMenuItem.Click -= MenuOptionsOnClick;
            optionsmenu.Remove(_toolsMenuSeparator);
            optionsmenu.Remove(_toolsMenuConfigMenuItem);

            var openmenu = Host.MainWindow.MainMenu.Items["m_menuFile"] as ToolStripMenuItem;
            if (openmenu != null)
            {
                _fileOpenMenuItem.Click -= MenuFileOpenFromOneDriveOnClick;
                openmenu.DropDownItems.Remove(_fileOpenMenuItem);
            }

            _fileOfflineMenuItem.Click -= MenuFileOneDriveSyncOfflineOnClick;
            openmenu.DropDownItems.Remove(_fileOfflineMenuItem);
        }

        /// <summary>
        /// Triggered when a KeePass database is closing after the database has been saved
        /// </summary>
        private void OnKeePassDatabaseClosing(object sender, FileClosingEventArgs e)
        {
            // Check if there's still a process running that we need to wait for before allowing KeePass to terminate
            if (Configuration.IsSomethingStillRunning)
            {
                Host.MainWindow.SetStatusEx("Waiting for OneDriveSync to complete...");

                // Record the time until we want to wait at most before exiting
                var waitUntil = DateTime.Now.AddSeconds(30);

                do
                {
                    System.Threading.Thread.Sleep(1);
                    Application.DoEvents();
                } while (Configuration.IsSomethingStillRunning && DateTime.Now < waitUntil);
            }
        }

        /// <summary>
        /// Triggered when a KeePass database is being saved
        /// </summary>
        private async void MainWindowOnFileSaved(object sender, FileSavedEventArgs fileSavedEventArgs)
        {
            var databasePath = fileSavedEventArgs.Database.IOConnectionInfo.Path;

            var config = Configuration.GetPasswordDatabaseConfiguration(databasePath);
            config.KeePassDatabase = fileSavedEventArgs.Database;

            // Check if we should sync this database
            if (config.DoNotSync) return;
            if(!_fileOfflineMenuItem.Checked)
            {
                Host.MainWindow.SetStatusEx(string.Format("OneDriveSync has been set to offline, skipping sync for database {0}", fileSavedEventArgs.Database.Name));
                return;
            }

            // Make sure it's not a remote database on i.e. an FTP or website
            if (!fileSavedEventArgs.Database.IOConnectionInfo.IsLocalFile())
            {
                MessageBox.Show("KeePass OneDriveSync does not support synchronizing databases from remote locations and will therefore not be available for this database", "KeePass OneDriveSync", MessageBoxButtons.OK, MessageBoxIcon.Information);

                config.DoNotSync = true;
                Configuration.Save();
                return;
            }

            await KeePassDatabase.SyncDatabase(databasePath, KeePassDatabase.UpdateStatus, true, config);
            
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
        /// Triggered when clicking on the Open from OneDrive menu item under File > Open
        /// </summary>
        private async static void MenuFileOpenFromOneDriveOnClick(object sender, EventArgs e)
        {
            await KeePassDatabase.OpenDatabaseFromCloudService(KeePassDatabase.UpdateStatus);
        }

        /// <summary>
        /// Triggered when clicking on the OneDriveSync Offline Mode menu item under File
        /// </summary>
        private void MenuFileOneDriveSyncOfflineOnClick(object sender, EventArgs e)
        {
            _fileOfflineMenuItem.Checked = !_fileOfflineMenuItem.Checked;
            _fileOfflineMenuItem.Text = _fileOfflineMenuItem.Checked ? _fileOfflineMenuItem.Text = "OneDriveSync is Online" : _fileOfflineMenuItem.Text = "OneDriveSync is Offline";
        }

        /// <summary>
        /// Triggered when a KeePass database is being opened
        /// </summary>
        private async void OnKeePassDatabaseOpened(object sender, FileOpenedEventArgs fileOpenedEventArgs)
        {
            var databasePath = fileOpenedEventArgs.Database.IOConnectionInfo.Path;            

            // Add the KeePass database instance to the already available configuration
            var config = Configuration.GetPasswordDatabaseConfiguration(databasePath);
            config.KeePassDatabase = fileOpenedEventArgs.Database;

            // Check if we should sync this database
            if (config.DoNotSync || !fileOpenedEventArgs.Database.IOConnectionInfo.IsLocalFile()) return;
            if(!_fileOfflineMenuItem.Checked)
            {
                Host.MainWindow.SetStatusEx(string.Format("OneDriveSync has been set to offline, skipping sync for database {0}", fileOpenedEventArgs.Database.Name));
                return;
            }

            // Check if the database configuration of the opened KeePass database is set to retrieve the OneDrive Refresh Token from the KeePass database itself
            if (config.RefreshTokenStorage == OneDriveRefreshTokenStorage.KeePassDatabase && string.IsNullOrEmpty(config.RefreshToken))
            {
                // Retrieve the OneDrive Refresh Token from the KeePass database that is being opened
                config.RefreshToken = Utilities.GetRefreshTokenFromKeePassDatabase(fileOpenedEventArgs.Database);
            }

            await KeePassDatabase.SyncDatabase(databasePath, KeePassDatabase.UpdateStatus, false, config);

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
