using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace KoenZomersKeePassOneDriveSync.Forms
{
    public partial class SharePointDocumentLibraryPickerDialog : System.Windows.Forms.Form
    {
        #region Properties

        /// <summary>
        /// Returns the server relative URL of the selected SharePoint Document Library
        /// </summary>
        public string SelectedDocumentLibraryServerRelativeUrl { get { return currentViewServerRelativeUrl ?? (SharePointDocumentLibraryPicker.SelectedItems.Count > 0 ? SharePointDocumentLibraryPicker.SelectedItems[0].Tag.ToString() : null); } }

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
        public bool ShowHiddenLibraries { get; set; }

        #endregion

        /// <summary>
        /// Instance of the HttpClient that can be used to communicate with SharePoint
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// The server relative url of the currently selected document library
        /// </summary>
        private string documentLibraryServerRelativeUrl = null;

        /// <summary>
        /// The server relative url of the currently shown view
        /// </summary>
        private string currentViewServerRelativeUrl = null;

        public SharePointDocumentLibraryPickerDialog(HttpClient httpClient)
        {
            InitializeComponent();

            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets the document library items and renders them in the form
        /// </summary>
        public async Task LoadDocumentLibraryItems()
        {
            SharePointDocumentLibraryPicker.Items.Clear();

            // Request all document libraries
            var response = await _httpClient.GetAsync("web/lists?$select=Title,RootFolder/ServerRelativeUrl&$filter=BaseTemplate eq 101" + (ShowHiddenLibraries ? "" : " and Hidden eq false") + "&$expand=RootFolder");

            // Validate if the request was successful
            if (!response.IsSuccessStatusCode)
            {
                // Request was unsuccessful
                return;
            }

            // Request was succesful. Parse the JSON result.
            var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());

            // Loop through each document library in the result
            foreach (var listViewItem in responseJson["value"])
            {
                // Create a new tile in the form for each document library
                SharePointDocumentLibraryPicker.Items.Add(new ListViewItem
                {
                    Text = listViewItem["Title"].ToString(),
                    Tag = listViewItem["RootFolder"]["ServerRelativeUrl"].ToString(),
                    ImageKey = "DocLib"
                });
            }

            // Do not allow the use of the up button or the root context menu option as this is the highest level
            UpButton.Enabled = false;
            goupToolStripMenuItem.Enabled = false;
            goToRootToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;
            renameToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// Gets the folders and files inside a document library items and renders them in the form
        /// </summary>
        public async Task LoadDocumentLibraryFileAndFolderItems(string serverRelativeUrl)
        {
            currentViewServerRelativeUrl = serverRelativeUrl;

            SharePointDocumentLibraryPicker.Items.Clear();

            // Request all folders
            using (var foldersResponse = await _httpClient.GetAsync("web/GetFolderByServerRelativeUrl('" + serverRelativeUrl + "')/Folders?$select=Name,ServerRelativeUrl"))
            {
                // Validate if the request was successful
                if (!foldersResponse.IsSuccessStatusCode)
                {
                    // Request was unsuccessful
                    return;
                }

                // Request was succesful. Parse the JSON result.
                var foldersResponseJson = JObject.Parse(await foldersResponse.Content.ReadAsStringAsync());

                // Loop through each document library in the result
                foreach (var listViewItem in foldersResponseJson["value"])
                {
                    // Create a new tile in the form for each document library
                    SharePointDocumentLibraryPicker.Items.Add(new ListViewItem
                    {
                        Text = listViewItem["Name"].ToString(),
                        Tag = listViewItem["ServerRelativeUrl"].ToString(),
                        ImageKey = "Folder"
                    });
                }
            }

            // Request all files
            using (var filesResponse = await _httpClient.GetAsync("web/GetFolderByServerRelativeUrl('" + serverRelativeUrl + "')/Files?$select=Name,ServerRelativeUrl"))
            {
                // Validate if the request was successful
                if (!filesResponse.IsSuccessStatusCode)
                {
                    // Request was unsuccessful
                    return;
                }

                // Request was succesful. Parse the JSON result.
                var filesResponseJson = JObject.Parse(await filesResponse.Content.ReadAsStringAsync());

                // Loop through each document library in the result
                foreach (var listViewItem in filesResponseJson["value"])
                {
                    // Create a new tile in the form for each document library
                    SharePointDocumentLibraryPicker.Items.Add(new ListViewItem
                    {
                        Text = listViewItem["Name"].ToString(),
                        Tag = listViewItem["ServerRelativeUrl"].ToString(),
                        ImageKey = "File"
                    });
                }
            }

            // Allow the use of the up button and root context menu option as you can always navigate up to the document library list level
            UpButton.Enabled = true;
            goupToolStripMenuItem.Enabled = true;
            goToRootToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = false;
            renameToolStripMenuItem.Enabled = false;
        }

        private async void SharePointDocumentLibraryPicker_DoubleClick(object sender, EventArgs e)
        {
            if (SharePointDocumentLibraryPicker.SelectedItems.Count == 0) return;
            var selectedItem = SharePointDocumentLibraryPicker.SelectedItems[0];

            switch (selectedItem.ImageKey)
            {
                case "DocLib":
                    documentLibraryServerRelativeUrl = selectedItem.Tag.ToString();
                    await LoadDocumentLibraryFileAndFolderItems(selectedItem.Tag.ToString());
                    break;

                case "Folder":
                    await LoadDocumentLibraryFileAndFolderItems(selectedItem.Tag.ToString());
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
            if(string.IsNullOrEmpty(SelectedDocumentLibraryServerRelativeUrl))
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
            if (SharePointDocumentLibraryPicker.SelectedItems.Count > 0 && SharePointDocumentLibraryPicker.SelectedItems[0].ImageKey == "File")
            {
                FileName = SharePointDocumentLibraryPicker.SelectedItems[0].Text;
            }
            
            // Enable the OK button only when an item has been selected and a filename has been provided
            OKButton.Enabled = SharePointDocumentLibraryPicker.SelectedItems.Count > 0 && !string.IsNullOrEmpty(FileName);

            // Don't allow deletion or renames of complete document libraries
            deleteToolStripMenuItem.Enabled = renameToolStripMenuItem.Enabled = SharePointDocumentLibraryPicker.SelectedItems.Count > 0 && SharePointDocumentLibraryPicker.SelectedItems[0].ImageKey != "DocLib";
        }

        private void showHiddenLibrariesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowHiddenLibraries = !ShowHiddenLibraries;
            showHiddenLibrariesToolStripMenuItem.Checked = ShowHiddenLibraries;

            refreshToolStripMenuItem_Click(sender, e);
        }

        private async void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentViewServerRelativeUrl))
            {
                await LoadDocumentLibraryFileAndFolderItems(currentViewServerRelativeUrl);
            }
            else
            {
                await LoadDocumentLibraryItems();
            }
        }

        private async void UpButton_Click(object sender, EventArgs e)
        {
            var newServerRelativeUrl = currentViewServerRelativeUrl.Remove(currentViewServerRelativeUrl.LastIndexOf('/'));
            if(newServerRelativeUrl.Length < documentLibraryServerRelativeUrl.Length)
            {
                await LoadDocumentLibraryItems();
            }
            else
            {
                await LoadDocumentLibraryFileAndFolderItems(newServerRelativeUrl);
            }
        }

        private async void goToRootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await LoadDocumentLibraryItems();
        }

        private void goupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpButton_Click(sender, e);
        }

        private void FileNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && OKButton.Enabled)
            {
                OKButton_Click(sender, e);
            }
        }

        private async void newFoldertoolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newFolderDialog = new OneDriveRequestInputDialog
            {
                FormTitle = "Create new folder"
            };
            newFolderDialog.ShowDialog(this);
            if (newFolderDialog.DialogResult != DialogResult.OK) return;

            try
            {
                var newFolderServerRelativeUrl = await Providers.SharePointProvider.CreateFolder(newFolderDialog.InputValue, SelectedDocumentLibraryServerRelativeUrl, _httpClient);
                MessageBox.Show("Folder has been created", "New Folder", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh the items shown
                refreshToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Folder could not be created (" + ex.Message + ")", "New Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SharePointDocumentLibraryPicker.SelectedItems.Count == 0 || SharePointDocumentLibraryPicker.SelectedItems[0].ImageKey == "DocLib") return;

            var selectedItem = SharePointDocumentLibraryPicker.SelectedItems[0];
            var confirm = MessageBox.Show("Are you sure you want to delete the selected " + selectedItem.ImageKey.ToLowerInvariant() + "? ", "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (confirm != DialogResult.Yes) return;

            bool operationSuccessful = false;
            switch(selectedItem.ImageKey)
            {
                case "File":
                    operationSuccessful = await Providers.SharePointProvider.DeleteFile(selectedItem.Tag.ToString(), _httpClient);
                    break;

                case "Folder":
                    operationSuccessful = await Providers.SharePointProvider.DeleteFolder(selectedItem.Tag.ToString(), _httpClient);
                    break;

                default:
                    MessageBox.Show("Item type '" + selectedItem.ImageKey + "' is not implemented for this operation.", "Delete item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
            }

            if(operationSuccessful)
            { 
                MessageBox.Show(selectedItem.ImageKey + " has been deleted", "Delete item", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Unable to delete " + selectedItem.ImageKey.ToLowerInvariant(), "Delete item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            // Refresh the items shown
            refreshToolStripMenuItem_Click(sender, e);
        }

        private async void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SharePointDocumentLibraryPicker.SelectedItems.Count == 0 || SharePointDocumentLibraryPicker.SelectedItems[0].ImageKey == "DocLib") return;

            var selectedItem = SharePointDocumentLibraryPicker.SelectedItems[0];

            var renameItemDialog = new OneDriveRequestInputDialog
            {
                FormTitle = "Enter new name",
                InputValue = selectedItem.Text
            };
            renameItemDialog.ShowDialog(this);
            if (renameItemDialog.DialogResult != DialogResult.OK) return;

            bool operationSuccessful = false;
            switch (selectedItem.ImageKey)
            {
                case "File":
                    operationSuccessful = await Providers.SharePointProvider.RenameFile(renameItemDialog.InputValue, selectedItem.Tag.ToString(), _httpClient);
                    break;

                case "Folder":
                    operationSuccessful = await Providers.SharePointProvider.RenameFolder(renameItemDialog.InputValue, selectedItem.Tag.ToString(), _httpClient);
                    break;

                default:
                    MessageBox.Show("Item type '" + selectedItem.ImageKey + "' is not implemented for this operation.", "Rename item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
            }

            if (operationSuccessful)
            {
                MessageBox.Show(selectedItem.ImageKey + " has been renamed", "Rename item", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Unable to rename " + selectedItem.ImageKey.ToLowerInvariant(), "Rename item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            // Refresh the items shown
            refreshToolStripMenuItem_Click(sender, e);
        }
    }
}
