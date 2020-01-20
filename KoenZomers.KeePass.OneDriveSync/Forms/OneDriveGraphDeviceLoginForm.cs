using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using KoenZomers.OneDrive.Api;
using Newtonsoft.Json;

namespace KoenZomersKeePassOneDriveSync
{
    /// <summary>
    /// Performs authentication against the Microsoft Graph API using the Device Code Flow (https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-device-code)
    /// </summary>
    public partial class OneDriveGraphDeviceLoginForm : Form
    {
        /// <summary>
        /// URL to link the More information link to
        /// </summary>
        private const string _moreInformationUrl = "https://github.com/KoenZomers/KeePassOneDriveSync/blob/master/Faq.md#how-does-the-microsoft-graph-any-browser-option-work";

        /// <summary>
        /// URL to send a POST request to in order to retrieve a Device ID Code from Microsoft Graph
        /// </summary>
        private const string _microsoftGraphDeviceAuthorizationInitiationUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/devicecode";

        /// <summary>
        /// URL to send a POST request to in order to verify if the device session has been authenticated already
        /// </summary>
        private const string _microsoftGraphDeviceAuthorizationStatusUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/token";

        /// <summary>
        /// Counter to keep track of how many seconds there are left to authenticate the session
        /// </summary>
        private int _authenticatedCheckCountDownCount;

        /// <summary>
        /// Counter to keep track of how many checks to wait before actually contacting the Microsoft Graph API again to validate if the authentication has succeeded
        /// </summary>
        private int _authenticationCheckSkipCounter;

        /// <summary>
        /// Amount of validation checks to wait before actually contacting the Microsoft Graph API again to validate if the authentication has succeeded
        /// </summary>
        private int _authenticationCheckSkipValue;

        /// <summary>
        /// HTTP Client to use to communicate with the Microsoft Graph
        /// </summary>
        private HttpClient _httpClient;

        /// <summary>
        /// OneDrive Graph API instance
        /// </summary>
        public OneDriveGraphApi OneDriveApi { get; private set; }

        /// <summary>
        /// Returns the Refresh Token obtained through the Device ID login
        /// </summary>
        public string RefreshToken { get; private set; }

        public OneDriveGraphDeviceLoginForm(OneDriveApi oneDriveApi)
        {
            InitializeComponent();

            // Cast the OneDrive instance to a Graph API instance so it can be used in this form
            OneDriveApi = (OneDriveGraphApi)oneDriveApi;

            // When the form is being closed, ensure the Timer processes are being stopped to avoid exceptions
            FormClosing += OneDriveGraphDeviceLoginForm_FormClosing;

            // Set the defaults of the components on the form
            StatusLabel.Text = "Communicating with the Microsoft Graph API...";
            DeviceAuthLinkLabel.Visible = false;
            DeviceIdTextBox.Visible = false;
            OpenBrowserButton.Enabled = false;
            CopyDeviceIdButton.Enabled = false;
            AuthenticationCheckTimer.Enabled = false;
            AuthenticationCompleteTimer.Enabled = false;

            // Set the Microsoft logo on the form
            MicrosoftLogoPictureBox.BackgroundImage = Resources.MicrosoftLogo;

            // Set up a HttpClient obeying the possible proxy settings to communicate with the Microsoft Graph API
            _httpClient = CreateHttpClient();

            // Enable a timer to initiate the communication with Microsoft Graph so the UI thread doesn't block
            StartSessionTimer.Enabled = true;
        }

        /// <summary>
        /// When the form is closed, ensure Timer resources are being stopped
        /// </summary>
        private void OneDriveGraphDeviceLoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(AuthenticationCheckTimer != null && AuthenticationCheckTimer.Enabled)
            {
                AuthenticationCheckTimer.Enabled = false;
            }
            if(StartSessionTimer != null && StartSessionTimer.Enabled)
            {
                StartSessionTimer.Enabled = false;
            }
            if (AuthenticationCompleteTimer != null && AuthenticationCompleteTimer.Enabled)
            {
                AuthenticationCompleteTimer.Enabled = false;
            }
        }

