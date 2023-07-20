using Newtonsoft.Json;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.CustomComands.CommandsSettings.ServiceSettings;
using ProBotTelegramClient.FormControler.Forms.FormQuestion;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenPointArgs;
using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenSettingsArgs;
using ProBotTelegramClient.FormControler.Main.Scripts.SelectorRuls;
using ProBotTelegramClient.Inputs.InputHook;
using ProBotTelegramClient.Simulator.Base;
using ProBotTelegramWinForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSimulator.Inputs.InputHook;

namespace ProBotTelegramClient.CustomComands.CommandVarians.ClipboardArgs
{
    public class ClipPaste : BaseCommand, IScreenClient, ICommandNet
	{
		[JsonConstructor]
		public ClipPaste(string preferanceSave) : base(preferanceSave) { }
		public ClipPaste(BasePreferance preferance) : base(preferance) { }

		[JsonIgnore]
		public string ViewerName { get; set; }
		[JsonIgnore]
		public SelectorSettings SelectorSettings { get; set; }

		public event Action<MainPoint> OnGetMainPoint;

		public void AddToNet() => ICommandNet.CommandNet.Add(Preferance.FullName, this);
		protected override void BaseSettings()
		{
			base.BaseSettings();

			ViewerName = Preferance.Name;

			if (ServiceController is null)
			{
				ServiceController = new ServiceController(this, new ServicePreferance[]
				{
					new CyclePreferance(Preferance.Name, new CycleType[]
					{
						CycleType.Amount,
						CycleType.Maximum,
					}),
				});
			}

			AddToNet();

			Preferance.BaseSettings();
		}

		public override Control DrawDisplay(int width, int height, int x, int y)
		{
			ScreenSettings settings = ScreenSettings.CommandSettings;

			Panel panel = base.DrawDisplay(width, height, x, y) as Panel;
			panel.DoubleClick += (s, e) =>
			{
				MainScreenController.Show(this, new ScreenSettings
				{
					objHeight = 30,
					Padding = new Padding(20),
				});
			};

			Button useButton = new Button();
			useButton.Text = "Use";
			useButton.Width = 130;
			useButton.Height = 20;
			useButton.Left = settings.Padding.Left;
			useButton.Top = settings.Padding.Top;
			useButton.Click += (s, e) =>
			{
				MainScreenController.AddMainPoint(this);
			};
			useButton.FlatStyle = FlatStyle.System;

			foreach (Control item in panel.Controls)
			{
				switch (item.Name)
				{
					case "Delete":
						{
							item.Click += (s, e) =>
							{
								var set = new QuestionForm($"Delete this command: {Preferance.Name}");

								List<IAnswer> answer = new List<IAnswer>()
								{
									new Answer<Form>("Yes", (f) =>
									{
										panel.Dispose();
										MainForm.Instance.keyAssignmentForm.RemoveCommand(this);
										f.Close();
									}, set),
									new Answer<Form>("No", (f) => { f.Close(); }, set),
								};

								set.Answers = answer;
								set.Open();
							};
						}
						break;
					case "Settings":
						{

						}
						break;
					case "Services":
						{

						}
						break;
				}
			}

			panel.Controls.Add(useButton);

			return panel;
		}

		public void View(Panel screen)
		{
			ScreenSettings settings = MainScreenController.Settings;
			Panel? subSelect = null;

			TextBox textBox = new TextBox();
			textBox.Text = LoadData();
			textBox.Multiline = true;
			textBox.Dock = DockStyle.Fill;
			TextBoxSettings(textBox);

			screen.Controls.Add(textBox);
		}
		public virtual string LoadData()
		{
			string result = ((ClipPastePreferance)Preferance).LoadData();

			return result;
		}

		public async virtual Task<bool> Execute()
		{
			if (!MainForm.CancelingToken.Value) return false;

			bool result = await Preferance.Execute();

			return result;
		}
		protected virtual void TextBoxSettings(TextBox textBox)
		{
			((ClipPastePreferance)Preferance).TextBoxSettings(textBox);
		}

		public MainPoint GetMainPoint()
		{
			var point = new MainPoint(Preferance, async () => { return await MainExecute(Execute); });
			OnGetMainPoint?.Invoke(point);
			return point;
		}
	}
}
