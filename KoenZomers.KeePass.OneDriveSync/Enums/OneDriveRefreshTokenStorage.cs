namespace KoenZomers.KeePass.OneDriveSync.Enums
{
    /// <summary>
    /// Defines the possible storage locations for the OneDrive Refresh Token
    /// </summary>
    public enum OneDriveRefreshTokenStorage : short
    {
        /// <summary>
        /// Saves the RefreshToken plain text in KeePass.config.xml located in %APPDATA%\KeePass 
        /// </summary>
        Disk = 0,

        /// <summary>
        /// Saves the RefreshToken encrypted in the Windows Credential Manager
        /// </summary>
        WindowsCredentialManager = 1,

        /// <summary>
        /// Saves the RefreshToken in the encrypted KeePass database
        /// </summary>
        KeePassDatabase = 2
    }
}