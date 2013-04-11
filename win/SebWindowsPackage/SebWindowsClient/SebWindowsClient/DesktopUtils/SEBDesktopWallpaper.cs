// -------------------------------------------------------------
//     Viktor tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SebWindowsClient.DesktopUtils
{
    public class SEBDesktopWallpaper
    {
        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        public SEBDesktopWallpaper()
        {
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(
            int uAction, int uParam, string lpvParam, int fuWinIni);

        public enum Style : int
        {
            Tiled, Centered, Stretched
        }

        public void SetWallpaper(string path, Style style)
        {
            if (System.IO.File.Exists(path))
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);

                switch (style)
                {
                    case Style.Stretched:
                        key.SetValue(@"WallpaperStyle", "2");
                        key.SetValue(@"TileWallpaper", "0");
                        break;
                    case Style.Centered:
                        key.SetValue(@"WallpaperStyle", "1");
                        key.SetValue(@"TileWallpaper", "0");
                        break;
                    case Style.Tiled:
                        key.SetValue(@"WallpaperStyle", "1");
                        key.SetValue(@"TileWallpaper", "1");
                        break;
                }
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            }
            else
            {
                throw new System.IO.FileNotFoundException();
            }
        }
    }
}
