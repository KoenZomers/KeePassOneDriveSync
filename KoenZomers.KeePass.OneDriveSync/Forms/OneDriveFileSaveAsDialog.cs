using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using KoenZomers.OneDrive.Api;
using KoenZomers.OneDrive.Api.Entities;

namespace KoenZomersKeePassOneDriveSync.Forms
{
     public partial class OneDriveFileSaveAsDialog : Form
    {
        /// <summary>
        /// Instance of the OneDrive API that can be used to communicate with the cloud service
        /// </summary>
        private readonly OneDriveApi _oneDriveApi;

        /// <summary>
        /// Reference to the currently displayed folder on OneDrive
        /// </summary>
        public OneDriveItem CurrentOneDriveItem;

        /// <summary>
        /// Gets or the filename in the textbox on the screen
        /// </summary>
        public string FileName
        {
            get { return FileNameTextBox.Text; }
            set { FileNameTextBox.Text = value; }
        }

        public OneDriveFileSaveAsDialog(OneDriveApi oneDriveApi)
        {
            InitializeComponent();

            _oneDriveApi = oneDriveApi;
        }

         public async Task LoadFolderItems(string parentItemId = null)
         {
             CloudLocationPicker.Items.Clear();

             OneDriveItemCollection itemCollection;
             if (parentItemId == null)
             {
                 itemCollection = await _oneDriveApi.GetDriveRootChildren();
                 CloudLocationPath.Text = "/drive/root:";
                 UpButton.Enabled = false;
                 CurrentOneDriveItem = null;
                 CurrentOneDriveItem = await _oneDriveApi.GetDriveRoot();
             }
             else
             {
                 itemCollection = await _oneDriveApi.GetChildrenByFolderId(parentItemId);
                 CurrentOneDriveItem = await _oneDriveApi.GetItemById(parentItemId);
                 UpButton.Enabled = CurrentOneDriveItem.ParentReference != null;
                 UpButton.Tag = CurrentOneDriveItem.ParentReference != null ? CurrentOneDriveItem.ParentReference.Id : null;
                 CloudLocationPath.Text = CurrentOneDriveItem.ParentReference != null ? CurrentOneDriveItem.ParentReference.Path + "/" + CurrentOneDriveItem.Name : "";
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
         }

         private async void CloudLocationPicker_DoubleClick(object sender, EventArgs e)
         {
             if (CloudLocationPicker.SelectedItems.Count == 0) return;

             var selectedItem = CloudLocationPicker.SelectedItems[0];
             if (selectedItem.ImageKey == "File" && OKButton.Enabled)
             {
                 OKButton_Click(sender, e);
             }

             await LoadFolderItems(selectedItem.Tag.ToString());
         }

         private async void UpButton_Click(object sender, EventArgs e)
        {
            var childItem = UpButton.Tag as string;
            await LoadFolderItems(childItem);
        }

        private async void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await LoadFolderItems(CurrentOneDriveItem != null ? CurrentOneDriveItem.Id : null);
        }

        private void OneDriveFileSaveAsDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && UpButton.Enabled)
            {
                UpButton_Click(sender, null);
            }
        }

        private async void OKButton_Click(object sender, EventArgs e)
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

        private async void goToRootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await LoadFolderItems();
        }

        private void ListViewContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            renameToolStripMenuItem.Enabled = deleteToolStripMenuItem.Enabled = CloudLocationPicker.SelectedItems.Count > 0;
            goToRootToolStripMenuItem.Enabled = CloudLocationPath.Text != "/drive/root:";
        }

        private void OneDriveFileSaveAsDialog_Load(object sender, EventArgs e)
        {

        }

        private async void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newFolderDialog = new OneDriveRequestInputDialog
            {
                FormTitle = "Create new folder"
            };
            newFolderDialog.ShowDialog(this);
            if (newFolderDialog.DialogResult != DialogResult.OK) return;

            try
            {
                var newFolderItem = await _oneDriveApi.CreateFolder(CurrentOneDriveItem, newFolderDialog.InputValue);
                MessageBox.Show("Folder has been created", "New Folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadFolderItems(newFolderItem.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Folder could not be created", "New Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
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
                await LoadFolderItems(CurrentOneDriveItem.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Item could not be deleted", "Delete item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void FileNameTextBox_TextChanged(object sender, EventArgs e)
        {
            OKButton.Enabled = !string.IsNullOrEmpty(FileNameTextBox.Text);
        }

        private async void renameToolStripMenuItem_Click(object sender, EventArgs e)
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
                    await LoadFolderItems(CurrentOneDriveItem != null ? CurrentOneDriveItem.Id : null);
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

        private void goupToolStripMenuItem_Click(object sender, EventArgs e)
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
    }
}
