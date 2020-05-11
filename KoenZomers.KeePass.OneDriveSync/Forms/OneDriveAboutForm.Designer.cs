namespace KoenZomersKeePassOneDriveSync
{
    partial class OneDriveAboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OneDriveAboutForm));
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.NameLabel = new System.Windows.Forms.Label();
            this.ExplanationLabel = new System.Windows.Forms.Label();
            this.WebsiteLinkLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderLabel.Location = new System.Drawing.Point(18, 14);
            this.HeaderLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(561, 35);
            this.HeaderLabel.TabIndex = 2;
            this.HeaderLabel.Text = "KeePass OneDriveSync";
            this.HeaderLabel.UseMnemonic = false;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CloseButton.Location = new System.Drawing.Point(431, 194);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(148, 51);
            this.CloseButton.TabIndex = 5;
            this.CloseButton.Text = "&Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // NameLabel
            // 
            this.NameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameLabel.Location = new System.Drawing.Point(13, 194);
            this.NameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(405, 28);
            this.NameLabel.TabIndex = 6;
            this.NameLabel.Text = "Koen Zomers (koen@zomers.eu)";
            this.NameLabel.UseMnemonic = false;
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExplanationLabel.Location = new System.Drawing.Point(19, 71);
            this.ExplanationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ExplanationLabel.MinimumSize = new System.Drawing.Size(558, 118);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(558, 118);
            this.ExplanationLabel.TabIndex = 7;
            this.ExplanationLabel.Text = resources.GetString("ExplanationLabel.Text");
            // 
            // WebsiteLinkLabel
            // 
            this.WebsiteLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.WebsiteLinkLabel.AutoSize = true;
            this.WebsiteLinkLabel.Location = new System.Drawing.Point(15, 222);
            this.WebsiteLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.WebsiteLinkLabel.Name = "WebsiteLinkLabel";
            this.WebsiteLinkLabel.Size = new System.Drawing.Size(287, 20);
            this.WebsiteLinkLabel.TabIndex = 9;
            this.WebsiteLinkLabel.TabStop = true;
            this.WebsiteLinkLabel.Text = "https://zomers.eu/keepassonedrivesync";
            this.WebsiteLinkLabel.Click += new System.EventHandler(this.WebsiteLinkLabel_Click);
            // 
            // OneDriveAboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 262);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.WebsiteLinkLabel);
            this.Controls.Add(this.ExplanationLabel);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.HeaderLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(619, 250);
            this.Name = "OneDriveAboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About KeePass OneDriveSync";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.LinkLabel WebsiteLinkLabel;
    }
}