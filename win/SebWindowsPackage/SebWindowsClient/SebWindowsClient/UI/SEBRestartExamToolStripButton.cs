using System;
using System.Drawing;
using System.Windows.Forms;
using SebShared.CryptographyUtils;
using SebShared;
using SebShared.Properties;
using SebWindowsClient.ConfigurationUtils;
using SebWindowsClient.Properties;
using SebWindowsClient.XULRunnerCommunication;


namespace SebWindowsClient.UI
{
    public class SEBRestartExamToolStripButton : SEBToolStripButton
    {
        public SEBRestartExamToolStripButton()
        {
            // Get text (title/tool tip) for restarting exam
            string restartExamTitle = (String)SEBClientInfo.getSebSetting(SebSettings.KeyRestartExamText)[SebSettings.KeyRestartExamText];
            // If there was no individual restart exam text set, we use the default text (which is localized)
            if (String.IsNullOrEmpty(restartExamTitle))
            {
                restartExamTitle = SEBUIStrings.restartExamDefaultTitle;
            }
            this.ToolTipText = restartExamTitle;
            base.Image = (Bitmap)Resources.ResourceManager.GetObject("restartExam");
            this.Alignment = ToolStripItemAlignment.Right;
        }

        protected override void OnClick(EventArgs e)
        {
            if ((Boolean)SEBClientInfo.getSebSetting(SebSettings.KeyRestartExamPasswordProtected)[SebSettings.KeyRestartExamPasswordProtected])
            {
                var quitPassword = (String)SEBClientInfo.getSebSetting(SebSettings.KeyHashedQuitPassword)[SebSettings.KeyHashedQuitPassword];
                // Get text (title/tool tip) for restarting exam
                string restartExamTitle = (String)SEBClientInfo.getSebSetting(SebSettings.KeyRestartExamText)[SebSettings.KeyRestartExamText];
                // If there was no individual restart exam text set, we use the default text (which is localized)
                if (String.IsNullOrEmpty(restartExamTitle)) {
                    restartExamTitle = SEBUIStrings.restartExamDefaultTitle;
                }
                if (!String.IsNullOrWhiteSpace(quitPassword))
                {
                    var password = SebPasswordDialogForm.ShowPasswordDialogForm(restartExamTitle, SEBUIStrings.restartExamMessage);
                    if (String.IsNullOrWhiteSpace(password)) return;
                    var hashedPassword = SebProtectionController.ComputePasswordHash(password);
                    if (String.Compare(quitPassword, hashedPassword, StringComparison.OrdinalIgnoreCase) != 0)
                        return;
                }
            }
            SEBXULRunnerWebSocketServer.SendRestartExam();
        }
    }
}
