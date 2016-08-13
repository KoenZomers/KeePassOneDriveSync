using System;
using System.Windows.Forms;

namespace KoenZomersKeePassOneDriveSync.Forms
{
    public partial class OneDriveRequestInputDialog : Form
    {
        /// <summary>
        /// Gets or sets the value in the textbox
        /// </summary>
        public string InputValue
        {
            get { return FolderNameTextBox.Text; }
            set { FolderNameTextBox.Text = value; }
        }

        /// <summary>
        /// Title shown on the dialog
        /// </summary>
        public string FormTitle
        {
            get { return Text; }
            set { Text = value; }
        }

        public OneDriveRequestInputDialog()
        {
            InitializeComponent();
        }

        private void FolderNameTextBox_TextChanged(object sender, EventArgs e)
        {
            OKButton.Enabled = FolderNameTextBox.Text.Length > 0;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
