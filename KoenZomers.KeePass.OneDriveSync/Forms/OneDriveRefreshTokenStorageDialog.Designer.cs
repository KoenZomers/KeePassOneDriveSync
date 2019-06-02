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
            this.ExplanationLabel.Location = new System.Drawing.Point(14, 11);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(744, 43);
            this.ExplanationLabel.TabIndex = 0;
            this.ExplanationLabel.Text = "You\'ve successfully authenticated to OneDrive. Where should KeeOneDriveSync store" +
    " the OneDrive Refresh Token so it can keep your local database in sync with its " +
    "equivalent on OneDrive?";
            // 
            // InKeePassDatabaseRadio
            // 
            this.InKeePassDatabaseRadio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InKeePassDatabaseRadio.AutoSize = true;
            this.InKeePassDatabaseRadio.Location = new System.Drawing.Point(18, 76);
            this.InKeePassDatabaseRadio.Name = "InKeePassDatabaseRadio";
            this.InKeePassDatabaseRadio.Size = new System.Drawing.Size(250, 24);
            this.InKeePassDatabaseRadio.TabIndex = 2;
            this.InKeePassDatabaseRadio.Text = "In the KeePass database itself";
            this.InKeePassDatabaseRadio.UseVisualStyleBackColor = true;
            // 
            // OnDiskRadio
            // 
            this.OnDiskRadio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OnDiskRadio.AutoSize = true;
            this.OnDiskRadio.Checked = true;
            this.OnDiskRadio.Location = new System.Drawing.Point(18, 108);
            this.OnDiskRadio.Name = "OnDiskRadio";
            this.OnDiskRadio.Size = new System.Drawing.Size(186, 24);
            this.OnDiskRadio.TabIndex = 3;
            this.OnDiskRadio.TabStop = true;
            this.OnDiskRadio.Text = "On my local computer";
            this.OnDiskRadio.UseVisualStyleBackColor = true;
            // 
            // FinishButton
            // 
            this.FinishButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FinishButton.Location = new System.Drawing.Point(278, 194);
            this.FinishButton.Name = "FinishButton";
            this.FinishButton.Size = new System.Drawing.Size(195, 63);
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
            this.HelpMeChooseLinkLabel.Location = new System.Drawing.Point(20, 157);
            this.HelpMeChooseLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.HelpMeChooseLinkLabel.Name = "HelpMeChooseLinkLabel";
            this.HelpMeChooseLinkLabel.Size = new System.Drawing.Size(228, 20);
            this.HelpMeChooseLinkLabel.TabIndex = 7;
            this.HelpMeChooseLinkLabel.TabStop = true;
            this.HelpMeChooseLinkLabel.Text = "I have no idea, help me choose";
            this.HelpMeChooseLinkLabel.UseMnemonic = false;
            this.HelpMeChooseLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HelpMeChooseLinkLabel_LinkClicked);
            // 
            // OneDriveRefreshTokenStorageDialog
            // 
            this.AcceptButton = this.FinishButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 283);
            this.Controls.Add(this.HelpMeChooseLinkLabel);
            this.Controls.Add(this.FinishButton);
            this.Controls.Add(this.OnDiskRadio);
            this.Controls.Add(this.InKeePassDatabaseRadio);
            this.Controls.Add(this.ExplanationLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(602, 313);
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
        private System.Windows.Forms.RadioButton InKeePassDatabaseRadio;
        private System.Windows.Forms.RadioButton OnDiskRadio;
        private System.Windows.Forms.LinkLabel HelpMeChooseLinkLabel;
    }
}