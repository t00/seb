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
        public SEBLoading()
        {
            InitializeComponent();
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
            this.Refresh();
        }
    }
}
