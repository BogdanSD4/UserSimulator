using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenPointArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Main.MainScreen.ScreenSettingsArgs
{
	public interface IScreenClient : IScreenViewer
	{
		public event Action<MainPoint> OnGetMainPoint;
		public abstract MainPoint GetMainPoint();
	}
}
