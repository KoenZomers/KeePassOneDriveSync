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
            this.StorageNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StorageProviderColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LastSyncedColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ConfigurationListViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ConfigurationListViewContextItemViewDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationListViewContextItemRenameStorage = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationListViewContextItemSyncNow = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationListViewContextItemOpenFileLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationListViewContextItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseButton = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.AboutButton = new System.Windows.Forms.Button();
            this.ConfigurationListViewContextItemOpenDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.ConfigurationListViewContextItemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationListViewContextItemSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationListViewContextItemSelectNotAvailableLocally = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationListViewContextItemSelectNotSynced = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationListViewContextItemSelectNotSynced24Hours = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationListViewContextItemSelectNotSyncedWeek = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationListViewContextItemSelectNotSynced2Weeks = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationListViewContextItemSelectNotSyncedMonth = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationListViewContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExplanationLabel.Location = new System.Drawing.Point(13, 36);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(433, 31);
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
            this.HeaderLabel.Size = new System.Drawing.Size(434, 23);
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
            this.StorageNameColumn,
            this.StorageProviderColumn,
            this.LastSyncedColumn});
            this.ConfigurationListView.ContextMenuStrip = this.ConfigurationListViewContextMenu;
            this.ConfigurationListView.FullRowSelect = true;
            this.ConfigurationListView.GridLines = true;
            this.ConfigurationListView.HideSelection = false;
            this.ConfigurationListView.Location = new System.Drawing.Point(15, 55);
            this.ConfigurationListView.Name = "ConfigurationListView";
            this.ConfigurationListView.Size = new System.Drawing.Size(430, 219);
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
            // StorageNameColumn
            // 
            this.StorageNameColumn.Text = "Storage Name";
            this.StorageNameColumn.Width = 150;
            // 
            // StorageProviderColumn
            // 
            this.StorageProviderColumn.Text = "Storage Provider";
            this.StorageProviderColumn.Width = 175;
            // 
            // LastSyncedColumn
            // 
            this.LastSyncedColumn.Text = "Last Synced";
            this.LastSyncedColumn.Width = 175;
            // 
            // ConfigurationListViewContextMenu
            // 
            this.ConfigurationListViewContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ConfigurationListViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConfigurationListViewContextItemViewDetails,
            this.ConfigurationListViewContextItemRenameStorage,
            this.ConfigurationListViewContextItemDelete,
            this.toolStripMenuItem2,
            this.ConfigurationListViewContextItemSyncNow,
            this.toolStripMenuItem1,
            this.selectToolStripMenuItem,
            this.ConfigurationListViewContextItemRefresh,
            this.toolStripMenuItem3,
            this.ConfigurationListViewContextItemOpenFileLocation,
            this.ConfigurationListViewContextItemOpenDatabase});
            this.ConfigurationListViewContextMenu.Name = "ConfigurationListViewContextMenu";
            this.ConfigurationListViewContextMenu.Size = new System.Drawing.Size(218, 198);
            this.ConfigurationListViewContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ConfigurationListViewContextMenu_Opening);
            // 
            // ConfigurationListViewContextItemViewDetails
            // 
            this.ConfigurationListViewContextItemViewDetails.Name = "ConfigurationListViewContextItemViewDetails";
            this.ConfigurationListViewContextItemViewDetails.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.ConfigurationListViewContextItemViewDetails.Size = new System.Drawing.Size(217, 22);
            this.ConfigurationListViewContextItemViewDetails.Text = "&View details";
            this.ConfigurationListViewContextItemViewDetails.Click += new System.EventHandler(this.ConfigurationListViewContextItemViewDetails_Click);
            // 
            // ConfigurationListViewContextItemRenameStorage
            // 
            this.ConfigurationListViewContextItemRenameStorage.Name = "ConfigurationListViewContextItemRenameStorage";
            this.ConfigurationListViewContextItemRenameStorage.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.ConfigurationListViewContextItemRenameStorage.Size = new System.Drawing.Size(217, 22);
            this.ConfigurationListViewContextItemRenameStorage.Text = "&Rename storage";
            this.ConfigurationListViewContextItemRenameStorage.Click += new System.EventHandler(this.ConfigurationListViewContextItemRenameStorage_Click);
            // 
            // ConfigurationListViewContextItemSyncNow
            // 
            this.ConfigurationListViewContextItemSyncNow.Name = "ConfigurationListViewContextItemSyncNow";
            this.ConfigurationListViewContextItemSyncNow.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.ConfigurationListViewContextItemSyncNow.Size = new System.Drawing.Size(217, 22);
            this.ConfigurationListViewContextItemSyncNow.Text = "&Sync Now";
            this.ConfigurationListViewContextItemSyncNow.Click += new System.EventHandler(this.ConfigurationListViewContextItemSyncNow_Click);
            // 
            // ConfigurationListViewContextItemOpenFileLocation
            // 
            this.ConfigurationListViewContextItemOpenFileLocation.Name = "ConfigurationListViewContextItemOpenFileLocation";
            this.ConfigurationListViewContextItemOpenFileLocation.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.ConfigurationListViewContextItemOpenFileLocation.Size = new System.Drawing.Size(217, 22);
            this.ConfigurationListViewContextItemOpenFileLocation.Text = "Open file &location";
            this.ConfigurationListViewContextItemOpenFileLocation.Click += new System.EventHandler(this.ConfigurationListViewContextItemOpenFileLocation_Click);
            // 
            // ConfigurationListViewContextItemDelete
            // 
            this.ConfigurationListViewContextItemDelete.Name = "ConfigurationListViewContextItemDelete";
            this.ConfigurationListViewContextItemDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.ConfigurationListViewContextItemDelete.Size = new System.Drawing.Size(217, 22);
            this.ConfigurationListViewContextItemDelete.Text = "&Delete configuration";
            this.ConfigurationListViewContextItemDelete.Click += new System.EventHandler(this.ConfigurationListViewContextItemDelete_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CloseButton.Location = new System.Drawing.Point(347, 280);
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
            this.StatusLabel.Size = new System.Drawing.Size(223, 33);
            this.StatusLabel.TabIndex = 5;
            this.StatusLabel.Text = "-";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StatusLabel.UseMnemonic = false;
            // 
            // AboutButton
            // 
            this.AboutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AboutButton.Location = new System.Drawing.Point(242, 280);
            this.AboutButton.Name = "AboutButton";
            this.AboutButton.Size = new System.Drawing.Size(99, 33);
            this.AboutButton.TabIndex = 6;
            this.AboutButton.Text = "&About";
            this.AboutButton.UseVisualStyleBackColor = true;
            this.AboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // ConfigurationListViewContextItemOpenDatabase
            // 
            this.ConfigurationListViewContextItemOpenDatabase.Name = "ConfigurationListViewContextItemOpenDatabase";
            this.ConfigurationListViewContextItemOpenDatabase.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.ConfigurationListViewContextItemOpenDatabase.Size = new System.Drawing.Size(217, 22);
            this.ConfigurationListViewContextItemOpenDatabase.Text = "Open &KeePass database";
            this.ConfigurationListViewContextItemOpenDatabase.Click += new System.EventHandler(this.ConfigurationListViewContextItemOpenDatabase_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(214, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(214, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(214, 6);
            // 
            // ConfigurationListViewContextItemRefresh
            // 
            this.ConfigurationListViewContextItemRefresh.Name = "ConfigurationListViewContextItemRefresh";
            this.ConfigurationListViewContextItemRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.ConfigurationListViewContextItemRefresh.Size = new System.Drawing.Size(217, 22);
            this.ConfigurationListViewContextItemRefresh.Text = "Refresh &list";
            this.ConfigurationListViewContextItemRefresh.Click += new System.EventHandler(this.ConfigurationListViewContextItemRefresh_Click);
            // 
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConfigurationListViewContextItemSelectAll,
            this.ConfigurationListViewContextItemSelectNotAvailableLocally,
            this.ConfigurationListViewContextItemSelectNotSynced});
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            this.selectToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.selectToolStripMenuItem.Text = "S&elect";
            // 
            // ConfigurationListViewContextItemSelectAll
            // 
            this.ConfigurationListViewContextItemSelectAll.Name = "ConfigurationListViewContextItemSelectAll";
            this.ConfigurationListViewContextItemSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.ConfigurationListViewContextItemSelectAll.Size = new System.Drawing.Size(254, 22);
            this.ConfigurationListViewContextItemSelectAll.Text = "&All";
            this.ConfigurationListViewContextItemSelectAll.Click += new System.EventHandler(this.ConfigurationListViewContextItemSelectAll_Click);
            // 
            // ConfigurationListViewContextItemSelectNotAvailableLocally
            // 
            this.ConfigurationListViewContextItemSelectNotAvailableLocally.Name = "ConfigurationListViewContextItemSelectNotAvailableLocally";
            this.ConfigurationListViewContextItemSelectNotAvailableLocally.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.ConfigurationListViewContextItemSelectNotAvailableLocally.Size = new System.Drawing.Size(254, 22);
            this.ConfigurationListViewContextItemSelectNotAvailableLocally.Text = "&Not available locally";
            this.ConfigurationListViewContextItemSelectNotAvailableLocally.Click += new System.EventHandler(this.ConfigurationListViewContextItemSelectNotAvailableLocally_Click);
            // 
            // ConfigurationListViewContextItemSelectNotSynced
            // 
            this.ConfigurationListViewContextItemSelectNotSynced.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConfigurationListViewContextItemSelectNotSynced24Hours,
            this.ConfigurationListViewContextItemSelectNotSyncedWeek,
            this.ConfigurationListViewContextItemSelectNotSynced2Weeks,
            this.ConfigurationListViewContextItemSelectNotSyncedMonth});
            this.ConfigurationListViewContextItemSelectNotSynced.Name = "ConfigurationListViewContextItemSelectNotSynced";
            this.ConfigurationListViewContextItemSelectNotSynced.Size = new System.Drawing.Size(254, 22);
            this.ConfigurationListViewContextItemSelectNotSynced.Text = "Not synced in the &past...";
            // 
            // ConfigurationListViewContextItemSelectNotSynced24Hours
            // 
            this.ConfigurationListViewContextItemSelectNotSynced24Hours.Name = "ConfigurationListViewContextItemSelectNotSynced24Hours";
            this.ConfigurationListViewContextItemSelectNotSynced24Hours.Size = new System.Drawing.Size(180, 22);
            this.ConfigurationListViewContextItemSelectNotSynced24Hours.Text = "24 &hours";
            this.ConfigurationListViewContextItemSelectNotSynced24Hours.Click += new System.EventHandler(this.ConfigurationListViewContextItemSelectNotSynced24Hours_Click);
            // 
            // ConfigurationListViewContextItemSelectNotSyncedWeek
            // 
            this.ConfigurationListViewContextItemSelectNotSyncedWeek.Name = "ConfigurationListViewContextItemSelectNotSyncedWeek";
            this.ConfigurationListViewContextItemSelectNotSyncedWeek.Size = new System.Drawing.Size(180, 22);
            this.ConfigurationListViewContextItemSelectNotSyncedWeek.Text = "&Week";
            this.ConfigurationListViewContextItemSelectNotSyncedWeek.Click += new System.EventHandler(this.ConfigurationListViewContextItemSelectNotSyncedWeek_Click);
            // 
            // ConfigurationListViewContextItemSelectNotSynced2Weeks
            // 
            this.ConfigurationListViewContextItemSelectNotSynced2Weeks.Name = "ConfigurationListViewContextItemSelectNotSynced2Weeks";
            this.ConfigurationListViewContextItemSelectNotSynced2Weeks.Size = new System.Drawing.Size(180, 22);
            this.ConfigurationListViewContextItemSelectNotSynced2Weeks.Text = "&2 Weeks";
            this.ConfigurationListViewContextItemSelectNotSynced2Weeks.Click += new System.EventHandler(this.ConfigurationListViewContextItemSelectNotSynced2Weeks_Click);
            // 
            // ConfigurationListViewContextItemSelectNotSyncedMonth
            // 
            this.ConfigurationListViewContextItemSelectNotSyncedMonth.Name = "ConfigurationListViewContextItemSelectNotSyncedMonth";
            this.ConfigurationListViewContextItemSelectNotSyncedMonth.Size = new System.Drawing.Size(180, 22);
            this.ConfigurationListViewContextItemSelectNotSyncedMonth.Text = "&Month";
            this.ConfigurationListViewContextItemSelectNotSyncedMonth.Click += new System.EventHandler(this.ConfigurationListViewContextItemSelectNotSyncedMonth_Click);
            // 
            // OneDriveConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(458, 325);
            this.Controls.Add(this.AboutButton);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ConfigurationListView);
            this.Controls.Add(this.HeaderLabel);
            this.Controls.Add(this.ExplanationLabel);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(386, 217);
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
        private System.Windows.Forms.ColumnHeader StorageNameColumn;
        private System.Windows.Forms.ContextMenuStrip ConfigurationListViewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemDelete;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemViewDetails;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemSyncNow;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Button AboutButton;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemOpenFileLocation;
        private System.Windows.Forms.CheckBox UseSystemProxyCheckBox;
        private System.Windows.Forms.ColumnHeader StorageProviderColumn;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemRenameStorage;
        private System.Windows.Forms.ColumnHeader LastSyncedColumn;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemOpenDatabase;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemRefresh;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemSelectAll;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemSelectNotAvailableLocally;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemSelectNotSynced;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemSelectNotSynced24Hours;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemSelectNotSyncedWeek;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemSelectNotSynced2Weeks;
        private System.Windows.Forms.ToolStripMenuItem ConfigurationListViewContextItemSelectNotSyncedMonth;
    }
}