using Instruments;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using ProBotTelegramClient.CustomComands.CommandVarians;
using ProBotTelegramClient.CustomComands.CommandVarians.ClipboardArgs;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields;
using ProBotTelegramClient.FormControler.Forms.CommandSettingsMenu;
using ProBotTelegramClient.FormControler.Forms.ServiceMenu;
using ProBotTelegramClient.FormControler.Main.ErrorLable;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenPointArgs;
using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenSettingsArgs;
using ProBotTelegramClient.FormControler.Main.Scripts.SelectorRuls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;

namespace ProBotTelegramClient.CustomComands.CommandsSettings.ServiceSettings
{
    public class CyclePreferance : ServicePreferance, IScreenField, ISelector
	{
		[JsonConstructor]
		public CyclePreferance()
		{
			ServiceType = "Cycle";
		}

        public CyclePreferance(string name, IEnumerable<CycleType> types)
        {
			ServiceType = "Cycle";
			ViewerName = $"{ServiceType}({name})";
			allowedTypes = types;
		}

        public CycleSettings Settings { get; set; }
		public IEnumerable<CycleType> allowedTypes { get; set; }
		public string jsonPoints { get; set; }

		[JsonIgnore]
		public string ViewerName { get; set; }
		[JsonIgnore]
		public MainList Points { get; set; }
		[JsonIgnore]
		public SelectorSettings SelectorSettings { get; set; }

		public override void BaseSettings()
		{
			SelectorSettings = SelectorSettings.Default;

			Points = new MainList();
			CommandCollection.AfterLoadEvent += () =>
			{
				if (string.IsNullOrEmpty(jsonPoints))
				{
					Points = new MainList();
				}
				else
				{
					Points = JsonConvert.DeserializeObject<MainList>(jsonPoints);
					jsonPoints = null;
				}
			};
		}
		
		public override Control GetComponent(Form? parent = null)
		{
			Panel panel = base.GetComponent() as Panel;
			
			if(Settings is null)
			{
				Button button = new Button();
				button.Text = ServiceType;
				button.Dock = DockStyle.Fill;
				button.Font = new Font(button.Font.FontFamily, 14);
				button.Click += (s, e) => 
				{
					new CommandSettingsForm(this, ViewerName) { Previous = parent };

					parent?.Close();
				};

				panel.Controls.Add(button);
			}
			else
			{
				int buttonWidth = panel.Width * 70 / 100;
				int settingsWidth = panel.Width - buttonWidth;

				Button button = new Button();
				button.Text = ServiceType;
				button.Width = buttonWidth;
				button.Dock = DockStyle.Left;
				button.Font = new Font(button.Font.FontFamily, 14);
				button.Click += (s, e) =>
				{
					MainScreenController.Show(this, new ScreenSettings
					{
						objHeight = 30,
						Selector = new Selector(SelectorSettings)
					});
					parent?.Close();
				};

				Button settings = new Button();
				settings.Text = "";
				settings.Width = settingsWidth;
				settings.BackgroundImage = Image.FromFile("C:\\Users\\dokto\\OneDrive\\Рабочий стол\\TelegramBotKPI\\InfoMailing\\ProBotTelegramClient\\Images\\Icon\\free-icon-cogwheel-44427.png");
				settings.BackgroundImageLayout = ImageLayout.Zoom;
				settings.Dock = DockStyle.Right;
				settings.Font = new Font(button.Font.FontFamily, 14);
				settings.Click += (s, e) =>
				{
					new CommandSettingsForm(this, ViewerName) { Previous = parent };

					parent?.Close();
				};

				panel.Controls.Add(settings);
				panel.Controls.Add(button);
			}

			return panel;
		}

		public override FieldController CreateFieldController()
		{
			var list = allowedTypes.Select(x => x.ToString());
			int startIndex = Settings is null ? 0 : list.GetIndex(Settings.Type.ToString());
			var field_Type = new FieldComboBox("CycleType", "Cycle type", list, startIndex);
			var field_Amount = new FieldIntager("Amount", "Cycle amount", (t) =>
			{
				int num = 0;
				if (!int.TryParse(t.textBox.Text, out num) || num < 1 || num > 1000)
				{
					ErrorBox.Message("Invalid number (1 - 1000)");
					var color = t.label.ForeColor;
					t.label.ForeColor = Color.Red;
					t.textBox.Click += (s, e) => { t.label.ForeColor = color; };
					return false;
				}

				return true;
			})
			{
				startText = Settings is null ? "" : Settings.Amount == 0 ? "" : Settings.Amount.ToString(),
			};
			field_Type.comboBox.SelectedValueChanged += (s, e) =>
			{
				var comboBox = field_Type.comboBox;
				CycleType type = (CycleType)Enum.Parse(typeof(CycleType), comboBox.Items[comboBox.SelectedIndex].ToString());
				switch (type)
				{
					case CycleType.Amount:
						field_Amount.Activate();
						break;
					case CycleType.Maximum:
						field_Amount.Hide();
						break;
				}
			};

			var controller = new FieldController(ScreenSettings.CommandSettings, new BaseField[]
			{
				field_Type,
				field_Amount,
			});

			fieldController = controller;

			return controller;
		}
		public override void SavePreferance()
		{
			Settings = new CycleSettings();

			string value = (string)fieldController.valuePairs["CycleType"];
			CycleType type = (CycleType)Enum.Parse(typeof(CycleType), value);
			Settings.Type = type;

			switch (type)
			{
				case CycleType.Amount:
					int num = int.Parse((string)fieldController.valuePairs["Amount"]);
					Settings.Amount = num;
					ServiceMenuForm.CurrentCommand.OnExecute = OnTypeAmount;
					break;
				case CycleType.Maximum:
					ServiceMenuForm.CurrentCommand.OnExecute = OnTypeMaximum;
					break;
			}
		}
		public override string GetJson()
		{
			jsonPoints = JsonConvert.SerializeObject(Points);
			return base.GetJson();
		}
		public override void LoadCommandData(BaseCommand command)
		{
			if(command is IScreenClient screenClient)
			{
				screenClient.OnGetMainPoint += (m) => 
				{
					if(Points is null) return;
					m.SubPoints = new MainList(Points.Items);
				};
			}

			if(Settings is not null)
			{
				command.OnExecute += Settings.Type switch
				{
					CycleType.Amount => OnTypeAmount,
					CycleType.Maximum => OnTypeMaximum,
					_ => null,
				};
			}
		}

		public void AddCommand(MainPoint point)
		{
			Points.Add(point);
		}
		public void View(Panel screen)
		{
			MainScreenController.DrawUI(Points.Items);
		}

		public virtual async Task<bool> OnTypeAmount(Func<Task<bool>> func)
		{
			int count = Settings.Amount;

			while (count-- > 0)
			{
				await func.Invoke();
				foreach (var item in Points.Items)
				{
					await item.Execute.Invoke();
				}
			}

			return true;
		} 
		public virtual async Task<bool> OnTypeMaximum(Func<Task<bool>> func)
		{
			while (await func.Invoke())
			{
				foreach (var item in Points.Items)
				{
					await item.Execute.Invoke();
				}
			}

			return true;
		}
	}
}
