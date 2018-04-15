using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using KoenZomers.OneDrive.Api;
using KoenZomers.OneDrive.Api.Entities;

namespace KoenZomersKeePassOneDriveSync.Forms
{
     public partial class OneDriveFilePickerDialog : Form
    {
        /// <summary>
        /// Instance of the OneDrive API that can be used to communicate with the cloud service
        /// </summary>
        private readonly OneDriveApi _oneDriveApi;

        /// <summary>
        /// Reference to the currently displayed folder on OneDrive for the My files tab
        /// </summary>
        private OneDriveItem CurrentMyOneDriveItem;

        /// <summary>
        /// Reference to the currently displayed folder on OneDrive for the Shared With Me tab
        /// </summary>
        private OneDriveItem CurrentSharedWithMeOneDriveItem;

        /// <summary>
        /// Reference to the currently displayed folder on OneDrive for the active tab
        /// </summary>
        public OneDriveItem CurrentOneDriveItem
        {
            get { return FilesTabControl.SelectedIndex == 0 ? CurrentMyOneDriveItem : CurrentSharedWithMeOneDriveItem; }
        }

        /// <summary>
        /// Gets or the filename in the textbox on the screen
        /// </summary>
        public string FileName
        {
            get { return FileNameTextBox.Text; }
            set { FileNameTextBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets the text to explain the purpose of the dialog
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

        public OneDriveFilePickerDialog(OneDriveApi oneDriveApi)
        {
            InitializeComponent();

            _oneDriveApi = oneDriveApi;

            var sharedWithMeDisabled = (oneDriveApi is OneDriveForBusinessO365Api);

            SharedWithMePicker.Visible = !sharedWithMeDisabled;
            SharedWithMeUpButton.Visible = !sharedWithMeDisabled;
            SharedWithMeNotAvailableLabel.Visible = sharedWithMeDisabled;
        }

        /// <summary>
        /// Loads the folders in the "My files" view
        /// </summary>
        /// <param name="parentItemId">Folder to display the child folders and items of. If not provided or NULL, the root folder will be used.</param>
         public async Task LoadFolderItems(string parentItemId = null)
         {
             CloudLocationPicker.Items.Clear();

            // Check if there is a parent folder to display its children of
             OneDriveItemCollection itemCollection;
             if (parentItemId == null)
             {
                // No parent folder, get the items under the root
                itemCollection = await _oneDriveApi.GetDriveRootChildren();
                CloudLocationPath.Text = "/drive/root:";
                UpButton.Enabled = false;
                GoToRootToolStripMenuItem.Enabled = false;
                CurrentMyOneDriveItem = await _oneDriveApi.GetDriveRoot();
             }
             else
             {
                // Parent folder provided, get its children
                itemCollection = await _oneDriveApi.GetChildrenByFolderId(parentItemId);
                CurrentMyOneDriveItem = await _oneDriveApi.GetItemById(parentItemId);
                GoToRootToolStripMenuItem.Enabled = true;
                UpButton.Enabled = CurrentMyOneDriveItem.ParentReference != null;
                UpButton.Tag = CurrentMyOneDriveItem.ParentReference != null ? CurrentMyOneDriveItem.ParentReference.Id : null;
                CloudLocationPath.Text = CurrentMyOneDriveItem.ParentReference != null ? CurrentMyOneDriveItem.ParentReference.Path + "/" + CurrentMyOneDriveItem.Name : "";
             }

             foreach (var listViewItem in itemCollection.Collection.Select(oneDriveItem => new ListViewItem
             {
                 Text = oneDriveItem.Name,
                 Tag = oneDriveItem.RemoteItem != null ? oneDriveItem.RemoteItem.Id : oneDriveItem.Id,
                 ImageKey = oneDriveItem.Folder != null ? "Folder" : oneDriveItem.RemoteItem != null ? "RemoteFolder" : "File"
             }))
             {
                 CloudLocationPicker.Items.Add(listViewItem);
             }

            // Define if the OK button should be enabled
            FileNameTextBox_TextChanged(null, null);
        }

        /// <summary>
        /// Loads the items shared with the user in the "Shared with me" overview
        /// </summary>
        /// <param name="parentItem">OneDriveItem to display the child folders and items of. If not provided or NULL, the root folder will be used.</param>
        public async Task LoadSharedWithMeItems(OneDriveItem parentItem = null)
        {
            SharedWithMePicker.Items.Clear();

            if (parentItem == null)
            {
                // Get the root of the shared with me items
                var itemCollection = await _oneDriveApi.GetSharedWithMe();
                SharedWithMeUpButton.Enabled = false;
                CurrentSharedWithMeOneDriveItem = null;
                GoToSharedWithMeRootTtoolStripMenuItem.Enabled = false;
                SharedWithMePath.Text = string.Empty;

                foreach (var listViewItem in itemCollection.Collection.OrderBy(i => i.Name).OrderBy(i => i.Folder == null).OrderBy(i => i.RemoteItem.Folder == null).Select(oneDriveItem => new ListViewItem
                {
                    Text = oneDriveItem.Name,
                    Tag = oneDriveItem,
                    ImageKey = (oneDriveItem.RemoteItem != null && oneDriveItem.RemoteItem.Folder != null) || oneDriveItem.Folder != null ? "RemoteFolder" : "File"
                }))
                {
                    SharedWithMePicker.Items.Add(listViewItem);
                }
            }
            else
            {
                // Parent folder provided, get its children
                var itemCollection = await _oneDriveApi.GetAllChildrenFromDriveByFolderId(parentItem.RemoteItem != null ? parentItem.RemoteItem.ParentReference.DriveId : parentItem.ParentReference.DriveId, parentItem.RemoteItem != null ? parentItem.RemoteItem.ParentReference != null ? string.IsNullOrEmpty(parentItem.RemoteItem.ParentReference.Id) ? parentItem.RemoteItem.Id : parentItem.RemoteItem.ParentReference.Id : parentItem.RemoteItem.Id : parentItem.Id);

                SharedWithMeUpButton.Tag = parentItem.Name == "root" || parentItem.ParentReference == null || parentItem.ParentReference.Id == null || (parentItem.ParentReference.Path != null && parentItem.ParentReference.Path.EndsWith("root:")) ? null : await _oneDriveApi.GetItemFromDriveById(parentItem.ParentReference.Id, parentItem.ParentReference.DriveId);
                CurrentSharedWithMeOneDriveItem = parentItem;
                GoToSharedWithMeRootTtoolStripMenuItem.Enabled = true;
                SharedWithMeUpButton.Enabled = CurrentSharedWithMeOneDriveItem.ParentReference != null;

                if (CurrentSharedWithMeOneDriveItem.ParentReference != null && CurrentSharedWithMeOneDriveItem.ParentReference.Path != null)
                {
                    var rootLocation = CurrentSharedWithMeOneDriveItem.ParentReference.Path.IndexOf("root:");
                    SharedWithMePath.Text = ((rootLocation == -1 ? CurrentSharedWithMeOneDriveItem.ParentReference.Path : CurrentSharedWithMeOneDriveItem.ParentReference.Path.Remove(0, rootLocation + 5).TrimStart(new[] {'/'})) + "/" + CurrentSharedWithMeOneDriveItem.Name).TrimStart(new[] { '/' });
                }
                else
                {
                    SharedWithMePath.Text = CurrentSharedWithMeOneDriveItem.Name;
                }

                foreach (var listViewItem in itemCollection.Select(oneDriveItem => new ListViewItem
                {
                    Text = oneDriveItem.Name,
                    Tag = oneDriveItem,
                    ImageKey = oneDriveItem.Folder != null ? "RemoteFolder" : "File"
                }))
                {
                    SharedWithMePicker.Items.Add(listViewItem);
                }
            }

            // Define if the OK button should be enabled
            FileNameTextBox_TextChanged(null, null);
        }

        private async void CloudLocationPicker_DoubleClick(object sender, EventArgs e)
         {
             if (CloudLocationPicker.SelectedItems.Count == 0) return;

             var selectedItem = CloudLocationPicker.SelectedItems[0];
             if (selectedItem.ImageKey == "File" && OKButton.Enabled)
             {
                OKButton_Click(sender, e);
                return;
             }

             await LoadFolderItems(selectedItem.Tag.ToString());
         }

         private async void UpButton_Click(object sender, EventArgs e)
        {
            var childItem = UpButton.Tag as string;
            await LoadFolderItems(childItem);
        }

        private async void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await LoadFolderItems(CurrentMyOneDriveItem != null ? CurrentMyOneDriveItem.Id : null);
        }

        private void OneDriveFileSaveAsDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && UpButton.Enabled)
            {
                UpButton_Click(sender, null);
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CloudLocationPicker_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (CloudLocationPicker.SelectedItems.Count == 0) return;

            var selectedItem = CloudLocationPicker.SelectedItems[0];
            if (selectedItem.ImageKey == "File")
            {
                FileNameTextBox.Text = selectedItem.Text;
            }
        }

