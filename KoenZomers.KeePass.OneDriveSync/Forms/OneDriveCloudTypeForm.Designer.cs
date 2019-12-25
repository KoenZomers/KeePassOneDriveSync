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
            this.OneDriveTab = new System.Windows.Forms.TabPage();
            this.GraphDeviceLoginPictureButton = new System.Windows.Forms.Button();
            this.GraphPictureButton = new System.Windows.Forms.Button();
            this.SharePointTab = new System.Windows.Forms.TabPage();
            this.SharePointPictureButton = new System.Windows.Forms.Button();
            this.OtherTab = new System.Windows.Forms.TabPage();
            this.OtherLabel = new System.Windows.Forms.Label();
            this.OneDriveConsumerPictureButton = new System.Windows.Forms.Button();
            this.CloudPlatformTabs.SuspendLayout();
            this.OneDriveTab.SuspendLayout();
            this.SharePointTab.SuspendLayout();
            this.OtherTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(133, 281);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(112, 28);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExplanationLabel.Location = new System.Drawing.Point(15, 6);
            this.ExplanationLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(354, 24);
            this.ExplanationLabel.TabIndex = 0;
            this.ExplanationLabel.Text = "Choose the cloud service you wish to store the KeePass database on:";
            // 
            // CloudPlatformTabs
            // 
            this.CloudPlatformTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CloudPlatformTabs.Controls.Add(this.OneDriveTab);
            this.CloudPlatformTabs.Controls.Add(this.SharePointTab);
            this.CloudPlatformTabs.Controls.Add(this.OtherTab);
            this.CloudPlatformTabs.Location = new System.Drawing.Point(17, 31);
            this.CloudPlatformTabs.Margin = new System.Windows.Forms.Padding(2);
            this.CloudPlatformTabs.Name = "CloudPlatformTabs";
            this.CloudPlatformTabs.SelectedIndex = 0;
            this.CloudPlatformTabs.Size = new System.Drawing.Size(343, 246);
            this.CloudPlatformTabs.TabIndex = 0;
            // 
            // OneDriveTab
            // 
            this.OneDriveTab.Controls.Add(this.GraphDeviceLoginPictureButton);
            this.OneDriveTab.Controls.Add(this.GraphPictureButton);
            this.OneDriveTab.Location = new System.Drawing.Point(4, 22);
            this.OneDriveTab.Margin = new System.Windows.Forms.Padding(2);
            this.OneDriveTab.Name = "OneDriveTab";
            this.OneDriveTab.Size = new System.Drawing.Size(335, 220);
            this.OneDriveTab.TabIndex = 0;
            this.OneDriveTab.Text = "OneDrive";
            this.OneDriveTab.UseVisualStyleBackColor = true;
            // 
            // GraphDeviceLoginPictureButton
            // 
            this.GraphDeviceLoginPictureButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GraphDeviceLoginPictureButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.GraphDeviceLoginPictureButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.GraphDeviceLoginPictureButton.Location = new System.Drawing.Point(7, 10);
            this.GraphDeviceLoginPictureButton.Margin = new System.Windows.Forms.Padding(2);
            this.GraphDeviceLoginPictureButton.Name = "GraphDeviceLoginPictureButton";
            this.GraphDeviceLoginPictureButton.Size = new System.Drawing.Size(322, 98);
            this.GraphDeviceLoginPictureButton.TabIndex = 0;
            this.GraphDeviceLoginPictureButton.Text = "&Any browser (recommended)";
            this.GraphDeviceLoginPictureButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.GraphDeviceLoginPictureButton.UseVisualStyleBackColor = false;
            this.GraphDeviceLoginPictureButton.Click += new System.EventHandler(this.GraphDeviceLoginPictureButton_Click);
            // 
            // GraphPictureButton
            // 
            this.GraphPictureButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GraphPictureButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.GraphPictureButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.GraphPictureButton.Location = new System.Drawing.Point(7, 114);
            this.GraphPictureButton.Margin = new System.Windows.Forms.Padding(2);
            this.GraphPictureButton.Name = "GraphPictureButton";
            this.GraphPictureButton.Size = new System.Drawing.Size(322, 98);
            this.GraphPictureButton.TabIndex = 1;
            this.GraphPictureButton.Text = "&Built-in browser";
            this.GraphPictureButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.GraphPictureButton.UseVisualStyleBackColor = false;
            this.GraphPictureButton.Click += new System.EventHandler(this.GraphPictureButton_Click);
            // 
            // SharePointTab
            // 
            this.SharePointTab.Controls.Add(this.SharePointPictureButton);
            this.SharePointTab.Location = new System.Drawing.Point(4, 22);
            this.SharePointTab.Margin = new System.Windows.Forms.Padding(2);
            this.SharePointTab.Name = "SharePointTab";
            this.SharePointTab.Size = new System.Drawing.Size(335, 220);
            this.SharePointTab.TabIndex = 1;
            this.SharePointTab.Text = "SharePoint";
            this.SharePointTab.UseVisualStyleBackColor = true;
            // 
            // SharePointPictureButton
            // 
            this.SharePointPictureButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SharePointPictureButton.BackColor = System.Drawing.Color.White;
            this.SharePointPictureButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SharePointPictureButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SharePointPictureButton.Location = new System.Drawing.Point(8, 11);
            this.SharePointPictureButton.Margin = new System.Windows.Forms.Padding(2);
            this.SharePointPictureButton.Name = "SharePointPictureButton";
            this.SharePointPictureButton.Size = new System.Drawing.Size(322, 84);
            this.SharePointPictureButton.TabIndex = 8;
            this.SharePointPictureButton.Text = "SharePoint 2013, 2016, 2019 & Online";
            this.SharePointPictureButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.SharePointPictureButton.UseMnemonic = false;
            this.SharePointPictureButton.UseVisualStyleBackColor = false;
            this.SharePointPictureButton.Click += new System.EventHandler(this.SharePointPictureButton_Click);
            // 
            // OtherTab
            // 
            this.OtherTab.Controls.Add(this.OtherLabel);
            this.OtherTab.Controls.Add(this.OneDriveConsumerPictureButton);
            this.OtherTab.Location = new System.Drawing.Point(4, 22);
            this.OtherTab.Margin = new System.Windows.Forms.Padding(2);
            this.OtherTab.Name = "OtherTab";
            this.OtherTab.Size = new System.Drawing.Size(335, 220);
            this.OtherTab.TabIndex = 2;
            this.OtherTab.Text = "Other";
            this.OtherTab.UseVisualStyleBackColor = true;
            // 
            // OtherLabel
            // 
            this.OtherLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OtherLabel.Location = new System.Drawing.Point(10, 11);
            this.OtherLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.OtherLabel.Name = "OtherLabel";
            this.OtherLabel.Size = new System.Drawing.Size(319, 22);
            this.OtherLabel.TabIndex = 9;
            this.OtherLabel.Text = "Only use this if the OneDrive tab doesn\'t work";
            this.OtherLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OneDriveConsumerPictureButton
            // 
            this.OneDriveConsumerPictureButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OneDriveConsumerPictureButton.BackColor = System.Drawing.Color.White;
            this.OneDriveConsumerPictureButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.OneDriveConsumerPictureButton.ForeColor = System.Drawing.Color.White;
            this.OneDriveConsumerPictureButton.Location = new System.Drawing.Point(8, 36);
            this.OneDriveConsumerPictureButton.Margin = new System.Windows.Forms.Padding(2);
            this.OneDriveConsumerPictureButton.Name = "OneDriveConsumerPictureButton";
            this.OneDriveConsumerPictureButton.Size = new System.Drawing.Size(322, 84);
            this.OneDriveConsumerPictureButton.TabIndex = 7;
            this.OneDriveConsumerPictureButton.Text = "OneDrive";
            this.OneDriveConsumerPictureButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.OneDriveConsumerPictureButton.UseVisualStyleBackColor = false;
            this.OneDriveConsumerPictureButton.Click += new System.EventHandler(this.OneDriveConsumerPictureButton_Click);
            // 
            // OneDriveCloudTypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 318);
            this.Controls.Add(this.CloudPlatformTabs);
            this.Controls.Add(this.ExplanationLabel);
            this.Controls.Add(this.CancelButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OneDriveCloudTypeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Cloud Platform";
            this.CloudPlatformTabs.ResumeLayout(false);
            this.OneDriveTab.ResumeLayout(false);
            this.SharePointTab.ResumeLayout(false);
            this.OtherTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.TabControl CloudPlatformTabs;
        private System.Windows.Forms.TabPage OneDriveTab;
        private System.Windows.Forms.Button GraphPictureButton;
        private System.Windows.Forms.TabPage SharePointTab;
        private System.Windows.Forms.TabPage OtherTab;
        private System.Windows.Forms.Label OtherLabel;
        private System.Windows.Forms.Button OneDriveConsumerPictureButton;
        private System.Windows.Forms.Button SharePointPictureButton;
        private System.Windows.Forms.Button GraphDeviceLoginPictureButton;
    }
}