        /// <summary>
        /// Makes a request to the Microsoft Graph API to set up a new device code flow session and shows the results on the form
        /// </summary>
        private void GetMicrosoftGraphDeviceId()
        {
            // Construct the mandatory form post parameters to send
            var postFormValues = new List<KeyValuePair<string, string>>();
            postFormValues.Add(new KeyValuePair<string, string>("client_id", KoenZomersKeePassOneDriveSyncExt.GraphApiApplicationId));
            postFormValues.Add(new KeyValuePair<string, string>("scope", OneDriveApi.DefaultScopes.Aggregate((x, y) => string.Format("{0} {1}", x, y))));

            StatusLabel.Text = "Requesting Device ID code...";

            // Construct the message towards the Microsoft Graph API
            using (var request = new HttpRequestMessage(HttpMethod.Post, _microsoftGraphDeviceAuthorizationInitiationUrl))
            {
                request.Content = new FormUrlEncodedContent(postFormValues);

                // Request the response from the webservice
                HttpResponseMessage response;
                try
                {
                    response = _httpClient.SendAsync(request).Result;
                }
                catch(Exception e)
                {
                    while (e.InnerException != null) e = e.InnerException;
                    StatusLabel.Text = string.Format("Failed to connect to the Graph API ({0})", e.Message);
                    return;
                }

                Providers.MicrosoftGraph.MicrosoftGraphDeviceCodeResponse microsoftGraphDeviceCodeResponse;
                try
                {                    
                    microsoftGraphDeviceCodeResponse = JsonConvert.DeserializeObject<Providers.MicrosoftGraph.MicrosoftGraphDeviceCodeResponse>(response.Content.ReadAsStringAsync().Result);
                }
                catch(Exception e)
                {
                    while (e.InnerException != null) e = e.InnerException;
                    StatusLabel.Text = string.Format("Failed to parse the Graph API response ({0})", e.Message);
                    return;
                }

                // Ensure the required information has been received
                if (microsoftGraphDeviceCodeResponse != null && !string.IsNullOrEmpty(microsoftGraphDeviceCodeResponse.DeviceCode) && !string.IsNullOrEmpty(microsoftGraphDeviceCodeResponse.UserCode) && microsoftGraphDeviceCodeResponse.VerificationUri != null)
                {
                    DeviceIdTextBox.Text = microsoftGraphDeviceCodeResponse.UserCode;
                    DeviceIdTextBox.Tag = microsoftGraphDeviceCodeResponse.DeviceCode;
                    DeviceIdTextBox.Visible = true;

                    DeviceAuthLinkLabel.Text = microsoftGraphDeviceCodeResponse.VerificationUri.AbsoluteUri;
                    DeviceAuthLinkLabel.Visible = true;

                    CopyDeviceIdButton.Enabled = true;
                    OpenBrowserButton.Enabled = true;
                    OpenBrowserButton.Focus();

                    _authenticatedCheckCountDownCount = microsoftGraphDeviceCodeResponse.ExpiresIn.GetValueOrDefault(180);

                    _authenticationCheckSkipValue = microsoftGraphDeviceCodeResponse.Interval.GetValueOrDefault(2);

                    AuthenticationCheckTimer.Enabled = true;
                }
                else
                {
                    StatusLabel.Text = "Invalid response received from Microsoft Graph";
                }
            }
        }

        /// <summary>
        /// Tries to parse the provided JSON string into the provided type T
        /// </summary>
        /// <typeparam name="T">Type to try to parse the JSON structure in</typeparam>
        /// <param name="json">JSON as a string to parse</param>
        /// <param name="result">Object of type T containing the parsed JSON</param>
        /// <returns>True if the parsing succeeded, false if it failed</returns>
        public static bool TryParseJson<T>(string json, out T result)
        {
            bool success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            result = JsonConvert.DeserializeObject<T>(json, settings);
            return success;
        }

        /// <summary>
        /// Makes a request to the Microsoft Graph API to validate if the device code session has authenticated already
        /// </summary>
        /// <returns>NULL if the authentication did not take place yet or a refresh token if it has</returns>
        private string HasUserAuthenticated()
        {
            // Construct the mandatory form post parameters to send
            var postFormValues = new List<KeyValuePair<string, string>>();
            postFormValues.Add(new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:device_code"));
            postFormValues.Add(new KeyValuePair<string, string>("device_code", DeviceIdTextBox.Tag.ToString()));
            postFormValues.Add(new KeyValuePair<string, string>("client_id", KoenZomersKeePassOneDriveSyncExt.GraphApiApplicationId));

            // Construct the message towards the Microsoft Graph API
            using (var request = new HttpRequestMessage(HttpMethod.Post, _microsoftGraphDeviceAuthorizationStatusUrl))
            {
                request.Content = new FormUrlEncodedContent(postFormValues);

                // Request the response from the Microsoft Graph API
                using (var response = _httpClient.SendAsync(request).Result)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;

                    // Try and see if the result can be parsed as an error result typically indicating that the authentication did not take place yet
                    Providers.MicrosoftGraph.MicrosoftGraphDeviceCodeAuthCheckFailedResponse failedResult;
                    if (TryParseJson(responseString, out failedResult))
                    {
                        switch (failedResult.Error)
                        {
                            case "authorization_pending":
                                // all good, just wait for it to be done
                                break;

                            case "authorization_declined":
                                AuthenticationCheckTimer.Enabled = false;
                                StatusLabel.Text = "Authorization declined";
                                break;

                            case "expired_token":
                                AuthenticationCheckTimer.Enabled = false;
                                StatusLabel.Text = "Authentication timed out";
                                break;

                            case "bad_verification_code":
                                AuthenticationCheckTimer.Enabled = false;
                                StatusLabel.Text = "Device code not recognized";
                                break;

                            default:
                                AuthenticationCheckTimer.Enabled = false;
                                StatusLabel.Text = string.Format("Unexpected error: {0}", failedResult.ErrorDescription);
                                break;
                        }
                        return null;
                    }

                    // Try and see if the result can be parsed as a successful result indicating that the authentication has taken place
                    Providers.MicrosoftGraph.MicrosoftGraphDeviceCodeAuthCheckSucceededResponse succeededResult;
                    if (TryParseJson(responseString, out succeededResult))
                    {
                        return succeededResult.RefreshToken;
                    }

                    // Unknown response received
                    AuthenticationCheckTimer.Enabled = false;
                    StatusLabel.Text = "Unknown response received";
                    return null;
                }
            }
        }

