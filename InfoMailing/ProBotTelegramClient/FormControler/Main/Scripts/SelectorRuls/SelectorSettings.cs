using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace ProBotTelegramClient.FormControler.Main.Scripts.SelectorRuls
{
	public struct SelectorSettings
	{
		public Color NoneEffect { get; set; }
		public Color OverColor { get; set; }
		public Color SelectColor { get; set; }

		public static SelectorSettings Default { get 
			{
				return new SelectorSettings
				{
					NoneEffect = SystemColors.Control,
					OverColor = Color.LightGray,
					SelectColor = SystemColors.Info,
				};
			} 
		}
	}
}
