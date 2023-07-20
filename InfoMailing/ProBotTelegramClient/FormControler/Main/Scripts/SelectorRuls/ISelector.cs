using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Main.Scripts.SelectorRuls
{
	public interface ISelector
	{
		public SelectorSettings SelectorSettings { get; set; }
	}
}
