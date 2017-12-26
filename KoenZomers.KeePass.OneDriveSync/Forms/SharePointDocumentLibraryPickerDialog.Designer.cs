namespace KoenZomersKeePassOneDriveSync.Forms
{
    partial class SharePointDocumentLibraryPickerDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SharePointDocumentLibraryPickerDialog));
            this.SharePointDocumentLibraryPicker = new System.Windows.Forms.ListView();
            this.IconsList = new System.Windows.Forms.ImageList(this.components);
            this.CancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.CloudLocationPath = new System.Windows.Forms.TextBox();
            this.ExplanationLabel = new System.Windows.Forms.Label();
            this.FileNameLabel = new System.Windows.Forms.Label();
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.UpButton = new System.Windows.Forms.Button();
            this.ListViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToRootToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showHiddenLibrariesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ListViewContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // SharePointDocumentLibraryPicker
            // 
            this.SharePointDocumentLibraryPicker.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SharePointDocumentLibraryPicker.ContextMenuStrip = this.ListViewContextMenu;
            this.SharePointDocumentLibraryPicker.LargeImageList = this.IconsList;
            this.SharePointDocumentLibraryPicker.Location = new System.Drawing.Point(12, 52);
            this.SharePointDocumentLibraryPicker.MultiSelect = false;
            this.SharePointDocumentLibraryPicker.Name = "SharePointDocumentLibraryPicker";
            this.SharePointDocumentLibraryPicker.Size = new System.Drawing.Size(674, 432);
            this.SharePointDocumentLibraryPicker.SmallImageList = this.IconsList;
            this.SharePointDocumentLibraryPicker.TabIndex = 0;
            this.SharePointDocumentLibraryPicker.TileSize = new System.Drawing.Size(244, 70);
            this.SharePointDocumentLibraryPicker.UseCompatibleStateImageBehavior = false;
            this.SharePointDocumentLibraryPicker.SelectedIndexChanged += new System.EventHandler(this.SharePointDocumentLibraryPicker_SelectedIndexChanged);
            this.SharePointDocumentLibraryPicker.DoubleClick += new System.EventHandler(this.SharePointDocumentLibraryPicker_DoubleClick);
            // 
            // IconsList
            // 
            this.IconsList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IconsList.ImageStream")));
            this.IconsList.TransparentColor = System.Drawing.Color.Transparent;
            this.IconsList.Images.SetKeyName(0, "File");
            this.IconsList.Images.SetKeyName(1, "Folder");
            this.IconsList.Images.SetKeyName(2, "DocLib");
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(590, 491);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(96, 39);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "&Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.Location = new System.Drawing.Point(487, 491);
            this.OKButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(96, 39);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "&OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CloudLocationPath
            // 
            this.CloudLocationPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CloudLocationPath.BackColor = System.Drawing.SystemColors.Control;
            this.CloudLocationPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CloudLocationPath.Location = new System.Drawing.Point(15, 31);
            this.CloudLocationPath.Name = "CloudLocationPath";
            this.CloudLocationPath.Size = new System.Drawing.Size(610, 15);
            this.CloudLocationPath.TabIndex = 6;
            this.CloudLocationPath.TabStop = false;
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExplanationLabel.Location = new System.Drawing.Point(12, 9);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(674, 37);
            this.ExplanationLabel.TabIndex = 8;
            this.ExplanationLabel.Text = "Select the document library in which you want to store the KeePass database.";
            // 
            // FileNameLabel
            // 
            this.FileNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FileNameLabel.Location = new System.Drawing.Point(12, 499);
            this.FileNameLabel.Name = "FileNameLabel";
            this.FileNameLabel.Size = new System.Drawing.Size(77, 22);
            this.FileNameLabel.TabIndex = 11;
            this.FileNameLabel.Text = "Filename:";
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileNameTextBox.Location = new System.Drawing.Point(95, 499);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(386, 22);
            this.FileNameTextBox.TabIndex = 10;
            // 
            // UpButton
            // 
            this.UpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UpButton.Enabled = false;
            this.UpButton.Location = new System.Drawing.Point(631, 6);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(55, 40);
            this.UpButton.TabIndex = 12;
            this.UpButton.Text = "Up";
            this.UpButton.UseVisualStyleBackColor = true;
            this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // ListViewContextMenu
            // 
            this.ListViewContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ListViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.goToRootToolStripMenuItem,
            this.goupToolStripMenuItem,
            this.toolStripSeparator1,
            this.showHiddenLibrariesToolStripMenuItem});
            this.ListViewContextMenu.Name = "ListViewContextMenu";
            this.ListViewContextMenu.Size = new System.Drawing.Size(222, 134);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
            this.refreshToolStripMenuItem.Text = "&Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // goToRootToolStripMenuItem
            // 
            this.goToRootToolStripMenuItem.Name = "goToRootToolStripMenuItem";
            this.goToRootToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.goToRootToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
            this.goToRootToolStripMenuItem.Text = "&Go to root";
            this.goToRootToolStripMenuItem.Click += new System.EventHandler(this.goToRootToolStripMenuItem_Click);
            // 
            // goupToolStripMenuItem
            // 
            this.goupToolStripMenuItem.Enabled = false;
            this.goupToolStripMenuItem.Name = "goupToolStripMenuItem";
            this.goupToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.goupToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
            this.goupToolStripMenuItem.Text = "Go &up";
            this.goupToolStripMenuItem.Click += new System.EventHandler(this.goupToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(218, 6);
            // 
            // showHiddenLibrariesToolStripMenuItem
            // 
            this.showHiddenLibrariesToolStripMenuItem.Name = "showHiddenLibrariesToolStripMenuItem";
            this.showHiddenLibrariesToolStripMenuItem.Size = new System.Drawing.Size(221, 24);
            this.showHiddenLibrariesToolStripMenuItem.Text = "Show hidden libraries";
            this.showHiddenLibrariesToolStripMenuItem.Click += new System.EventHandler(this.showHiddenLibrariesToolStripMenuItem_Click);
            // 
            // SharePointDocumentLibraryPickerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 539);
            this.Controls.Add(this.UpButton);
            this.Controls.Add(this.FileNameLabel);
            this.Controls.Add(this.FileNameTextBox);
            this.Controls.Add(this.ExplanationLabel);
            this.Controls.Add(this.CloudLocationPath);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.SharePointDocumentLibraryPicker);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "SharePointDocumentLibraryPickerDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select location";
            this.ListViewContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView SharePointDocumentLibraryPicker;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.TextBox CloudLocationPath;
        private System.Windows.Forms.ImageList IconsList;
        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.Label FileNameLabel;
        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.ContextMenuStrip ListViewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToRootToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goupToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem showHiddenLibrariesToolStripMenuItem;
    }
}