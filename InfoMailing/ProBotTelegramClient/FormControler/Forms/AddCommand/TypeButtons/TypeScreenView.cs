using ProBotTelegramWinForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProBotTelegramClient.CustomComands.CommandVarians;
using ProBotTelegramClient.CustomComands.CommandVarians.ScreenViewArgs;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using FileSystemTree;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using System.Diagnostics;
using ProBotTelegramClient.FormControler.Main.ErrorLable;
using ProBotTelegramClient.CustomComands.CommandsSettings.PostActions;
using ProBotTelegramClient.CustomComands.CommandVarians.ClipboardArgs;
using ProBotTelegramClient.CustomComands.CommandVarians.ScreenViewArgs.ScreenViewSettings;
using ProBotTelegramClient.CustomComands;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand.TypeButtons
{
	public class TypeScreenView : TypeCommand
	{
		public TypeScreenView(Control parent) : base(parent)
		{
			Text = "View";
		}

		public override void Preferance()
		{
			ScreenSettings settings = new ScreenSettings()
			{
				Padding = new Padding(20),
				IntervalY = 10
			};

			#region BuildTypeFields
			
			var field_XPos = new FieldIntager("XPos", "Position X");
			var field_YPos = new FieldIntager("YPos", "Position Y");
			var field_ActionPosType = new FieldComboBox("ActionPosType", "Action position", typeof(PostActionPosType)) { IsStatic = false, BaseFields = new BaseField[]
			{
				field_XPos,
				field_YPos,
			}};
			field_ActionPosType.OnShow = () => 
			{
				var comboBox = field_ActionPosType.comboBox;
				PostActionPosType type = (PostActionPosType)Enum.Parse(typeof(PostActionPosType), comboBox.Items[comboBox.SelectedIndex].ToString());

				switch (type)
				{
					case PostActionPosType.Center:
						field_XPos.Hide();
						field_YPos.Hide();
						break;
					case PostActionPosType.ChooseFromChenter or PostActionPosType.ChooseOnScreen:
						field_XPos.Activate();
						field_YPos.Activate();
						break;
					default:
						break;
				}
			};
			field_ActionPosType.comboBox.SelectedValueChanged += (s, e) =>
			{
				var comboBox = field_ActionPosType.comboBox;
				PostActionPosType type = (PostActionPosType)Enum.Parse(typeof(PostActionPosType), comboBox.Items[comboBox.SelectedIndex].ToString());

				switch (type)
				{
					case PostActionPosType.Center:
						field_XPos.Hide();
						field_YPos.Hide();
						break;
					case PostActionPosType.ChooseFromChenter or PostActionPosType.ChooseOnScreen:
						field_XPos.Activate();
						field_YPos.Activate();
						break;
					default:
						break;
				}
			};

			var field_ActionType = new FieldComboBox("ActionType", "Action type", typeof(PostActionType));

			var field_PostAction = new FieldBoolean("PostAction", "Post action") { BaseFields = new BaseField[]
			{
				field_ActionType,
				field_ActionPosType,
			}};
			field_PostAction.OnShow = () => 
			{
				bool check = field_PostAction.checkBox.Checked;
				if (check)
				{
					field_ActionType.Activate();
					field_ActionPosType.Activate();
				}
				else
				{
					field_ActionType.Hide();
					field_ActionPosType.Hide();
				}
			};
			field_PostAction.checkBox.CheckedChanged += (s, e) =>
			{
				bool check = field_PostAction.checkBox.Checked;
				if (check)
				{
					field_ActionType.Activate();
					field_ActionPosType.Activate();
				}
				else
				{
					field_ActionType.Hide();
					field_ActionPosType.Hide();
				}
			};

			var field_Search = new FieldComboBox("SearchType", "Search type", typeof(ScreenSimilaritySearchType));
			var field_WorkMode = new FieldComboBox("WorkMode", "Work mode", typeof(ScreenSearchWorkMode)) { IsStatic = false, BaseFields = new BaseField[]
			{
				field_Search,
				field_PostAction,
			}};

			var field_File = new FieldDrop("Data", "Save dir") { IsStatic = false };
			var field_CaptureType = new FieldComboBox("CaptureType", "Capture type", typeof(ScreenCaptureType)) { IsStatic = false, BaseFields = new BaseField[]
			{
				field_File,
			}};
			field_CaptureType.OnShow = () => 
			{
				var comboBox = field_CaptureType.comboBox;
				ScreenCaptureType type = Enum.Parse<ScreenCaptureType>((string)comboBox.Items[comboBox.SelectedIndex]);

				switch (type)
				{
					case ScreenCaptureType.Folder:
						field_File.Activate();
						break;
					case ScreenCaptureType.Telegram:
						field_File.Hide();
						break;
					default:
						break;
				}
			};
			field_CaptureType.comboBox.SelectedValueChanged += (s, e) =>
			{
				var comboBox = field_CaptureType.comboBox;
				ScreenCaptureType type = Enum.Parse<ScreenCaptureType>((string)comboBox.Items[comboBox.SelectedIndex]);

				switch (type)
				{
					case ScreenCaptureType.Folder:
						field_File.Activate();
						break;
					case ScreenCaptureType.Telegram:
						field_File.Hide();
						break;
					default:
						break;
				}
			};

			var field_Mode = new FieldComboBox("Mode", "Mode", typeof(ScreenViewMode)) { BaseFields = new BaseField[]
			{
				field_CaptureType,
				field_WorkMode,
			}};
			field_Mode.OnShow = () => 
			{
				var comboBox = field_Mode.comboBox;
				ScreenViewMode type = Enum.Parse<ScreenViewMode>((string)comboBox.Items[comboBox.SelectedIndex]);

				switch (type)
				{
					case ScreenViewMode.Search:
						field_Mode.ActivateNonStatic(field_WorkMode);
						break;
					case ScreenViewMode.Capture:
						field_Mode.ActivateNonStatic(field_Search);
						break;
					default:
						break;
				}
			};
			field_Mode.comboBox.SelectedValueChanged += (s, e) =>
			{
				var comboBox = field_Mode.comboBox;
				ScreenViewMode type = Enum.Parse<ScreenViewMode>((string)comboBox.Items[comboBox.SelectedIndex]);

				switch (type)
				{
					case ScreenViewMode.Search:
						field_Mode.ActivateNonStatic(field_WorkMode);
						break;
					case ScreenViewMode.Capture:
						field_Mode.ActivateNonStatic(field_CaptureType);
						break;
					default:
						break;
				}
			};

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
				field_Mode,
			});
		}

		public override void Result()
		{
			var dictionary = fieldController.valuePairs;

			string fileName = (string)dictionary["FileName"];
			string name = (string)dictionary["Name"];
			string extension = (string)dictionary["Extension"];
			ScreenViewMode mode = Enum.Parse<ScreenViewMode>((string)dictionary["Mode"]);

			ScreenCaptureType captureType = 
				dictionary.ContainsKey("CaptureType")? Enum.Parse<ScreenCaptureType>((string)dictionary["CaptureType"]) : default;
			ScreenSearchWorkMode workMode = 
				dictionary.ContainsKey("WorkMode") ? Enum.Parse<ScreenSearchWorkMode>((string)dictionary["WorkMode"]) : default;
			ScreenSimilaritySearchType searchType = 
				dictionary.ContainsKey("SearchType") ? Enum.Parse<ScreenSimilaritySearchType>((string)dictionary["SearchType"]) : default;
			string path = 
				dictionary.ContainsKey("Data") ? (string)fieldController.valuePairs["Data"] : "";
			bool postAction = 
				dictionary.ContainsKey("PostAction") ? (bool)fieldController.valuePairs["PostAction"] : default;
			PostActionType postActionType = 
				dictionary.ContainsKey("ActionType") ? Enum.Parse<PostActionType>((string)dictionary["ActionType"]) : default;
			PostActionPosType actionPosType = 
				dictionary.ContainsKey("ActionPosType") ? Enum.Parse<PostActionPosType>((string)dictionary["ActionPosType"]) : default;
			int xPos = 
				dictionary.ContainsKey("XPos") ? (int)dictionary["XPos"] : default;
			int yPos = 
				dictionary.ContainsKey("YPos") ? (int)dictionary["YPos"] : default;

			BasePreferance preferance = new ScreenViewPreferance(fileName, "." + extension)
			{
				Name = string.IsNullOrEmpty(name) ? fileName : name,
				Type = Text,
				ScreenViewMode = mode,
				ScreenCaptureType = captureType,
				WorkMode = workMode,
				SearchType = searchType,
				DataPath = path,
				HavePostAction = postAction,
				ActionType = postActionType,
				ActionPosType = actionPosType,
				XPos = xPos,
				YPos = yPos,
			};

			var command = new ScreenView(preferance); 

			MainForm.Instance.keyAssignmentForm.AddCommand(command);
		}
	}
}
