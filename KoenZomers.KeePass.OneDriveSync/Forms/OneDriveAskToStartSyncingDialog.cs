﻿using System;
using System.Windows.Forms;
using KoenZomers.KeePass.OneDriveSync;

namespace KoenZomersKeePassOneDriveSync
{
    public partial class OneDriveAskToStartSyncingDialog : Form
    {
        /// <summary>
        /// Configuration of the database for which this question is being asked for
        /// </summary>
        private readonly Configuration _configuration;

        public OneDriveAskToStartSyncingDialog(Configuration configuration)
        {
            InitializeComponent();

            _configuration = configuration;

            OneDriveDatabasePath.Text = configuration.KeePassDatabase.IOConnectionInfo.Path;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (NotNowRadio.Checked)
            {
                DialogResult = DialogResult.Cancel;
            }
            if (YesRadio.Checked)
            {
                DialogResult = DialogResult.OK;
            }
            if (NoNeverAskAgainRadio.Checked)
            {
                _configuration.DoNotSync = NoNeverAskAgainRadio.Checked;
                Configuration.Save();
                DialogResult = DialogResult.Cancel;
            }

            Close();
        }
    }
}
