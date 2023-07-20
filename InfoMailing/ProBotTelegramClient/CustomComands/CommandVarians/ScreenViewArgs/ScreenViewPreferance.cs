using EyesSimulator.PhotoEditor.Settings;
using EyesSimulator.PhotoEditor;
using OpenCvSharp;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.CustomComands.CommandsSettings.PostActions;
using ProBotTelegramClient.CustomComands.CommandVarians.ScreenViewArgs.ScreenViewSettings;
using ProBotTelegramClient.Simulator.Base;
using ProBotTelegramWinForm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using FileSystemTree;
using Newtonsoft.Json;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramClient.FormControler.Main.ErrorLable;

namespace ProBotTelegramClient.CustomComands.CommandVarians.ScreenViewArgs
{
	public class ScreenViewPreferance : BasePreferance
	{
		[JsonConstructor]
		public ScreenViewPreferance(string fileName, string extension) : base(fileName, extension) { }
		public ScreenViewPreferance(string fileName, ExtensionType extension) : base(fileName, extension) { }

		public ScreenViewMode ScreenViewMode { get; set; }
		public ScreenCaptureType ScreenCaptureType { get; set; }
		public ScreenSearchWorkMode WorkMode { get; set; }
		public ScreenSimilaritySearchType SearchType { get; set; }

		public string DataPath { get; set; }
		public bool HavePostAction { get; set; }
		public PostActionType ActionType { get; set; }
		public PostActionPosType ActionPosType { get; set; }
		public int XPos { get; set; }
		public int YPos { get; set; }

		public override FieldController CreateFieldController()
		{
			#region BuildTypeFields

			var field_XPos = new FieldIntager("XPos", "Position X") { startText = XPos.ToString()};
			var field_YPos = new FieldIntager("YPos", "Position Y") { startText = YPos.ToString()};
			var field_ActionPosType = new FieldComboBox("ActionPosType", "Action position", typeof(PostActionPosType), (int)ActionPosType)
			{
				IsStatic = false,
				BaseFields = new BaseField[]
			{
				field_XPos,
				field_YPos,
			}
			};
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

			var field_ActionType = new FieldComboBox("ActionType", "Action type", typeof(PostActionType), (int)ActionType);

			var field_PostAction = new FieldBoolean("PostAction", "Post action")
			{
				startValue = HavePostAction,
				BaseFields = new BaseField[]
			{
				field_ActionType,
				field_ActionPosType,
			}
			};
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

			var field_Search = new FieldComboBox("SearchType", "Search type", typeof(ScreenSimilaritySearchType), (int)SearchType);
			var field_WorkMode = new FieldComboBox("WorkMode", "Work mode", typeof(ScreenSearchWorkMode), (int)WorkMode)
			{
				IsStatic = false,
				BaseFields = new BaseField[]
			{
				field_Search,
				field_PostAction,
			}
			};

			var field_File = new FieldDrop("Data", "Save dir")
			{
				IsStatic = false,
				startText = ScreenViewMode is ScreenViewMode.Capture ? DataPath : "", 
			};
			var field_CaptureType = new FieldComboBox("CaptureType", "Capture type", typeof(ScreenCaptureType), (int)ScreenCaptureType)
			{
				IsStatic = false,
				BaseFields = new BaseField[]
			{
				field_File,
			}
			};
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

			var field_Mode = new FieldComboBox("Mode", "Mode", typeof(ScreenViewMode), (int)ScreenViewMode)
			{
				BaseFields = new BaseField[]
			{
				field_CaptureType,
				field_WorkMode,
			}
			};
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

			var field_Name = new FieldText("Name", "Name") { startText = Name };
			#endregion

			fieldController = new FieldController(ScreenSettings.FieldSettings, new BaseField[]
			{
				field_Name,
				field_Mode,
			});

			return fieldController;
		}

