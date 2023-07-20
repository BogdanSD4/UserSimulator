using ProBotTelegramClient.FormControler.Main.Scripts.SelectorRuls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace ProBotTelegramClient.FormControler.Main.MainScreen
{
	public class ScreenSettings
	{
		public Selector Selector { get; set; }
		public Padding Padding { get; set; } = new Padding(5, 5, 25, 5);
		public int IntervalY { get; set; } = 2;
		public int IntervalX { get; set; }
		public int MaxWidth { get; set; } = int.MaxValue;
		public int MaxHeight { get; set; } = int.MaxValue;
		public int objWidth { get; set; }
		public int objHeight { get; set; }

		public static ScreenSettings MainScreenSettings { get
			{
				return new ScreenSettings()
				{
					objHeight = 30,
					Selector = new Selector(new SelectorSettings 
					{
						NoneEffect = SystemColors.Control,
						OverColor = Color.LightGray,
						SelectColor = SystemColors.Info,
					}),
				};
			} 
		}
		public static ScreenSettings CommandSettings { get 
			{
				return new ScreenSettings()
				{
					Padding = new Padding(10),
					IntervalX = 10,
					IntervalY = 20,
				};
			}
		}
		public static ScreenSettings ServiceMenu
		{
			get
			{
				return new ScreenSettings()
				{
					Padding = new Padding(20,30,20,20),
					IntervalY = 20,
					IntervalX = 20,
				};
			}
		}
		public static ScreenSettings FieldSettings { get 
			{
				return new ScreenSettings()
				{
					Padding = new Padding(20),
					IntervalY = 10
				};
			}
		}
	}
}
