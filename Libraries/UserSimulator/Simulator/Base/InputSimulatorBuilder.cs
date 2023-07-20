using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;
using WindowsInput;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenCvSharp;
using Newtonsoft.Json;

namespace ProBotTelegramClient.Simulator.Base
{
    public class InputSimulatorBuilder
    {
        private static InputSimulator inputSimulator;
		private readonly string printScreenPath = "C:\\Users\\dokto\\OneDrive\\Рабочий стол\\TelegramBotKPI\\Libraries\\UserSimulator\\ScreenPrints\\";

		private static InputSimulator simulator
        {
            get
            {
                if (inputSimulator is null)
                {
                    inputSimulator = new InputSimulator();
                }

                return inputSimulator;
            }
        }

        public static void SimulateKeyPress(Keys key)
        {
            simulator.Keyboard.KeyPress((VirtualKeyCode)key);
        }
        public static void SimulateKeyDown(Keys key)
        {
            simulator.Keyboard.KeyDown((VirtualKeyCode)key);
        }
        public static void SimulateKeyUp(Keys key)
        {
            simulator.Keyboard.KeyUp((VirtualKeyCode)key);
        }
        public static void SimulateModifiedKey(ModifiedKey modifiedKey)
        {
            simulator.Keyboard.ModifiedKeyStroke(modifiedKey.modifiedKey, modifiedKey.simpleKey);
        }
		public static void SimulateTextInput(string text)
		{
			simulator.Keyboard.TextEntry(text);
		}

		public static void SimulateMouseClick(MouseButtons button, int x, int y)
        {
			SimulateMouseMove(x, y);

			switch (button)
            {
                case MouseButtons.Left:
                    simulator.Mouse.LeftButtonClick();
                    break;
                case MouseButtons.Right:
                    simulator.Mouse.RightButtonClick();
                    break;
                case MouseButtons.Middle:
					simulator.Mouse.XButtonClick(2);
					break;
                default:
                    break;
            }
            return;
        }
		public static void SimulateMouseDown(MouseButtons button, int x, int y)
		{
			SimulateMouseMove(x, y);

			switch (button)
			{
				case MouseButtons.Left:
					simulator.Mouse.LeftButtonDown();
					break;
				case MouseButtons.Right:
					simulator.Mouse.RightButtonDown();
					break;
				case MouseButtons.Middle:
					simulator.Mouse.XButtonDown(2);
					break;
				default:
					break;
			}
			return;
		}
		public static void SimulateMouseUp(MouseButtons button, int x, int y)
		{
			SimulateMouseMove(x, y);

			switch (button)
			{
				case MouseButtons.Left:
					simulator.Mouse.LeftButtonUp();
					break;
				case MouseButtons.Right:
					simulator.Mouse.RightButtonUp();
					break;
				case MouseButtons.Middle:
					simulator.Mouse.XButtonUp(2);
					break;
				default:
					break;
			}
			return;
		}
		public static void SimulateMouseDouble(MouseButtons button, int x, int y)
		{
			SimulateMouseMove(x, y);

			switch (button)
			{
				case MouseButtons.Left:
					simulator.Mouse.LeftButtonDoubleClick();
					break;
				case MouseButtons.Right:
					simulator.Mouse.RightButtonDoubleClick();
					break;
				case MouseButtons.Middle:
					simulator.Mouse.XButtonDown(2);
					break;
				default:
					break;
			}
			return;
		}
		public static void SimulateMouseWeel(int x, int y, int delta)
		{
			SimulateMouseMove(x, y);

			simulator.Mouse.VerticalScroll(delta);
		}
		public static void SimulateMouseMove(int x, int y)
		{
			var absoluteX = (int)((double)x / Screen.PrimaryScreen.Bounds.Width * 65535);
			var absoluteY = (int)((double)y / Screen.PrimaryScreen.Bounds.Height * 65535);

			simulator.Mouse.MoveMouseTo(absoluteX, absoluteY);
		}

		public static string PrintScreen(string? path = null)
		{
			Rectangle bounds = Screen.GetBounds(System.Drawing.Point.Empty);

			using (var bitmap = new Bitmap(bounds.Width, bounds.Height))
			{
				using (var g = Graphics.FromImage(bitmap))
				{
					g.CopyFromScreen(System.Drawing.Point.Empty, System.Drawing.Point.Empty, bounds.Size);
				}

				Mat mat = ConvertBitmapToMat(bitmap);

				return SerializeMat(mat);
			}

			#region Methods
			Mat ConvertBitmapToMat(Bitmap bitmap)
			{
				Mat mat = new Mat(bitmap.Height, bitmap.Width, MatType.CV_8UC3);

				using (var stream = new MemoryStream())
				{
					bitmap.Save(stream, ImageFormat.Png);
					mat = Cv2.ImDecode(stream.ToArray(), ImreadModes.Color);
				}

				return mat;
			}
			string SerializeMat(Mat mat)
			{
				byte[] matData = mat.ToBytes();
				return JsonConvert.SerializeObject(matData);
			}
			Mat DeserializeMat(string jsonString)
			{
				byte[] matData = JsonConvert.DeserializeObject<byte[]>(jsonString);
				Mat mat = Cv2.ImDecode(matData, ImreadModes.Color);
				return mat;
			}
			#endregion
		}

		public static void Paste()
		{
			SimulateModifiedKey(
				new ModifiedKey(
					new Keys[]
					{
						Keys.ControlKey,
					},
					new Keys[]
					{
						Keys.V
					}
				)
			);
		}
	}
}
