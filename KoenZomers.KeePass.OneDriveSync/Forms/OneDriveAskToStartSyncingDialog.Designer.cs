﻿namespace KoenZomersKeePassOneDriveSync
 {
     partial class OneDriveAskToStartSyncingDialog
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
            this.OneDriveDatabasePath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExplanationLabel.Location = new System.Drawing.Point(9, 7);
            this.ExplanationLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(375, 35);
            this.ExplanationLabel.TabIndex = 0;
            this.ExplanationLabel.Text = "The following KeePass database is not being synced with OneDrive. Would you like " +
    "to set up a synchronization connection now?";
            // 
            // YesRadio
            // 
            this.YesRadio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.YesRadio.AutoSize = true;
            this.YesRadio.Checked = true;
            this.YesRadio.Location = new System.Drawing.Point(11, 75);
            this.YesRadio.Margin = new System.Windows.Forms.Padding(2);
            this.YesRadio.Name = "YesRadio";
            this.YesRadio.Size = new System.Drawing.Size(43, 17);
            this.YesRadio.TabIndex = 1;
            this.YesRadio.TabStop = true;
            this.YesRadio.Text = "Yes";
            this.YesRadio.UseVisualStyleBackColor = true;
            // 
            // NotNowRadio
            // 
            this.NotNowRadio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NotNowRadio.AutoSize = true;
            this.NotNowRadio.Location = new System.Drawing.Point(11, 97);
            this.NotNowRadio.Margin = new System.Windows.Forms.Padding(2);
            this.NotNowRadio.Name = "NotNowRadio";
            this.NotNowRadio.Size = new System.Drawing.Size(65, 17);
            this.NotNowRadio.TabIndex = 2;
            this.NotNowRadio.Text = "Not now";
            this.NotNowRadio.UseVisualStyleBackColor = true;
            // 
            // NoNeverAskAgainRadio
            // 
            this.NoNeverAskAgainRadio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NoNeverAskAgainRadio.AutoSize = true;
            this.NoNeverAskAgainRadio.Location = new System.Drawing.Point(11, 118);
            this.NoNeverAskAgainRadio.Margin = new System.Windows.Forms.Padding(2);
            this.NoNeverAskAgainRadio.Name = "NoNeverAskAgainRadio";
            this.NoNeverAskAgainRadio.Size = new System.Drawing.Size(220, 17);
            this.NoNeverAskAgainRadio.TabIndex = 3;
            this.NoNeverAskAgainRadio.Text = "No and never ask again for this database";
            this.NoNeverAskAgainRadio.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OKButton.Location = new System.Drawing.Point(127, 154);
            this.OKButton.Margin = new System.Windows.Forms.Padding(2);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(74, 41);
            this.OKButton.TabIndex = 5;
            this.OKButton.Text = "&OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(206, 154);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(74, 41);
            this.CancelButton.TabIndex = 6;
            this.CancelButton.Text = "&Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // OneDriveDatabasePath
            // 
            this.OneDriveDatabasePath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OneDriveDatabasePath.BackColor = System.Drawing.SystemColors.Control;
            this.OneDriveDatabasePath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OneDriveDatabasePath.Location = new System.Drawing.Point(12, 45);
            this.OneDriveDatabasePath.Name = "OneDriveDatabasePath";
            this.OneDriveDatabasePath.ReadOnly = true;
            this.OneDriveDatabasePath.Size = new System.Drawing.Size(369, 13);
            this.OneDriveDatabasePath.TabIndex = 7;
            this.OneDriveDatabasePath.Text = "?";
            // 
            // OneDriveAskToStartSyncingDialog
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 205);
            this.Controls.Add(this.OneDriveDatabasePath);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.NoNeverAskAgainRadio);
            this.Controls.Add(this.NotNowRadio);
            this.Controls.Add(this.YesRadio);
            this.Controls.Add(this.ExplanationLabel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(409, 244);
            this.Name = "OneDriveAskToStartSyncingDialog";
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
         private System.Windows.Forms.RadioButton YesRadio;
         private System.Windows.Forms.RadioButton NotNowRadio;
         private System.Windows.Forms.RadioButton NoNeverAskAgainRadio;
        private System.Windows.Forms.TextBox OneDriveDatabasePath;
    }
 }