using EyesSimulator.PhotoEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Point = System.Drawing.Point;

namespace ProBotTelegramClient.CustomComands.CommandVarians.ScreenViewArgs
{
	public struct ViewData
	{
        public static readonly ViewData Empty = new ViewData();

        public ViewData(string stream, Point p1, Point p2)
        {
            data = stream;
            start = p1;
            end = p2;
        }
        public string data;
		public Point start;
		public Point end;

        public static bool operator ==(ViewData arg1, ViewData arg2)
        {
            if(arg1.data != arg2.data) return false;
            if(arg1.start != arg2.start) return false;
            if(arg1.end != arg2.end) return false;

            return true;
        }
		public static bool operator !=(ViewData arg1, ViewData arg2)
		{
			if (arg1.data == arg2.data) return false;
			if (arg1.start == arg2.start) return false;
			if (arg1.end == arg2.end) return false;

			return true;
		}
	}
}
