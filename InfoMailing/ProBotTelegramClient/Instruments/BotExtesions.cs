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

		public static int GetIndex<TValue>(this IEnumerable<TValue> values, TValue value)
		{
			List<TValue> list = values.ToList();
			for (int i = 0; i < list.Count; i++)
			{
				if ((object)list[i] == (object)value)
				{
					return i;
				}
			}

			return -1;
		}
	}
}
