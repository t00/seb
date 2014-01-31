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
            //SebKeyCapture.FilterKeys = false;

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SebWindowsClientForm));
            this.ilProcessIcons = new System.Windows.Forms.ImageList(this.components);
            this.btn_Exit = new System.Windows.Forms.Button();
            this.taskbarToolStrip = new SebWindowsClient.TaskbarToolStrip();
            this.SuspendLayout();
            // 
            // ilProcessIcons
            // 
            this.ilProcessIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilProcessIcons.ImageStream")));
            this.ilProcessIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilProcessIcons.Images.SetKeyName(0, "AcrobatReader");
            this.ilProcessIcons.Images.SetKeyName(1, "calc");
            this.ilProcessIcons.Images.SetKeyName(2, "notepad");
            this.ilProcessIcons.Images.SetKeyName(3, "xulrunner");
            // 
            // btn_Exit
            // 
            this.btn_Exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Exit.CausesValidation = false;
            this.btn_Exit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Exit.Location = new System.Drawing.Point(788, 9);
            this.btn_Exit.MaximumSize = new System.Drawing.Size(102, 23);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(102, 23);
            this.btn_Exit.TabIndex = 4;
            this.btn_Exit.Text = "Quit";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // taskbarToolStrip
            // 
            this.taskbarToolStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.taskbarToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.taskbarToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.taskbarToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.taskbarToolStrip.Location = new System.Drawing.Point(0, 15);
            this.taskbarToolStrip.Name = "taskbarToolStrip";
            this.taskbarToolStrip.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.taskbarToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.taskbarToolStrip.Size = new System.Drawing.Size(896, 25);
            this.taskbarToolStrip.TabIndex = 3;
            this.taskbarToolStrip.Text = "taskbarToolStrip";
            // 
            // SebWindowsClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(896, 40);
            this.ControlBox = false;
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.taskbarToolStrip);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SebWindowsClientForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TransparencyKey = System.Drawing.Color.Black;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SebWindowsClientForm_FormClosing);
            this.Load += new System.EventHandler(this.SebWindowsClientForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList ilProcessIcons;
        private TaskbarToolStrip taskbarToolStrip;
        private System.Windows.Forms.Button btn_Exit;
    }
}

