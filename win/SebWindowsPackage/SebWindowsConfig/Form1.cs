using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SebWindowsConfig
{
    public partial class Form1 : Form
    {
        String sebStarterIniPath = "";
        String sebMsgHookIniPath = "";
        StreamReader streamReaderSebStarterIni;
        StreamWriter streamWriterSebStarterIni;

        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = !checkBox1.Checked;
        }

        private void labelBrowseSebStarterFolder_Click(object sender, EventArgs e)
        {
            DialogResult browseFolderName = folderBrowserDialogBrowseSebStarterIni.ShowDialog();
        }

        private void labelOpenSebStarterIniFile_Click(object sender, EventArgs e)
        {
            DialogResult openFileDialogResult = openFileDialogSebStarterIni.ShowDialog();
            sebStarterIniPath                 = openFileDialogSebStarterIni.FileName;
            labelSebStarterIniPath.Text       = sebStarterIniPath;

            try 
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(sebStarterIniPath)) 
                {
                    String line;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null) 
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception streamReadException) 
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(streamReadException.Message);
            }

        }

        private void labelSaveSebStarterIniFile_Click(object sender, EventArgs e)
        {
            DialogResult saveFileName = saveFileDialogSebStarterIni.ShowDialog();
        }
    }
}
