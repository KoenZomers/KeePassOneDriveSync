using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using KoenZomersKeePassOneDriveSync.Providers.MicrosoftGraph;

namespace KoenZomersKeePassOneDriveSync.Forms
{
    public partial class SharePointDocumentLibraryPickerDialog : System.Windows.Forms.Form
    {
        #region Properties

        /// <summary>
        /// Returns the id of the selected SharePoint document library drive.
        /// </summary>
        public string SelectedDriveId { get; private set; }

        /// <summary>
        /// Returns the id of the selected SharePoint folder.
        /// </summary>
        public string SelectedFolderId { get; private set; }

        /// <summary>
        /// Returns the id of the selected SharePoint file, if one was selected.
        /// </summary>
        public string SelectedFileId { get; private set; }

        /// <summary>
        /// Returns the selected SharePoint location id.
        /// </summary>
        public string SelectedDocumentLibraryServerRelativeUrl
        {
            get
            {
                return _httpClient != null
                    ? currentViewServerRelativeUrl ?? (SharePointDocumentLibraryPicker.SelectedItems.Count > 0 ? SharePointDocumentLibraryPicker.SelectedItems[0].Tag.ToString() : null)
                    : SelectedFolderId;
            }
        }

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

        private readonly SharePointGraphClient _graphClient;
        private readonly string _siteId;
        private readonly HttpClient _httpClient;
        private GraphDriveItem _currentFolder;
        private string _currentDriveId;
        private string _currentDriveName;
        private GraphDriveItem _currentDriveRoot;
        private string documentLibraryServerRelativeUrl = null;
        private string currentViewServerRelativeUrl = null;

        internal SharePointDocumentLibraryPickerDialog(SharePointGraphClient graphClient, string siteId)
        {
            InitializeComponent();

            _graphClient = graphClient;
            _siteId = siteId;
        }

