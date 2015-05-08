using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SebWindowsClient.ConfigurationUtils;

namespace SebWindowsClient.UI
{
    public class SEBToolStripButton : ToolStripButton
    {
        public SEBToolStripButton()
        {
            this.ImageScaling = ToolStripItemImageScaling.SizeToFit;
        }

        public string Identifier
        { get; set; }

        public string WindowHandlingProcess
        { get; set; }

        public int FontSize
        {
            get
            {
                if ((Boolean) SEBClientInfo.getSebSetting(SEBSettings.KeyTouchOptimized)[SEBSettings.KeyTouchOptimized])
                {
                    return 14;
                }
                else
                {
                    return 10;
                }
            }
        }

        protected override void OnMouseHover(EventArgs e)
        {
            if (this.Parent != null)
                Parent.Focus();
            base.OnMouseHover(e);
        } 
    }
}
