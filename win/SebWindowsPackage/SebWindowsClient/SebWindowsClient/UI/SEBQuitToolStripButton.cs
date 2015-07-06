using System.Drawing;
using System.Windows.Forms;
using SebShared.Properties;
using SebWindowsClient.Properties;

namespace SebWindowsClient.UI
{
    public class SEBQuitToolStripButton : SEBToolStripButton
    {
        public SEBQuitToolStripButton()
        {
            this.ToolTipText = SEBUIStrings.confirmQuitting;
            this.Alignment = ToolStripItemAlignment.Right;
            base.Image = (Bitmap) Resources.ResourceManager.GetObject("quit");
        }
    }
}
