using Newtonsoft.Json;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.CustomComands.CommandVarians
{
	public class CommandCollectionPreferance : BasePreferance
	{
		[JsonConstructor]
		public CommandCollectionPreferance(string fileName, string extension) : base(fileName, extension) { }
		public CommandCollectionPreferance(string fileName, ExtensionType extension) : base(fileName, extension) { }

		public override FieldController CreateFieldController()
		{
			#region BuildTypeFields
			var field_Name = new FieldText("Name", "Name") { startText = Name };
			#endregion

			fieldController = new FieldController(ScreenSettings.FieldSettings, new BaseField[]
			{
				field_Name,
			});

			return fieldController;
		}

		public override async Task<bool> Execute()
		{
			return true;
		}

		public override void SavePreferance()
		{
			Name = (string)fieldController.valuePairs["Name"];

			base.SavePreferance();
		}
	}
}
