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

            OneDriveConsumerPictureButton.BackgroundImage = Resources.OneDrive;
            GraphPictureButton.BackgroundImage = Resources.MSGraph;
            GraphDeviceLoginPictureButton.BackgroundImage = Resources.MSGraph;
            SharePointPictureButton.BackgroundImage = Resources.SharePoint;
        }

        private void OneDriveConsumerPicture_Click(object sender, EventArgs e)
        {
            ChosenCloudStorageType = CloudStorageType.OneDriveConsumer;
            DialogResult = DialogResult.OK;

            Close();
        }

        private void OneDriveConsumerPictureButton_Click(object sender, EventArgs e)
        {
            ChosenCloudStorageType = CloudStorageType.OneDriveConsumer;
            DialogResult = DialogResult.OK;

            Close();
        }

        private void GraphPictureButton_Click(object sender, EventArgs e)
        {
            ChosenCloudStorageType = CloudStorageType.MicrosoftGraph;
            DialogResult = DialogResult.OK;

            Close();
        }

        private void SharePointPictureButton_Click(object sender, EventArgs e)
        {
            ChosenCloudStorageType = CloudStorageType.SharePoint;
            DialogResult = DialogResult.OK;

            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {

        }

        private void GraphDeviceLoginPictureButton_Click(object sender, EventArgs e)
        {
            ChosenCloudStorageType = CloudStorageType.MicrosoftGraphDeviceLogin;
            DialogResult = DialogResult.OK;

            Close();
        }
    }
}
