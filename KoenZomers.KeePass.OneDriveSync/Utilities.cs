using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using KoenZomers.KeePass.OneDriveSync;
using KoenZomers.OneDrive.Api;

namespace KoenZomersKeePassOneDriveSync
{
    /// <summary>
    /// Utilities for the KeePass plugin
    /// </summary>
    public static class Utilities
    {
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

        /// <summary>
        /// Returns an active OneDriveApi instance. If a RefreshToken is available, it will set up an instance based on that, otherwise it will show the login dialog
        /// </summary>
        /// <param name="databaseConfig">Configuration of the KeePass database</param>
        /// <param name="oneDriveClientId">ClientID to get access to OneDrive</param>
        /// <param name="oneDriveClientSecret">ClientSecret to get access to OneDrive</param>
        /// <returns>Active OneDrive instance or NULL if unable to get an authenticated instance</returns>
        public static async Task<OneDriveApi> GetOneDriveApi(Configuration databaseConfig, string oneDriveClientId, string oneDriveClientSecret)
        {
            if (string.IsNullOrEmpty(databaseConfig.RefreshToken))
            {                
                var oneDriveAuthenticateForm = new OneDriveAuthenticateForm(oneDriveClientId, oneDriveClientSecret);
                var result = oneDriveAuthenticateForm.ShowDialog();

                if (result != DialogResult.OK)
                {
                    return null;
                }                

                // Save the configuration so we keep the Refresh Token
                var oneDriveApi = oneDriveAuthenticateForm.OneDriveApi;
                databaseConfig.RefreshToken = oneDriveApi.AccessToken.RefreshToken;

                Configuration.Save();
                
                return oneDriveApi;
            }

            return await OneDriveApi.GetOneDriveApiFromRefreshToken(oneDriveClientId, oneDriveClientSecret, databaseConfig.RefreshToken);
        }
    }
}
