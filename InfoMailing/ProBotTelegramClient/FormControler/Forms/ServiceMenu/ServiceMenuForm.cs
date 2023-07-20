using ProBotTelegramClient.CustomComands;
using ProBotTelegramClient.CustomComands.CommandsSettings.ServiceSettings;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramClient.FormControler.Main.Scripts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProBotTelegramClient.FormControler.Forms.ServiceMenu
{
	public partial class ServiceMenuForm : Form
	{ 
		public static ServiceMenuForm Instance { get; set; }
		public static BaseCommand CurrentCommand { get; set; }
		public ServiceMenuForm(BaseCommand command, string name)
		{
			InitializeComponent();
			BaseSettings();

			Name = name;
			label1.Text = $"Service ({name})";

			CurrentCommand = command;

			foreach (var item in command.ServiceController.ServicePreferanse)
			{
				Panel panel = new Panel();
				panel.BorderStyle = BorderStyle.FixedSingle;
				AddComponent(item.GetComponent(this));
			}

			Instance?.Close();

			Show();

			Instance = this;
		}

		private string Name;
		private ScreenSettings settings;
		private int controlNum = 1;
		private int posTop;
		private Control last;

		private void BaseSettings()
		{
			settings = ScreenSettings.ServiceMenu;
			StartPosition = FormStartPosition.CenterScreen;
			posTop = settings.Padding.Top;

			LayersController.HigeImportance.Add(this);
		}

		public void AddComponent(Control control)
		{
			if (controlNum % 2 != 0)
			{
				if(controlNum == 1)
				{
					if (control.Width + settings.Padding.Left + settings.Padding.Right > panel2.Width)
					{
						int maxWidth = control.Width + settings.Padding.Left + settings.Padding.Right;
						Width += maxWidth - panel2.Width;
					}
					else
					{
						panel2.Width = control.Width + settings.Padding.Left + settings.Padding.Right;
						int lastWidth = Width;
						Width = control.Width + settings.Padding.Left + settings.Padding.Right + Padding.Left + Padding.Right;
						panel1.Width += Width - lastWidth;
					}
					if(control.Height + settings.Padding.Top + settings.Padding.Bottom > panel2.Height)
					{
						int maxHeight = control.Height + settings.Padding.Top + settings.Padding.Bottom;
						int val = maxHeight - panel2.Height;
						panel2.Height += val;
						Height += val;
					}
					else
					{
						int val = control.Height + settings.Padding.Top + settings.Padding.Bottom;
						int lastHeight = panel2.Height;
						panel2.Height = val;
						Height += panel2.Height - lastHeight;
					}
				}
				else
				{
					int val = control.Height + settings.IntervalY;
					panel2.Height += val;
					Height += val;
					posTop += val;
				}
				control.Left = panel2.Width / 2 - control.Width / 2;
			}
			else
			{
				if(controlNum == 2)
				{
					if (control.Width + last.Right + settings.IntervalX + settings.Padding.Left + settings.Padding.Right > panel2.Width)
					{
						int maxWidth = control.Width + last.Right + settings.IntervalX + settings.Padding.Left + settings.Padding.Right;
						int lastWidth = Width;
						Width += maxWidth - panel2.Width;
						panel1.Width += Width - lastWidth;
					}
				}
				last.Left = panel2.Width / 4 - last.Width / 2;
				control.Left = panel2.Width - panel2.Width / 4 - control.Width / 2;
			}

			Label label = new Label();
			label.Text = controlNum.ToString();
			control.Controls.Add(label);
			control.Top = posTop;
			panel2.Controls.Add(control);
			last = control;
			controlNum++;
		}
	}
}