        public SharePointDocumentLibraryPickerDialog(HttpClient httpClient)
        {
            InitializeComponent();

            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets the document libraries and renders them in the form
        /// </summary>
        public async Task LoadDocumentLibraryItems()
        {
            if (_httpClient != null)
            {
                await LoadRestDocumentLibraryItems();
                return;
            }

            SharePointDocumentLibraryPicker.Items.Clear();
            CloudLocationPath.Text = string.Empty;
            _currentDriveId = null;
            _currentDriveName = null;
            _currentDriveRoot = null;
            _currentFolder = null;
            SelectedDriveId = null;
            SelectedFolderId = null;
            SelectedFileId = null;

            var drives = await _graphClient.GetSiteDrives(_siteId);
            foreach (var drive in drives.OrderBy(drive => drive.Name))
            {
                SharePointDocumentLibraryPicker.Items.Add(new ListViewItem
                {
                    Text = drive.Name,
                    Tag = drive,
                    ImageKey = "DocLib",
                    Selected = drive.Name.Equals(FileNameTextBox.Text, StringComparison.InvariantCultureIgnoreCase)
                });
            }

            UpButton.Enabled = false;
            goupToolStripMenuItem.Enabled = false;
            goToRootToolStripMenuItem.Enabled = false;
            newFoldertoolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;
            renameToolStripMenuItem.Enabled = false;
            showHiddenLibrariesToolStripMenuItem.Enabled = false;
            OKButton.Enabled = SharePointDocumentLibraryPicker.SelectedItems.Count > 0 && !string.IsNullOrEmpty(FileName);
        }

        private async Task LoadRestDocumentLibraryItems()
        {
            SharePointDocumentLibraryPicker.Items.Clear();
            currentViewServerRelativeUrl = "";

            var response = await _httpClient.GetAsync("web/lists?$select=Title,RootFolder/ServerRelativeUrl&$filter=BaseTemplate eq 101" + (ShowHiddenLibraries ? "" : " and Hidden eq false") + "&$expand=RootFolder");
            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            using (var responseJson = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
            {
                foreach (var listViewItem in responseJson.RootElement.GetProperty("value").EnumerateArray())
                {
                    var title = listViewItem.GetProperty("Title").GetString();
                    SharePointDocumentLibraryPicker.Items.Add(new ListViewItem
                    {
                        Text = title,
                        Tag = listViewItem.GetProperty("RootFolder").GetProperty("ServerRelativeUrl").GetString(),
                        ImageKey = "DocLib",
                        Selected = title.Equals(FileNameTextBox.Text, StringComparison.InvariantCultureIgnoreCase)
                    });
                }
            }

            UpButton.Enabled = false;
            goupToolStripMenuItem.Enabled = false;
            goToRootToolStripMenuItem.Enabled = false;
            newFoldertoolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;
            renameToolStripMenuItem.Enabled = false;
            showHiddenLibrariesToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// Gets the folders and files inside a document library and renders them in the form
        /// </summary>
        public async Task LoadDocumentLibraryFileAndFolderItems(string folderId)
        {
            if (_httpClient != null)
            {
                await LoadRestDocumentLibraryFileAndFolderItems(folderId);
                return;
            }

            if (string.IsNullOrEmpty(_currentDriveId))
            {
                await LoadDocumentLibraryItems();
                return;
            }

            var folder = string.IsNullOrEmpty(folderId) || (_currentDriveRoot != null && folderId == _currentDriveRoot.Id)
                ? _currentDriveRoot ?? await _graphClient.GetDriveRootItem(_currentDriveId)
                : await _graphClient.GetDriveItem(_currentDriveId, folderId);

            await LoadDriveFolderItems(folder);
        }

        private async Task LoadRestDocumentLibraryFileAndFolderItems(string serverRelativeUrl)
        {
            currentViewServerRelativeUrl = serverRelativeUrl;
            SharePointDocumentLibraryPicker.Items.Clear();

            using (var foldersResponse = await _httpClient.GetAsync("web/GetFolderByServerRelativeUrl('" + serverRelativeUrl + "')/Folders?$select=Name,ServerRelativeUrl,ItemCount,TimeCreated,TimeLastModified"))
            {
                if (!foldersResponse.IsSuccessStatusCode)
                {
                    return;
                }

                using (var foldersResponseJson = JsonDocument.Parse(await foldersResponse.Content.ReadAsStringAsync()))
                {
                    foreach (var listViewItem in foldersResponseJson.RootElement.GetProperty("value").EnumerateArray())
                    {
                        var folderItem = new ListViewItem
                        {
                            Text = listViewItem.GetProperty("Name").GetString(),
                            Tag = listViewItem.GetProperty("ServerRelativeUrl").GetString(),
                            ImageKey = "Folder"
                        };

                        JsonElement itemCountElement;
                        long itemCount;
                        if (listViewItem.TryGetProperty("ItemCount", out itemCountElement) && long.TryParse(itemCountElement.ToString(), out itemCount))
                        {
                            folderItem.ToolTipText += string.Format("Items inside: {0}", itemCount) + Environment.NewLine;
                        }
                        JsonElement createdElement;
                        DateTime created;
                        if (listViewItem.TryGetProperty("TimeCreated", out createdElement) && DateTime.TryParse(createdElement.ToString(), out created))
                        {
                            folderItem.ToolTipText += string.Format("Created: {0:d MMMM yyyy HH:mm:ss}", created) + Environment.NewLine;
                        }
                        JsonElement lastModifiedElement;
                        DateTime lastModified;
                        if (listViewItem.TryGetProperty("TimeLastModified", out lastModifiedElement) && DateTime.TryParse(lastModifiedElement.ToString(), out lastModified))
                        {
                            folderItem.ToolTipText += string.Format("Last modified: {0:d MMMM yyyy HH:mm:ss}", lastModified) + Environment.NewLine;
                        }

                        SharePointDocumentLibraryPicker.Items.Add(folderItem);
                    }
                }
            }

            using (var filesResponse = await _httpClient.GetAsync("web/GetFolderByServerRelativeUrl('" + serverRelativeUrl + "')/Files?$select=Name,ServerRelativeUrl,TimeLastModified,TimeCreated,UIVersionLabel,Length"))
            {
                if (!filesResponse.IsSuccessStatusCode)
                {
                    return;
                }

                using (var filesResponseJson = JsonDocument.Parse(await filesResponse.Content.ReadAsStringAsync()))
                {
                    foreach (var listViewItem in filesResponseJson.RootElement.GetProperty("value").EnumerateArray())
                    {
                        var name = listViewItem.GetProperty("Name").GetString();
                        var fileItem = new ListViewItem
                        {
                            Text = name,
                            Tag = listViewItem.GetProperty("ServerRelativeUrl").GetString(),
                            ImageKey = "File",
                            Selected = name.Equals(FileNameTextBox.Text, StringComparison.InvariantCultureIgnoreCase)
                        };

                        JsonElement lengthElement;
                        long fileSize;
                        if (listViewItem.TryGetProperty("Length", out lengthElement) && long.TryParse(lengthElement.ToString(), out fileSize))
                        {
                            fileItem.ToolTipText += string.Format("Size: {0:n0} bytes", fileSize) + Environment.NewLine;
                        }
                        JsonElement uiVersionLabelElement;
                        if (listViewItem.TryGetProperty("UIVersionLabel", out uiVersionLabelElement) && uiVersionLabelElement.ToString().Length > 0)
                        {
                            fileItem.ToolTipText += string.Format("Version: {0}", uiVersionLabelElement) + Environment.NewLine;
                        }
                        JsonElement createdElement;
                        DateTime created;
                        if (listViewItem.TryGetProperty("TimeCreated", out createdElement) && DateTime.TryParse(createdElement.ToString(), out created))
                        {
                            fileItem.ToolTipText += string.Format("Created: {0:d MMMM yyyy HH:mm:ss}", created) + Environment.NewLine;
                        }
                        JsonElement lastModifiedElement;
                        DateTime lastModified;
                        if (listViewItem.TryGetProperty("TimeLastModified", out lastModifiedElement) && DateTime.TryParse(lastModifiedElement.ToString(), out lastModified))
                        {
                            fileItem.ToolTipText += string.Format("Last modified: {0:d MMMM yyyy HH:mm:ss}", lastModified) + Environment.NewLine;
                        }

                        SharePointDocumentLibraryPicker.Items.Add(fileItem);
                    }
                }
            }

            CloudLocationPath.Text = serverRelativeUrl;
            UpButton.Enabled = true;
            goupToolStripMenuItem.Enabled = true;
            goToRootToolStripMenuItem.Enabled = true;
            newFoldertoolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = false;
            renameToolStripMenuItem.Enabled = false;
        }

        private async Task LoadDriveFolderItems(GraphDriveItem folder)
        {
            _currentFolder = folder;
            SelectedDriveId = _currentDriveId;
            SelectedFolderId = folder.Id;
            SelectedFileId = null;
            SharePointDocumentLibraryPicker.Items.Clear();

            var children = folder.Id == _currentDriveRoot.Id
                ? await _graphClient.GetDriveRootChildren(_currentDriveId)
                : await _graphClient.GetDriveItemChildren(_currentDriveId, folder.Id);

            foreach (var item in children.OrderBy(item => item.Folder == null).ThenBy(item => item.Name))
            {
                var listViewItem = new ListViewItem
                {
                    Text = item.Name,
                    Tag = item,
                    ImageKey = item.Folder != null ? "Folder" : "File",
                    Selected = item.Name.Equals(FileNameTextBox.Text, StringComparison.InvariantCultureIgnoreCase)
                };

                if (item.Size.HasValue && item.Size.Value > 0)
                {
                    listViewItem.ToolTipText += string.Format("Size: {0:n0} bytes", item.Size.Value) + Environment.NewLine;
                }
                if (item.CreatedDateTime.HasValue)
                {
                    listViewItem.ToolTipText += string.Format("Created: {0:d MMMM yyyy HH:mm:ss}", item.CreatedDateTime.Value.LocalDateTime) + Environment.NewLine;
                }
                if (item.LastModifiedDateTime.HasValue)
                {
                    listViewItem.ToolTipText += string.Format("Last modified: {0:d MMMM yyyy HH:mm:ss}", item.LastModifiedDateTime.Value.LocalDateTime) + Environment.NewLine;
                }

                SharePointDocumentLibraryPicker.Items.Add(listViewItem);
            }

            CloudLocationPath.Text = _currentDriveName + (_currentFolder.ParentReference != null && _currentFolder.ParentReference.Path != null ? _currentFolder.ParentReference.Path.Substring(_currentFolder.ParentReference.Path.IndexOf("root:", StringComparison.Ordinal) + 5) + "/" + _currentFolder.Name : string.Empty);
            UpButton.Enabled = folder.Id != _currentDriveRoot.Id;
            goupToolStripMenuItem.Enabled = UpButton.Enabled;
            goToRootToolStripMenuItem.Enabled = true;
            newFoldertoolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = false;
            renameToolStripMenuItem.Enabled = false;
        }

        private async void SharePointDocumentLibraryPicker_DoubleClick(object sender, EventArgs e)
        {
            if (SharePointDocumentLibraryPicker.SelectedItems.Count == 0) return;
            var selectedItem = SharePointDocumentLibraryPicker.SelectedItems[0];

            if (_httpClient != null)
            {
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
                return;
            }

            switch (selectedItem.ImageKey)
            {
                case "DocLib":
                    var drive = (GraphDrive)selectedItem.Tag;
                    _currentDriveId = drive.Id;
                    _currentDriveName = drive.Name;
                    _currentDriveRoot = await _graphClient.GetDriveRootItem(drive.Id);
                    await LoadDriveFolderItems(_currentDriveRoot);
                    break;

                case "Folder":
                    await LoadDriveFolderItems((GraphDriveItem)selectedItem.Tag);
                    break;

                case "File":
                    if (OKButton.Enabled)
                    {
                        OKButton_Click(sender, e);
                    }
                    break;
            }
        }

        private async void OKButton_Click(object sender, EventArgs e)
        {
            if (_httpClient != null)
            {
                if (string.IsNullOrEmpty(SelectedDocumentLibraryServerRelativeUrl))
                {
                    MessageBox.Show(AllowEnteringNewFileName ? "Select a document library to store the KeePass database in" : "Select the KeePass file to download", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(FileName))
                {
                    MessageBox.Show(AllowEnteringNewFileName ? "Enter the filename under which you wish to store the database" : "Select the KeePass file to download", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            if (SharePointDocumentLibraryPicker.SelectedItems.Count > 0)
            {
                var selectedItem = SharePointDocumentLibraryPicker.SelectedItems[0];
                if (selectedItem.ImageKey == "DocLib")
                {
                    var drive = (GraphDrive)selectedItem.Tag;
                    SelectedDriveId = drive.Id;
                    SelectedFolderId = (await _graphClient.GetDriveRootItem(drive.Id)).Id;
                    SelectedFileId = null;
                }
                else if (selectedItem.ImageKey == "File")
                {
                    var file = (GraphDriveItem)selectedItem.Tag;
                    SelectedDriveId = _currentDriveId;
                    SelectedFolderId = file.ParentReference != null ? file.ParentReference.Id : _currentFolder.Id;
                    SelectedFileId = file.Id;
                    FileName = file.Name;
                }
                else if (selectedItem.ImageKey == "Folder")
                {
                    var folder = (GraphDriveItem)selectedItem.Tag;
                    SelectedDriveId = _currentDriveId;
                    SelectedFolderId = folder.Id;
                    SelectedFileId = null;
                }
            }
            else if (_currentFolder != null)
            {
                SelectedDriveId = _currentDriveId;
                SelectedFolderId = _currentFolder.Id;
                SelectedFileId = null;
            }

            if (string.IsNullOrEmpty(SelectedDriveId) || string.IsNullOrEmpty(SelectedFolderId))
            {
                MessageBox.Show(AllowEnteringNewFileName ? "Select a document library or folder to store the KeePass database in" : "Select the KeePass file to download", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(FileName))
            {
                MessageBox.Show(AllowEnteringNewFileName ? "Enter the filename under which you wish to store the database" : "Select the KeePass file to download", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!AllowEnteringNewFileName && string.IsNullOrEmpty(SelectedFileId))
            {
                MessageBox.Show("Select the KeePass file to download", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            OKButton.Enabled = !string.IsNullOrEmpty(FileName) && (SharePointDocumentLibraryPicker.SelectedItems.Count > 0 || _currentFolder != null);
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
            if (_httpClient != null)
            {
                if (!string.IsNullOrEmpty(currentViewServerRelativeUrl))
                {
                    await LoadDocumentLibraryFileAndFolderItems(currentViewServerRelativeUrl);
                }
                else
                {
                    await LoadDocumentLibraryItems();
                }
                return;
            }

            if (_currentFolder != null)
            {
                await LoadDriveFolderItems(_currentFolder);
            }
            else
            {
                await LoadDocumentLibraryItems();
            }
        }

        private async void UpButton_Click(object sender, EventArgs e)
        {
            if (_httpClient != null)
            {
                var newServerRelativeUrl = currentViewServerRelativeUrl.Remove(currentViewServerRelativeUrl.LastIndexOf('/'));
                if (newServerRelativeUrl.Length < documentLibraryServerRelativeUrl.Length)
                {
                    await LoadDocumentLibraryItems();
                }
                else
                {
                    await LoadDocumentLibraryFileAndFolderItems(newServerRelativeUrl);
                }
                return;
            }

            if (_currentFolder == null || _currentDriveRoot == null)
            {
                await LoadDocumentLibraryItems();
                return;
            }

            if (_currentFolder.Id == _currentDriveRoot.Id || _currentFolder.ParentReference == null || string.IsNullOrEmpty(_currentFolder.ParentReference.Id))
            {
                await LoadDocumentLibraryItems();
                return;
            }

            await LoadDriveFolderItems(await _graphClient.GetDriveItem(_currentDriveId, _currentFolder.ParentReference.Id));
        }

        private async void goToRootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_httpClient != null)
            {
                await LoadDocumentLibraryItems();
                return;
            }

            if (!string.IsNullOrEmpty(_currentDriveId))
            {
                await LoadDriveFolderItems(_currentDriveRoot ?? await _graphClient.GetDriveRootItem(_currentDriveId));
                return;
            }

            await LoadDocumentLibraryItems();
        }

        private void goupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpButton_Click(sender, e);
        }

        private void FileNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            OKButton.Enabled = !string.IsNullOrEmpty(FileName) && (SharePointDocumentLibraryPicker.SelectedItems.Count > 0 || _currentFolder != null);
            if (e.KeyCode == Keys.Enter && OKButton.Enabled)
            {
                OKButton_Click(sender, e);
            }
        }

        private async void newFoldertoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_httpClient != null)
            {
                var restNewFolderDialog = new OneDriveRequestInputDialog
                {
                    FormTitle = "Create new folder"
                };
                restNewFolderDialog.ShowDialog(this);
                if (restNewFolderDialog.DialogResult != DialogResult.OK) return;

                try
                {
                    await Providers.SharePointProvider.CreateFolder(restNewFolderDialog.InputValue, SelectedDocumentLibraryServerRelativeUrl, _httpClient);
                    MessageBox.Show("Folder has been created", "New Folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refreshToolStripMenuItem_Click(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Folder could not be created (" + ex.Message + ")", "New Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                return;
            }

            if (string.IsNullOrEmpty(_currentDriveId) || _currentFolder == null) return;

            var newFolderDialog = new OneDriveRequestInputDialog
            {
                FormTitle = "Create new folder"
            };
            newFolderDialog.ShowDialog(this);
            if (newFolderDialog.DialogResult != DialogResult.OK) return;

            try
            {
                await _graphClient.CreateFolder(newFolderDialog.InputValue, _currentDriveId, _currentFolder.Id);
                MessageBox.Show("Folder has been created", "New Folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (_httpClient != null)
            {
                var confirmRest = MessageBox.Show("Are you sure you want to delete the selected " + selectedItem.ImageKey.ToLowerInvariant() + "? " + (selectedItem.ImageKey == "Folder" ? "Note that folders can only be removed if there's nothing inside anymore. " : ""), "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (confirmRest != DialogResult.Yes) return;

                bool operationSuccessful;
                switch (selectedItem.ImageKey)
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

                MessageBox.Show(operationSuccessful ? selectedItem.ImageKey + " has been deleted" : "Unable to delete " + selectedItem.ImageKey.ToLowerInvariant(), "Delete item", MessageBoxButtons.OK, operationSuccessful ? MessageBoxIcon.Information : MessageBoxIcon.Exclamation);
                refreshToolStripMenuItem_Click(sender, e);
                return;
            }

            var selectedDriveItem = (GraphDriveItem)selectedItem.Tag;
            var confirm = MessageBox.Show("Are you sure you want to delete the selected " + selectedItem.ImageKey.ToLowerInvariant() + "? " + (selectedItem.ImageKey == "Folder" ? "Note that folders can only be removed if there's nothing inside anymore. " : ""), "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (confirm != DialogResult.Yes) return;

            try
            {
                await _graphClient.DeleteItem(_currentDriveId, selectedDriveItem.Id);
                MessageBox.Show(selectedItem.ImageKey + " has been deleted", "Delete item", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to delete " + selectedItem.ImageKey.ToLowerInvariant() + " (" + ex.Message + ")", "Delete item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            refreshToolStripMenuItem_Click(sender, e);
        }

        private async void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SharePointDocumentLibraryPicker.SelectedItems.Count == 0 || SharePointDocumentLibraryPicker.SelectedItems[0].ImageKey == "DocLib") return;

            var selectedItem = SharePointDocumentLibraryPicker.SelectedItems[0];
            if (_httpClient != null)
            {
                var restRenameItemDialog = new OneDriveRequestInputDialog
                {
                    FormTitle = "Enter new name",
                    InputValue = selectedItem.Text
                };
                restRenameItemDialog.ShowDialog(this);
                if (restRenameItemDialog.DialogResult != DialogResult.OK) return;

                bool operationSuccessful;
                switch (selectedItem.ImageKey)
                {
                    case "File":
                        operationSuccessful = await Providers.SharePointProvider.RenameFile(restRenameItemDialog.InputValue, selectedItem.Tag.ToString(), _httpClient);
                        break;

                    case "Folder":
                        operationSuccessful = await Providers.SharePointProvider.RenameFolder(restRenameItemDialog.InputValue, selectedItem.Tag.ToString(), _httpClient);
                        break;

                    default:
                        MessageBox.Show("Item type '" + selectedItem.ImageKey + "' is not implemented for this operation.", "Rename item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                }

                MessageBox.Show(operationSuccessful ? selectedItem.ImageKey + " has been renamed" : "Unable to rename " + selectedItem.ImageKey.ToLowerInvariant(), "Rename item", MessageBoxButtons.OK, operationSuccessful ? MessageBoxIcon.Information : MessageBoxIcon.Exclamation);
                refreshToolStripMenuItem_Click(sender, e);
                return;
            }

            var selectedDriveItem = (GraphDriveItem)selectedItem.Tag;

            var renameItemDialog = new OneDriveRequestInputDialog
            {
                FormTitle = "Enter new name",
                InputValue = selectedItem.Text
            };
            renameItemDialog.ShowDialog(this);
            if (renameItemDialog.DialogResult != DialogResult.OK) return;

            try
            {
                await _graphClient.RenameItem(renameItemDialog.InputValue, _currentDriveId, selectedDriveItem.Id);
                MessageBox.Show(selectedItem.ImageKey + " has been renamed", "Rename item", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to rename " + selectedItem.ImageKey.ToLowerInvariant() + " (" + ex.Message + ")", "Rename item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            refreshToolStripMenuItem_Click(sender, e);
        }
    }
}
