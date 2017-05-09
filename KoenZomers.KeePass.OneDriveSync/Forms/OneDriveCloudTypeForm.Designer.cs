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
            this.OneDriveConsumerPictureButton = new System.Windows.Forms.Button();
            this.OneDriveForBusinessPictureButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(177, 346);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(149, 35);
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExplanationLabel.Location = new System.Drawing.Point(20, 7);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(472, 39);
            this.ExplanationLabel.TabIndex = 3;
            this.ExplanationLabel.Text = "Choose the cloud service you wish to store the KeePass database on:";
            // 
            // OneDriveConsumerPictureButton
            // 
            this.OneDriveConsumerPictureButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OneDriveConsumerPictureButton.BackColor = System.Drawing.Color.White;
            this.OneDriveConsumerPictureButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.OneDriveConsumerPictureButton.Location = new System.Drawing.Point(23, 57);
            this.OneDriveConsumerPictureButton.Name = "OneDriveConsumerPictureButton";
            this.OneDriveConsumerPictureButton.Size = new System.Drawing.Size(457, 138);
            this.OneDriveConsumerPictureButton.TabIndex = 0;
            this.OneDriveConsumerPictureButton.Text = "OneDrive";
            this.OneDriveConsumerPictureButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.OneDriveConsumerPictureButton.UseVisualStyleBackColor = false;
            this.OneDriveConsumerPictureButton.Click += new System.EventHandler(this.OneDriveConsumerPictureButton_Click);
            // 
            // OneDriveForBusinessPictureButton
            // 
            this.OneDriveForBusinessPictureButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OneDriveForBusinessPictureButton.BackColor = System.Drawing.Color.White;
            this.OneDriveForBusinessPictureButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.OneDriveForBusinessPictureButton.Location = new System.Drawing.Point(23, 203);
            this.OneDriveForBusinessPictureButton.Name = "OneDriveForBusinessPictureButton";
            this.OneDriveForBusinessPictureButton.Size = new System.Drawing.Size(457, 138);
            this.OneDriveForBusinessPictureButton.TabIndex = 2;
            this.OneDriveForBusinessPictureButton.Text = "OneDrive for Business";
            this.OneDriveForBusinessPictureButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.OneDriveForBusinessPictureButton.UseVisualStyleBackColor = false;
            this.OneDriveForBusinessPictureButton.Click += new System.EventHandler(this.OneDriveForBusinessPictureButton_Click);
            // 
            // OneDriveCloudTypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 392);
            this.Controls.Add(this.OneDriveForBusinessPictureButton);
            this.Controls.Add(this.OneDriveConsumerPictureButton);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.Button OneDriveConsumerPictureButton;
        private System.Windows.Forms.Button OneDriveForBusinessPictureButton;
    }
}