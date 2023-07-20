using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Image = System.Drawing.Image;

namespace ProBotTelegramClient.Instruments
{
	public class Converter
	{
		public static Image MatToImage(Mat mat)
		{
			using (var data = mat.ToMemoryStream())
			{
				return Image.FromStream(data);
			}
		}
	}
}
