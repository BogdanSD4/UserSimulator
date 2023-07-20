using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Point = System.Drawing.Point;

namespace EyesSimulator.PhotoEditor.Settings
{
    public struct Box
    {
        public Box(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;

            if (X1 > X2)
            {
                int num = X1;
                X1 = X2;
                X2 = num;
            }
            if (Y1 > Y2)
            {
                int num = Y1;
                Y1 = Y2;
                Y2 = num;
            }

            Width = Math.Abs(X1 - X2);
            Height = Math.Abs(Y1 - Y2);
        }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public bool Contains(Point point)
        {
            if (point.X < X1 && point.X > X2)
            {
                if (point.Y < Y1 && point.X > Y2) return true;
            }
            return false;
        }
        public bool ContainsH(Point point)
        {
            if (point.Y < Y1 && point.X > Y2) return true;
            return false;
        }
        public bool ContainsV(Point point)
        {
            if (point.X < X1 && point.X > X2) return true;
            return false;
        }
    }
}
