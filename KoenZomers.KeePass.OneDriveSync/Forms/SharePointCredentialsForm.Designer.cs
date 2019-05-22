namespace KoenZomersKeePassOneDriveSync.Forms
{
    partial class SharePointCredentialsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.SharePointUrlTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ClientIdTextBox = new System.Windows.Forms.TextBox();
            this.ClientSecretTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.TestButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExplanationLabel.Location = new System.Drawing.Point(9, 7);
            this.ExplanationLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(423, 29);
            this.ExplanationLabel.TabIndex = 1;
            this.ExplanationLabel.Text = "Enter the details of the SharePoint 2013, 2016, 2019 or Online environment you wi" +
    "sh to store the KeePass database on.";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(9, 54);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "SharePoint site URL:";
            // 
            // SharePointUrlTextBox
            // 
            this.SharePointUrlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SharePointUrlTextBox.Location = new System.Drawing.Point(121, 54);
            this.SharePointUrlTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SharePointUrlTextBox.Name = "SharePointUrlTextBox";
            this.SharePointUrlTextBox.Size = new System.Drawing.Size(312, 20);
            this.SharePointUrlTextBox.TabIndex = 0;
            this.SharePointUrlTextBox.TextChanged += new System.EventHandler(this.SharePointUrlTextBox_TextChanged);
            this.SharePointUrlTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SharePointUrlTextBox_KeyUp);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Location = new System.Drawing.Point(9, 80);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = "Client Id:";
            // 
            // ClientIdTextBox
            // 
            this.ClientIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ClientIdTextBox.Location = new System.Drawing.Point(121, 78);
            this.ClientIdTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ClientIdTextBox.Name = "ClientIdTextBox";
            this.ClientIdTextBox.Size = new System.Drawing.Size(312, 20);
            this.ClientIdTextBox.TabIndex = 1;
            this.ClientIdTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ClientIdTextBox_KeyUp);
            // 
            // ClientSecretTextBox
            // 
            this.ClientSecretTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ClientSecretTextBox.Location = new System.Drawing.Point(121, 101);
            this.ClientSecretTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ClientSecretTextBox.Name = "ClientSecretTextBox";
            this.ClientSecretTextBox.Size = new System.Drawing.Size(312, 20);
            this.ClientSecretTextBox.TabIndex = 2;
            this.ClientSecretTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ClientSecretTextBox_KeyUp);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Location = new System.Drawing.Point(9, 103);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 19);
            this.label3.TabIndex = 6;
            this.label3.Text = "Client Secret:";
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(320, 131);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(112, 28);
            this.CancelButton.TabIndex = 5;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OKButton.Location = new System.Drawing.Point(204, 131);
            this.OKButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(112, 28);
            this.OKButton.TabIndex = 4;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // TestButton
            // 
            this.TestButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TestButton.Location = new System.Drawing.Point(88, 131);
            this.TestButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(112, 28);
            this.TestButton.TabIndex = 3;
            this.TestButton.Text = "Test Connection";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // SharePointCredentialsForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 168);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ClientSecretTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ClientIdTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SharePointUrlTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ExplanationLabel);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SharePointCredentialsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Provide SharePoint details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SharePointUrlTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ClientIdTextBox;
        private System.Windows.Forms.TextBox ClientSecretTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button TestButton;
    }
}