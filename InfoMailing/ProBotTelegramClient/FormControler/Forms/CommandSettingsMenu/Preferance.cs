using Newtonsoft.Json;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Forms.CommandSettingsMenu
{
	public abstract class Preferance
	{
		[JsonIgnore]
		public FieldController fieldController { get; set; }

		public abstract FieldController CreateFieldController();
		public abstract void SavePreferance();
	}
}
