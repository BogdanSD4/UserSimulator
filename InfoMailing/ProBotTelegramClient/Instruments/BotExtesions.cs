using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.Instruments
{
	public static class BotExtesions
	{
		public static string ArrayToString (this IEnumerable<object> objects)
		{
			StringBuilder sb = new StringBuilder ();
			foreach (object obj in objects)
			{
				sb.Append ($"{obj.ToString()}{Environment.NewLine}");
			}
			return sb.ToString();
		}
	}
}
