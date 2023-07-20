using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProBotTelegramClient.FormControler.Main.ErrorLable;
using FileSystemTree;
using ProBotTelegramClient.CustomComands.CommandVarians.ClipboardArgs;
using ProBotTelegramClient.CustomComands.CommandVarians.CommandArgs;
using ProBotTelegramWinForm;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ProBotTelegramClient.CustomComands.CommandVarians.ClipboardArgs.ClipPasteSettings;
using ProBotTelegramClient.CustomComands;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand.TypeButtons
{
	public class TypeClipPaste : TypeCommand
	{
		public TypeClipPaste(Control parent) : base(parent)
		{
			Text = "ClipPaste";
		}

		public override void Preferance()
		{
			ScreenSettings settings = new ScreenSettings()
			{
				Padding = new Padding(20),
				IntervalY = 10
			};

			#region BuildTypeFields

			var field_Text = new FieldText("Data", "Text") { IsStatic = false };

			var field_DelAfterUse = new FieldBoolean("DelAfterUse", "Del after use") { IsStatic = false };
			var field_FileReadType = new FieldComboBox("FileReadType", "Read type", typeof(ClipPasteFileReadType)) { BaseFields = new BaseField[]
			{
				field_DelAfterUse,
			} };
			field_FileReadType.OnShow = () =>
			{
				var comboBox = field_FileReadType.comboBox;
				ClipPasteFileReadType type = Enum.Parse<ClipPasteFileReadType>((string)comboBox.Items[comboBox.SelectedIndex]);
				switch (type)
				{
					case ClipPasteFileReadType.InOrder:
						field_DelAfterUse.Activate();
						break;
					case ClipPasteFileReadType.ByValue:
						field_DelAfterUse.Hide();
						break;
					default:
						break;
				}
			};
			field_FileReadType.comboBox.SelectedValueChanged += (s, e) =>
			{
				var comboBox = field_FileReadType.comboBox;
				ClipPasteFileReadType type = Enum.Parse<ClipPasteFileReadType>((string)comboBox.Items[comboBox.SelectedIndex]);
				switch (type)
				{
					case ClipPasteFileReadType.InOrder:
						field_DelAfterUse.Activate();
						break;
					case ClipPasteFileReadType.ByValue:
						field_DelAfterUse.Hide();
						break;
					default:
						break;
				}
			};

			var field_File = new FieldDrop("Data", "File", (f) =>
			{
				if (string.IsNullOrEmpty(f.textBox.Text))
				{
					ErrorBox.Message("File can not be empty");
					return false;
				}
				return true;
			}) 
			{ IsStatic = false, BaseFields = new BaseField[]
			{
				field_FileReadType,
			}};

			var field_PasteType = new FieldComboBox("PasteType", "Paste type", typeof(PasteType), 1) { IsStatic = false, BaseFields = new BaseField[]
			{
				field_Text,
				field_File,
			} };
			field_PasteType.OnShow = () => 
			{
				var comboBox = field_PasteType.comboBox;
				PasteType type = Enum.Parse<PasteType>((string)comboBox.Items[comboBox.SelectedIndex]);
				switch (type)
				{
					case PasteType.Text:
						field_PasteType.ActivateNonStatic(field_Text);
						break;
					case PasteType.File:
						field_PasteType.ActivateNonStatic(field_File);
						break;
				}
			};
			field_PasteType.comboBox.SelectedValueChanged += (sender, e) => 
			{
				var comboBox = field_PasteType.comboBox;
				PasteType type = Enum.Parse<PasteType>((string)comboBox.Items[comboBox.SelectedIndex]);
				switch (type)
				{
					case PasteType.Text:
						field_PasteType.ActivateNonStatic(field_Text);
						break;
					case PasteType.File:
						field_PasteType.ActivateNonStatic(field_File);
						break;
				}
			};

			var field_PasteImmidiately = new FieldBoolean("PasteImmidiately", "Paste at once");
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
				field_PasteImmidiately,
				field_PasteType,
			});
		}

		public override void Result()
		{
			var dictionary = fieldController.valuePairs;

			string fileName = (string)dictionary["FileName"];
			string name = (string)dictionary["Name"];
			string extension = (string)dictionary["Extension"];
			string data = (string)dictionary["Data"];
			bool pasteImmidiately = (bool)dictionary["PasteImmidiately"];
			PasteType pasteType = Enum.Parse<PasteType>((string)dictionary["PasteType"]);

			ClipPasteFileReadType fileReadType = 
				dictionary.ContainsKey("FileReadType")? Enum.Parse<ClipPasteFileReadType>((string)dictionary["FileReadType"]) : default;
			bool delAfterUse = 
				dictionary.ContainsKey("DelAfterUse") ? (bool)dictionary["DelAfterUse"] : default;

			var preferance = new ClipPastePreferance(fileName, "." + extension)
			{
				Name = string.IsNullOrEmpty(name) ? fileName : name,
				Type = Text,
				PasteImmidiately = pasteImmidiately,
				PasteType = pasteType,
				Data = data,
				FileReadType = fileReadType,
				DelAfterUse = delAfterUse,
			};

			var command = new ClipPaste(preferance);

			MainForm.Instance.keyAssignmentForm.AddCommand(command);
		}
	}
}
