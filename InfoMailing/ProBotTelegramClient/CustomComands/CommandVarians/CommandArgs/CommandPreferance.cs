using FileSystemTree;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields;
using ProBotTelegramClient.FormControler.Forms.CommandSettingsMenu;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramClient.Inputs.InputsCompilerDir;
using ProBotTelegramWinForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.CustomComands.CommandVarians.CommandArgs
{
	public class CommandPreferance : BasePreferance
	{
		public CommandPreferance(string fileName, string extension) : base(fileName, extension) { }

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
			var data = ((SimpleCommand)Command).Data;

			var inputs = InputIOControler.DownloadInputs(data);

			foreach (var item in inputs)
			{
				if (!MainForm.CancelingToken.Value) return false;

				item.SimulateInput();
				await Task.Delay(1000);
			}

			return true;
		}

		public override void SavePreferance()
		{
			Name = (string)fieldController.valuePairs["Name"];

			base.SavePreferance();
		}
	}
}
