using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace ProBotTelegramClient.FormControler.Main.TimerDir
{
    public class TimerController
    {
        private static Timer timer;
        private static List<TimerPoint> actions;
        private static int emptyNum;

        public static void Initial(Timer mainTimer)
        {
            timer = mainTimer;
            timer.Interval = 100;
            actions = new List<TimerPoint>();

			timer.Tick += (s, e) =>
			{
                for (int i = 0; i < actions.Count; i++)
                {
					if ((actions[i].Time -= timer.Interval) <= 0)
					{
                        actions[i].Action.Invoke();
                        actions.RemoveAt(i);
					}
				}

                if (actions.Count == 0)
                {
                    timer.Stop();
                }
			};
		}

		public static void SetEvent(string name, int time, Action func)
		{
            foreach (var action in actions)
            {
                if(action.Name == name)
                {
                    action.Time = time;
                    action.Action = func;
                    return;
                }
            }
            actions.Add(new TimerPoint(name, time, func));

			if(!timer.Enabled) timer.Start();
		}
	}
}
