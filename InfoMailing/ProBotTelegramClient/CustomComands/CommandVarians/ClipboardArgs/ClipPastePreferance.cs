using Newtonsoft.Json;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.CustomComands.CommandVarians.ClipboardArgs.ClipPasteSettings;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields;
using ProBotTelegramClient.FormControler.Forms.CommandSettingsMenu;
using ProBotTelegramClient.FormControler.Main.ErrorLable;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramClient.Instruments;
using ProBotTelegramClient.Simulator.Base;
using ProBotTelegramWinForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.CustomComands.CommandVarians.ClipboardArgs
{
	public class ClipPastePreferance : BasePreferance
	{
		[JsonConstructor]
		public ClipPastePreferance(string fileName, string extension) : base(fileName, extension) { }
		public ClipPastePreferance(string fileName, ExtensionType extension) : base(fileName, extension) { }

		public bool PasteImmidiately { get; set; }
		public PasteType PasteType { get; set; }
		public string Data { get; set; }
		public ClipPasteFileReadType FileReadType { get; set; }
		public bool DelAfterUse { get; set; }

		public string[] Pastes;
		public int lineNum;

		public override async Task<bool> Execute()
		{
			switch (PasteType)
			{
				case PasteType.Text:
					{
						Clipboard.SetText(Data);

						await Task.Delay(200);
					}
					break;
				case PasteType.File:
					{
						switch (FileReadType)
						{
							case ClipPasteFileReadType.InOrder:
								{
									if (DelAfterUse)
									{
										string[] lines = File.ReadAllLines(Data);
										if (lines.Length == 0) return false;

										Clipboard.SetText(lines[0]);
										lines = lines[1..];

										File.WriteAllText(Data, lines.ArrayToString());
									}
									else
									{
										if (lineNum >= Pastes.Length) return false;
										Clipboard.SetText(Pastes[lineNum++]);
									}
								}
								break;
							case ClipPasteFileReadType.ByValue:
								{
									string res = File.ReadAllText(Data);
									Clipboard.SetText(res);
								}
								break;
							default:
								break;
						}
					}
					break;
				default:
					break;
			}

			if (PasteImmidiately) InputSimulatorBuilder.Paste();
			await Task.Delay(500);

			return true;
		}

		public override FieldController CreateFieldController()
		{
			#region BuildTypeFields

			var field_Text = new FieldText("Data", "Text") { IsStatic = false, startText = PasteType is PasteType.Text? Data : ""};

			var field_DelAfterUse = new FieldBoolean("DelAfterUse", "Del after use") { IsStatic = false, startValue = DelAfterUse };
			var field_FileReadType = new FieldComboBox("FileReadType", "Read type", typeof(ClipPasteFileReadType), (int)FileReadType)
			{
				BaseFields = new BaseField[]
			{
				field_DelAfterUse,
			}
			};
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
			{
				startText = PasteType is PasteType.File ? Data : "",
				IsStatic = false,
				BaseFields = new BaseField[]
			{
				field_FileReadType,
			}};

			var field_PasteType = new FieldComboBox("PasteType", "Paste type", typeof(PasteType), (int)PasteType )
			{
				IsStatic = false,
				BaseFields = new BaseField[]
			{
				field_Text,
				field_File,
			}
			};
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

			var field_PasteImmidiately = new FieldBoolean("PasteImmidiately", "Paste at once") { startValue = PasteImmidiately };
			var field_Name = new FieldText("Name", "Name") { startText = Name };
			#endregion

			fieldController = new FieldController(ScreenSettings.FieldSettings, new BaseField[]
			{
				field_Name,
				field_PasteImmidiately,
				field_PasteType,
			});

			return fieldController;
		}

		public override void SavePreferance()
		{
			var dictionary = fieldController.valuePairs;

			Name = (string)dictionary["Name"];
			Data = (string)dictionary["Data"];
			PasteImmidiately = (bool)dictionary["PasteImmidiately"];
			PasteType = Enum.Parse<PasteType>((string)dictionary["PasteType"]);
			FileReadType = dictionary.ContainsKey("FileReadType") ? Enum.Parse<ClipPasteFileReadType>((string)dictionary["FileReadType"]) : default;
			DelAfterUse = dictionary.ContainsKey("DelAfterUse") ? (bool)dictionary["DelAfterUse"] : default;

			base.SavePreferance();
		}
		public override void BaseSettings()
		{
			switch (PasteType)
			{
				case PasteType.Text:
					break;
				case PasteType.File:
					{
						if (DelAfterUse)
						{

						}
						else
						{
							MainForm.Instance.OnImitateStart += () =>
							{
								List<string> lines = new List<string>();
								using (var stream = new StreamReader(Data))
								{
									while (!stream.EndOfStream)
									{
										lines.Add(stream.ReadLine());
									}

									Pastes = lines.ToArray();
									lineNum = 0;
								}
							};
						}
					}
					break;
				default:
					break;
			}
		}

		public virtual string LoadData()
		{
			switch (PasteType)
			{
				case PasteType.Text: return Data;
				case PasteType.File: return File.ReadAllText(Data);
				default: throw new NotImplementedException();
			}
		}
		public virtual void TextBoxSettings(TextBox textBox)
		{
			switch (PasteType)
			{
				case PasteType.Text:
					{
						textBox.TextChanged += (s, e) =>
						{
							Data = textBox.Text;
						};
					}
					break;
				case PasteType.File:
					{
						textBox.ReadOnly = true;
					}
					break;
				default:
					break;
			}
			
		}
	}
}
