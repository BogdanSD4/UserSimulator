using FileSystemTree;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.CustomComands.CommandsSettings.ServiceSettings;
using ProBotTelegramClient.FormControler.Forms.FormQuestion;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenPointArgs;
using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenSettingsArgs;
using ProBotTelegramClient.FormControler.Main.Scripts.SelectorRuls;
using ProBotTelegramClient.Inputs.InputHook;
using ProBotTelegramClient.Inputs.InputsCompilerDir;
using ProBotTelegramWinForm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSimulator.Inputs.InputHook;
using Color = System.Drawing.Color;

namespace ProBotTelegramClient.CustomComands.CommandVarians.CommandArgs
{
    public class SimpleCommand : BaseCommand, IScreenClient, ISelector, ICommandNet
    {
        [JsonConstructor]
        public SimpleCommand(string preferanceSave) : base(preferanceSave) { }
        public SimpleCommand(BasePreferance preferance) : base(preferance) 
        {
            DataStatus = CommandDataStatus.NotFound;
        }

        [JsonIgnore]
        public string ViewerName { get; set; }
		[JsonIgnore]
        public CommandDataStatus DataStatus { get; set; }
        [JsonIgnore]
        public SelectorSettings SelectorSettings { get; set; }

		public List<string> Data { get; set; }

		public event Action<MainPoint> OnGetMainPoint;

		protected override void BaseSettings()
        {
			base.BaseSettings();

            ViewerName = Preferance.Name;

			SelectorSettings = new SelectorSettings
			{
				NoneEffect = SystemColors.Control,
				OverColor = Color.LightGray,
				SelectColor = SystemColors.Info,
			};

			if (ServiceController is null)
            {
                ServiceController = new ServiceController(this, new ServicePreferance[]
                {
                    new CyclePreferance(Preferance.Name, new CycleType[]
                    {
                        CycleType.Amount
                    }),
                });
            }

            AddToNet();
		}

        public void AddToNet() => ICommandNet.CommandNet.Add(Preferance.FullName, this);

        public override Control DrawDisplay(int width, int height, int x, int y)
        {
            ScreenSettings settings = ScreenSettings.CommandSettings;

            Panel panel = (Panel)base.DrawDisplay(width, height, x, y);
            panel.DoubleClick += (s, e) =>
            {
                if (DataStatus is CommandDataStatus.NotFound) return;

                MainScreenController.Show(this, new ScreenSettings
                {
                    objHeight = 30,
                    Selector = new Selector(SelectorSettings)
                });
            };

            Button setButton = new Button();
            setButton.Text = "Set";
            setButton.Width = 60;
            setButton.Height = 20;
			setButton.Left = settings.Padding.Left;
			setButton.Top = settings.Padding.Top;
			setButton.BackColor = DataStatus switch
            {
                CommandDataStatus.Success => Color.Green,
                CommandDataStatus.NotFound => Color.Red,
            };
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
                            OnHookStop = () =>
                            {
                                var list = InputsCompiler.Compilation();
                                Data = list.Select(x => x.Value).ToList();

								MainScreenController.Show(this, new ScreenSettings
								{
									objHeight = 30,
									Selector = new Selector(SelectorSettings)
								});

								if (Data is not null && Data.Count > 0)
                                {
                                    button.BackColor = Color.Green;
                                    DataStatus = CommandDataStatus.Success;
                                }
                            },
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

            panel.Controls.Add(setButton);
            panel.Controls.Add(useButton);

            return panel;
        }

        public void View(Panel screen)
        {
            ScreenSettings settings = MainScreenController.Settings;
            Panel? subSelect = null;

            int counter = 0;
            foreach (var item in Data)
            {
                Panel panel = new Panel();
                Label textBox = new Label();

                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.BackColor = SelectorSettings.NoneEffect;

                int width = settings.objWidth == 0 ? screen.Width - settings.Padding.Left - settings.Padding.Right : settings.objWidth;
                panel.Width = width > settings.MaxWidth ? settings.MaxWidth : width;
                panel.Height = settings.objHeight > settings.MaxHeight ? settings.MaxHeight : settings.objHeight;
                panel.Left = settings.Padding.Left;
                panel.Top = settings.Padding.Top + panel.Height * counter + settings.IntervalY * counter;
                panel.Click += OnClick;
                settings.Selector.Activate(panel, new CustomPoint<string>(panel, item) { Remove = (s) => { Remove(s); } });

                textBox.AutoSize = true;
                textBox.Text = item;
                textBox.Top = panel.Height / 2 - textBox.Height / 2;
                textBox.Enabled = true;
                textBox.Click += OnClick;
                settings.Selector.Activate(textBox, new CustomPoint<string>(panel, item) { Remove = (s) => { Remove(s); } });

                panel.Controls.Add(textBox);

                screen.Controls.Add(panel);
                counter++;

                void OnClick(object sender, EventArgs e)
                {
                    if (e is MouseEventArgs mouse)
                    {
                        if (mouse.Button is MouseButtons.Right)
                        {
                            Button delete = new Button();
                            Panel panel1 = new Panel();

                            panel1.BackColor = Color.WhiteSmoke;
                            panel1.BorderStyle = BorderStyle.FixedSingle;
                            var loc = MainForm.Instance.PointToClient(Cursor.Position);
                            panel1.SetBounds(loc.X - 5, loc.Y + 5 - panel1.Height / 2, 70, 50);
                            panel1.MouseLeave += (s, e) =>
                            {
                                if (!delete.Bounds.Contains(panel1.PointToClient(Cursor.Position)))
                                {
                                    panel1.Dispose();
                                }
                            };

                            delete.Text = "del";
                            delete.Width = 60;
                            delete.Left = 5;
                            delete.Top = 5;
                            delete.Click += (s, e) =>
                            {
                                MainScreenController.Remove(settings.Selector.choosedPoints);

                                panel1.Dispose();
                            };
                            panel1.Controls.Add(delete);

                            MainForm.Instance.Controls.Add(panel1);
                            panel1.BringToFront();
                        }
                    }
                }
            }
        }
        public void Remove(string value)
        {
            for (int i = 0; i < Data.Count; i++)
            {
                if (Data[i] == value) Data.RemoveAt(i);
            }
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
            OnGetMainPoint.Invoke(point);
            return point;
		}

		public override void OnPreferanceSaved()
		{
			base.OnPreferanceSaved();

            ViewerName = Preferance.Name;
		}
		public override void Load()
		{
			base.Load();

            if (Data is null || Data.Count == 0)
            {
                DataStatus = CommandDataStatus.NotFound;
            }
            else DataStatus = CommandDataStatus.Success;
		}
	}
}