        /// <summary>
        /// Instantiates a new HttpClient preconfigured for use. Note that the caller is responsible for disposing this object.
        /// </summary>
        /// <returns>HttpClient instance</returns>
        private HttpClient CreateHttpClient()
        {
            var proxySettings = Utilities.GetProxySettings();
            var proxyCredentials = Utilities.GetProxyCredentials();

            // Define the HttpClient settings
            var httpClientHandler = new HttpClientHandler
            {
                UseDefaultCredentials = proxyCredentials == null,
                UseProxy = proxySettings != null,
                Proxy = proxySettings
            };

            // Check if we need specific credentials for the proxy
            if (proxyCredentials != null && httpClientHandler.Proxy != null)
            {
                httpClientHandler.Proxy.Credentials = proxyCredentials;
            }

            // Create the new HTTP Client
            var httpClient = new HttpClient(httpClientHandler);
            return httpClient;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            AuthenticationCheckTimer.Enabled = false;

            Close();
        }

        /// <summary>
        /// Triggers when the communication with the Microsoft Graph should start
        /// </summary>
        private void StartSessionTimer_Tick(object sender, EventArgs e)
        {
            // Only perform this action once
            StartSessionTimer.Enabled = false;

            // Initiate the retrieval of a Device ID
            GetMicrosoftGraphDeviceId();
        }

        /// <summary>
        /// Triggers to validate if the device id session has been authenticated
        /// </summary>
        private void AuthenticationCheckTimer_Tick(object sender, EventArgs e)
        {
            _authenticatedCheckCountDownCount--;

            if(_authenticatedCheckCountDownCount == 0)
            {
                StatusLabel.Text = "Session authentication has timed out";
                AuthenticationCheckTimer.Enabled = false;
                return;
            }

            var timeRemaining = TimeSpan.FromSeconds(_authenticatedCheckCountDownCount);
            StatusLabel.Text = string.Format("Waiting for authentication to complete ({0:D2}:{1:D2})", timeRemaining.Minutes, timeRemaining.Seconds);

            // Don't connect to the Microsoft Graph API every second as otherwise it will throw an error. Wait for the amount of times as instructed by the Microsoft Graph API when setting up the device login session.
            _authenticationCheckSkipCounter++;
            if(_authenticationCheckSkipCounter < _authenticationCheckSkipValue)
            {
                return;
            }
            _authenticationCheckSkipCounter = 0;

            var result = HasUserAuthenticated();
            if (result != null)
            {
                AuthenticationCheckTimer.Enabled = false;
                RefreshToken = result;
                DialogResult = DialogResult.OK;
                StatusLabel.Text = "Authentication successful";
                AuthenticationCompleteTimer.Enabled = true;
            }
        }

        /// <summary>
        /// Copies the Device ID to the Windows Clipboard
        /// </summary>
        private void CopyDeviceIdButton_Click(object sender, EventArgs e)
        {
            if (!DeviceIdTextBox.Enabled) return;

            Clipboard.SetText(DeviceIdTextBox.Text);
        }

        private void OpenBrowserButton_Click(object sender, EventArgs e)
        {
            if (!OpenBrowserButton.Enabled) return;

            System.Diagnostics.Process.Start(DeviceAuthLinkLabel.Text);
        }

        private void DeviceAuthLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!DeviceAuthLinkLabel.Visible) return;

            System.Diagnostics.Process.Start(DeviceAuthLinkLabel.Text);
        }

        private void OneDriveGraphDeviceLoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
            }
        }

        private void AuthenticationCompleteTimer_Tick(object sender, EventArgs e)
        {
            AuthenticationCompleteTimer.Enabled = false;
            Close();
        }

        private void MoreInformationLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(_moreInformationUrl);
        }
    }
}
