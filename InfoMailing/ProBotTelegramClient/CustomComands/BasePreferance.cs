using Newtonsoft.Json;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.FormControler.Forms.CommandSettingsMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.CustomComands
{
	public abstract class BasePreferance : Preferance
	{
        public BasePreferance(string fileName, string extension)
        {
            Name = fileName;
			FileName = fileName;
			Extension = extension;
        }
		public BasePreferance(string fileName, ExtensionType extension)
		{
			Name = fileName;
			FileName = fileName;
			Extension = extension switch
			{
				ExtensionType.none => "",
				ExtensionType.txt => ".txt",
				ExtensionType.png => ".png",
			};
		}

		public string Type { get; set; }
		public string Name { get; set; }
		public string FileName { get; private set; }
		public string Extension { get; private set; }

		[JsonIgnore]
		public BaseCommand Command { get; set; }
		[JsonIgnore]
		public string FullName { get => $"{FileName}{Extension}"; }
		
		public event Action<BasePreferance> OnPreferanceSaved;

		public virtual void BaseSettings() { }
		public abstract Task<bool> Execute();

		public override void SavePreferance()
		{
			OnPreferanceSaved?.Invoke(this);
		}
	}
}
