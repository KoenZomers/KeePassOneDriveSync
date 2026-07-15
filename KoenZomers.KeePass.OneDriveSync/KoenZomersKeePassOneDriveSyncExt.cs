using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
        /// <summary>
        /// Registers an AssemblyResolve handler as early as possible (before any of this plugin's
        /// dependencies are touched) so that requests for our dependency assemblies (e.g.
        /// System.Text.Json and its transitive dependencies) are always satisfied by the exact
        /// DLLs shipped alongside this plugin, regardless of the version being requested.
        /// This avoids relying on binding redirects in KeePass.exe.config, which is shared with
        /// (and could conflict with) other plugins.
        /// </summary>
        static KoenZomersKeePassOneDriveSyncExt()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveDependencyAssembly;
        }

        /// <summary>
        /// Resolves requests for this plugin's dependency assemblies by loading them directly
        /// from the plugin's own folder, bypassing strict assembly version matching.
        /// </summary>
        private static Assembly ResolveDependencyAssembly(object sender, ResolveEventArgs args)
        {
            var requestedAssemblyName = new AssemblyName(args.Name);

            // This plugin is compiled against a specific KeePass.exe version (the one present on the
            // build machine), which embeds that exact version into the strong name reference. Older
            // (or newer) KeePass hosts that don't exactly match that version would otherwise fail to
            // load the plugin with a FileNotFoundException, even though KeePass.exe is already loaded
            // in the process. To support any KeePass version, redirect requests for the "KeePass"
            // assembly to whichever KeePass assembly is already loaded in the current AppDomain,
            // regardless of the requested version.
            if (string.Equals(requestedAssemblyName.Name, "KeePass", StringComparison.OrdinalIgnoreCase))
            {
                var loadedKeePassAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(assembly => string.Equals(assembly.GetName().Name, "KeePass", StringComparison.OrdinalIgnoreCase));
                if (loadedKeePassAssembly != null)
                {
                    return loadedKeePassAssembly;
                }
            }

            var pluginDirectory = Path.GetDirectoryName(typeof(KoenZomersKeePassOneDriveSyncExt).Assembly.Location);
            if (string.IsNullOrEmpty(pluginDirectory))
            {
                return null;
            }

            var candidatePath = Path.Combine(pluginDirectory, requestedAssemblyName.Name + ".dll");
            return File.Exists(candidatePath) ? Assembly.LoadFrom(candidatePath) : null;
        }

        #region Constants

        /// <summary>
        /// Application ID to use for communication with the Microsoft Graph API
        /// </summary>
        internal const string GraphApiApplicationId = "554972ab-b8be-43a5-8ccd-27c0eb46a202";

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
            get { return "https://raw.githubusercontent.com/KoenZomers/KeePassOneDriveSync/master/version.txt"; }
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

            await KeePassDatabase.SyncDatabase(databasePath, KeePassDatabase.UpdateStatus, false, config);
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
