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
            this.SharePointOnlinePictureButton = new System.Windows.Forms.Button();
            this.OneDrivePictureButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(200, 458);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(168, 43);
            this.CancelButton.TabIndex = 4;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExplanationLabel.Location = new System.Drawing.Point(22, 9);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(531, 37);
            this.ExplanationLabel.TabIndex = 0;
            this.ExplanationLabel.Text = "Choose the cloud service you wish to store the KeePass database on:";
            // 
            // SharePointOnlinePictureButton
            // 
            this.SharePointOnlinePictureButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SharePointOnlinePictureButton.BackColor = System.Drawing.Color.White;
            this.SharePointOnlinePictureButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SharePointOnlinePictureButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SharePointOnlinePictureButton.Location = new System.Drawing.Point(26, 181);
            this.SharePointOnlinePictureButton.Name = "SharePointOnlinePictureButton";
            this.SharePointOnlinePictureButton.Size = new System.Drawing.Size(505, 129);
            this.SharePointOnlinePictureButton.TabIndex = 2;
            this.SharePointOnlinePictureButton.Text = "SharePoint Online";
            this.SharePointOnlinePictureButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.SharePointOnlinePictureButton.UseMnemonic = false;
            this.SharePointOnlinePictureButton.UseVisualStyleBackColor = false;
            this.SharePointOnlinePictureButton.Click += new System.EventHandler(this.SharePointOnlinePictureButton_Click);
            // 
            // OneDrivePictureButton
            // 
            this.OneDrivePictureButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OneDrivePictureButton.BackColor = System.Drawing.Color.White;
            this.OneDrivePictureButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.OneDrivePictureButton.Location = new System.Drawing.Point(26, 49);
            this.OneDrivePictureButton.Name = "OneDrivePictureButton";
            this.OneDrivePictureButton.Size = new System.Drawing.Size(505, 126);
            this.OneDrivePictureButton.TabIndex = 1;
            this.OneDrivePictureButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.OneDrivePictureButton.UseVisualStyleBackColor = false;
            this.OneDrivePictureButton.Click += new System.EventHandler(this.OneDrivePictureButton_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Location = new System.Drawing.Point(26, 316);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(505, 129);
            this.button1.TabIndex = 3;
            this.button1.Text = "SharePoint 2013, 2016, 2019 & SPSE";
            this.button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.UseMnemonic = false;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.SharePointOnPremisesPictureButton_Click);
            // 
            // OneDriveCloudTypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 515);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.OneDrivePictureButton);
            this.Controls.Add(this.SharePointOnlinePictureButton);
            this.Controls.Add(this.ExplanationLabel);
            this.Controls.Add(this.CancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OneDriveCloudTypeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Cloud Platform";
            this.ResumeLayout(false);

        }

        #endregion

        private new System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.Button SharePointOnlinePictureButton;
        private System.Windows.Forms.Button OneDrivePictureButton;
        private System.Windows.Forms.Button button1;
    }
}