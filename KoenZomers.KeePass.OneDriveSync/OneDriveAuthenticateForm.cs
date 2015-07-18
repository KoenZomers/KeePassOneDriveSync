using System.Windows.Forms;
using KoenZomers.OneDrive.Api;

namespace KoenZomersKeePassOneDriveSync
{
    public partial class OneDriveAuthenticateForm : Form
    {
        /// <summary>
        /// OneDrive API instance
        /// </summary>
        public OneDriveApi OneDriveApi { get; private set; }

        public OneDriveAuthenticateForm(string oneDriveClientId, string oneDriveClientSecret)
        {
            InitializeComponent();

            OneDriveApi = new OneDriveApi(oneDriveClientId, oneDriveClientSecret);

            SignOut();
        }

        /// <summary>
        /// Sign the current user out of OneDrive
        /// </summary>
        public void SignOut()
        {
            // First sign the current user out to make sure he/she needs to authenticate again
            var signoutUri = OneDriveApi.GetSignOutUri();
            WebBrowser.Navigate(signoutUri);
        }

        private async void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            // Check if the current URL contains the authorization token
            var authorizationCode = OneDriveApi.GetAuthorizationTokenFromUrl(e.Url.ToString());

            // Verify if an authorization token was successfully extracted
            if (!string.IsNullOrEmpty(authorizationCode))
            {
                // Get an access token based on the authorization token that we now have
                await OneDriveApi.GetAccessToken();
                if (OneDriveApi.AccessToken != null)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }
            }

            // If we're on this page, but we didn't get an authorization token, it means that we just signed out, proceed with signing in again
            if (e.Url.ToString().StartsWith("https://login.live.com/oauth20_desktop.srf"))
            {
                var authenticateUri = OneDriveApi.GetAuthenticationUri("wl.offline_access wl.skydrive_update");
                WebBrowser.Navigate(authenticateUri);
            }
        }
    }
}
