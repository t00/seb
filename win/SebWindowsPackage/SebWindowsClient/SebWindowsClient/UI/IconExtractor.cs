using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.IconLib;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Forms;

namespace SebWindowsClient.UI
{
    public static class Iconextractor
    {
        public static Bitmap ExtractHighResIconImage(string path)
        {
            var mi = new MultiIcon();
            mi.Load(path);
            var si = mi.FirstOrDefault();
            if (si != null)
            {
                var max = si.Max(_i => _i.Size.Height);
                var icon = si.FirstOrDefault(i => i.Size.Height == max);
                if(icon != null)
                    return icon.Transparent;
            }
            return null;
        }
    }
}