        private async void GoToRootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await LoadFolderItems();
        }

        private void ListViewContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            renameToolStripMenuItem.Enabled = deleteToolStripMenuItem.Enabled = CloudLocationPicker.SelectedItems.Count > 0;
        }

        private void OneDriveFileSaveAsDialog_Load(object sender, EventArgs e)
        {

        }

        private async void NewFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newFolderDialog = new OneDriveRequestInputDialog
            {
                FormTitle = "Create new folder"
            };
            newFolderDialog.ShowDialog(this);
            if (newFolderDialog.DialogResult != DialogResult.OK) return;

            try
            {
                var newFolderItem = await _oneDriveApi.CreateFolder(CurrentMyOneDriveItem, newFolderDialog.InputValue);
                MessageBox.Show("Folder has been created", "New Folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadFolderItems(newFolderItem.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Folder could not be created", "New Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private async void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to delete the selected item?", "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (confirm != DialogResult.Yes) return;
            if (CloudLocationPicker.SelectedItems.Count == 0) return;
            if (CloudLocationPicker.SelectedItems[0].Tag == null) return;

            try
            {
                var itemToDelete = await _oneDriveApi.GetItemById(CloudLocationPicker.SelectedItems[0].Tag.ToString());
                await _oneDriveApi.Delete(itemToDelete);
                MessageBox.Show("Item has been deleted", "Delete item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadFolderItems(CurrentMyOneDriveItem.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Item could not be deleted", "Delete item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void FileNameTextBox_TextChanged(object sender, EventArgs e)
        {
            OKButton.Enabled = CurrentOneDriveItem != null && !string.IsNullOrEmpty(FileNameTextBox.Text);
        }

        private async void RenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CloudLocationPicker.SelectedItems.Count == 0) return;
            if (CloudLocationPicker.SelectedItems[0].Tag == null) return;

            var renameItemDialog = new OneDriveRequestInputDialog
            {
                FormTitle = "Enter new name",
                InputValue = CloudLocationPicker.SelectedItems[0].Text
            };
            renameItemDialog.ShowDialog(this);
            if (renameItemDialog.DialogResult != DialogResult.OK) return;

            try
            {
                var oneDriveItemToRename = await _oneDriveApi.GetItemById(CloudLocationPicker.SelectedItems[0].Tag.ToString());
                var operationSuccessful = await _oneDriveApi.Rename(oneDriveItemToRename, renameItemDialog.InputValue);
                if (operationSuccessful)
                {
                    MessageBox.Show("Item has been renamed", "Rename item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadFolderItems(CurrentMyOneDriveItem != null ? CurrentMyOneDriveItem.Id : null);
                }
                else
                {
                    MessageBox.Show("Item could not be renamed", "Rename item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Item could not be renamed", "Rename item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void GroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpButton_Click(sender, e);
        }

        private void UpButton_EnabledChanged(object sender, EventArgs e)
        {
            goupToolStripMenuItem.Enabled = UpButton.Enabled;
        }

        private void CloudLocationPicker_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CloudLocationPicker_DoubleClick(sender, e);
            }
        }

        private void FileNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && OKButton.Enabled)
            {
                OKButton_Click(sender, e);
            }
        }

        private async void FilesTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When switching to the "Shared with me" tab and the items have not been loaded yet, load them now
            if(FilesTabControl.SelectedIndex == 1 && SharedWithMePicker.Items.Count == 0)
            {
                await LoadSharedWithMeItems();
            }

            // Ensure the OK button is properly enabled/disabled when switching tabs
            switch (FilesTabControl.SelectedIndex)
            {
                case 1:
                    SharedWithMePicker_ItemSelectionChanged(sender, null);
                    break;

                case 0:
                    CloudLocationPicker_ItemSelectionChanged(sender, null);
                    break;
            }
        }

        private async void RefreshSharedWithMeFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await LoadSharedWithMeItems(CurrentSharedWithMeOneDriveItem);
        }

        private async void SharedWithMePicker_DoubleClick(object sender, EventArgs e)
        {
            if (SharedWithMePicker.SelectedItems.Count == 0) return;

            var selectedItem = SharedWithMePicker.SelectedItems[0];
            if (selectedItem.ImageKey == "File" && OKButton.Enabled)
            {
                OKButton_Click(sender, e);
                return;
            }

            await LoadSharedWithMeItems(selectedItem.Tag as OneDriveItem);
        }

        private void SharedWithMePicker_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SharedWithMePicker_DoubleClick(sender, e);
            }
        }

        private async void SharedWithMeUpButton_Click(object sender, EventArgs e)
        {
            var childItem = SharedWithMeUpButton.Tag as OneDriveItem;
            await LoadSharedWithMeItems(childItem);
        }

        private async void GoToSharedWithMeRootTtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            await LoadSharedWithMeItems();
        }

        private async void GoUpSharedWithMeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void SharedWithMeContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void SharedWithMePicker_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (SharedWithMePicker.SelectedItems.Count == 0)
            {
                // If we're not in a folder yet, disable the button as a new file can't be created in Shared With Me outside a folder
                if(string.IsNullOrEmpty(SharedWithMePath.Text))
                {
                    OKButton.Enabled = false;
                }
                return;
            }

            var selectedItem = SharedWithMePicker.SelectedItems[0];
            if (selectedItem.ImageKey == "File")
            {
                FileNameTextBox.Text = selectedItem.Text;
                OKButton.Enabled = true;
                CurrentSharedWithMeOneDriveItem = selectedItem.Tag as OneDriveItem;
            }
        }
    }
}
