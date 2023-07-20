using System.Diagnostics;
using System.Numerics;
using System.Windows.Forms;

namespace ProBotTelegramClient.Inputs.InputsData
{
    public struct MouseData
    {
        public MouseData(MouseEventArgs key)
        {
            Button = key.Button;
            x1 = key.X;
            y1 = key.Y;
            x2 = y2 = -1;
            time = TimeOnly.FromDateTime(DateTime.Now);
            delta = key.Delta;
        }
        public MouseData(MouseData child, int newDelta, int x ,int y)
        {
			Button = child.Button;
			x1 = x;
			y1 = y;
			time = child.time;
            delta = child.delta + newDelta;
		}
        public MouseData(MouseData child, int x, int y)
        {
            Button = child.Button;
			x1 = child.x1;
			y1 = child.y1;
            x2 = x;
            y2 = y;
            time = child.time;
            delta = child.delta;
		}
        public MouseData()
        {
            Button = MouseButtons.None;
        }

        public MouseButtons Button;
        public int x1;
        public int y1;
        public int x2;
        public int y2;
        public int delta;
        public TimeOnly time;

        public string GetPos()
        {
            if(x2 < 0)
            {
				return $"{x1} | {y1}";
			}
			else
            {
				return $"From: {x2} | {y2} To: {x1} | {y1}";
			}
		}

        public bool IsTimeCorrect(MouseData data)
        {
            if (this != data) return false;

            if ((data.time - time).Milliseconds > 300) return false;
            return true;
        }

        public static bool operator ==(MouseData arg1, MouseData arg2)
        {
            if (arg1.Button != arg2.Button) return false;
            if (arg1.x1 != arg2.x1) return false;
            if (arg1.y1 != arg2.y1) return false;

            return true;
        }
        public static bool operator !=(MouseData arg1, MouseData arg2)
        {
            if (arg1.Button == arg2.Button)
            if (arg1.x1 == arg2.x1)
            if (arg1.y1 == arg2.y1) return false;

            return true;
        }
    }
}
