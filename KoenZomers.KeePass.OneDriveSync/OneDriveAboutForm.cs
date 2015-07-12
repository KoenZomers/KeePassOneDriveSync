using System;
using System.Reflection;
using System.Windows.Forms;

namespace KoenZomersKeePassOneDriveSync
{
    /// <summary>
    /// About Form
    /// </summary>
    public partial class OneDriveAboutForm : Form
    {
        public OneDriveAboutForm()
        {
            InitializeComponent();

            var assemblyVersion = Assembly.GetCallingAssembly().GetName().Version;

            HeaderLabel.Text = string.Format("KeePass OneDriveSync v{0}.{1}.{2}", assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Build);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {

        }
    }
}
