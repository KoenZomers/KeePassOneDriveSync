using System;
using System.IO;
using System.Security.Cryptography;
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
        private async void CheckForNewerDatabase(string localKeePassDatabasePath)
        {
            using (var fileStream = new FileStream(localKeePassDatabasePath, FileMode.Open))
            {
                using (var cryptoProvider = new SHA1CryptoServiceProvider())
                {
                    string hash = BitConverter.ToString(cryptoProvider.ComputeHash(fileStream));

                }
            }
            // Retrieve the KeePassOneDriveSync settings for the database that got opened
            var databaseConfig = Configuration.GetPasswordDatabaseConfiguration(localKeePassDatabasePath);

            // Make sure configuration exists for the database
            if (databaseConfig == null) return;

            //var oneDriveApi = new OneDrive.Api.OneDriveApi(OneDriveClientId, OneDriveClientSecret);
            //await oneDriveApi.DownloadItem()



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
