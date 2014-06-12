using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Ink;

namespace SebWindowsClient
{
    public partial class SEBLoading : Form
    {
        public SEBLoading()
        {
            InitializeComponent();

            var t = new Timer {Interval = 200};
            t.Tick += (sender, args) => Progress();
            t.Start();
        }

        public void Progress()
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

        public void KillMe(object o, EventArgs e)
        {
            this.Close();
        }
    }
}
