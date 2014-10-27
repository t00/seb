using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.CryptographyUtils;
using SebWindowsClient.ProcessUtils;
using SebWindowsClient.Properties;
using SebWindowsClient.XULRunnerCommunication;


namespace SebWindowsClient.UI
{
    public class SEBRestartExamToolStripButton : SEBToolStripButton
    {
        public SEBRestartExamToolStripButton()
        {
            this.ToolTipText = (String)SEBClientInfo.getSebSetting(SEBSettings.KeyRestartExamText)[SEBSettings.KeyRestartExamText];
            base.Image = (Bitmap)Resources.ResourceManager.GetObject("restartExam");
            this.Alignment = ToolStripItemAlignment.Right;
        }

        protected override void OnClick(EventArgs e)
        {
            if ((Boolean)SEBClientInfo.getSebSetting(SEBSettings.KeyRestartExamPasswordProtected)[SEBSettings.KeyRestartExamPasswordProtected])
            {
                var quitPassword = (String)SEBClientInfo.getSebSetting(SEBSettings.KeyHashedQuitPassword)[SEBSettings.KeyHashedQuitPassword];
                if (String.IsNullOrWhiteSpace(quitPassword))
                    return;
                var password = SebPasswordDialogForm.ShowPasswordDialogForm((String)SEBClientInfo.getSebSetting(SEBSettings.KeyRestartExamText)[SEBSettings.KeyRestartExamText], SEBUIStrings.restartExamMessage);
                if (String.IsNullOrWhiteSpace(password)) return;
                var hashedPassword = SEBProtectionController.ComputePasswordHash(password);
                if (String.Compare(quitPassword, hashedPassword, StringComparison.OrdinalIgnoreCase) != 0)
                    return;
            }
            SEBXULRunnerWebSocketServer.SendRestartExam();
        }
    }
}
