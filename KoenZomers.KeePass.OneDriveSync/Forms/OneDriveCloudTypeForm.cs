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

            OneDrivePictureButton.BackgroundImage = Resources.OneDriveBothClouds;
            SharePointOnlinePictureButton.BackgroundImage = Resources.SharePoint;
            button1.BackgroundImage = Resources.SharePoint;
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

        private void OneDrivePictureButton_Click(object sender, EventArgs e)
        {
            ChosenCloudStorageType = CloudStorageType.MicrosoftGraphMsalLogin;
            DialogResult = DialogResult.OK;

            Close();
        }

        private void SharePointOnlinePictureButton_Click(object sender, EventArgs e)
        {
            ChosenCloudStorageType = CloudStorageType.SharePoint;
            DialogResult = DialogResult.OK;

            Close();
        }

        private void SharePointOnPremisesPictureButton_Click(object sender, EventArgs e)
        {
            ChosenCloudStorageType = CloudStorageType.SharePointOnPremises;
            DialogResult = DialogResult.OK;

            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {

        }
    }
}
