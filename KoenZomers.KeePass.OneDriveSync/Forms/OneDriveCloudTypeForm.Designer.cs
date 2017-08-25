namespace KoenZomersKeePassOneDriveSync
{
    partial class OneDriveCloudTypeForm
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
            this.CancelButton = new System.Windows.Forms.Button();
            this.ExplanationLabel = new System.Windows.Forms.Label();
            this.CloudPlatformTabs = new System.Windows.Forms.TabControl();
            this.OnlineTab = new System.Windows.Forms.TabPage();
            this.GraphPictureButton = new System.Windows.Forms.Button();
            this.OnPremisesTab = new System.Windows.Forms.TabPage();
            this.OnPremisesComingLabel = new System.Windows.Forms.Label();
            this.OtherTab = new System.Windows.Forms.TabPage();
            this.OtherLabel = new System.Windows.Forms.Label();
            this.OneDriveForBusinessPictureButton = new System.Windows.Forms.Button();
            this.OneDriveConsumerPictureButton = new System.Windows.Forms.Button();
            this.CloudPlatformTabs.SuspendLayout();
            this.OnlineTab.SuspendLayout();
            this.OnPremisesTab.SuspendLayout();
            this.OtherTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(199, 432);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(168, 44);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExplanationLabel.Location = new System.Drawing.Point(22, 9);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(531, 36);
            this.ExplanationLabel.TabIndex = 0;
            this.ExplanationLabel.Text = "Choose the cloud service you wish to store the KeePass database on:";
            // 
            // CloudPlatformTabs
            // 
            this.CloudPlatformTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CloudPlatformTabs.Controls.Add(this.OnlineTab);
            this.CloudPlatformTabs.Controls.Add(this.OnPremisesTab);
            this.CloudPlatformTabs.Controls.Add(this.OtherTab);
            this.CloudPlatformTabs.Location = new System.Drawing.Point(26, 48);
            this.CloudPlatformTabs.Name = "CloudPlatformTabs";
            this.CloudPlatformTabs.SelectedIndex = 0;
            this.CloudPlatformTabs.Size = new System.Drawing.Size(514, 379);
            this.CloudPlatformTabs.TabIndex = 0;
            // 
            // OnlineTab
            // 
            this.OnlineTab.Controls.Add(this.GraphPictureButton);
            this.OnlineTab.Location = new System.Drawing.Point(4, 29);
            this.OnlineTab.Name = "OnlineTab";
            this.OnlineTab.Size = new System.Drawing.Size(506, 346);
            this.OnlineTab.TabIndex = 0;
            this.OnlineTab.Text = "Online";
            this.OnlineTab.UseVisualStyleBackColor = true;
            // 
            // GraphPictureButton
            // 
            this.GraphPictureButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GraphPictureButton.BackColor = System.Drawing.Color.White;
            this.GraphPictureButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.GraphPictureButton.Location = new System.Drawing.Point(11, 17);
            this.GraphPictureButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GraphPictureButton.Name = "GraphPictureButton";
            this.GraphPictureButton.Size = new System.Drawing.Size(483, 150);
            this.GraphPictureButton.TabIndex = 0;
            this.GraphPictureButton.Text = "OneDrive & OneDrive for Business";
            this.GraphPictureButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.GraphPictureButton.UseMnemonic = false;
            this.GraphPictureButton.UseVisualStyleBackColor = false;
            this.GraphPictureButton.Click += new System.EventHandler(this.GraphPictureButton_Click);
            // 
            // OnPremisesTab
            // 
            this.OnPremisesTab.Controls.Add(this.OnPremisesComingLabel);
            this.OnPremisesTab.Location = new System.Drawing.Point(4, 29);
            this.OnPremisesTab.Name = "OnPremisesTab";
            this.OnPremisesTab.Size = new System.Drawing.Size(506, 346);
            this.OnPremisesTab.TabIndex = 1;
            this.OnPremisesTab.Text = "On-Premises";
            this.OnPremisesTab.UseVisualStyleBackColor = true;
            // 
            // OnPremisesComingLabel
            // 
            this.OnPremisesComingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OnPremisesComingLabel.Location = new System.Drawing.Point(14, 12);
            this.OnPremisesComingLabel.Name = "OnPremisesComingLabel";
            this.OnPremisesComingLabel.Size = new System.Drawing.Size(474, 320);
            this.OnPremisesComingLabel.TabIndex = 0;
            this.OnPremisesComingLabel.Text = "Coming in the future";
            this.OnPremisesComingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.OnPremisesComingLabel.UseMnemonic = false;
            // 
            // OtherTab
            // 
            this.OtherTab.Controls.Add(this.OtherLabel);
            this.OtherTab.Controls.Add(this.OneDriveForBusinessPictureButton);
            this.OtherTab.Controls.Add(this.OneDriveConsumerPictureButton);
            this.OtherTab.Location = new System.Drawing.Point(4, 29);
            this.OtherTab.Name = "OtherTab";
            this.OtherTab.Size = new System.Drawing.Size(506, 346);
            this.OtherTab.TabIndex = 2;
            this.OtherTab.Text = "Other";
            this.OtherTab.UseVisualStyleBackColor = true;
            // 
            // OtherLabel
            // 
            this.OtherLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OtherLabel.Location = new System.Drawing.Point(16, 17);
            this.OtherLabel.Name = "OtherLabel";
            this.OtherLabel.Size = new System.Drawing.Size(478, 34);
            this.OtherLabel.TabIndex = 9;
            this.OtherLabel.Text = "Only use these if the Online tab doesn\'t work";
            this.OtherLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OneDriveForBusinessPictureButton
            // 
            this.OneDriveForBusinessPictureButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OneDriveForBusinessPictureButton.BackColor = System.Drawing.Color.White;
            this.OneDriveForBusinessPictureButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.OneDriveForBusinessPictureButton.Location = new System.Drawing.Point(11, 196);
            this.OneDriveForBusinessPictureButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OneDriveForBusinessPictureButton.Name = "OneDriveForBusinessPictureButton";
            this.OneDriveForBusinessPictureButton.Size = new System.Drawing.Size(483, 130);
            this.OneDriveForBusinessPictureButton.TabIndex = 8;
            this.OneDriveForBusinessPictureButton.Text = "OneDrive for Business";
            this.OneDriveForBusinessPictureButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.OneDriveForBusinessPictureButton.UseVisualStyleBackColor = false;
            this.OneDriveForBusinessPictureButton.Click += new System.EventHandler(this.OneDriveForBusinessPictureButton_Click);
            // 
            // OneDriveConsumerPictureButton
            // 
            this.OneDriveConsumerPictureButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OneDriveConsumerPictureButton.BackColor = System.Drawing.Color.White;
            this.OneDriveConsumerPictureButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.OneDriveConsumerPictureButton.ForeColor = System.Drawing.Color.White;
            this.OneDriveConsumerPictureButton.Location = new System.Drawing.Point(11, 55);
            this.OneDriveConsumerPictureButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OneDriveConsumerPictureButton.Name = "OneDriveConsumerPictureButton";
            this.OneDriveConsumerPictureButton.Size = new System.Drawing.Size(483, 130);
            this.OneDriveConsumerPictureButton.TabIndex = 7;
            this.OneDriveConsumerPictureButton.Text = "OneDrive";
            this.OneDriveConsumerPictureButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.OneDriveConsumerPictureButton.UseVisualStyleBackColor = false;
            this.OneDriveConsumerPictureButton.Click += new System.EventHandler(this.OneDriveConsumerPictureButton_Click);
            // 
            // OneDriveCloudTypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 490);
            this.Controls.Add(this.CloudPlatformTabs);
            this.Controls.Add(this.ExplanationLabel);
            this.Controls.Add(this.CancelButton);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OneDriveCloudTypeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Cloud Platform";
            this.CloudPlatformTabs.ResumeLayout(false);
            this.OnlineTab.ResumeLayout(false);
            this.OnPremisesTab.ResumeLayout(false);
            this.OtherTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.TabControl CloudPlatformTabs;
        private System.Windows.Forms.TabPage OnlineTab;
        private System.Windows.Forms.Button GraphPictureButton;
        private System.Windows.Forms.TabPage OnPremisesTab;
        private System.Windows.Forms.Label OnPremisesComingLabel;
        private System.Windows.Forms.TabPage OtherTab;
        private System.Windows.Forms.Label OtherLabel;
        private System.Windows.Forms.Button OneDriveForBusinessPictureButton;
        private System.Windows.Forms.Button OneDriveConsumerPictureButton;
    }
}