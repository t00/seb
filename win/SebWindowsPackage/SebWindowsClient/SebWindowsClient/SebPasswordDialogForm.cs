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
        /// ----------------------------------------------------------------------------------------
        /// <summary>
        /// Show SEB Password Dialog Form.
        /// </summary>
        /// ----------------------------------------------------------------------------------------
        public static string ShowPasswordDialogForm(string title, string passwordRequestText)
        {
            using (SebPasswordDialogForm sebPasswordDialogForm = new SebPasswordDialogForm())
            {
                // Set the title of the dialog window
                sebPasswordDialogForm.Text = title;
                // Set the text of the dialog
                sebPasswordDialogForm.LabelText = passwordRequestText;
                sebPasswordDialogForm.txtSEBPassword.Focus();
                // If we are running in SebWindowsClient we need to activate it before showing the password dialog
                //if (SEBClientInfo.SebWindowsClientForm != null) SebWindowsClientForm.SEBToForeground(); //SEBClientInfo.SebWindowsClientForm.Activate();
                // Show password dialog as a modal dialog and determine if DialogResult = OK.
                if (sebPasswordDialogForm.ShowDialog() == DialogResult.OK)
                {
                    // Read the contents of testDialog's TextBox.
                    string password = sebPasswordDialogForm.txtSEBPassword.Text;
                    sebPasswordDialogForm.txtSEBPassword.Text = "";
                    //sebPasswordDialogForm.txtSEBPassword.Focus();
                    return password;
                }
                else
                {
                    return null;
                }
            }
        }

        public SebPasswordDialogForm()
        {
            InitializeComponent();
            //this.txtSEBPassword.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //SEBClientInfo.SebWindowsClientForm.sebPassword = null;
            this.txtSEBPassword.Text = "";
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
