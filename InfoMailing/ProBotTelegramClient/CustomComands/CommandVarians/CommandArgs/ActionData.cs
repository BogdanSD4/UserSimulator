using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.CustomComands.CommandVarians.CommandArgs
{
	public struct ActionData
	{
		public ActionData(string name, string data) 
		{
			Name = name;
			Data = data;
		}
		public string Name; 
		public string Data;
	}
}
