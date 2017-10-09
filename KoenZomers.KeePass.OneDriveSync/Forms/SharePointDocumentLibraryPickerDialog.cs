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
        /// Returns the Id of the selected SharePoint Document Library
        /// </summary>
        public string SelectedDocumentLibraryId { get { return SharePointDocumentLibraryPicker.SelectedItems.Count == 0 ? null : SharePointDocumentLibraryPicker.SelectedItems[0].Tag.ToString(); } }

        #endregion

        /// <summary>
        /// Instance of the SharePoint ClientContext that can be used to communicate with SharePoint
        /// </summary>
        private readonly ClientContext _clientContext;

        public SharePointDocumentLibraryPickerDialog(ClientContext clientContext)
        {
            InitializeComponent();

            _clientContext = clientContext;
        }

        public void LoadDocumentLibraryItems()
        {
            SharePointDocumentLibraryPicker.Items.Clear();

            _clientContext.Load(_clientContext.Web.Lists, l => l.Include(li => li.Id, li => li.Title, li => li.BaseTemplate));
            _clientContext.ExecuteQuery();

            foreach (var listViewItem in _clientContext.Web.Lists.Where(l => l.BaseTemplate == 101).Select(documentLibraryItem => new ListViewItem
            {
                Text = documentLibraryItem.Title,
                Tag = documentLibraryItem.Id,
                ImageKey = "Folder"
            }))
            {
                SharePointDocumentLibraryPicker.Items.Add(listViewItem);
            }
        }

        private void SharePointDocumentLibraryPicker_DoubleClick(object sender, EventArgs e)
        {
            if (SharePointDocumentLibraryPicker.SelectedItems.Count == 0) return;

            var selectedItem = SharePointDocumentLibraryPicker.SelectedItems[0];
            if (OKButton.Enabled)
            {
                OKButton_Click(sender, e);
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void SharePointDocumentLibraryPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            OKButton.Enabled = SharePointDocumentLibraryPicker.SelectedItems.Count > 0;
        }
    }
}
