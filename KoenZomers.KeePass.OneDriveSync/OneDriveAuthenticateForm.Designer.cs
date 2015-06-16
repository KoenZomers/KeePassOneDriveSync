namespace KoenZomersKeePassOneDriveSync
{
    partial class OneDriveAuthenticateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.WebBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // WebBrowser
            // 
            this.WebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WebBrowser.Location = new System.Drawing.Point(-1, 0);
            this.WebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.WebBrowser.Name = "WebBrowser";
            this.WebBrowser.Size = new System.Drawing.Size(532, 602);
            this.WebBrowser.TabIndex = 0;
            this.WebBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.WebBrowser_Navigated);
            // 
            // OneDriveAuthenticateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 601);
            this.Controls.Add(this.WebBrowser);
            this.MinimizeBox = false;
            this.Name = "OneDriveAuthenticateForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Authenticate to OneDrive";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser WebBrowser;
    }
}