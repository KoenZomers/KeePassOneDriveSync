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
            get { return InputTextBox.Text; }
            set { InputTextBox.Text = value; }
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

        private void OKButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void InputTextBoxTextBox_TextChanged(object sender, EventArgs e)
        {
            OKButton.Enabled = InputTextBox.Text.Length > 0;
        }

        private void InputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void InputTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) OKButton_Click(sender, e);
            if (e.KeyCode == Keys.Escape) Close();
        }
    }
}
