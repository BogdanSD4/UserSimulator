using Newtonsoft.Json;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir;
using ProBotTelegramClient.FormControler.Forms.CommandSettingsMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.CustomComands.CommandsSettings.ServiceSettings
{
	public abstract class ServicePreferance : Preferance
	{
        public ServicePreferance()
        {
			BaseSettings();
        }

		[JsonIgnore]
		public string ServiceType { get; set; }

		public virtual void BaseSettings() { }
		public virtual Control GetComponent(Form? parent = null)
		{
			Panel panel = new Panel();
			panel.Width = 150;
			panel.Height = 50;
			panel.BorderStyle = BorderStyle.FixedSingle;
			panel.Padding = new Padding(3);

			return panel;
		}
		public virtual string GetJson()
		{
			return JsonConvert.SerializeObject(this);
		}

		public abstract void LoadCommandData(BaseCommand command);
	}
}
