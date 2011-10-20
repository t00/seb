namespace SebWindowsConfig
{
    partial class Form1
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labelSaveSebStarterIniFile = new System.Windows.Forms.Label();
            this.labelOpenSebStarterIniFile = new System.Windows.Forms.Label();
            this.labelBrowseSebStarterIniFolder = new System.Windows.Forms.Label();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.folderBrowserDialogBrowseSebStarterIni = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialogSebStarterIni = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogSebStarterIni = new System.Windows.Forms.SaveFileDialog();
            this.labelSebStarterIniPath = new System.Windows.Forms.Label();
            this.labelMsgHookIniPath = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.labelMsgHookIniPath);
            this.splitContainer1.Panel1.Controls.Add(this.labelSebStarterIniPath);
            this.splitContainer1.Panel1.Controls.Add(this.labelSaveSebStarterIniFile);
            this.splitContainer1.Panel1.Controls.Add(this.labelOpenSebStarterIniFile);
            this.splitContainer1.Panel1.Controls.Add(this.labelBrowseSebStarterIniFolder);
            this.splitContainer1.Panel1.Controls.Add(this.checkBox7);
            this.splitContainer1.Panel1.Controls.Add(this.checkBox6);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.checkBox5);
            this.splitContainer1.Panel1.Controls.Add(this.checkBox4);
            this.splitContainer1.Panel1.Controls.Add(this.checkBox3);
            this.splitContainer1.Panel1.Controls.Add(this.checkBox2);
            this.splitContainer1.Panel1.Controls.Add(this.checkBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(962, 509);
            this.splitContainer1.SplitterDistance = 679;
            this.splitContainer1.TabIndex = 0;
            // 
            // labelSaveSebStarterIniFile
            // 
            this.labelSaveSebStarterIniFile.AutoSize = true;
            this.labelSaveSebStarterIniFile.Location = new System.Drawing.Point(54, 462);
            this.labelSaveSebStarterIniFile.Name = "labelSaveSebStarterIniFile";
            this.labelSaveSebStarterIniFile.Size = new System.Drawing.Size(152, 17);
            this.labelSaveSebStarterIniFile.TabIndex = 10;
            this.labelSaveSebStarterIniFile.Text = "Save SebStarter.ini file";
            this.labelSaveSebStarterIniFile.Click += new System.EventHandler(this.labelSaveSebStarterIniFile_Click);
            // 
            // labelOpenSebStarterIniFile
            // 
            this.labelOpenSebStarterIniFile.AutoSize = true;
            this.labelOpenSebStarterIniFile.Location = new System.Drawing.Point(54, 430);
            this.labelOpenSebStarterIniFile.Name = "labelOpenSebStarterIniFile";
            this.labelOpenSebStarterIniFile.Size = new System.Drawing.Size(155, 17);
            this.labelOpenSebStarterIniFile.TabIndex = 9;
            this.labelOpenSebStarterIniFile.Text = "Open SebStarter.ini file";
            this.labelOpenSebStarterIniFile.Click += new System.EventHandler(this.labelOpenSebStarterIniFile_Click);
            // 
            // labelBrowseSebStarterIniFolder
            // 
            this.labelBrowseSebStarterIniFolder.AutoSize = true;
            this.labelBrowseSebStarterIniFolder.Location = new System.Drawing.Point(54, 401);
            this.labelBrowseSebStarterIniFolder.Name = "labelBrowseSebStarterIniFolder";
            this.labelBrowseSebStarterIniFolder.Size = new System.Drawing.Size(184, 17);
            this.labelBrowseSebStarterIniFolder.TabIndex = 8;
            this.labelBrowseSebStarterIniFolder.Text = "Browse SebStarter.ini folder";
            this.labelBrowseSebStarterIniFolder.Click += new System.EventHandler(this.labelBrowseSebStarterFolder_Click);
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(57, 340);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(127, 21);
            this.checkBox7.TabIndex = 7;
            this.checkBox7.Text = "Ease of Access";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(57, 302);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(70, 21);
            this.checkBox6.TabIndex = 6;
            this.checkBox6.Text = "Logoff";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "SebStarter options";
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(57, 263);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(65, 21);
            this.checkBox5.TabIndex = 4;
            this.checkBox5.Text = "Close";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(57, 227);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(144, 21);
            this.checkBox4.TabIndex = 3;
            this.checkBox4.Text = "Change Password";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(57, 190);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(121, 21);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Text = "Task Manager";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(57, 151);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(139, 21);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "Lock Workstation";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(57, 112);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(154, 21);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Fast User Switching";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(88, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "MsgHook options";
            // 
            // openFileDialogSebStarterIni
            // 
            this.openFileDialogSebStarterIni.FileName = "openFileDialogSebStarterIni";
            // 
            // labelSebStarterIniPath
            // 
            this.labelSebStarterIniPath.AutoSize = true;
            this.labelSebStarterIniPath.Location = new System.Drawing.Point(52, 50);
            this.labelSebStarterIniPath.Name = "labelSebStarterIniPath";
            this.labelSebStarterIniPath.Size = new System.Drawing.Size(126, 17);
            this.labelSebStarterIniPath.TabIndex = 11;
            this.labelSebStarterIniPath.Text = "SebStarter.ini path";
            // 
            // labelMsgHookIniPath
            // 
            this.labelMsgHookIniPath.AutoSize = true;
            this.labelMsgHookIniPath.Location = new System.Drawing.Point(54, 78);
            this.labelMsgHookIniPath.Name = "labelMsgHookIniPath";
            this.labelMsgHookIniPath.Size = new System.Drawing.Size(117, 17);
            this.labelMsgHookIniPath.TabIndex = 12;
            this.labelMsgHookIniPath.Text = "MsgHook.ini path";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 509);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "SEB Windows Preferences Window";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogBrowseSebStarterIni;
        private System.Windows.Forms.OpenFileDialog openFileDialogSebStarterIni;
        private System.Windows.Forms.SaveFileDialog saveFileDialogSebStarterIni;
        private System.Windows.Forms.Label labelBrowseSebStarterIniFolder;
        private System.Windows.Forms.Label labelOpenSebStarterIniFile;
        private System.Windows.Forms.Label labelSaveSebStarterIniFile;
        private System.Windows.Forms.Label labelSebStarterIniPath;
        private System.Windows.Forms.Label labelMsgHookIniPath;
    }
}

