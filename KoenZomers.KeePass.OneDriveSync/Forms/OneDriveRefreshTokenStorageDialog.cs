using System;
using System.Windows.Forms;
using KoenZomers.KeePass.OneDriveSync;
using KoenZomers.KeePass.OneDriveSync.Enums;

namespace KoenZomersKeePassOneDriveSync
{
    public partial class OneDriveRefreshTokenStorageDialog : Form
    {
        /// <summary>
        /// Configuration of the database for which this question is being asked for
        /// </summary>
        private readonly Configuration _configuration;

        public OneDriveRefreshTokenStorageDialog(Configuration configuration)
        {
            InitializeComponent();
            
            _configuration = configuration;
        }

        private void HelpMeChooseLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/KoenZomers/KeePassOneDriveSync/blob/master/OneDriveRefreshToken.md");
        }

        private void FinishButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OneDriveRefreshTokenStorageDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (InKeePassDatabaseRadio.Checked)
            {
                _configuration.RefreshTokenStorage = OneDriveRefreshTokenStorage.KeePassDatabase;
            }
            if (WindowsCredentialManagerRadio.Checked)
            {
                _configuration.RefreshTokenStorage = OneDriveRefreshTokenStorage.WindowsCredentialManager;
            }
            if (OnDiskRadio.Checked)
            {
                _configuration.RefreshTokenStorage = OneDriveRefreshTokenStorage.Disk;
            }
        }
    }
}
