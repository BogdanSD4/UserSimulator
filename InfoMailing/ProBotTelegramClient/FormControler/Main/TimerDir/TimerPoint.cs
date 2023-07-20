using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Main.TimerDir
{
	public class TimerPoint
	{
        public TimerPoint(string name, int time, Action action)
        {
            Name = name;
            Time = time;
            Action = action;
        }
        public string Name;
        public int Time;
		public Action Action;
	}
}
