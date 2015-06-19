namespace KoenZomersKeePassOneDriveSync
{
    partial class OneDriveAskToStartSyncing
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
            this.YesRadio = new System.Windows.Forms.RadioButton();
            this.NotNowRadio = new System.Windows.Forms.RadioButton();
            this.NoNeverAskAgainRadio = new System.Windows.Forms.RadioButton();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExplanationLabel.Location = new System.Drawing.Point(12, 9);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(535, 60);
            this.ExplanationLabel.TabIndex = 0;
            this.ExplanationLabel.Text = "This KeePass database is not being synced with OneDrive. Would you like to set up" +
    " a synchronization connect now?";
            // 
            // YesRadio
            // 
            this.YesRadio.AutoSize = true;
            this.YesRadio.Checked = true;
            this.YesRadio.Location = new System.Drawing.Point(15, 72);
            this.YesRadio.Name = "YesRadio";
            this.YesRadio.Size = new System.Drawing.Size(50, 20);
            this.YesRadio.TabIndex = 1;
            this.YesRadio.TabStop = true;
            this.YesRadio.Text = "Yes";
            this.YesRadio.UseVisualStyleBackColor = true;
            // 
            // NotNowRadio
            // 
            this.NotNowRadio.AutoSize = true;
            this.NotNowRadio.Location = new System.Drawing.Point(15, 98);
            this.NotNowRadio.Name = "NotNowRadio";
            this.NotNowRadio.Size = new System.Drawing.Size(74, 20);
            this.NotNowRadio.TabIndex = 2;
            this.NotNowRadio.Text = "Not now";
            this.NotNowRadio.UseVisualStyleBackColor = true;
            // 
            // NoNeverAskAgainRadio
            // 
            this.NoNeverAskAgainRadio.AutoSize = true;
            this.NoNeverAskAgainRadio.Location = new System.Drawing.Point(15, 124);
            this.NoNeverAskAgainRadio.Name = "NoNeverAskAgainRadio";
            this.NoNeverAskAgainRadio.Size = new System.Drawing.Size(169, 20);
            this.NoNeverAskAgainRadio.TabIndex = 3;
            this.NoNeverAskAgainRadio.Text = "No and never ask again";
            this.NoNeverAskAgainRadio.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OKButton.Location = new System.Drawing.Point(169, 165);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(99, 51);
            this.OKButton.TabIndex = 5;
            this.OKButton.Text = "&OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(274, 165);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(99, 51);
            this.CancelButton.TabIndex = 6;
            this.CancelButton.Text = "&Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OneDriveAskToStartSyncing
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 228);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.NoNeverAskAgainRadio);
            this.Controls.Add(this.NotNowRadio);
            this.Controls.Add(this.YesRadio);
            this.Controls.Add(this.ExplanationLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OneDriveAskToStartSyncing";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Start syncing with OneDrive";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        public System.Windows.Forms.RadioButton YesRadio;
        public System.Windows.Forms.RadioButton NotNowRadio;
        public System.Windows.Forms.RadioButton NoNeverAskAgainRadio;
    }
}