using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using KeePassLib;
using KoenZomers.KeePass.OneDriveSync;
using KoenZomers.KeePass.OneDriveSync.Enums;
using KoenZomers.OneDrive.Api;
using Microsoft.Identity.Client;

namespace KoenZomersKeePassOneDriveSync
{
    /// <summary>
    /// Utilities for the KeePass plugin
    /// </summary>
    public static class Utilities
    {
        #region File Hashes

        /// <summary>
        /// Calculates the SHA1 file hash of the provided KeePass database
        /// </summary>
        /// <param name="localKeePassDatabasePath">Full path to where the KeePass database resides locally</param>
        /// <returns>SHA1 file hash</returns>
        public static string GetDatabaseFileHash(string localKeePassDatabasePath)
        {
            using (var fileStream = new FileStream(localKeePassDatabasePath, FileMode.Open))
            {
                using (var cryptoProvider = new SHA1CryptoServiceProvider())
                {
                    var hash = BitConverter.ToString(cryptoProvider.ComputeHash(fileStream)).Replace("-", string.Empty);
                    return hash;
                }
            }
        }

        #endregion

        #region OneDrive

        /// <summary>
        /// Returns an active OneDriveGraphApi instance. If a cached MSAL token is available, it will set up an instance based on that, otherwise it will show the login dialog
        /// </summary>
        /// <param name="databaseConfig">Configuration of the KeePass database</param>
        /// <returns>Active OneDrive instance or NULL if unable to get an authenticated instance</returns>
        public static async Task<OneDriveGraphApi> GetOneDriveApi(Configuration databaseConfig)
        {
            // All OneDrive traffic (consumer and business) goes through Microsoft Graph, authenticating interactively via
            // MSAL's system browser flow (opens the default OS browser and listens for the redirect on http://localhost).
            // Legacy CloudStorageType values (OneDriveConsumer, MicrosoftGraph "built-in browser") are no longer
            // selectable from the UI, but existing configurations using them will simply be treated the same way.
            var cloudStorage = new OneDriveGraphApi(KoenZomersKeePassOneDriveSyncExt.GraphApiApplicationId);
            ApplyProxySettings(cloudStorage);

            // Try to restore a previously cached MSAL token cache so the user doesn't need to log in again
            if (!string.IsNullOrEmpty(databaseConfig.RefreshToken) && cloudStorage.PublicClientApplication != null)
            {
                try
                {
                    var cacheBytes = Convert.FromBase64String(databaseConfig.RefreshToken);
                    ((ITokenCacheSerializer)cloudStorage.PublicClientApplication.UserTokenCache).DeserializeMsalV3(cacheBytes);

                    var accounts = await cloudStorage.PublicClientApplication.GetAccountsAsync();
                    var account = accounts.FirstOrDefault();

                    if (account != null)
                    {
                        var silentResult = await cloudStorage.PublicClientApplication.AcquireTokenSilent(cloudStorage.GetDefaultScopes(), account).ExecuteAsync();
                        cloudStorage.SetAuthenticationResult(silentResult);

                        return cloudStorage;
                    }
                }
                catch (MsalUiRequiredException)
                {
                    // The cached token can no longer be used silently (e.g. expired or revoked), fall through to interactive login
                }
                catch (Exception)
                {
                    // The cached token is invalid, corrupt or from an incompatible version, fall through to interactive login
                }
            }

            // No usable cached token available, perform an interactive login by opening the system's default browser
            // and listening for the redirect on http://localhost (MSAL's system browser interactive flow)
            try
            {
                var interactiveResult = await cloudStorage.PublicClientApplication.AcquireTokenInteractive(cloudStorage.GetDefaultScopes())
                    .WithUseEmbeddedWebView(false)
                    .WithSystemWebViewOptions(new SystemWebViewOptions
                    {
                        HtmlMessageSuccess = BuildAuthResultHtmlPage(success: true),
                        HtmlMessageError = BuildAuthResultHtmlPage(success: false)
                    })
                    .ExecuteAsync();

                cloudStorage.SetAuthenticationResult(interactiveResult);
            }
            catch (MsalException)
            {
                // The user cancelled the sign-in or it failed for another reason
                return null;
            }

            // Persist the serialized MSAL token cache (DPAPI-encrypted on disk by Configuration.Save) so it can be silently reused next time
            databaseConfig.RefreshToken = Convert.ToBase64String(((ITokenCacheSerializer)cloudStorage.PublicClientApplication.UserTokenCache).SerializeMsalV3());
            Configuration.Save();

            return cloudStorage;
        }

