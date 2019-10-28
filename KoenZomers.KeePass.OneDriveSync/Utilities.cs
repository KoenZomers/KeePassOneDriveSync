using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CredentialManagement;
using KeePassLib;
using KoenZomers.KeePass.OneDriveSync;
using KoenZomers.KeePass.OneDriveSync.Enums;
using KoenZomers.OneDrive.Api;

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
        /// Returns an active OneDriveApi instance. If a RefreshToken is available, it will set up an instance based on that, otherwise it will show the login dialog
        /// </summary>
        /// <param name="databaseConfig">Configuration of the KeePass database</param>
        /// <returns>Active OneDrive instance or NULL if unable to get an authenticated instance</returns>
        public static async Task<OneDriveApi> GetOneDriveApi(Configuration databaseConfig)
        {
            OneDriveApi cloudStorage;

            switch (databaseConfig.CloudStorageType.GetValueOrDefault(CloudStorageType.OneDriveConsumer))
            {
                case CloudStorageType.OneDriveConsumer:
                    cloudStorage = new OneDriveConsumerApi(KoenZomersKeePassOneDriveSyncExt.OneDriveConsumerClientId, KoenZomersKeePassOneDriveSyncExt.OneDriveConsumerClientSecret);
                    break;

                case CloudStorageType.OneDriveForBusiness:
                    cloudStorage = new OneDriveForBusinessO365Api(KoenZomersKeePassOneDriveSyncExt.OneDriveForBusinessClientId, KoenZomersKeePassOneDriveSyncExt.OneDriveForBusinessClientSecret);
                    break;

                case CloudStorageType.MicrosoftGraph:
                case CloudStorageType.MicrosoftGraphDeviceLogin:
                    cloudStorage = new OneDriveGraphApi(KoenZomersKeePassOneDriveSyncExt.GraphApiApplicationId);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(string.Format("Cloud storage type {0} is not supported", databaseConfig.CloudStorageType));
            }

            // Ensure we have a refresh token
            if (string.IsNullOrEmpty(databaseConfig.RefreshToken))
            {
                // No refresh token available, perform the login process
                if (databaseConfig.CloudStorageType.GetValueOrDefault(CloudStorageType.OneDriveConsumer) == CloudStorageType.MicrosoftGraphDeviceLogin)
                {
                    // Using Microsoft Graph Device ID Code Flow
                    var oneDriveGraphDeviceLoginForm = new OneDriveGraphDeviceLoginForm(cloudStorage);
                    var deviceLoginResult = oneDriveGraphDeviceLoginForm.ShowDialog();

                    if (deviceLoginResult != System.Windows.Forms.DialogResult.OK)
                    {
                        return null;
                    }

                    // Save the configuration so we keep the Refresh Token
                    databaseConfig.RefreshToken = oneDriveGraphDeviceLoginForm.RefreshToken;
                    Configuration.Save();
                }
                else
                {
                    // Using any of the other authentication options
                    var oneDriveAuthenticateForm = new OneDriveAuthenticateForm(cloudStorage);
                    var result = oneDriveAuthenticateForm.ShowDialog();

                    if (result != System.Windows.Forms.DialogResult.OK)
                    {
                        return null;
                    }

                    // Check if we already know where to store the Refresh Token for this database
                    if (!databaseConfig.RefreshTokenStorage.HasValue)
                    {
                        // We don't know yet where the Refresh Token for this database should be stored, ask the user to choose
                        var oneDriveRefreshTokenStorageForm = new OneDriveRefreshTokenStorageDialog(databaseConfig);
                        oneDriveRefreshTokenStorageForm.ShowDialog();
                    }

                    // Save the configuration so we keep the Refresh Token
                    var oneDriveApi = oneDriveAuthenticateForm.OneDriveApi;
                    databaseConfig.RefreshToken = oneDriveApi.AccessToken.RefreshToken;

                    Configuration.Save();

                    return oneDriveApi;
                }
            }

            try
            {
                ApplyProxySettings(cloudStorage);
                await cloudStorage.AuthenticateUsingRefreshToken(databaseConfig.RefreshToken);
                return cloudStorage;
            }
            catch (KoenZomers.OneDrive.Api.Exceptions.TokenRetrievalFailedException e)
            {
                // Something went wrong with retrieving the access token
                throw;
            }
            catch (Exception e)
            {
                // If specifically the HttpRequestException occurred, it likely contains more information on what went wrong, so pass it up to the callstack
                if (e.GetType().ToString() == "System.Net.Http.HttpRequestException")
                {
                    throw;
                }
                // Occurs if no connection can be made with the OneDrive service. It will be handled properly in the calling code.
                return null;
            }
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
        /// Applies the correct web proxy settings to the provided OneDriveApi instance based on KeePass proxy configuration
        /// </summary>
        /// <param name="oneDriveApi">OneDriveApi instance to apply the proper proxy settings to</param>
        public static void ApplyProxySettings(OneDriveApi oneDriveApi)
        {
            // Set the WebProxy to use
            oneDriveApi.ProxyConfiguration = GetProxySettings();

            // Configure the credentials to use for the proxy
            oneDriveApi.ProxyCredential = GetProxyCredentials();
        }

        #endregion

        #region Windows Credential Manager

        /// <summary>
        /// Saves the provided OneDrive Refresh Token in the Windows Credential Manager
        /// </summary>
        /// <param name="databaseFilePath">Full local path to the KeePass database for which to save the OneDrive Refresh Token</param>
        /// <param name="refreshToken">The OneDrive Refresh Token to store securely in the Windows Credential Manager</param>
        public static void SaveRefreshTokenInWindowsCredentialManager(string databaseFilePath, string refreshToken)
        {
            // Some refresh tokens can be longer than the maximum permitted 512 characters for a password field. If this is the case, put the first 512 characters of the refresh token in the password field and the rest in the comments field. Also the comments field has a limit. So far I have not run into this limit yet.
            string passwordPart1;
            string passwordPart2;

            if (refreshToken != null && refreshToken.Length > 512)
            {
                passwordPart1 = refreshToken.Remove(512);
                passwordPart2 = refreshToken.Remove(0, 512);
            }
            else
            {
                passwordPart1 = refreshToken;
                passwordPart2 = string.Empty;
            }

            using (var saved = new Credential(databaseFilePath, passwordPart1, string.Concat("KoenZomers.KeePass.OneDriveSync:", databaseFilePath), CredentialType.Generic) {PersistanceType = PersistanceType.LocalComputer})
            {
                saved.Description = passwordPart2;
                saved.Save();
            }
        }

        /// <summary>
        /// Retrieves a OneDrive Refresh Token from the Windows Credential Manager
        /// </summary>
        /// <param name="databaseFilePath">Full local path to the KeePass database for which to retrieve the OneDrive Refresh Token</param>
        /// <returns>OneDrive Refresh Token if available or NULL if no Refresh Token found for the provided database</returns>
        public static string GetRefreshTokenFromWindowsCredentialManager(string databaseFilePath)
        {
            using (var credential = new Credential {Target = string.Concat("KoenZomers.KeePass.OneDriveSync:", databaseFilePath), Type = CredentialType.Generic})
            {
                credential.Load();

                // Concatenate the contents of the password and comments fields to retrieve the refresh token
                return credential.Exists() ? credential.Password + (!string.IsNullOrEmpty(credential.Description) ? credential.Description : string.Empty) + (!string.IsNullOrEmpty(credential.Target) ? credential.Target : string.Empty) : null;
            }
        }

        /// <summary>
        /// Deletes a OneDrive Refresh Token from the Windows Credential Manager
        /// </summary>
        /// <param name="databaseFilePath">Full local path to the KeePass database for which to delete the OneDrive Refresh Token</param>
        public static void DeleteRefreshTokenFromWindowsCredentialManager(string databaseFilePath)
        {
            using (var credential = new Credential {Target = string.Concat("KoenZomers.KeePass.OneDriveSync:", databaseFilePath), Type = CredentialType.Generic})
            {
                // Verify if we have stored a token for this database
                if (credential.Exists())
                {
                    // Delete the Windows Credential Manager entry
                    credential.Delete();
                }
            }
        }

        #endregion

        #region KeePass Database Storage

        /// <summary>
        /// Saves the provided OneDrive Refresh Token in the provided KeePass database
        /// </summary>
        /// <param name="keePassDatabase">KeePass database instance to store the Refresh Token in</param>
        /// <param name="refreshToken">The OneDrive Refresh Token to store securely in the KeePass database</param>
        public static void SaveRefreshTokenInKeePassDatabase(PwDatabase keePassDatabase, string refreshToken)
        {
            keePassDatabase.CustomData.Set("KoenZomers.KeePass.OneDriveSync.RefreshToken", refreshToken);
        }

        /// <summary>
        /// Retrieves a OneDrive Refresh Token from the provided KeePass database
        /// </summary>
        /// <param name="keePassDatabase">KeePass database instance to get the Refresh Token from</param>
        /// <returns>OneDrive Refresh Token if available or NULL if no Refresh Token found for the provided database</returns>
        public static string GetRefreshTokenFromKeePassDatabase(PwDatabase keePassDatabase)
        {
            var refreshToken = keePassDatabase.CustomData.Get("KoenZomers.KeePass.OneDriveSync.RefreshToken");
            return refreshToken;
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
            // Decode Base64-encoded encrypted data
            var rawEncryptedToken = Convert.FromBase64String(encryptedRefreshToken);

            try
            {
                // Decrypt using DPAPI with user scope, only possible if the currently logged-in user is the same as the one who encrypted it
                var rawToken = ProtectedData.Unprotect(rawEncryptedToken, null, DataProtectionScope.CurrentUser);

                // Get string data from byte array
                var token = Encoding.ASCII.GetString(rawToken);

                return token;
            }
            catch (Exception)
            {
                // If decryption fails, lose the token
                return null;
            }
        }

        #endregion
    }
}
