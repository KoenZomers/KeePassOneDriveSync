using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KoenZomersKeePassOneDriveSync.Forms
{
    public partial class SharePointCredentialsForm : Form
    {
        private readonly bool _useMicrosoftGraph;

        #region Properties

        /// <summary>
        /// The entered SharePoint URL
        /// </summary>
        public string SharePointUrl {  get { return SharePointUrlTextBox.Text; } }

        /// <summary>
        /// The entered SharePoint Client Id
        /// </summary>
        public string SharePointClientId { get { return ClientIdTextBox.Text; } }

        /// <summary>
        /// The entered SharePoint Client Secret
        /// </summary>
        public string SharePointClientSecret { get { return ClientSecretTextBox.Text; } }

        /// <summary>
        /// Boolean indicating if the SharePointUrl field contains a value
        /// </summary>
        public bool AllFieldsContainText
        {
            get
            {
                return !string.IsNullOrWhiteSpace(SharePointUrlTextBox.Text) &&
                       (_useMicrosoftGraph || (!string.IsNullOrWhiteSpace(ClientIdTextBox.Text) && !string.IsNullOrWhiteSpace(ClientSecretTextBox.Text)));
            }
        }
        #endregion

        public SharePointCredentialsForm(bool useMicrosoftGraph = true)
        {
            _useMicrosoftGraph = useMicrosoftGraph;

            InitializeComponent();

            ClientIdTextBox.Visible = !useMicrosoftGraph;
            ClientSecretTextBox.Visible = !useMicrosoftGraph;
            ClientIdLabel.Visible = !useMicrosoftGraph;
            ClientSecretLabel.Visible = !useMicrosoftGraph;
            ExplanationLabel.Text = useMicrosoftGraph
                ? "Enter the URL of the SharePoint Online site collection you wish to store the KeePass database on."
                : "Enter the details of the SharePoint 2013, 2016, 2019 or Subscription Edition environment you wish to store the KeePass database on.";
        }

        /// <summary>
        /// Triggered when the user clicks the OK button
        /// </summary>
        private void OKButton_Click(object sender, EventArgs e)
        {
            if(!EnsureAllFieldsEntered())
            {
                DialogResult = DialogResult.None;
                return;
            }
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Validates if all fields contain a value and if not, will display a notification to the nd user
        /// </summary>
        /// <returns>True if all fields contain a value, False if this is not the case</returns>
        private bool EnsureAllFieldsEntered()
        {
            var allChecksPassed = AllFieldsContainText;

            if(!allChecksPassed)
            {
                MessageBox.Show(this, "All fields are required fields", "Please enter all fields", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return allChecksPassed;
        }

        /// <summary>
        /// Uses the information provided in the form to try to establish a connection with SharePoint
        /// </summary>
        private async void TestButton_Click(object sender, EventArgs e)
        {
            // Ensure all fields contain a value
            if(!EnsureAllFieldsEntered())
            {
                return;
            }

            // Ensure the entered URL is a valid URI
            Uri SharePointUri;
            if (!Uri.TryCreate(SharePointUrl, UriKind.Absolute, out SharePointUri))
            {
                MessageBox.Show(this, "SharePoint site URL field does not contain a valid URL", "Invalid data entered", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                SharePointUrlTextBox.SelectAll();
                SharePointUrlTextBox.Focus();
                return;
            }

            // Test the connection
            try
            {
                if (_useMicrosoftGraph)
                {
                    var testConfiguration = new KoenZomers.KeePass.OneDriveSync.Configuration
                    {
                        RemoteDatabasePath = SharePointUri.ToString()
                    };

                    using (var graphClient = await Providers.SharePointProvider.CreateSharePointGraphClient(testConfiguration))
                    {
                        if (graphClient != null && await Providers.SharePointProvider.TestConnection(graphClient, testConfiguration))
                        {
                            MessageBox.Show("Connection successful", "Testing SharePoint Connectivity", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Connection failed", "Testing SharePoint Connectivity", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    return;
                }

                using (var clientContext = Providers.SharePointProvider.CreateSharePointHttpClient(SharePointUri, SharePointClientId, SharePointClientSecret))
                {
                    MessageBox.Show(await Providers.SharePointProvider.TestConnection(clientContext) ? "Connection successful" : "Connection failed", "Testing SharePoint Connectivity", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Connection failed: '" + ex.Message + "'. Check the entered SharePoint details and your permissions to access it.", "Testing SharePoint Connectivity", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (_useMicrosoftGraph)
                {
                    SharePointUrlTextBox.SelectAll();
                    SharePointUrlTextBox.Focus();
                }
                else
                {
                    ClientIdTextBox.SelectAll();
                    ClientIdTextBox.Focus();
                }
                return;
            }
        }

        private void SharePointUrlTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Control)
            {
                var clipText = Clipboard.GetText();
                var clipTextSplitted = clipText.Split(new[] { '\r', '\n' });
                if (_useMicrosoftGraph && clipTextSplitted.Length > 0)
                {
                    SharePointUrlTextBox.Text = clipTextSplitted[0];
                    e.SuppressKeyPress = false;
                }
                else if (!_useMicrosoftGraph && clipTextSplitted.Length == 5)
                {
                    SharePointUrlTextBox.Text = clipTextSplitted[0];
                    ClientIdTextBox.Text = clipTextSplitted[2];
                    ClientSecretTextBox.Text = clipTextSplitted[4];
                    e.SuppressKeyPress = false;
                }
            }
            e.SuppressKeyPress = true;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void SharePointUrlTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void ClientIdTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            SharePointUrlTextBox_KeyUp(sender, e);
        }

        private void ClientSecretTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            SharePointUrlTextBox_KeyUp(sender, e);
        }
    }
}
