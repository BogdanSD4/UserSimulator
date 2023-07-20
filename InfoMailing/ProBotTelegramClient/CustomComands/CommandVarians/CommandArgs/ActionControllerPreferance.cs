using Newtonsoft.Json;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramWinForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.CustomComands.CommandVarians.CommandArgs
{
	public class ActionControllerPreferance : BasePreferance
	{
		[JsonConstructor]
		public ActionControllerPreferance(string fileName, string extension) : base(fileName, extension) { }
		public ActionControllerPreferance(string fileName, ExtensionType extension) : base(fileName, extension) { }

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
			var command = (ActionCollection)Command;

			foreach (var point in command.Points.Items)
			{
				if (!MainForm.CancelingToken.Value) return false;

				await point.Execute();
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