		public override async Task<bool> Execute()
		{
			var command = (ScreenView)Command;
			Rect position = new Rect();
			bool correct = false;

			switch (ScreenViewMode)
			{
				case ScreenViewMode.Search:
					{
						switch (WorkMode)
						{
							case ScreenSearchWorkMode.WaitSuccess:
								{
									Box box = new Box(command.Data.start.X, command.Data.start.Y, command.Data.end.X, command.Data.end.Y);
									Photo current = new Photo(command.Data.data, box);

									while (!correct)
									{
										if (!MainForm.CancelingToken.Value) return false;

										string data = InputSimulatorBuilder.PrintScreen();
										string target = ViewEdit.CutPhoto(new Photo(data, box));

										correct = ViewEdit.FrameEquals(new Photo(target, box), current);

										await Task.Delay(1000);
									}
									//ClientAnswerRuntime.SendPhoto(path, 393379820);
									break;
								}
							case ScreenSearchWorkMode.Once:
								{
									Box box = new Box(command.Data.start.X, command.Data.start.Y, command.Data.end.X, command.Data.end.Y);
									Photo current = new Photo(command.Data.data, box);

									if (!MainForm.CancelingToken.Value) return false;

									string path = InputSimulatorBuilder.PrintScreen();
									string target;

									switch (SearchType)
									{
										case ScreenSimilaritySearchType.Single:
											target = ViewEdit.CutPhoto(new Photo(path, box));
											correct = ViewEdit.FrameEquals(new Photo(target, box), current);
											break;
										case ScreenSimilaritySearchType.Vertical:
											int y1 = 0;
											int y2 = Screen.PrimaryScreen.Bounds.Height;
											Box verticalBox = new Box(box.X1, y1, box.X2, y2);
											target = ViewEdit.CutPhoto(new Photo(path, verticalBox));
											correct = ViewEdit.FrameEqualsVertical(new Photo(target, verticalBox), current, ref position);
											Debug.WriteLine(position.X);
											Debug.WriteLine(position.Y);
											Debug.WriteLine(position.Width);
											Debug.WriteLine(position.Height);
											break;
										case ScreenSimilaritySearchType.Horizontal:
											break;
										case ScreenSimilaritySearchType.Full:
											break;
										default:
											break;
									}

									Debug.WriteLine(correct);
									await Task.Delay(1000);
									break;
								}
							default:
								break;
						}

						if (HavePostAction && correct)
						{
							switch (ActionType)
							{
								case PostActionType.Click:
									switch (ActionPosType)
									{
										case PostActionPosType.Center:
											break;
										case PostActionPosType.ChooseFromChenter:
											int centerX = position.X + position.Width / 2;
											int centerY = position.Y + position.Height / 2;
											InputSimulatorBuilder.SimulateMouseClick(MouseButtons.Left, centerX - 300, centerY);
											break;
										case PostActionPosType.ChooseOnScreen:
											break;
										default:
											break;
									}
									break;
								case PostActionType.DoubleClick:
									break;
								default:
									break;
							}
						}
					}
					break;
				case ScreenViewMode.Capture:
					{
						switch (ScreenCaptureType)
						{
							case ScreenCaptureType.Folder:
								InputSimulatorBuilder.PrintScreen(DataPath);
								correct = true;
								break;
							case ScreenCaptureType.Telegram:
								break;
							default:
								break;
						}
					}
					break;
				default:
					break;
			}



			return correct;
		}

		public override void SavePreferance()
		{
			var dictionary = fieldController.valuePairs;

			Name = (string)dictionary["Name"];
			ScreenViewMode = Enum.Parse<ScreenViewMode>((string)dictionary["Mode"]);
			ScreenCaptureType = dictionary.ContainsKey("CaptureType") ? Enum.Parse<ScreenCaptureType>((string)dictionary["CaptureType"]) : default;
			WorkMode = dictionary.ContainsKey("WorkMode") ? Enum.Parse<ScreenSearchWorkMode>((string)dictionary["WorkMode"]) : default;
			SearchType = dictionary.ContainsKey("SearchType") ? Enum.Parse<ScreenSimilaritySearchType>((string)dictionary["SearchType"]) : default;
			DataPath = dictionary.ContainsKey("Data") ? (string)fieldController.valuePairs["Data"] : "";
			HavePostAction = dictionary.ContainsKey("PostAction") ? (bool)fieldController.valuePairs["PostAction"] : default;
			ActionType = dictionary.ContainsKey("ActionType") ? Enum.Parse<PostActionType>((string)dictionary["ActionType"]) : default;
			ActionPosType = dictionary.ContainsKey("ActionPosType") ? Enum.Parse<PostActionPosType>((string)dictionary["ActionPosType"]) : default;
			XPos = dictionary.ContainsKey("XPos") ? (int)dictionary["XPos"] : default;
			YPos = dictionary.ContainsKey("YPos") ? (int)dictionary["YPos"] : default;

			base.SavePreferance();
		}
	}
}
