namespace KoenZomersKeePassOneDriveSync
{
    partial class OneDriveConfigDetailsForm
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
            this.OneDriveNameLabel = new System.Windows.Forms.Label();
            this.LocalKeePassPathLabel = new System.Windows.Forms.Label();
            this.RemoteKeePassPathLabel = new System.Windows.Forms.Label();
            this.OneDriveRefreshTokenLabel = new System.Windows.Forms.Label();
            this.RemoteKeePassPathTextbox = new System.Windows.Forms.TextBox();
            this.OneDriveRefreshTokenTextbox = new System.Windows.Forms.TextBox();
            this.LocalKeePassPathTextbox = new System.Windows.Forms.TextBox();
            this.OneDriveNameTextbox = new System.Windows.Forms.TextBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.LastSyncedLabel = new System.Windows.Forms.Label();
            this.LastSyncedTextbox = new System.Windows.Forms.TextBox();
            this.LocalKeePassFileHashLabel = new System.Windows.Forms.Label();
            this.LocalKeePassFileHashTextbox = new System.Windows.Forms.TextBox();
            this.ForceSyncButton = new System.Windows.Forms.Button();
            this.LastVerifiedLabel = new System.Windows.Forms.Label();
            this.LastVerifiedTextbox = new System.Windows.Forms.TextBox();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.RefreshTokenStorageTextBox = new System.Windows.Forms.TextBox();
            this.RefreshTokenStorageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OneDriveNameLabel
            // 
            this.OneDriveNameLabel.Location = new System.Drawing.Point(12, 9);
            this.OneDriveNameLabel.Name = "OneDriveNameLabel";
            this.OneDriveNameLabel.Size = new System.Drawing.Size(152, 16);
            this.OneDriveNameLabel.TabIndex = 0;
            this.OneDriveNameLabel.Text = "OneDrive:";
            this.OneDriveNameLabel.UseMnemonic = false;
            // 
            // LocalKeePassPathLabel
            // 
            this.LocalKeePassPathLabel.Location = new System.Drawing.Point(12, 29);
            this.LocalKeePassPathLabel.Name = "LocalKeePassPathLabel";
            this.LocalKeePassPathLabel.Size = new System.Drawing.Size(152, 16);
            this.LocalKeePassPathLabel.TabIndex = 2;
            this.LocalKeePassPathLabel.Text = "Local KeePass path:";
            this.LocalKeePassPathLabel.UseMnemonic = false;
            // 
            // RemoteKeePassPathLabel
            // 
            this.RemoteKeePassPathLabel.Location = new System.Drawing.Point(12, 50);
            this.RemoteKeePassPathLabel.Name = "RemoteKeePassPathLabel";
            this.RemoteKeePassPathLabel.Size = new System.Drawing.Size(152, 16);
            this.RemoteKeePassPathLabel.TabIndex = 4;
            this.RemoteKeePassPathLabel.Text = "Remote KeePass path:";
            this.RemoteKeePassPathLabel.UseMnemonic = false;
            // 
            // OneDriveRefreshTokenLabel
            // 
            this.OneDriveRefreshTokenLabel.Location = new System.Drawing.Point(12, 71);
            this.OneDriveRefreshTokenLabel.Name = "OneDriveRefreshTokenLabel";
            this.OneDriveRefreshTokenLabel.Size = new System.Drawing.Size(152, 16);
            this.OneDriveRefreshTokenLabel.TabIndex = 6;
            this.OneDriveRefreshTokenLabel.Text = "OneDrive Refresh Token:";
            this.OneDriveRefreshTokenLabel.UseMnemonic = false;
            // 
            // RemoteKeePassPathTextbox
            // 
            this.RemoteKeePassPathTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoteKeePassPathTextbox.BackColor = System.Drawing.SystemColors.Control;
            this.RemoteKeePassPathTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RemoteKeePassPathTextbox.Location = new System.Drawing.Point(173, 50);
            this.RemoteKeePassPathTextbox.Name = "RemoteKeePassPathTextbox";
            this.RemoteKeePassPathTextbox.ReadOnly = true;
            this.RemoteKeePassPathTextbox.Size = new System.Drawing.Size(355, 13);
            this.RemoteKeePassPathTextbox.TabIndex = 4;
            this.RemoteKeePassPathTextbox.Text = "?";
            // 
            // OneDriveRefreshTokenTextbox
            // 
            this.OneDriveRefreshTokenTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OneDriveRefreshTokenTextbox.BackColor = System.Drawing.SystemColors.Control;
            this.OneDriveRefreshTokenTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OneDriveRefreshTokenTextbox.Location = new System.Drawing.Point(173, 71);
            this.OneDriveRefreshTokenTextbox.Name = "OneDriveRefreshTokenTextbox";
            this.OneDriveRefreshTokenTextbox.ReadOnly = true;
            this.OneDriveRefreshTokenTextbox.Size = new System.Drawing.Size(355, 13);
            this.OneDriveRefreshTokenTextbox.TabIndex = 5;
            this.OneDriveRefreshTokenTextbox.Text = "?";
            // 
            // LocalKeePassPathTextbox
            // 
            this.LocalKeePassPathTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LocalKeePassPathTextbox.BackColor = System.Drawing.SystemColors.Control;
            this.LocalKeePassPathTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LocalKeePassPathTextbox.Location = new System.Drawing.Point(173, 29);
            this.LocalKeePassPathTextbox.Name = "LocalKeePassPathTextbox";
            this.LocalKeePassPathTextbox.ReadOnly = true;
            this.LocalKeePassPathTextbox.Size = new System.Drawing.Size(355, 13);
            this.LocalKeePassPathTextbox.TabIndex = 3;
            this.LocalKeePassPathTextbox.Text = "?";
            // 
            // OneDriveNameTextbox
            // 
            this.OneDriveNameTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OneDriveNameTextbox.BackColor = System.Drawing.SystemColors.Control;
            this.OneDriveNameTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OneDriveNameTextbox.Location = new System.Drawing.Point(173, 9);
            this.OneDriveNameTextbox.Name = "OneDriveNameTextbox";
            this.OneDriveNameTextbox.ReadOnly = true;
            this.OneDriveNameTextbox.Size = new System.Drawing.Size(355, 13);
            this.OneDriveNameTextbox.TabIndex = 2;
            this.OneDriveNameTextbox.Text = "?";
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CloseButton.Location = new System.Drawing.Point(429, 185);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(99, 33);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "&Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteButton.Location = new System.Drawing.Point(15, 185);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(99, 33);
            this.DeleteButton.TabIndex = 13;
            this.DeleteButton.Text = "&Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // LastSyncedLabel
            // 
            this.LastSyncedLabel.Location = new System.Drawing.Point(12, 113);
            this.LastSyncedLabel.Name = "LastSyncedLabel";
            this.LastSyncedLabel.Size = new System.Drawing.Size(152, 16);
            this.LastSyncedLabel.TabIndex = 14;
            this.LastSyncedLabel.Text = "Last synced with OneDrive:";
            this.LastSyncedLabel.UseMnemonic = false;
            // 
            // LastSyncedTextbox
            // 
            this.LastSyncedTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LastSyncedTextbox.BackColor = System.Drawing.SystemColors.Control;
            this.LastSyncedTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LastSyncedTextbox.Location = new System.Drawing.Point(173, 113);
            this.LastSyncedTextbox.Name = "LastSyncedTextbox";
            this.LastSyncedTextbox.ReadOnly = true;
            this.LastSyncedTextbox.Size = new System.Drawing.Size(355, 13);
            this.LastSyncedTextbox.TabIndex = 15;
            this.LastSyncedTextbox.Text = "?";
            // 
            // LocalKeePassFileHashLabel
            // 
            this.LocalKeePassFileHashLabel.Location = new System.Drawing.Point(12, 155);
            this.LocalKeePassFileHashLabel.Name = "LocalKeePassFileHashLabel";
            this.LocalKeePassFileHashLabel.Size = new System.Drawing.Size(152, 16);
            this.LocalKeePassFileHashLabel.TabIndex = 16;
            this.LocalKeePassFileHashLabel.Text = "Local KeePass SHA1 hash:";
            this.LocalKeePassFileHashLabel.UseMnemonic = false;
            // 
            // LocalKeePassFileHashTextbox
            // 
            this.LocalKeePassFileHashTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LocalKeePassFileHashTextbox.BackColor = System.Drawing.SystemColors.Control;
            this.LocalKeePassFileHashTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LocalKeePassFileHashTextbox.Location = new System.Drawing.Point(173, 155);
            this.LocalKeePassFileHashTextbox.Name = "LocalKeePassFileHashTextbox";
            this.LocalKeePassFileHashTextbox.ReadOnly = true;
            this.LocalKeePassFileHashTextbox.Size = new System.Drawing.Size(355, 13);
            this.LocalKeePassFileHashTextbox.TabIndex = 17;
            this.LocalKeePassFileHashTextbox.Text = "?";
            // 
            // ForceSyncButton
            // 
            this.ForceSyncButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ForceSyncButton.Location = new System.Drawing.Point(324, 185);
            this.ForceSyncButton.Name = "ForceSyncButton";
            this.ForceSyncButton.Size = new System.Drawing.Size(99, 33);
            this.ForceSyncButton.TabIndex = 18;
            this.ForceSyncButton.Text = "&Sync Now";
            this.ForceSyncButton.UseVisualStyleBackColor = true;
            this.ForceSyncButton.Click += new System.EventHandler(this.ForceSyncButton_Click);
            // 
            // LastVerifiedLabel
            // 
            this.LastVerifiedLabel.Location = new System.Drawing.Point(12, 134);
            this.LastVerifiedLabel.Name = "LastVerifiedLabel";
            this.LastVerifiedLabel.Size = new System.Drawing.Size(152, 16);
            this.LastVerifiedLabel.TabIndex = 19;
            this.LastVerifiedLabel.Text = "Last verified with OneDrive:";
            this.LastVerifiedLabel.UseMnemonic = false;
            // 
            // LastVerifiedTextbox
            // 
            this.LastVerifiedTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LastVerifiedTextbox.BackColor = System.Drawing.SystemColors.Control;
            this.LastVerifiedTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LastVerifiedTextbox.Location = new System.Drawing.Point(173, 134);
            this.LastVerifiedTextbox.Name = "LastVerifiedTextbox";
            this.LastVerifiedTextbox.ReadOnly = true;
            this.LastVerifiedTextbox.Size = new System.Drawing.Size(355, 13);
            this.LastVerifiedTextbox.TabIndex = 20;
            this.LastVerifiedTextbox.Text = "?";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusLabel.Location = new System.Drawing.Point(120, 185);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(198, 36);
            this.StatusLabel.TabIndex = 21;
            this.StatusLabel.Text = "Ready";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.StatusLabel.UseMnemonic = false;
            // 
            // RefreshTokenStorageTextBox
            // 
            this.RefreshTokenStorageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshTokenStorageTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.RefreshTokenStorageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RefreshTokenStorageTextBox.Location = new System.Drawing.Point(173, 92);
            this.RefreshTokenStorageTextBox.Name = "RefreshTokenStorageTextBox";
            this.RefreshTokenStorageTextBox.ReadOnly = true;
            this.RefreshTokenStorageTextBox.Size = new System.Drawing.Size(355, 13);
            this.RefreshTokenStorageTextBox.TabIndex = 22;
            this.RefreshTokenStorageTextBox.Text = "?";
            // 
            // RefreshTokenStorageLabel
            // 
            this.RefreshTokenStorageLabel.Location = new System.Drawing.Point(12, 92);
            this.RefreshTokenStorageLabel.Name = "RefreshTokenStorageLabel";
            this.RefreshTokenStorageLabel.Size = new System.Drawing.Size(152, 16);
            this.RefreshTokenStorageLabel.TabIndex = 23;
            this.RefreshTokenStorageLabel.Text = "Refresh Token Storage:";
            this.RefreshTokenStorageLabel.UseMnemonic = false;
            // 
            // OneDriveConfigDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(540, 230);
            this.Controls.Add(this.RefreshTokenStorageTextBox);
            this.Controls.Add(this.RefreshTokenStorageLabel);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.LastVerifiedTextbox);
            this.Controls.Add(this.LastVerifiedLabel);
            this.Controls.Add(this.ForceSyncButton);
            this.Controls.Add(this.LocalKeePassFileHashTextbox);
            this.Controls.Add(this.LocalKeePassFileHashLabel);
            this.Controls.Add(this.LastSyncedTextbox);
            this.Controls.Add(this.LastSyncedLabel);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.OneDriveNameTextbox);
            this.Controls.Add(this.LocalKeePassPathTextbox);
            this.Controls.Add(this.OneDriveRefreshTokenTextbox);
            this.Controls.Add(this.RemoteKeePassPathTextbox);
            this.Controls.Add(this.OneDriveRefreshTokenLabel);
            this.Controls.Add(this.RemoteKeePassPathLabel);
            this.Controls.Add(this.LocalKeePassPathLabel);
            this.Controls.Add(this.OneDriveNameLabel);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(516, 238);
            this.Name = "OneDriveConfigDetailsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OneDriveSync Configuration";
            this.Load += new System.EventHandler(this.OneDriveConfigDetailsForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OneDriveConfigDetailsForm_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label OneDriveNameLabel;
        private System.Windows.Forms.Label LocalKeePassPathLabel;
        private System.Windows.Forms.Label RemoteKeePassPathLabel;
        private System.Windows.Forms.Label OneDriveRefreshTokenLabel;
        private System.Windows.Forms.TextBox RemoteKeePassPathTextbox;
        private System.Windows.Forms.TextBox OneDriveRefreshTokenTextbox;
        private System.Windows.Forms.TextBox LocalKeePassPathTextbox;
        private System.Windows.Forms.TextBox OneDriveNameTextbox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Label LastSyncedLabel;
        private System.Windows.Forms.TextBox LastSyncedTextbox;
        private System.Windows.Forms.Label LocalKeePassFileHashLabel;
        private System.Windows.Forms.TextBox LocalKeePassFileHashTextbox;
        private System.Windows.Forms.Button ForceSyncButton;
        private System.Windows.Forms.Label LastVerifiedLabel;
        private System.Windows.Forms.TextBox LastVerifiedTextbox;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.TextBox RefreshTokenStorageTextBox;
        private System.Windows.Forms.Label RefreshTokenStorageLabel;
    }
}