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
            this.OneDriveConsumerPicture = new System.Windows.Forms.PictureBox();
            this.OneDriveForBusinessPicture = new System.Windows.Forms.PictureBox();
            this.ExplanationLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.OneDriveConsumerPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OneDriveForBusinessPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(189, 370);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(168, 44);
            this.CancelButton.TabIndex = 0;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // OneDriveConsumerPicture
            // 
            this.OneDriveConsumerPicture.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OneDriveConsumerPicture.Location = new System.Drawing.Point(114, 61);
            this.OneDriveConsumerPicture.Name = "OneDriveConsumerPicture";
            this.OneDriveConsumerPicture.Size = new System.Drawing.Size(347, 124);
            this.OneDriveConsumerPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.OneDriveConsumerPicture.TabIndex = 1;
            this.OneDriveConsumerPicture.TabStop = false;
            this.OneDriveConsumerPicture.Click += new System.EventHandler(this.OneDriveConsumerPicture_Click);
            // 
            // OneDriveForBusinessPicture
            // 
            this.OneDriveForBusinessPicture.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OneDriveForBusinessPicture.Location = new System.Drawing.Point(114, 205);
            this.OneDriveForBusinessPicture.Name = "OneDriveForBusinessPicture";
            this.OneDriveForBusinessPicture.Size = new System.Drawing.Size(347, 124);
            this.OneDriveForBusinessPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.OneDriveForBusinessPicture.TabIndex = 2;
            this.OneDriveForBusinessPicture.TabStop = false;
            this.OneDriveForBusinessPicture.Click += new System.EventHandler(this.OneDriveForBusinessPicture_Click);
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExplanationLabel.Location = new System.Drawing.Point(22, 9);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(531, 49);
            this.ExplanationLabel.TabIndex = 3;
            this.ExplanationLabel.Text = "Choose the cloud service you wish to store the KeePass database on:";
            // 
            // OneDriveCloudTypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 455);
            this.Controls.Add(this.ExplanationLabel);
            this.Controls.Add(this.OneDriveForBusinessPicture);
            this.Controls.Add(this.OneDriveConsumerPicture);
            this.Controls.Add(this.CancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OneDriveCloudTypeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Cloud Platform";
            ((System.ComponentModel.ISupportInitialize)(this.OneDriveConsumerPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OneDriveForBusinessPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.PictureBox OneDriveConsumerPicture;
        private System.Windows.Forms.PictureBox OneDriveForBusinessPicture;
        private System.Windows.Forms.Label ExplanationLabel;
    }
}