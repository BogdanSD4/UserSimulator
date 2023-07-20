using FileSystemTree;
using Newtonsoft.Json;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.FormControler.Forms.FormQuestion;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenPointArgs;
using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenSettingsArgs;
using ProBotTelegramClient.FormControler.Main.Scripts;
using ProBotTelegramClient.FormControler.Main.Scripts.SelectorRuls;
using ProBotTelegramClient.Inputs.InputHook;
using ProBotTelegramClient.Inputs.InputsCompilerDir;
using ProBotTelegramWinForm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UserSimulator.Inputs.InputHook;

namespace ProBotTelegramClient.CustomComands.CommandVarians.CommandArgs
{
    public class ActionCollection : BaseCommand, IScreenField, IScreenClient, ISelector, ICommandNet
	{
		[JsonConstructor]
		public ActionCollection(string preferanceSave) : base(preferanceSave) { }
		public ActionCollection(BasePreferance preferance) : base(preferance) 
		{
			DataStatus = CommandDataStatus.NotFound;
		}

		[JsonIgnore]
		public string ViewerName { get; set; }
		[JsonIgnore]
		public CommandDataStatus DataStatus { get; set; }
		[JsonIgnore]
		public MainList Points { get; set; }
		[JsonIgnore]
		public SelectorSettings SelectorSettings { get; set; }

		public string PointsListSave { get; set; }

		public event Action<MainPoint> OnGetMainPoint;

		public void AddToNet() => ICommandNet.CommandNet.Add(Preferance.FullName, this);
		protected override void BaseSettings()
		{
			base.BaseSettings();

			ViewerName = Preferance.Name;

			Points = new MainList();

			CommandCollection.AfterLoadEvent += () =>
			{
				if (!string.IsNullOrEmpty(PointsListSave))
				{
					Points = JsonConvert.DeserializeObject<MainList>(PointsListSave);
					PointsListSave = string.Empty;
				}
			};

			SelectorSettings = SelectorSettings.Default;

			AddToNet();
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
					Selector = new Selector(SelectorSettings)
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
				if (DataStatus is CommandDataStatus.NotFound) return;
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
			MainScreenController.DrawUI(Points.Items);
		}
		public void Remove(int index)
		{
			throw new NotImplementedException();
		}
		public void AddCommand(MainPoint point)
		{
			Points.Add(point);
		}

		public async Task<bool> Execute()
		{
			if (!MainForm.CancelingToken.Value) return false;

			bool result = await Preferance.Execute();

			return result;
		}

		public MainPoint GetMainPoint()
		{
			var point = new MainPoint(Preferance, async () => { return await MainExecute(Execute); });
			OnGetMainPoint?.Invoke(point);
			return point;
		}
		public override void Load()
		{
			base.Load();

			if (Points is null || Points.Count == 0)
			{
				DataStatus = CommandDataStatus.NotFound;
			}
			else DataStatus = CommandDataStatus.Success;
		}
		public override void PreSaveActions()
		{
			base.PreSaveActions();

			PointsListSave = JsonConvert.SerializeObject(Points);
		}
	}
}
