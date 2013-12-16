using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.CryptographyUtils;

namespace SebWindowsClient
{
    public partial class SebPasswordDialogForm : Form
    {
        public SebPasswordDialogForm sebPasswordDialogForm;

        public SebPasswordDialogForm()
        {
            InitializeComponent();
            //this.txtSEBPassword.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //SEBClientInfo.SebWindowsClientForm.sebPassword = null;
            //this.txtSEBPassword.Text = "";
            //this.Visible = false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        // Expose the label for changing from outside of the form
        public string LabelText
        {
            get
            {
                return this.lblSEBPassword.Text;
            }
            set
            {
                this.lblSEBPassword.Text = value;
            }
        }

    }
}
