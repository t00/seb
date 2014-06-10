using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SebWindowsClient
{
    public partial class SEBLoading : Form
    {
        private DateTime lastUpdate;

        public SEBLoading()
        {
            InitializeComponent();

            this.Show();
            Application.DoEvents();

            lastUpdate = DateTime.Now;
        }
        
        public void Progress()
        {
            if (DateTime.Now - lastUpdate > new TimeSpan(0, 0, 0, 0, 200))
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
                this.lblLoading.Refresh();
                //Application.DoEvents();
                lastUpdate = DateTime.Now;
            }
        }
    }
}
