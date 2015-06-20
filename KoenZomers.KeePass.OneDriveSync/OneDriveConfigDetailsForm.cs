using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KoenZomers.KeePass.OneDriveSync;

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
            OneDriveNameTextbox.Text = _configuration.Value.DoNotSync ? "Not enabled for sync" : _configuration.Value.OneDriveName;
            LocalKeePassPathTextbox.Text = _configuration.Key;
            RemoteKeePassPathTextbox.Text = _configuration.Value.RemoteDatabasePath;
            OneDriveRefreshTokenTextbox.Text = _configuration.Value.RefreshToken;
            LastSyncedTextbox.Text = _configuration.Value.LastSyncedAt.HasValue ? _configuration.Value.LastSyncedAt.Value.ToString("dddd d MMMM yyyy HH:mm:ss") : "Never synced yet";
            LastVerifiedTextbox.Text = _configuration.Value.LastCheckedAt.HasValue ? _configuration.Value.LastCheckedAt.Value.ToString("dddd d MMMM yyyy HH:mm:ss") : "Never verified yet";
            LocalKeePassFileHashTextbox.Text = _configuration.Value.LocalFileHash;
            ForceSyncButton.Enabled = !_configuration.Value.DoNotSync && KoenZomersKeePassOneDriveSyncExt.Host.Database.IOConnectionInfo.Path.Equals(_configuration.Key, StringComparison.InvariantCultureIgnoreCase);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var answer = MessageBox.Show("Are you sure that you wish to delete the OneDrive configuration for this KeePass database?", "Confirm removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (answer != DialogResult.Yes) return;

            Configuration.PasswordDatabases.Remove(_configuration.Key);

            Close();
        }

        private async void ForceSyncButton_Click(object sender, EventArgs e)
        {
            UpdateStatus("Synchronizing");
            await KeePass.SyncDatabase(_configuration.Key, UpdateStatus);
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
    }
}
