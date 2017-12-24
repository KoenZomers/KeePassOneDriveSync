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
        public string SelectedDocumentLibraryServerRelativeUrl { get { return SharePointDocumentLibraryPicker.SelectedItems.Count == 0 ? null : SharePointDocumentLibraryPicker.SelectedItems[0].Tag.ToString(); } }

        #endregion

        /// <summary>
        /// Instance of the SharePoint ClientContext that can be used to communicate with SharePoint
        /// </summary>
        private readonly ClientContext _clientContext;

        /// <summary>
        /// Gets or the filename in the textbox on the screen
        /// </summary>
        public string FileName
        {
            get { return FileNameTextBox.Text; }
            set { FileNameTextBox.Text = value; }
        }

        public SharePointDocumentLibraryPickerDialog(ClientContext clientContext)
        {
            InitializeComponent();

            _clientContext = clientContext;
        }

        /// <summary>
        /// Gets the document library items that are not hidden and renders them in the form
        /// </summary>
        public void LoadDocumentLibraryItems()
        {
            SharePointDocumentLibraryPicker.Items.Clear();

            _clientContext.Load(_clientContext.Web.Lists, l => l.Include(li => li.RootFolder.ServerRelativeUrl, li => li.Title, li => li.BaseTemplate, li => li.Hidden));
            _clientContext.ExecuteQuery();

            foreach (var listViewItem in _clientContext.Web.Lists.Where(l => l.BaseTemplate == 101 && !l.Hidden).Select(documentLibraryItem => new ListViewItem
            {
                Text = documentLibraryItem.Title,
                Tag = documentLibraryItem.RootFolder.ServerRelativeUrl,
                ImageKey = "Folder"
            }))
            {
                SharePointDocumentLibraryPicker.Items.Add(listViewItem);
            }
        }

        private void SharePointDocumentLibraryPicker_DoubleClick(object sender, EventArgs e)
        {
            if (SharePointDocumentLibraryPicker.SelectedItems.Count == 0) return;

            if (OKButton.Enabled)
            {
                OKButton_Click(sender, e);
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if(SharePointDocumentLibraryPicker.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a document library to store the KeePass database in", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(string.IsNullOrWhiteSpace(FileName))
            {
                MessageBox.Show("Enter the filename under which you wish to store the database", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void SharePointDocumentLibraryPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
