using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Main.Scripts
{
	public class ImmitateCancelingToken
	{
		public bool Value { get; set; }

		public static ImmitateCancelingToken Standart { get
			{
				return new ImmitateCancelingToken {  Value = true };
			} 
		}
	}
}
