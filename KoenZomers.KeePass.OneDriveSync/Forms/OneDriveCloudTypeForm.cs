using System;
using System.Windows.Forms;
using KoenZomers.KeePass.OneDriveSync.Enums;

namespace KoenZomersKeePassOneDriveSync
{
    public partial class OneDriveCloudTypeForm : Form
    {
        /// <summary>
        /// The cloud storage provider that was chosen in this dialog
        /// </summary>
        public CloudStorageType ChosenCloudStorageType { get; private set; }

        /// <summary>
        /// Gets or sets the explanation text in the dialog
        /// </summary>
        public string ExplanationText
        {
            get { return ExplanationLabel.Text; }
            set { ExplanationLabel.Text = value; }
        }

        public OneDriveCloudTypeForm()
        {
            InitializeComponent();

            OneDriveConsumerPicture.Image = Resources.OneDriveConsumer;
            OneDriveForBusinessPicture.Image = Resources.OneDriveForBusiness;
        }

        private void OneDriveConsumerPicture_Click(object sender, EventArgs e)
        {
            ChosenCloudStorageType = CloudStorageType.OneDriveConsumer;
            DialogResult = DialogResult.OK;

            Close();
        }

        private void OneDriveForBusinessPicture_Click(object sender, EventArgs e)
        {
            ChosenCloudStorageType = CloudStorageType.OneDriveForBusiness;
            DialogResult = DialogResult.OK;

            Close();
        }
    }
}
