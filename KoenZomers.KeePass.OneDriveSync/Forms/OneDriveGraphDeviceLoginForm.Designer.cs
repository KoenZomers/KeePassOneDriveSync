namespace KoenZomersKeePassOneDriveSync
{
    partial class OneDriveGraphDeviceLoginForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OneDriveGraphDeviceLoginForm));
            this.MicrosoftLogoPictureBox = new System.Windows.Forms.PictureBox();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.DeviceIdTextBox = new System.Windows.Forms.TextBox();
            this.ExplanationLabel = new System.Windows.Forms.Label();
            this.DeviceAuthLinkLabel = new System.Windows.Forms.LinkLabel();
            this.OpenBrowserButton = new System.Windows.Forms.Button();
            this.CopyDeviceIdButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.StartSessionTimer = new System.Windows.Forms.Timer(this.components);
            this.AuthenticationCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.AuthenticationCompleteTimer = new System.Windows.Forms.Timer(this.components);
            this.MoreInformationLinkLabel = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.MicrosoftLogoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // MicrosoftLogoPictureBox
            // 
            this.MicrosoftLogoPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MicrosoftLogoPictureBox.BackColor = System.Drawing.Color.White;
            this.MicrosoftLogoPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.MicrosoftLogoPictureBox.Location = new System.Drawing.Point(12, 12);
            this.MicrosoftLogoPictureBox.Name = "MicrosoftLogoPictureBox";
            this.MicrosoftLogoPictureBox.Size = new System.Drawing.Size(497, 103);
            this.MicrosoftLogoPictureBox.TabIndex = 0;
            this.MicrosoftLogoPictureBox.TabStop = false;
            // 
            // StatusLabel
            // 
            this.StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusLabel.Location = new System.Drawing.Point(12, 244);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(497, 31);
            this.StatusLabel.TabIndex = 1;
            this.StatusLabel.Text = "Initializing";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DeviceIdTextBox
            // 
            this.DeviceIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DeviceIdTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.DeviceIdTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DeviceIdTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceIdTextBox.Location = new System.Drawing.Point(10, 284);
            this.DeviceIdTextBox.Name = "DeviceIdTextBox";
            this.DeviceIdTextBox.Size = new System.Drawing.Size(497, 37);
            this.DeviceIdTextBox.TabIndex = 2;
            this.DeviceIdTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DeviceIdTextBox.Visible = false;
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExplanationLabel.Location = new System.Drawing.Point(12, 121);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(497, 85);
            this.ExplanationLabel.TabIndex = 3;
            this.ExplanationLabel.Text = "A unique device ID will be retrieved from Microsoft to perform your logon to OneDrive or OneDrive for Business through the Microsoft Graph in your favorite web browser. Once the device ID is shown below, click on the link that shows up on this form to open your default browser, paste in the device ID and perform the authentication to continue with setting up your KeePass synchronization. Leave this form open while you authenticate in your browser. Once you've authenticated in your browser, you may close your browser and this application should automatically proceed.";
            this.ExplanationLabel.UseMnemonic = false;
            // 
            // DeviceAuthLinkLabel
            // 
            this.DeviceAuthLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DeviceAuthLinkLabel.Location = new System.Drawing.Point(12, 220);
            this.DeviceAuthLinkLabel.Name = "DeviceAuthLinkLabel";
            this.DeviceAuthLinkLabel.Size = new System.Drawing.Size(497, 23);
            this.DeviceAuthLinkLabel.TabIndex = 4;
            this.DeviceAuthLinkLabel.TabStop = true;
            this.DeviceAuthLinkLabel.Text = "Unknown";
            this.DeviceAuthLinkLabel.UseMnemonic = false;
            this.DeviceAuthLinkLabel.Visible = false;
            this.DeviceAuthLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.DeviceAuthLinkLabel_LinkClicked);
            // 
            // OpenBrowserButton
            // 
            this.OpenBrowserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OpenBrowserButton.Enabled = false;
            this.OpenBrowserButton.Location = new System.Drawing.Point(10, 334);
            this.OpenBrowserButton.Name = "OpenBrowserButton";
            this.OpenBrowserButton.Size = new System.Drawing.Size(106, 37);
            this.OpenBrowserButton.TabIndex = 5;
            this.OpenBrowserButton.Text = "&Open in browser";
            this.OpenBrowserButton.UseVisualStyleBackColor = true;
            this.OpenBrowserButton.Click += new System.EventHandler(this.OpenBrowserButton_Click);
            // 
            // CopyDeviceIdButton
            // 
            this.CopyDeviceIdButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CopyDeviceIdButton.Enabled = false;
            this.CopyDeviceIdButton.Location = new System.Drawing.Point(131, 334);
            this.CopyDeviceIdButton.Name = "CopyDeviceIdButton";
            this.CopyDeviceIdButton.Size = new System.Drawing.Size(106, 37);
            this.CopyDeviceIdButton.TabIndex = 6;
            this.CopyDeviceIdButton.Text = "&Copy Device ID";
            this.CopyDeviceIdButton.UseVisualStyleBackColor = true;
            this.CopyDeviceIdButton.Click += new System.EventHandler(this.CopyDeviceIdButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(403, 334);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(106, 37);
            this.CancelButton.TabIndex = 7;
            this.CancelButton.Text = "C&ancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // StartSessionTimer
            // 
            this.StartSessionTimer.Interval = 1000;
            this.StartSessionTimer.Tick += new System.EventHandler(this.StartSessionTimer_Tick);
            // 
            // AuthenticationCheckTimer
            // 
            this.AuthenticationCheckTimer.Interval = 1000;
            this.AuthenticationCheckTimer.Tick += new System.EventHandler(this.AuthenticationCheckTimer_Tick);
            // 
            // AuthenticationCompleteTimer
            // 
            this.AuthenticationCompleteTimer.Interval = 3000;
            this.AuthenticationCompleteTimer.Tick += new System.EventHandler(this.AuthenticationCompleteTimer_Tick);
            // 
            // MoreInformationLinkLabel
            // 
            this.MoreInformationLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MoreInformationLinkLabel.AutoSize = true;
            this.MoreInformationLinkLabel.Location = new System.Drawing.Point(259, 346);
            this.MoreInformationLinkLabel.Name = "MoreInformationLinkLabel";
            this.MoreInformationLinkLabel.Size = new System.Drawing.Size(85, 13);
            this.MoreInformationLinkLabel.TabIndex = 8;
            this.MoreInformationLinkLabel.TabStop = true;
            this.MoreInformationLinkLabel.Text = "More information";
            this.MoreInformationLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.MoreInformationLinkLabel_LinkClicked);
            // 
            // OneDriveGraphDeviceLoginForm
            // 
            this.AcceptButton = this.OpenBrowserButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 383);
            this.Controls.Add(this.MoreInformationLinkLabel);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.CopyDeviceIdButton);
            this.Controls.Add(this.OpenBrowserButton);
            this.Controls.Add(this.DeviceAuthLinkLabel);
            this.Controls.Add(this.ExplanationLabel);
            this.Controls.Add(this.DeviceIdTextBox);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.MicrosoftLogoPictureBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(459, 422);
            this.Name = "OneDriveGraphDeviceLoginForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Authenticate to OneDrive";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OneDriveGraphDeviceLoginForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.MicrosoftLogoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox MicrosoftLogoPictureBox;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.TextBox DeviceIdTextBox;
        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.LinkLabel DeviceAuthLinkLabel;
        private System.Windows.Forms.Button OpenBrowserButton;
        private System.Windows.Forms.Button CopyDeviceIdButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Timer StartSessionTimer;
        private System.Windows.Forms.Timer AuthenticationCheckTimer;
        private System.Windows.Forms.Timer AuthenticationCompleteTimer;
        private System.Windows.Forms.LinkLabel MoreInformationLinkLabel;
    }
}