        /// <summary>
        /// Builds the HTML page shown in the system browser tab after MSAL's interactive sign-in flow completes on the
        /// http://localhost loopback listener, replacing MSAL's plain default page with a simple styled message.
        /// </summary>
        /// <param name="success">True to render the success variant, false to render the error/failure variant</param>
        /// <returns>Self-contained HTML document (inline styles, no external resources) for the given result</returns>
        private static string BuildAuthResultHtmlPage(bool success)
        {
            var accentColor = success ? "#107C10" : "#D13438";
            var title = success ? "You're signed in" : "Sign-in failed";
            var message = success
                ? "You have successfully authenticated. You can close this browser tab and return to KeePass."
                : "Something went wrong while authenticating. You can close this browser tab and return to KeePass to try again.";

            return string.Format(@"<html><head><title>{0}</title></head>
<body style=""font-family: Segoe UI, Arial, sans-serif; text-align: center; margin-top: 15%;"">
<h1 style=""color: {1};"">{0}</h1>
<p>{2}</p>
</body></html>", title, accentColor, message);
        }

        #endregion

        #region Proxy Support

        /// <summary>
        /// Gets correct web proxy settings based on KeePass proxy configuration
        /// </summary>
        /// <returns>IWebProxy conforming instance with the proper proxy configuraton</returns>
        public static IWebProxy GetProxySettings()
        {
            IWebProxy webProxy = null;

            // Create the WebProxy object based on KeePass configuration
            switch (KeePass.Program.Config.Integration.ProxyType)
            {
                case ProxyServerType.None:
                    webProxy = null;
                    break;

                case ProxyServerType.Manual:
                    webProxy = new WebProxy(string.Concat(KeePass.Program.Config.Integration.ProxyAddress, ":", KeePass.Program.Config.Integration.ProxyPort));
                    break;

                case ProxyServerType.System:
                    webProxy = WebRequest.DefaultWebProxy;
                    break;
            }

            return webProxy;
        }

        /// <summary>
        /// Gets correct web proxy credentials based on KeePass proxy configuration
        /// </summary>
        /// <returns>NetworkCredential instance with the proper proxy credentials</returns>
        public static NetworkCredential GetProxyCredentials()
        {
            var networkCredential = KeePass.Program.Config.Integration.ProxyAuthType == ProxyAuthType.Manual ? new NetworkCredential(KeePass.Program.Config.Integration.ProxyUserName, KeePass.Program.Config.Integration.ProxyPassword) : null;
            return networkCredential;
        }

        /// <summary>
        /// Applies the correct web proxy settings to the provided OneDriveGraphApi instance based on KeePass proxy configuration
        /// </summary>
        /// <param name="oneDriveApi">OneDriveGraphApi instance to apply the proper proxy settings to</param>
        public static void ApplyProxySettings(OneDriveGraphApi oneDriveApi)
        {
            // Set the WebProxy to use
            oneDriveApi.ProxyConfiguration = GetProxySettings();

            // Configure the credentials to use for the proxy
            oneDriveApi.ProxyCredential = GetProxyCredentials();
        }

        #endregion

        #region DPAPI

        /// <summary>
        /// Encrypts a Refresh Token using the DPAPI Protect method with current user scope
        /// </summary>
        /// <param name="refreshToken">The Refresh Token to encrypt</param>
        /// <returns>The Base64-encoded encrypted refresh token or NULL if encryption fails</returns>
        public static string Protect(string refreshToken)
        {
            // Token should be plain ASCII text, get raw byte data for encoding
            var rawToken = Encoding.ASCII.GetBytes(refreshToken);

            try
            {
                // Encrypt using DPAPI with user scope, can only be decrypted by currently logged-in user
                var rawEncryptedToken = ProtectedData.Protect(rawToken, null, DataProtectionScope.CurrentUser);

                // Base64-encode encrypted token for JSON string compatibility
                var encryptedToken = Convert.ToBase64String(rawEncryptedToken);

                return encryptedToken;
            }
            catch (Exception)
            {
                // If encryption fails, lose the token
                return null;
            }
        }

        /// <summary>
        /// Decrypt a Refresh Token using the DPAPI Protect method with current user scope
        /// </summary>
        /// <param name="encryptedRefreshToken">The encrypted Refresh Token to decrypt, Base64-encoded</param>
        /// <returns>The decrypted refresh token or NULL if decryption fails</returns>
        public static string Unprotect(string encryptedRefreshToken)
        {
            try
            {
                // Decode Base64-encoded encrypted data
                var rawEncryptedToken = Convert.FromBase64String(encryptedRefreshToken);

                // Decrypt using DPAPI with user scope, only possible if the currently logged-in user is the same as the one who encrypted it
                var rawToken = ProtectedData.Unprotect(rawEncryptedToken, null, DataProtectionScope.CurrentUser);

                // Get string data from byte array
                var token = Encoding.ASCII.GetString(rawToken);

                return token;
            }
            catch (Exception)
            {
                // If decryption fails (e.g. the value is corrupt, was never encrypted to begin with, or was encrypted
                // by a different user/machine), lose the token so the user will simply be prompted to log in again
                return null;
            }
        }

        #endregion
    }
}
