using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SebWindowsClient.ProcessUtils;
using SebWindowsClient.Properties;


namespace SebWindowsClient.UI
{
    public class SEBReloadBrowserToolStripButton : SEBToolStripButton
    {
        public SEBReloadBrowserToolStripButton()
        {
            this.ToolTipText = "Refresh browser";
            base.Image = (Bitmap)Resources.ResourceManager.GetObject("reload");
            this.BackColor = Color.White;
            this.Alignment = ToolStripItemAlignment.Right;
        }

        protected override void OnClick(EventArgs e)
        {
            try
            {
                SEBWindowHandler.BringWindowToTop(
                    SEBWindowHandler.GetOpenWindows()
                        .First(w => w.Key.GetProcess().GetExecutableName().Contains("xul")).Key);
                SendKeys.Send("{F5}");
            }
            catch{}
        }
    }
}
