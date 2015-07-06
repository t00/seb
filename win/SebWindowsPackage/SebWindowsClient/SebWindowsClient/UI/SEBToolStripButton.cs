using System;
using System.Windows.Forms;
using SebShared;
using SebWindowsClient.ConfigurationUtils;

namespace SebWindowsClient.UI
{
    public class SEBToolStripButton : ToolStripButton
    {
        public SEBToolStripButton()
        {
            this.ImageScaling = ToolStripItemImageScaling.SizeToFit;
        }

        public string Identifier { get; set; }

        public string WindowHandlingProcess { get; set; }

        public int FontSize
        {
            get
            {
                float sebTaskBarHeight = (int) SebSettings.valueForDictionaryKey(SebSettings.settingsCurrent, SebSettings.KeyTaskBarHeight);
                float fontSize = 10 * (sebTaskBarHeight/40) * SEBClientInfo.scaleFactor;
                if ((Boolean) SEBClientInfo.getSebSetting(SebSettings.KeyTouchOptimized)[SebSettings.KeyTouchOptimized])
                {
                    return (int)Math.Round(1.7 * fontSize);
                }
                else
                {
                    return (int) Math.Round(fontSize);
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
