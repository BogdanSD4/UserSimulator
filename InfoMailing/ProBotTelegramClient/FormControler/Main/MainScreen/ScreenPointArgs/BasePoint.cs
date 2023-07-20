using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Main.MainScreen.ScreenPointArgs
{
	public abstract class BasePoint
	{
		[JsonIgnore]
		public Panel Body { get; set; }
		[JsonIgnore]
		public Action Remove { get; set; }

		public abstract void Delete(); 
	}
}
