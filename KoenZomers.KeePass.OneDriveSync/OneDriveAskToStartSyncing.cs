using System;
using System.Windows.Forms;
using KoenZomers.KeePass.OneDriveSync;

namespace KoenZomersKeePassOneDriveSync
{
    public partial class OneDriveAskToStartSyncing : Form
    {
        /// <summary>
        /// Configuration of the database for which this question is being asked for
        /// </summary>
        private Configuration _configuration;

        public OneDriveAskToStartSyncing(Configuration configuration)
        {
            InitializeComponent();
            
            _configuration = configuration;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            _configuration.DoNotSync = NoNeverAskAgainRadio.Checked;

            Configuration.Save();
            
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
