using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenPointArgs;
using ProBotTelegramWinForm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace ProBotTelegramClient.FormControler.Main.Scripts.SelectorRuls
{
    public class Selector
    {
        public SelectorSettings Settings { get; set; }

        public List<BasePoint> choosedPoints;
        public Point startSelectPoint;
        public bool isPressedLeftButton;

        public Selector(SelectorSettings settings)
        {
            Settings = settings;
			choosedPoints = new List<BasePoint>();
        }

		public void Activate(Control target, BasePoint selectedObj)
		{
			target.MouseEnter += (s, e) => { OnMouseEnter(selectedObj, e); };
			target.MouseLeave += (s, e) => { OnMouseLeave(selectedObj, e); };
			target.MouseDown += (s, e) => { OnMouseDown(selectedObj, e); };
			target.MouseUp += (s, e) => { OnMouseUp(selectedObj, e); };
			target.MouseMove += (s, e) => { OnMouseMove(selectedObj, e); };
			target.Click += (s, e) => { OnClick(selectedObj, e); };
		}

		private void OnMouseEnter(BasePoint sender, EventArgs e)
		{
			if (choosedPoints.Contains(sender)) return;
			sender.Body.BackColor = Settings.OverColor;
		}
		private void OnMouseLeave(BasePoint sender, EventArgs e)
		{
			if (choosedPoints.Contains(sender)) return;
			sender.Body.BackColor = Settings.NoneEffect;
		}
		private void OnMouseDown(BasePoint sender, EventArgs e)
        {
            if (e is MouseEventArgs mouse)
            {
                if (mouse.Button is MouseButtons.Left)
                {
                    Panel mainScreen = MainScreenController.ScreenPanel;

                    isPressedLeftButton = true;
                    startSelectPoint = mainScreen.PointToClient(sender.Body.PointToScreen(mouse.Location));
                    var list = mainScreen.Controls;
                    foreach (Panel item in list)
                    {
                        if (item != sender.Body)
                        {
                            item.BackColor = Settings.NoneEffect;
                        }
                    }
                    choosedPoints.Clear();
                }
                else if (mouse.Button is MouseButtons.Middle)
                {
                    sender.Body.BackColor = Settings.SelectColor;
                    choosedPoints.Add(sender);
                }
            }
        }
		private void OnMouseUp(BasePoint sender, EventArgs e)
		{
			if (e is MouseEventArgs mouse)
			{
				if (mouse.Button is MouseButtons.Left)
				{
					foreach (var panel in choosedPoints)
					{
						panel.Body.BackColor = Settings.SelectColor;
					}
					isPressedLeftButton = false;
				}
			}
		}
		private void OnMouseMove(BasePoint sender, EventArgs e)
		{
			if (isPressedLeftButton && e is MouseEventArgs mouse)
			{
				if (mouse.Button is MouseButtons.Left)
				{
					Panel mainScreen = MainScreenController.ScreenPanel;
					Point start = startSelectPoint;
					Point pos = mainScreen.PointToClient(sender.Body.PointToScreen(mouse.Location));
					var list = MainScreenController.MainPointList;

					foreach (var item in list.Items)
					{
						if (!choosedPoints.Contains(item))
						{
							if ((item.Body.Top > start.Y || item.Body.Bottom > start.Y) && item.Body.Top < pos.Y)
							{
								choosedPoints.Add(item);
								item.Body.BackColor = Color.LightGray;
							}
						}
						else
						{
							if ((item.Body.Top < start.Y && item.Body.Bottom < start.Y) || item.Body.Top > pos.Y)
							{
								choosedPoints.Remove(item);
								item.Body.BackColor = Settings.NoneEffect;
							}
						}
					}
				}
			}
		}
		private void OnClick(BasePoint sender, EventArgs e)
		{
			if (choosedPoints.Contains(sender)) return;

			foreach (var item in choosedPoints)
			{
				item.Body.BackColor = Settings.NoneEffect;
			}
			choosedPoints.Clear();

			sender.Body.BackColor = Settings.SelectColor;
			choosedPoints.Add(sender);
		}
	}
}
