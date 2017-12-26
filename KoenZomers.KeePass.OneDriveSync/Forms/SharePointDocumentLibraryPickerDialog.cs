using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.SharePoint.Client;

namespace KoenZomersKeePassOneDriveSync.Forms
{
    public partial class SharePointDocumentLibraryPickerDialog : System.Windows.Forms.Form
    {
        #region Properties

        /// <summary>
        /// Returns the server relative URL of the selected SharePoint Document Library
        /// </summary>
        public string SelectedDocumentLibraryServerRelativeUrl { get { return currentViewServerRelativeUrl; } }

        /// <summary>
        /// Gets or sets the filename in the textbox on the screen
        /// </summary>
        public string FileName
        {
            get { return FileNameTextBox.Text; }
            set { FileNameTextBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets the text shown at the top of the form
        /// </summary>
        public string ExplanationText
        {
            get { return ExplanationLabel.Text; }
            set { ExplanationLabel.Text = value; }
        }

        /// <summary>
        /// Gets or sets if the user can enter a filename which does not exist yet on their OneDrive
        /// </summary>
        public bool AllowEnteringNewFileName
        {
            get { return FileNameTextBox.Enabled; }
            set { FileNameTextBox.Enabled = value; }
        }

        /// <summary>
        /// Gets or sets if hidden document libraries should be shown
        /// </summary>
        public bool ShowHiddenLibraries { get; set; } = false;

        #endregion

        /// <summary>
        /// Instance of the SharePoint ClientContext that can be used to communicate with SharePoint
        /// </summary>
        private readonly ClientContext _clientContext;

        /// <summary>
        /// The server relative url of the currently selected document library
        /// </summary>
        private string documentLibraryServerRelativeUrl = null;

        /// <summary>
        /// The server relative url of the currently shown view
        /// </summary>
        private string currentViewServerRelativeUrl = null;

        public SharePointDocumentLibraryPickerDialog(ClientContext clientContext)
        {
            InitializeComponent();

            _clientContext = clientContext;
        }

        /// <summary>
        /// Gets the document library items and renders them in the form
        /// </summary>
        public void LoadDocumentLibraryItems()
        {
            SharePointDocumentLibraryPicker.Items.Clear();

            _clientContext.Load(_clientContext.Web.Lists, l => l.Include(li => li.RootFolder.ServerRelativeUrl, li => li.Title, li => li.BaseTemplate, li => li.Hidden));
            _clientContext.ExecuteQuery();

            foreach (var listViewItem in _clientContext.Web.Lists.Where(l => l.BaseTemplate == 101 && (ShowHiddenLibraries || !l.Hidden)).Select(documentLibraryItem => new ListViewItem
            {
                Text = documentLibraryItem.Title,
                Tag = documentLibraryItem.RootFolder.ServerRelativeUrl,
                ImageKey = "DocLib"
            }))
            {
                SharePointDocumentLibraryPicker.Items.Add(listViewItem);
            }

            UpButton.Enabled = false;
        }

        /// <summary>
        /// Gets the folders and files inside a document library items and renders them in the form
        /// </summary>
        public void LoadDocumentLibraryFileAndFolderItems(string serverRelativeUrl)
        {
            currentViewServerRelativeUrl = serverRelativeUrl;

            SharePointDocumentLibraryPicker.Items.Clear();

            var folder = _clientContext.Web.GetFolderByServerRelativeUrl(serverRelativeUrl);
            _clientContext.Load(folder.Folders, l => l.Include(li => li.Name, li => li.ServerRelativeUrl));
            _clientContext.Load(folder.Files, l => l.Include(li => li.Name, li => li.ServerRelativeUrl));
            _clientContext.ExecuteQuery();

            foreach (var folderItem in folder.Folders)
            {
                SharePointDocumentLibraryPicker.Items.Add(new ListViewItem
                {
                    Text = folderItem.Name,
                    Tag = folderItem.ServerRelativeUrl,
                    ImageKey = "Folder"
                });
            }

            foreach (var folderItem in folder.Files)
            {
                SharePointDocumentLibraryPicker.Items.Add(new ListViewItem
                {
                    Text = folderItem.Name,
                    Tag = folderItem.ServerRelativeUrl,
                    ImageKey = "File"
                });
            }

            UpButton.Enabled = true;
        }

        private void SharePointDocumentLibraryPicker_DoubleClick(object sender, EventArgs e)
        {
            if (SharePointDocumentLibraryPicker.SelectedItems.Count == 0) return;
            var selectedItem = SharePointDocumentLibraryPicker.SelectedItems[0];

            switch (selectedItem.ImageKey)
            {
                case "DocLib":
                    documentLibraryServerRelativeUrl = selectedItem.Tag.ToString();
                    LoadDocumentLibraryFileAndFolderItems(selectedItem.Tag.ToString());
                    break;

                case "Folder":
                    LoadDocumentLibraryFileAndFolderItems(selectedItem.Tag.ToString());
                    break;

                case "File":
                    if (OKButton.Enabled)
                    {
                        OKButton_Click(sender, e);
                    }
                    break;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if(SharePointDocumentLibraryPicker.SelectedItems.Count == 0)
            {
                MessageBox.Show(AllowEnteringNewFileName ? "Select a document library to store the KeePass database in" : "Select the KeePass file to download", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(string.IsNullOrWhiteSpace(FileName))
            {
                MessageBox.Show(AllowEnteringNewFileName ? "Enter the filename under which you wish to store the database" : "Select the KeePass file to download", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void SharePointDocumentLibraryPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileName = SharePointDocumentLibraryPicker.SelectedItems.Count > 0 && SharePointDocumentLibraryPicker.SelectedItems[0].ImageKey == "File" ? SharePointDocumentLibraryPicker.SelectedItems[0].Text : string.Empty;
        }

        private void showHiddenLibrariesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowHiddenLibraries = !ShowHiddenLibraries;
            showHiddenLibrariesToolStripMenuItem.Checked = ShowHiddenLibraries;

            refreshToolStripMenuItem_Click(sender, e);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentViewServerRelativeUrl))
            {
                LoadDocumentLibraryFileAndFolderItems(currentViewServerRelativeUrl);
            }
            else
            {
                LoadDocumentLibraryItems();
            }
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            var newServerRelativeUrl = currentViewServerRelativeUrl.Remove(currentViewServerRelativeUrl.LastIndexOf('/'));
            if(newServerRelativeUrl.Length < documentLibraryServerRelativeUrl.Length)
            {
                LoadDocumentLibraryItems();
            }
            else
            {
                LoadDocumentLibraryFileAndFolderItems(newServerRelativeUrl);
            }
        }

        private void goToRootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDocumentLibraryItems();
        }

        private void goupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpButton_Click(sender, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
