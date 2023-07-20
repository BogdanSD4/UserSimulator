using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenPointArgs;
using ProBotTelegramClient.FormControler.Main.Scripts.SelectorRuls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Main.MainScreen
{
    public interface IScreenViewer
	{
		public string ViewerName { get; set; }
		public abstract void View(Panel screen);
	}
}
