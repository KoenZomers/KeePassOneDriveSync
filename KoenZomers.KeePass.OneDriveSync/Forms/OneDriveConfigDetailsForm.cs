using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KoenZomers.KeePass.OneDriveSync;
using KoenZomers.KeePass.OneDriveSync.Enums;

namespace KoenZomersKeePassOneDriveSync
{
    /// <summary>
    /// Detail screen for a OneDriveSync configuration
    /// </summary>
    public partial class OneDriveConfigDetailsForm : Form
    {
        private KeyValuePair<string, Configuration> _configuration;

        /// <summary>
        /// Opens the configuration details screen for the provided configuration
        /// </summary>
        /// <param name="configuration"></param>
        public OneDriveConfigDetailsForm(KeyValuePair<string, Configuration> configuration)
        {
            InitializeComponent();

            _configuration = configuration;
        }

        private void OneDriveConfigDetailsForm_Load(object sender, EventArgs e)
        {
            ShowConfiguration();
        }

        private void ShowConfiguration()
        {
            LocationNameLabel.Text = _configuration.Value.CloudStorageType == CloudStorageType.SharePoint ? "SharePoint site:" : "OneDrive name:";
            LastSyncedLabel.Text = string.Format(LastSyncedLabel.Text, _configuration.Value.CloudStorageType);
            LastVerifiedLabel.Text = string.Format(LastVerifiedLabel.Text, _configuration.Value.CloudStorageType);
            OneDriveNameTextbox.Text = _configuration.Value.DoNotSync ? "Not enabled for sync" : _configuration.Value.CloudStorageType == CloudStorageType.SharePoint ? _configuration.Value.RemoteDatabasePath + (string.IsNullOrEmpty(_configuration.Value.OneDriveName) ? "" : " (" + _configuration.Value.OneDriveName + ")") : _configuration.Value.OneDriveName;
            CloudStorageTypeTextBox.Text = _configuration.Value.CloudStorageType.GetValueOrDefault(CloudStorageType.OneDriveConsumer).ToString();
            LocalDatabasePathLinkLabel.Text = _configuration.Key;
            LocalDatabasePathLinkLabel.LinkColor = System.IO.File.Exists(_configuration.Key) ? System.Drawing.Color.Blue : System.Drawing.Color.Red;
            RemoteKeePassPathTextbox.Text = _configuration.Value.CloudStorageType == CloudStorageType.SharePoint ? _configuration.Value.RemoteFolderId + "/" + _configuration.Value.RemoteFileName : _configuration.Value.RemoteDatabasePath;
            OneDriveRefreshTokenTextbox.Text = _configuration.Value.RefreshToken;
            RefreshTokenStorageTextBox.Text = _configuration.Value.RefreshTokenStorage.ToString();
            LastSyncedTextbox.Text = _configuration.Value.LastSyncedAt.HasValue ? _configuration.Value.LastSyncedAt.Value.ToString("dddd d MMMM yyyy HH:mm:ss") : "Never synced yet";
            LastVerifiedTextbox.Text = _configuration.Value.LastCheckedAt.HasValue ? _configuration.Value.LastCheckedAt.Value.ToString("dddd d MMMM yyyy HH:mm:ss") : "Never verified yet";
            LocalKeePassFileHashTextbox.Text = _configuration.Value.LocalFileHash;
            OneDriveEtagTextBox.Text = _configuration.Value.ETag;
            ForceSyncButton.Enabled = !_configuration.Value.DoNotSync;
            ItemIdTextBox.Text = _configuration.Value.RemoteItemId;
            FolderIdTextBox.Text = _configuration.Value.RemoteFolderId;
            DriveIdTextBox.Text = _configuration.Value.RemoteDriveId;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var answer = MessageBox.Show("Are you sure that you wish to delete the OneDrive configuration for the selected KeePass databases? This will NOT delete the actual KeePass database file, just its configuration for the KeeOneDriveSync plugin.", "Confirm removal", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if (answer != DialogResult.Yes) return;

            Configuration.DeleteConfig(_configuration.Key);

            Close();
        }

        private async void ForceSyncButton_Click(object sender, EventArgs e)
        {
            if(_configuration.Value.KeePassDatabase == null)
            {
                UpdateStatus("Database must be open before it can synchronize");
                return;
            }

            UpdateStatus("Synchronizing");
            await KeePassDatabase.SyncDatabase(_configuration.Key, UpdateStatus, true, null);
            ShowConfiguration();
        }

        private void UpdateStatus(string message)
        {
            StatusLabel.Text = message;
        }

        private void OneDriveConfigDetailsForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                ShowConfiguration();
            }
        }

        private void LocalDatabasePathLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(System.IO.File.Exists(LocalDatabasePathLinkLabel.Text))
            {
                // File still exists locally, open explorer and have it highlight the file
                System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + LocalDatabasePathLinkLabel.Text + "\"");
                return;
            }

            // Check if the parent directory still exists 
            var fileInfo = new System.IO.FileInfo(LocalDatabasePathLinkLabel.Text);
            if (System.IO.Directory.Exists(fileInfo.Directory.FullName))
            {
                // Parent directory still exists, open explorer in that folder
                System.Diagnostics.Process.Start("explorer.exe", "\"" + fileInfo.Directory.FullName + "\"");
                return;
            }

            // Both the file and the parent folder no longer exist, nothing we can do
        }

        private void ItemIdLabel_Click(object sender, EventArgs e)
        {

        }

        private void ItemIdTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
