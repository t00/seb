namespace SebWindowsClient
{
    partial class SEBSplashScreen
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
			this.lblLoading = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tbVersion = new System.Windows.Forms.TextBox();
			this.tbCopyright = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// lblLoading
			// 
			this.lblLoading.BackColor = System.Drawing.Color.Transparent;
			this.lblLoading.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblLoading.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLoading.Location = new System.Drawing.Point(0, 302);
			this.lblLoading.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblLoading.Name = "lblLoading";
			this.lblLoading.Padding = new System.Windows.Forms.Padding(12, 0, 0, 5);
			this.lblLoading.Size = new System.Drawing.Size(544, 22);
			this.lblLoading.TabIndex = 0;
			this.lblLoading.Text = "Loading ...";
			this.lblLoading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::SebWindowsClient.Properties.Resources.SebSplashImage;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(550, 300);
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// TxtVersion
			// 
			this.tbVersion.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.tbVersion.BackColor = System.Drawing.Color.White;
			this.tbVersion.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tbVersion.CausesValidation = false;
			this.tbVersion.Enabled = false;
			this.tbVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbVersion.Location = new System.Drawing.Point(327, 113);
			this.tbVersion.Multiline = true;
			this.tbVersion.Name = "tbVersion";
			this.tbVersion.Size = new System.Drawing.Size(217, 30);
			this.tbVersion.TabIndex = 2;
			// 
			// textBox2
			// 
			this.tbCopyright.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.tbCopyright.BackColor = System.Drawing.Color.White;
			this.tbCopyright.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tbCopyright.CausesValidation = false;
			this.tbCopyright.Enabled = false;
			this.tbCopyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbCopyright.Location = new System.Drawing.Point(327, 129);
			this.tbCopyright.Multiline = true;
			this.tbCopyright.Name = "tbCopyright";
			this.tbCopyright.Size = new System.Drawing.Size(217, 171);
			this.tbCopyright.TabIndex = 3;
			// 
			// SEBSplashScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(544, 324);
			this.ControlBox = false;
			this.Controls.Add(this.tbCopyright);
			this.Controls.Add(this.tbVersion);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.lblLoading);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MinimizeBox = false;
			this.Name = "SEBSplashScreen";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLoading;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox tbVersion;
        private System.Windows.Forms.TextBox tbCopyright;
    }
}