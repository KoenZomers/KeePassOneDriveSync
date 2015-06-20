using System;
using System.Windows.Forms;
using KoenZomers.KeePass.OneDriveSync;

namespace KoenZomersKeePassOneDriveSync
{
    public partial class OneDriveRemoteLocationDialog : Form
    {
        private readonly Configuration _databaseConfiguration;

        public OneDriveRemoteLocationDialog(Configuration databasecConfiguration)
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
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
