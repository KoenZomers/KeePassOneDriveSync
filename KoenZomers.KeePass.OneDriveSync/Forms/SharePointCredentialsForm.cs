﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KoenZomersKeePassOneDriveSync.Forms
{
    public partial class SharePointCredentialsForm : Form
    {
        #region Properties

        /// <summary>
        /// The entered SharePoint URL
        /// </summary>
        public string SharePointUrl {  get { return SharePointUrlTextBox.Text; } }

        /// <summary>
        /// The entered SharePoint Client Id
        /// </summary>
        public string SharePointClientId { get { return ClientIdTextBox.Text; } }

        /// <summary>
        /// The entered SharePoint Client Secret
        /// </summary>
        public string SharePointClientSecret { get { return ClientSecretTextBox.Text; } }

        /// <summary>
        /// Boolean indicating if the SharePointUrl, SharePointClientId and SharePointClientSecret fields all contain a value
        /// </summary>
        public bool AllFieldsContainText { get { return !string.IsNullOrWhiteSpace(SharePointUrlTextBox.Text) && !string.IsNullOrWhiteSpace(ClientIdTextBox.Text) && !string.IsNullOrWhiteSpace(ClientSecretTextBox.Text); } }
        #endregion

        public SharePointCredentialsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Triggered when the user clicks the OK button
        /// </summary>
        private void OKButton_Click(object sender, EventArgs e)
        {
            if(!EnsureAllFieldsEntered())
            {
                DialogResult = DialogResult.None;
            }
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Validates if all fields contain a value and if not, will display a notification to the nd user
        /// </summary>
        /// <returns>True if all fields contain a value, False if this is not the case</returns>
        private bool EnsureAllFieldsEntered()
        {
            if (!AllFieldsContainText)
            {
                MessageBox.Show(this, "All fields are required fields", "Please enter all fields", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return true;
        }

        /// <summary>
        /// Uses the information provided in the form to try to establish a connection with SharePoint
        /// </summary>
        private void TestButton_Click(object sender, EventArgs e)
        {
            // Ensure all fields contain a value
            if(!EnsureAllFieldsEntered())
            {
                return;
            }

            // Test the connection
            using (var clientContext = Providers.SharePointProvider.CreateSharePointClientContext(new Uri(SharePointUrl), SharePointClientId, SharePointClientSecret))
            {
                if (Providers.SharePointProvider.TestConnection(clientContext))
                {
                    MessageBox.Show("Connection successful", "Testing SharePoint Connectivity", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Connection failed", "Testing SharePoint Connectivity", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void SharePointUrlTextBox_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}