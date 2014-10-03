namespace SebWindowsClient
{
    partial class WindowChooser
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
            this.appList = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // appList
            // 
            this.appList.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.appList.BackColor = System.Drawing.SystemColors.Control;
            this.appList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.appList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appList.GridLines = true;
            this.appList.HoverSelection = true;
            this.appList.Location = new System.Drawing.Point(5, 5);
            this.appList.MultiSelect = false;
            this.appList.Name = "appList";
            this.appList.Scrollable = false;
            this.appList.ShowGroups = false;
            this.appList.Size = new System.Drawing.Size(65, 65);
            this.appList.TabIndex = 0;
            this.appList.UseCompatibleStateImageBehavior = false;
            // 
            // WindowChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(75, 75);
            this.Controls.Add(this.appList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WindowChooser";
            this.Opacity = 0.8D;
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "WindowChooser";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView appList;
    }
}