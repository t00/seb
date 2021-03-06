﻿using System;
using System.Drawing;
using System.Windows.Forms;
using SebShared;
using SebShared.Properties;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.DesktopUtils;

namespace SebWindowsClient
{
    public partial class SEBSplashScreen : Form
    {
        #region Instance
        public SEBSplashScreen()
        {
            InitializeComponent();
	        this.tbCopyright.Text = SEBUIStrings.sebSplashCopyright;
	        tbCopyright.Visible = !string.IsNullOrEmpty(tbCopyright.Text);
            this.tbVersion.Text = SEBUIStrings.sebSplashVersion;
            try
            {
				tbVersion.Visible = !string.IsNullOrEmpty(tbVersion.Text);
				this.tbVersion.Text += " " + Application.ProductVersion;
            }
            catch (Exception)
            {
                //No Version info available
            }

	        if(pictureBox1.Image != null && pictureBox1.Image.Height > 0)
	        {
				pictureBox1.Width = pictureBox1.Image.Width;
				pictureBox1.Height = pictureBox1.Image.Height;
				ClientSize = new Size(pictureBox1.Image.Width, pictureBox1.Image.Height + lblLoading.Height);
	        }
            this.Click += KillMe;
            this.pictureBox1.Click += KillMe;
            this.tbVersion.Click += KillMe;
            this.lblLoading.Click += KillMe;

            var t = new Timer {Interval = 200};
            t.Tick += (sender, args) => Progress();
            t.Start();
        }

        private void Progress()
        {
            switch (lblLoading.Text)
            {
                case "Loading":
                    lblLoading.Text = "Loading .";
                    break;
                case "Loading .":
                    lblLoading.Text = "Loading ..";
                    break;
                case "Loading ..":
                    lblLoading.Text = "Loading ...";
                    break;
                default:
                    lblLoading.Text = "Loading";
                    break;
            }
        }

        /// <summary>
        /// Closes the window - invoked via CloseSplash();
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void KillMe(object o, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region static Thread Access

        private static SEBSplashScreen splash;

        /// <summary>
        /// Call via separate thread
        /// var thread = new Thread(SEBLoading.StartSplash);
        /// thread.Start();
        /// </summary>
        static public void StartSplash()
        {
            //Set the threads desktop to the new desktop if "Create new Desktop" is activated
            if (SEBClientInfo.SEBNewlDesktop != null && SebInstance.Settings.Get<bool>(SebSettings.KeyCreateNewDesktop))
                SEBDesktopController.SetCurrent(SEBClientInfo.SEBNewlDesktop);
            else
                SEBDesktopController.SetCurrent(SEBClientInfo.OriginalDesktop);

            // Instance a splash form given the image names
            splash = new SEBSplashScreen();
            // Run the form
            Application.Run(splash);
        }

        /// <summary>
        /// Invokes the thread with the window and closes it
        /// </summary>
        public static void CloseSplash()
        {
            if (splash == null)
                return;
            try
            {
                // Shut down the splash screen
                splash.Invoke(new EventHandler(splash.KillMe));
                splash.Dispose();
                splash = null;
            }
            catch (Exception)
            { }
        }

        #endregion

    }
}
