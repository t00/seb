﻿namespace SebWindowsClient
{
    partial class SebCloseDialogForm
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
            this.txtQuitPassword = new System.Windows.Forms.MaskedTextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblQuitPassword = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtQuitPassword
            // 
            this.txtQuitPassword.Location = new System.Drawing.Point(120, 35);
            this.txtQuitPassword.Name = "txtQuitPassword";
            this.txtQuitPassword.PasswordChar = '*';
            this.txtQuitPassword.Size = new System.Drawing.Size(221, 20);
            this.txtQuitPassword.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(28, 90);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(266, 90);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblQuitPassword
            // 
            this.lblQuitPassword.AutoSize = true;
            this.lblQuitPassword.Location = new System.Drawing.Point(25, 42);
            this.lblQuitPassword.Name = "lblQuitPassword";
            this.lblQuitPassword.Size = new System.Drawing.Size(78, 13);
            this.lblQuitPassword.TabIndex = 3;
            this.lblQuitPassword.Text = "Quit Password:";
            // 
            // SebCloseDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 139);
            this.Controls.Add(this.lblQuitPassword);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtQuitPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SebCloseDialogForm";
            this.Text = "Close SEB Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblQuitPassword;
        public System.Windows.Forms.MaskedTextBox txtQuitPassword;
    }
}