using ProBotTelegramClient.CustomComands.CommandVarians.CommandArgs;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramWinForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using FileSystemTree;
using ProBotTelegramClient.FormControler.Main.ErrorLable;
using ProBotTelegramClient.CustomComands;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand.TypeButtons
{
	public class TypeSimpleCommand : TypeCommand
	{
        public TypeSimpleCommand(Control parent) : base(parent)
        {
			Text = "Command";
		}

		public override void Preferance()
		{
			ScreenSettings settings = new ScreenSettings()
			{
				Padding = new Padding(20),
				IntervalY = 10
			};

			#region BuildTypeFields
			var field_Extension = new FieldComboBox("Extension", "Extension", typeof(ExtensionType), 1);
			var field_Name = new FieldText("Name", "Name");
			var field_FileName = new FieldText("FileName", "File name", (t) =>
			{
				if (t.textBox.Text == "")
				{
					ErrorBox.Message("File name can not be null");
					return false;
				}

				var comboBox = field_Extension.comboBox;
				string text = comboBox.Items[comboBox.SelectedIndex] as string;
				string extansion = text == "none" ? "" : text;
				if (FileManager.Exist($"{t.textBox.Text}.{extansion}"))
				{
					ErrorBox.Message("This file name was exist");
					return false;
				}

				return true;
			});
			field_FileName.textBox.TextChanged += (s, e) =>
			{
				if (field_Name.textBox.Text == "")
				{
					field_Name.textBox.PlaceholderText = $"{field_FileName.textBox.Text}";
				}
			};
			#endregion

			fieldController = new FieldController(settings, new BaseField[]
			{
				field_FileName,
				field_Name,
				field_Extension,
			});
		}

		public override void Result()
		{
			string fileName = (string)fieldController.valuePairs["FileName"];
			string name = (string)fieldController.valuePairs["Name"];
			string extension = (string)fieldController.valuePairs["Extension"];

			BasePreferance preferance = new CommandPreferance(fileName, "." + extension)
			{
				Name = string.IsNullOrEmpty(name) ? fileName : name,
				Type = Text,
			};

			var command = new SimpleCommand(preferance);
			MainForm.Instance.keyAssignmentForm.AddCommand(command);
		}
	}
}
