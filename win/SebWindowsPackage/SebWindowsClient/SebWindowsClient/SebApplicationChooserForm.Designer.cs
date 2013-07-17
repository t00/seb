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
            this.btnRefresh = new System.Windows.Forms.Button();
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
            this.listApplications.Name = "listApplications";
            this.listApplications.Size = new System.Drawing.Size(523, 204);
            this.listApplications.TabIndex = 0;
            this.listApplications.UseCompatibleStateImageBehavior = false;
            this.listApplications.ItemActivate += new System.EventHandler(this.listApplications_ItemActivate);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::SebWindowsClient.Properties.Resources.refresh;
            this.btnRefresh.Location = new System.Drawing.Point(481, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(42, 36);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // SebApplicationChooserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 204);
            this.ControlBox = false;
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.listApplications);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SebApplicationChooserForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Seb Application Chooser";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listApplications;
        private System.Windows.Forms.Button btnRefresh;
    }
}