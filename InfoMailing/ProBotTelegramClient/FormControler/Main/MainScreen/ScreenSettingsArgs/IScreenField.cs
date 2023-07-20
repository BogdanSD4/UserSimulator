using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenPointArgs;

namespace ProBotTelegramClient.FormControler.Main.MainScreen
{
    public interface IScreenField : IScreenViewer
	{
		public MainList Points { get; }
		public abstract void AddCommand(MainPoint point);
	}
}
