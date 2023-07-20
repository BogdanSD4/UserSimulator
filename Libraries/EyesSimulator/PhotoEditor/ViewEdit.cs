using EyesSimulator.PhotoEditor.Settings;
using FileSystemTree;
using Newtonsoft.Json;
using OpenCvSharp;
using OpenCvSharp.Features2D;
using OpenCvSharp.XFeatures2D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace EyesSimulator.PhotoEditor
{
    public class ViewEdit
	{
		private static readonly string SavePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\temp.png";
		/// <summary>
		/// Return byte stream
		/// </summary>
		/// <param name="photo"></param>
		public static string CutPhoto(Photo photo)
		{
			using (Mat mat = DeserializeMat(photo.Data))
			{
				int x = photo.Box.X1;
				int y = photo.Box.Y1;
				int width = photo.Box.Width;
				int height = photo.Box.Height;

				Rect roi = new Rect(x, y, width, height);
				using (Mat croppedImage = new Mat(mat, roi))
				{
					return SerializeMat(croppedImage);
				}
			}
		}

		public static bool FrameEquals(Photo target, Photo frame, int needSimilary = 1)
		{
			using (Mat frame1 = Mat.FromArray(target.Data))
			using (Mat frame2 = Mat.FromArray(frame.Data))
			{
				Mat frame1Float = new Mat();
				Mat frame2Float = new Mat();

				frame1.ConvertTo(frame1Float, MatType.CV_32F);
				frame2.ConvertTo(frame2Float, MatType.CV_32F);

				double similarity = Cv2.CompareHist(frame1Float, frame2Float, HistCompMethods.Correl) + 2;

				if (similarity >= needSimilary) return true;
				
			}

			return false;
		}

		public static bool FrameEqualsVertical(Photo target, Photo frame, int needSimilary = 1)
		{
			int height = frame.Box.Height;
			int width = frame.Box.Width;
			int startY = 0;

			using (Mat frame1 = Mat.FromArray(target.Data))
			using (Mat frame2 = Mat.FromArray(frame.Data))
			{
				Mat frame1Float = new Mat();
				Mat frame2Float = new Mat();

				frame1.ConvertTo(frame1Float, MatType.CV_32F);
				frame2.ConvertTo(frame2Float, MatType.CV_32F);
				
				while(startY + height < target.Box.Height)
				{
					Rect roi = new Rect(0, startY++, width, height);
					Mat variant = new Mat(frame1Float, roi);

					double similarity = Cv2.CompareHist(variant, frame2Float, HistCompMethods.Correl) + 2;

					if (similarity >= needSimilary)
					{
						return true;
					}
				}
			}

			return false;
		}
		public static bool FrameEqualsVertical(Photo target, Photo frame, ref Rect position, int needSimilary = 1)
		{
			int height = frame.Box.Height;
			int width = frame.Box.Width;
			int startY = 0;

			using (Mat frame1 = Mat.FromArray(target.Data))
			using (Mat frame2 = Mat.FromArray(frame.Data))
			{
				Mat frame1Float = new Mat();
				Mat frame2Float = new Mat();

				frame1.ConvertTo(frame1Float, MatType.CV_32F);
				frame2.ConvertTo(frame2Float, MatType.CV_32F);

				while (startY + height < target.Box.Height)
				{
					Rect roi = new Rect(0, startY++, width, height);
					Mat variant = new Mat(frame1Float, roi);

					double similarity = Cv2.CompareHist(variant, frame2Float, HistCompMethods.Correl) + 2;

					if (similarity >= needSimilary)
					{
						position = new Rect(target.Box.X1, roi.Y, width, height);
						return true;
					}
				}
			}

			return false;
		}

		public static string SerializeMat(Mat mat)
		{
			byte[] matData = mat.ToBytes();
			return JsonConvert.SerializeObject(matData);
		}
		public static Mat DeserializeMat(string jsonString)
		{
			byte[] matData = JsonConvert.DeserializeObject<byte[]>(jsonString);
			Mat mat = Cv2.ImDecode(matData, ImreadModes.Color);
			return mat;
		}
	}
}
