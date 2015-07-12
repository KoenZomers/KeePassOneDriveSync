namespace KoenZomersKeePassOneDriveSync
{
    partial class OneDriveConfigForm
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
            this.ExplanationLabel = new System.Windows.Forms.Label();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.ConfigurationListView = new System.Windows.Forms.ListView();
            this.PathColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OneDriveColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ConfigurationListViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ConfigurationListViewContextItemViewDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationListViewContextItemSyncNow = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationListViewContextItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseButton = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.AboutButton = new System.Windows.Forms.Button();
            this.ConfigurationListViewContextItemOpenFileLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationListViewContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExplanationLabel.Location = new System.Drawing.Point(13, 36);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(427, 31);
            this.ExplanationLabel.TabIndex = 0;
            this.ExplanationLabel.Text = "Here you\'ll see all KeePass databases that are configured for use with OneDriveSy" +
    "nc.";
            this.ExplanationLabel.UseMnemonic = false;
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderLabel.Location = new System.Drawing.Point(12, 9);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(428, 23);
            this.HeaderLabel.TabIndex = 1;
            this.HeaderLabel.Text = "KeePass OneDriveSync";
            this.HeaderLabel.UseMnemonic = false;
            // 
            // ConfigurationListView
            // 
            this.ConfigurationListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConfigurationListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PathColumn,
            this.OneDriveColumn});
            this.ConfigurationListView.ContextMenuStrip = this.ConfigurationListViewContextMenu;
            this.ConfigurationListView.FullRowSelect = true;
            this.ConfigurationListView.GridLines = true;
            this.ConfigurationListView.Location = new System.Drawing.Point(16, 70);
            this.ConfigurationListView.MultiSelect = false;
            this.ConfigurationListView.Name = "ConfigurationListView";
            this.ConfigurationListView.Size = new System.Drawing.Size(424, 204);
            this.ConfigurationListView.TabIndex = 3;
            this.ConfigurationListView.UseCompatibleStateImageBehavior = false;
            this.ConfigurationListView.View = System.Windows.Forms.View.Details;
            this.ConfigurationListView.DoubleClick += new System.EventHandler(this.ConfigurationListView_DoubleClick);
            this.ConfigurationListView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ConfigurationListView_KeyUp);
            // 
            // PathColumn
            // 
            this.PathColumn.Text = "Local Path";
            this.PathColumn.Width = 275;
            // 
            // OneDriveColumn
            // 
            this.OneDriveColumn.Text = "OneDrive";
            this.OneDriveColumn.Width = 150;
            // 
            // ConfigurationListViewContextMenu
            // 
            this.ConfigurationListViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConfigurationListViewContextItemViewDetails,
            this.ConfigurationListViewContextItemSyncNow,
            this.ConfigurationListViewContextItemOpenFileLocation,
            this.ConfigurationListViewContextItemDelete});
            this.ConfigurationListViewContextMenu.Name = "ConfigurationListViewContextMenu";
            this.ConfigurationListViewContextMenu.Size = new System.Drawing.Size(169, 114);
            this.ConfigurationListViewContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ConfigurationListViewContextMenu_Opening);
            // 
            // ConfigurationListViewContextItemViewDetails
            // 
            this.ConfigurationListViewContextItemViewDetails.Name = "ConfigurationListViewContextItemViewDetails";
            this.ConfigurationListViewContextItemViewDetails.Size = new System.Drawing.Size(168, 22);
            this.ConfigurationListViewContextItemViewDetails.Text = "&View Details";
            this.ConfigurationListViewContextItemViewDetails.Click += new System.EventHandler(this.ConfigurationListViewContextItemViewDetails_Click);
            // 
            // ConfigurationListViewContextItemSyncNow
            // 
            this.ConfigurationListViewContextItemSyncNow.Name = "ConfigurationListViewContextItemSyncNow";
            this.ConfigurationListViewContextItemSyncNow.Size = new System.Drawing.Size(168, 22);
            this.ConfigurationListViewContextItemSyncNow.Text = "&Sync Now";
            this.ConfigurationListViewContextItemSyncNow.Click += new System.EventHandler(this.ConfigurationListViewContextItemSyncNow_Click);
            // 
            // ConfigurationListViewContextItemDelete
            // 
            this.ConfigurationListViewContextItemDelete.Name = "ConfigurationListViewContextItemDelete";
            this.ConfigurationListViewContextItemDelete.Size = new System.Drawing.Size(168, 22);
            this.ConfigurationListViewContextItemDelete.Text = "&Delete";
            this.ConfigurationListViewContextItemDelete.Click += new System.EventHandler(this.ConfigurationListViewContextItemDelete_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CloseButton.Location = new System.Drawing.Point(341, 280);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(99, 33);
            this.CloseButton.TabIndex = 4;
            this.CloseButton.Text = "&Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // StatusLabel
            // 
            this.StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusLabel.Location = new System.Drawing.Point(13, 280);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(217, 33);
            this.StatusLabel.TabIndex = 5;
            this.StatusLabel.Text = "-";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StatusLabel.UseMnemonic = false;
            // 
            // AboutButton
            // 
            this.AboutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AboutButton.Location = new System.Drawing.Point(236, 280);
            this.AboutButton.Name = "AboutButton";
            this.AboutButton.Size = new System.Drawing.Size(99, 33);
            this.AboutButton.TabIndex = 6;
            this.AboutButton.Text = "&About";
            this.AboutButton.UseVisualStyleBackColor = true;
            this.AboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // ConfigurationListViewContextItemOpenFileLocation
            // 
            this.ConfigurationListViewContextItemOpenFileLocation.Name = "ConfigurationListViewContextItemOpenFileLocation";
            this.ConfigurationListViewContextItemOpenFileLocation.Size = new System.Drawing.Size(168, 22);
            this.ConfigurationListViewContextItemOpenFileLocation.Text = "Open file &location";
            this.ConfigurationListViewContextItemOpenFileLocation.Click += new System.EventHandler(this.ConfigurationListViewContextItemOpenFileLocation_Click);
            // 
            // OneDriveConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(452, 325);
            this.Controls.Add(this.AboutButton);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ConfigurationListView);
            this.Controls.Add(this.HeaderLabel);
            this.Controls.Add(this.ExplanationLabel);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(386, 220);
            this.Name = "OneDriveConfigForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OneDrive Sync Configuration";
            this.Load += new System.EventHandler(this.OneDriveConfigForm_Load);
            this.ConfigurationListViewContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.ListView ConfigurationListView;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.ColumnHeader PathColumn;
        private System.Windows.Forms.ColumnHeader OneDriveColumn;
        private System.Windows.Forms.ContextMenuStrip ConfigurationListViewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemDelete;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemViewDetails;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemSyncNow;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Button AboutButton;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemOpenFileLocation;
    }
}