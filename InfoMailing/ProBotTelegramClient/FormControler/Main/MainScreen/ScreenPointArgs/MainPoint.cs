using Newtonsoft.Json;
using ProBotTelegramClient.CustomComands;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.CustomComands.CommandVarians.CommandArgs;
using ProBotTelegramClient.FormControler.Forms.CommandSettingsMenu;
using ProBotTelegramWinForm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace ProBotTelegramClient.FormControler.Main.MainScreen.ScreenPointArgs
{
    public class MainPoint : BasePoint
    {
        [JsonConstructor]
        public MainPoint(string nameWithExt)
        {
            NameWithExt = nameWithExt;

            var command = ICommandNet.CommandNet[nameWithExt];
			var func = command.MainExecute;
			Func<Task<bool>> arg = command is ICommandNet commandNet? commandNet.Execute : null;

			Execute = async () => { return await func(arg); };
            Name = command.Preferance.Name;
        }
        public MainPoint(BasePreferance preferance, Func<Task<bool>> action)
        {
            NameWithExt = preferance.FullName;
            Execute = action;
            Name = preferance.Name;
        }

        public string NameWithExt { get; set; }
        [JsonIgnore]
        public string Name { get; set; }
        [JsonIgnore]
        public Func<Task<bool>> Execute { get; set; }

        [JsonIgnore]
        public MainList SubPoints { get; set; }
        [JsonIgnore]
        public MainList Container { get; set; }

        public bool IsOpen;
        public bool Enable;
        public bool Visible;

        protected int shift;
        protected readonly int shiftValue = 10;

        public virtual Control DrawPoint(Panel screen, ScreenSettings settings, Control last)
        {
            Panel panel = new Panel();
            Button button = new Button();
            Label textBox = new Label();

            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.BackColor = settings.Selector.Settings.NoneEffect;

            int currentShift = shift * shiftValue;
            int width = settings.objWidth == 0 ? screen.Width - settings.Padding.Left - settings.Padding.Right : settings.objWidth;
            panel.Width = width > settings.MaxWidth ? settings.MaxWidth : width - currentShift;
            panel.Height = settings.objHeight > settings.MaxHeight ? settings.MaxHeight : settings.objHeight;
            panel.Left = settings.Padding.Left + currentShift;
            panel.Top = last is null ? settings.Padding.Top : last.Bottom + settings.IntervalY;
            panel.Click += OnClick;
            settings.Selector.Activate(panel, this);

            button.Width = 20;
            button.Height = 20;
			button.Top = panel.Height / 2 - button.Height / 2;
            button.Left = settings.Padding.Left;
            button.Click += (s,e) => 
            {
                if (IsOpen)
                {
                    Hide();
                }
                else
                {
                    Activate();
                }

                IsOpen = !IsOpen;
                //MainScreenController.Reordered();
            };

			textBox.AutoSize = true;
            textBox.Text = Name;
            textBox.Left = button.Right + 20;
            textBox.Top = panel.Height / 2 - textBox.Height / 2;
            textBox.Enabled = true;
            textBox.Click += OnClick;
            settings.Selector.Activate(textBox, this);

            if(SubPoints is not null && SubPoints.Items is not null && SubPoints.Items.Length > 0)
            {
				panel.Controls.Add(button);
			}
            
            panel.Controls.Add(textBox);

            screen.Controls.Add(panel);
            Body = panel;
			last = panel;

			if (SubPoints is not null)
            {
				foreach (var item in SubPoints.Items)
                {
                    item.shift = shift + 1;
                    last = item.DrawPoint(screen, settings, last);
					last.Visible = false;
                }
            }

            shift = 0;

            return panel;

            void OnClick(object sender, EventArgs e)
            {
                if (e is MouseEventArgs mouse)
                {
                    if (mouse.Button is MouseButtons.Right)
                    {
                        Button delete = new Button();
                        Panel panel1 = new Panel();

                        int shift = 5;
                        panel1.BackColor = Color.WhiteSmoke;
                        panel1.BorderStyle = BorderStyle.FixedSingle;
                        var loc = MainForm.Instance.PointToClient(Cursor.Position);
                        panel1.SetBounds(loc.X - shift, loc.Y + shift - panel1.Height / 2, 70, 50);
                        if(panel1.Top < panel.Top - shift * 2)
                        {
                            panel1.Top += panel1.Height;
                        }
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

        public void AddToList(List<MainPoint> points)
        {
            points.Add(this);
            Remove = () => { points.Remove(this); };
        }
        public override void Delete()
        {
            Container?.Remove(this);
            Remove?.Invoke();
            Body?.Dispose();
        }

        public void Activate()
        {
            foreach (var item in SubPoints.Items)
            {
                item.Enable = true;
            }
			SetVisible(true);
		}
        public void Hide()
        {
			foreach (var item in SubPoints.Items)
			{
				item.Enable = false;
			}
			SetVisible(false);
		}
        public void SetVisible(bool value)
        {
            if (SubPoints is null || SubPoints.Items is null) return;
            foreach (var item in SubPoints.Items)
            {
                if (value)
                {
                    if (!item.Enable) continue;
                }
                item.Body.Visible = value;
                item.SetVisible(value);
            }
        }
        public Control? Open(int pos)
        {
            Control? result = null;
            if(Enable && Visible)
            {
                Body.Top = pos;
                Body.Visible = true;
                pos += Body.Height + MainScreenController.Settings.IntervalY;

                if (SubPoints is null) return null;
				foreach (var item in SubPoints.Items)
				{
					result = item.Open(pos);
				}
			}
            return result ?? Body;
        }



		public virtual string Save()
        {
            List<string> datas = new List<string>();

            if (SubPoints is not null)
            {
                foreach (var item in SubPoints.Items)
                {
                    datas.Add(item.Save());
                }
            }

            var result = new MainPointData(NameWithExt, datas);
            
            return JsonConvert.SerializeObject(result);
        }
        public virtual void Load(string json)
        {
            Load(JsonConvert.DeserializeObject<List<string>>(json));
        }
        public virtual void Load(List<string> list)
        {
            if (list is null) return;

			MainPoint[] points = new MainPoint[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				MainPointData data = JsonConvert.DeserializeObject<MainPointData>(list[i]);
				MainPoint point = new MainPoint(data.Name);
				point.Load(data.Datas);
				points[i] = point;
			}

            SubPoints = new MainList(points);
		}
    }
}
