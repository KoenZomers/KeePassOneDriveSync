using System.Reflection;
using System.Windows.Forms;

namespace KoenZomersKeePassOneDriveSync
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            var assemblyVersion = Assembly.GetCallingAssembly().GetName().Version;

            HeaderLabel.Text = string.Format("KeePass OneDriveSync v{0}.{1}.{2}", assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Build);
        }

        private void CloseButton_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
