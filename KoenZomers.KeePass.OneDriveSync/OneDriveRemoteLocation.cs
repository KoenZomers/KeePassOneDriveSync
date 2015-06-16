using System;
using System.Windows.Forms;
using KoenZomers.KeePass.OneDriveSync;

namespace KoenZomersKeePassOneDriveSync
{
    public partial class OneDriveRemoteLocation : Form
    {
        private Configuration _databaseConfiguration;

        public OneDriveRemoteLocation(Configuration databasecConfiguration)
        {
            InitializeComponent();

            _databaseConfiguration = databasecConfiguration;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OneDriveRemotePathTextBox.Text))
            {
                MessageBox.Show("OneDrive remote path is compulsory", "OneDrive Location", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);              
            }
            else
            {
                _databaseConfiguration.RemoteDatabasePath = OneDriveRemotePathTextBox.Text;                
                
                Configuration.Save();

                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
