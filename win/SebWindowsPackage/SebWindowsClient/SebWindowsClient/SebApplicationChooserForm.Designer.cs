namespace SebWindowsClient
{
    partial class SebApplicationChooserForm
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
            this.listApplications = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // listApplications
            // 
            this.listApplications.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listApplications.BackColor = System.Drawing.SystemColors.Control;
            this.listApplications.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listApplications.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listApplications.GridLines = true;
            this.listApplications.HoverSelection = true;
            this.listApplications.Location = new System.Drawing.Point(0, 0);
            this.listApplications.MultiSelect = false;
            this.listApplications.Name = "listApplications";
            this.listApplications.Size = new System.Drawing.Size(523, 104);
            this.listApplications.TabIndex = 0;
            this.listApplications.UseCompatibleStateImageBehavior = false;
            // 
            // SebApplicationChooserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(523, 104);
            this.ControlBox = false;
            this.Controls.Add(this.listApplications);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "SebApplicationChooserForm";
            this.Opacity = 0.75D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listApplications;
    }
}