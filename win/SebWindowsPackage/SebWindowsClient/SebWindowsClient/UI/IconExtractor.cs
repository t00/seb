using System.Drawing;
using System.Linq;

#if !NO_HIGHRES_ICON
	using System.Drawing.IconLib;
#endif

namespace SebWindowsClient.UI
{
	public static class IconExtractor
	{
		public static Bitmap ExtractHighResIconImage(string path, int? size = null)
		{
#if !NO_HIGHRES_ICON
			var mi = new MultiIcon();
			mi.Load(path);
			var si = mi.FirstOrDefault();
			if(si != null)
			{
				IconImage icon;
				if(size != null)
				{
					if(size.Value <= 32)
					{
						try
						{
							return Icon.ExtractAssociatedIcon(path).ToBitmap();
						}
						catch
						{
						}
					}
					icon = si.Where(x => x.Size.Height >= size.Value).OrderBy(x => x.Size.Height).FirstOrDefault();
					if(icon != null)
					{
						return icon.Icon.ToBitmap();
					}
				}
				var max = si.Max(i => i.Size.Height);
				icon = si.FirstOrDefault(i => i.Size.Height == max);
				if(icon != null)
					return icon.Transparent;
			}
#endif
			return null;
		}
	}
}
