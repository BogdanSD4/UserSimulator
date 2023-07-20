using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Main.MainScreen.ScreenPointArgs
{
	public struct MainPointData
	{
        public MainPointData(string name, List<string> data)
        {
            Name = name;
            Datas = data;
        }
        public string Name;
		public List<string> Datas;
	}
}
