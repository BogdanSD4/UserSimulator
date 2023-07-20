using EyesSimulator.PhotoEditor;
using EyesSimulator.PhotoEditor.Settings;
using FileSystemTree;
using Google.Apis.Drive.v3.Data;
using Newtonsoft.Json;
using OpenCvSharp;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.FormControler.Forms.FormQuestion;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenPointArgs;
using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenSettingsArgs;
using ProBotTelegramClient.Inputs.InputHook;
using ProBotTelegramClient.Inputs.InputsCompilerDir;
using ProBotTelegramClient.Inputs.InputsData;
using ProBotTelegramClient.Instruments;
using ProBotTelegramClient.Simulator.Base;
using ProBotTelegramWinForm;
using ProBotTelegramWinForm.Inputs;
using System.Diagnostics;
using System.Text;
using UserSimulator.Inputs.InputHook;
using Color = System.Drawing.Color;
using File = System.IO.File;
using Point = System.Drawing.Point;

namespace ProBotTelegramClient.CustomComands.CommandVarians.ScreenViewArgs
{
    public class ScreenView : BaseCommand, IScreenClient, ICommandNet
    {
		[JsonConstructor]
		public ScreenView(string preferanceSave) : base(preferanceSave) { }
		public ScreenView(BasePreferance preferance) : base(preferance) 
		{
			DataStatus = CommandDataStatus.NotFound;
		}

		[JsonIgnore]
		public string ViewerName { get; set; }
		[JsonIgnore]
        public CommandDataStatus DataStatus { get; set; }   
		[JsonIgnore]
		public MainPoint MainPoint { get; set; }

		public ViewData Data { get; set; }

		public event Action<MainPoint> OnGetMainPoint;

		public void AddToNet() => ICommandNet.CommandNet.Add(Preferance.FullName, this);
		protected override void BaseSettings()
		{
			base.BaseSettings();

			ViewerName = Preferance.Name;

            AddToNet();
		}

		public override Control DrawDisplay(int width, int height, int x, int y)
        {
			ScreenSettings settings = ScreenSettings.CommandSettings;

			Panel panel = base.DrawDisplay(width, height, x, y) as Panel;
			panel.DoubleClick += (s, e) =>
            {
				if (DataStatus is CommandDataStatus.NotFound) return;

				MainScreenController.Show(this, new ScreenSettings() { Padding = new Padding(20)});
			};

            Button setButton = new Button();
            setButton.Text = "Set";
            setButton.Width = 60;
			setButton.Height = 20;
			setButton.BackColor = DataStatus switch
            {
                CommandDataStatus.Success => Color.Green,
                CommandDataStatus.NotFound => Color.Red,
            };
			setButton.Left = settings.Padding.Left;
			setButton.Top = settings.Padding.Top;
			setButton.Click += (s, e) =>
            {
                if (s is Button button)
                {
                    if (!InputHook.isRecord)
                    {
                        MainForm.Instance.button1.Text = "Stop";
                        button.BackColor = Color.Yellow;

                        HookSettings hookSettings = new HookSettings()
                        {
                            InputEventArgs = (b, i) =>
                            {
                                if (b is BaseInputType.Mouse)
                                {
                                    if (i.Type is InputType.MiddleMouseSelect && i is MouseInput mouseInput)
                                    {
                                        button.BackColor = Color.LawnGreen;
                                        string data = InputSimulatorBuilder.PrintScreen();

										MouseData pos = mouseInput.MouseData;
                                        string photo = ViewEdit.CutPhoto(new Photo(data, new Box(pos.x2, pos.y2, pos.x1, pos.y1)));

										var mat = ViewEdit.DeserializeMat(photo);

										Data = new ViewData(photo, new Point(pos.x1, pos.y1), new Point(pos.x2, pos.y2));

                                        MainScreenController.Show(this, new ScreenSettings() { Padding = new Padding(20) });
                                    }
                                }
                            },
                            OnHookStop = () =>
                            {
                                if (Data.data is null || Data.data.Length == 0)
                                {
									button.BackColor = Color.Red;
                                }
                                else
                                {
									button.BackColor = Color.Green;
									DataStatus = CommandDataStatus.Success;
								}
                            }
                        };

                        InputHook.Start(hookSettings);
                    }
                }
            };

			Button useButton = new Button();
			useButton.Text = "Use";
			useButton.Width = 60;
			useButton.Height = 20;
			useButton.Left = setButton.Right + settings.IntervalX;
			useButton.Top = settings.Padding.Top;
			useButton.Click += (s, e) =>
			{
				if (DataStatus != CommandDataStatus.Success) return;
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
            panel.Controls.Add(setButton);

            return panel;
        }

		public void View(Panel screen)
		{
			Mat mat = ViewEdit.DeserializeMat(Data.data);
            System.Drawing.Image res =  Converter.MatToImage(mat);

			PictureBox pictureBox = new PictureBox();

			pictureBox.Image = res;
			if (res.Width > screen.Width || res.Height > screen.Height)
			{
				pictureBox.Dock = DockStyle.Fill;
				pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			}
			else
			{
				pictureBox.Width = res.Width;
				pictureBox.Height = res.Height;

				pictureBox.Left = screen.Width / 2 - pictureBox.Width / 2;
				pictureBox.Top = screen.Height / 2 - pictureBox.Height / 2;
			}

			screen.Controls.Add(pictureBox);
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

			if (Data == ViewData.Empty)
			{
				DataStatus = CommandDataStatus.NotFound;
			}
			else DataStatus = CommandDataStatus.Success;
		}
	}
}
