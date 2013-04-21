using SebWindowsClient.BlockShortcutsUtils;
namespace SebWindowsClient
{
    partial class SebWindowsClientForm
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

            // Re-enable normal key events.
            SebKeyCapture.FilterKeys = false;

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Exit = new System.Windows.Forms.Button();
            this.lbl_User = new System.Windows.Forms.Label();
            this.tsPermittedProcesses = new System.Windows.Forms.ToolStrip();
            this.SuspendLayout();
            // 
            // btn_Exit
            // 
            this.btn_Exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Exit.Location = new System.Drawing.Point(812, 378);
            this.btn_Exit.MaximumSize = new System.Drawing.Size(102, 23);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(102, 23);
            this.btn_Exit.TabIndex = 0;
            this.btn_Exit.Text = "Quit";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // lbl_User
            // 
            this.lbl_User.AutoSize = true;
            this.lbl_User.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbl_User.Location = new System.Drawing.Point(851, 0);
            this.lbl_User.Name = "lbl_User";
            this.lbl_User.Size = new System.Drawing.Size(45, 13);
            this.lbl_User.TabIndex = 1;
            this.lbl_User.Text = "lbl_User";
            // 
            // tsPermittedProcesses
            // 
            this.tsPermittedProcesses.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tsPermittedProcesses.Location = new System.Drawing.Point(0, 376);
            this.tsPermittedProcesses.Name = "tsPermittedProcesses";
            this.tsPermittedProcesses.Size = new System.Drawing.Size(851, 25);
            this.tsPermittedProcesses.TabIndex = 2;
            this.tsPermittedProcesses.Text = "tsPermittedProcesses";
            // 
            // SebWindowsClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(896, 401);
            this.Controls.Add(this.tsPermittedProcesses);
            this.Controls.Add(this.lbl_User);
            this.Controls.Add(this.btn_Exit);
            this.Name = "SebWindowsClientForm";
            this.Text = "Safe Exam Browser";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SebWindowsClientForm_FormClosing);
            this.Load += new System.EventHandler(this.SebWindowsClientForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.Label lbl_User;
        private System.Windows.Forms.ToolStrip tsPermittedProcesses;
    }
}

