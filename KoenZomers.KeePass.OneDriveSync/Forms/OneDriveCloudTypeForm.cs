using System;
using System.Windows.Forms;
using KoenZomers.KeePass.OneDriveSync;
using KoenZomers.KeePass.OneDriveSync.Enums;

namespace KoenZomersKeePassOneDriveSync
{
    public partial class OneDriveCloudTypeForm : Form
    {
        /// <summary>
        /// Configuration of the database for which this question is being asked for
        /// </summary>
        private readonly Configuration _configuration;

        public OneDriveCloudTypeForm(Configuration configuration)
        {
            InitializeComponent();

            OneDriveConsumerPicture.Image = Resources.OneDriveConsumer;
            OneDriveForBusinessPicture.Image = Resources.OneDriveForBusiness;

            _configuration = configuration;
        }

        private void OneDriveConsumerPicture_Click(object sender, EventArgs e)
        {
            _configuration.CloudStorageType = CloudStorageType.OneDriveConsumer;
            Configuration.Save();
            DialogResult = DialogResult.OK;

            Close();
        }

        private void OneDriveForBusinessPicture_Click(object sender, EventArgs e)
        {
            _configuration.CloudStorageType = CloudStorageType.OneDriveForBusiness;
            Configuration.Save();
            DialogResult = DialogResult.OK;

            Close();
        }
    }
}
