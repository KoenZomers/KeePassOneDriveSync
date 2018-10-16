namespace KoenZomersKeePassOneDriveSync
{
    partial class OneDriveRefreshTokenStorageDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ExplanationLabel = new System.Windows.Forms.Label();
            this.WindowsCredentialManagerRadio = new System.Windows.Forms.RadioButton();
            this.InKeePassDatabaseRadio = new System.Windows.Forms.RadioButton();
            this.OnDiskRadio = new System.Windows.Forms.RadioButton();
            this.FinishButton = new System.Windows.Forms.Button();
            this.HelpMeChooseLinkLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExplanationLabel.Location = new System.Drawing.Point(9, 7);
            this.ExplanationLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(496, 49);
            this.ExplanationLabel.TabIndex = 0;
            this.ExplanationLabel.Text = "You\'ve successfully authenticated to OneDrive. Where should KeeOneDriveSync store" +
    " the OneDrive Refresh Token so it can keep your local database in sync with its " +
    "equivalent on OneDrive?";
            // 
            // WindowsCredentialManagerRadio
            // 
            this.WindowsCredentialManagerRadio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WindowsCredentialManagerRadio.AutoSize = true;
            this.WindowsCredentialManagerRadio.Checked = true;
            this.WindowsCredentialManagerRadio.Location = new System.Drawing.Point(11, 58);
            this.WindowsCredentialManagerRadio.Margin = new System.Windows.Forms.Padding(2);
            this.WindowsCredentialManagerRadio.Name = "WindowsCredentialManagerRadio";
            this.WindowsCredentialManagerRadio.Size = new System.Drawing.Size(194, 17);
            this.WindowsCredentialManagerRadio.TabIndex = 1;
            this.WindowsCredentialManagerRadio.TabStop = true;
            this.WindowsCredentialManagerRadio.Text = "In the Windows Credential Manager";
            this.WindowsCredentialManagerRadio.UseVisualStyleBackColor = true;
            // 
            // InKeePassDatabaseRadio
            // 
            this.InKeePassDatabaseRadio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InKeePassDatabaseRadio.AutoSize = true;
            this.InKeePassDatabaseRadio.Location = new System.Drawing.Point(11, 80);
            this.InKeePassDatabaseRadio.Margin = new System.Windows.Forms.Padding(2);
            this.InKeePassDatabaseRadio.Name = "InKeePassDatabaseRadio";
            this.InKeePassDatabaseRadio.Size = new System.Drawing.Size(168, 17);
            this.InKeePassDatabaseRadio.TabIndex = 2;
            this.InKeePassDatabaseRadio.Text = "In the KeePass database itself";
            this.InKeePassDatabaseRadio.UseVisualStyleBackColor = true;
            // 
            // OnDiskRadio
            // 
            this.OnDiskRadio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OnDiskRadio.AutoSize = true;
            this.OnDiskRadio.Location = new System.Drawing.Point(11, 101);
            this.OnDiskRadio.Margin = new System.Windows.Forms.Padding(2);
            this.OnDiskRadio.Name = "OnDiskRadio";
            this.OnDiskRadio.Size = new System.Drawing.Size(127, 17);
            this.OnDiskRadio.TabIndex = 3;
            this.OnDiskRadio.Text = "On my local computer";
            this.OnDiskRadio.UseVisualStyleBackColor = true;
            // 
            // FinishButton
            // 
            this.FinishButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FinishButton.Location = new System.Drawing.Point(185, 153);
            this.FinishButton.Margin = new System.Windows.Forms.Padding(2);
            this.FinishButton.Name = "FinishButton";
            this.FinishButton.Size = new System.Drawing.Size(130, 41);
            this.FinishButton.TabIndex = 5;
            this.FinishButton.Text = "&Finish";
            this.FinishButton.UseVisualStyleBackColor = true;
            this.FinishButton.Click += new System.EventHandler(this.FinishButton_Click);
            // 
            // HelpMeChooseLinkLabel
            // 
            this.HelpMeChooseLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HelpMeChooseLinkLabel.AutoSize = true;
            this.HelpMeChooseLinkLabel.Location = new System.Drawing.Point(26, 130);
            this.HelpMeChooseLinkLabel.Name = "HelpMeChooseLinkLabel";
            this.HelpMeChooseLinkLabel.Size = new System.Drawing.Size(156, 13);
            this.HelpMeChooseLinkLabel.TabIndex = 7;
            this.HelpMeChooseLinkLabel.TabStop = true;
            this.HelpMeChooseLinkLabel.Text = "I have no idea, help me choose";
            this.HelpMeChooseLinkLabel.UseMnemonic = false;
            this.HelpMeChooseLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HelpMeChooseLinkLabel_LinkClicked);
            // 
            // OneDriveRefreshTokenStorageDialog
            // 
            this.AcceptButton = this.FinishButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 205);
            this.Controls.Add(this.HelpMeChooseLinkLabel);
            this.Controls.Add(this.FinishButton);
            this.Controls.Add(this.OnDiskRadio);
            this.Controls.Add(this.InKeePassDatabaseRadio);
            this.Controls.Add(this.WindowsCredentialManagerRadio);
            this.Controls.Add(this.ExplanationLabel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(409, 223);
            this.Name = "OneDriveRefreshTokenStorageDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Where to store the OneDrive Refresh Token";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OneDriveRefreshTokenStorageDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.Button FinishButton;
        private System.Windows.Forms.RadioButton WindowsCredentialManagerRadio;
        private System.Windows.Forms.RadioButton InKeePassDatabaseRadio;
        private System.Windows.Forms.RadioButton OnDiskRadio;
        private System.Windows.Forms.LinkLabel HelpMeChooseLinkLabel;
    }
}