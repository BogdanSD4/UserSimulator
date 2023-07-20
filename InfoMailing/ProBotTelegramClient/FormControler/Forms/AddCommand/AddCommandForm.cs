using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeButtons;
using ProBotTelegramClient.FormControler.Main.Scripts;
using ProBotTelegramClient.FormControler.Main.Scripts.ScrollingRuls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand
{
	public partial class AddCommandForm : Form
	{
		private static Form Instance { get; set; }

		private Func<bool> requirements;
		private TypeCommand currentCommand;

		public AddCommandForm(Control parent)
		{
			InitializeComponent();

			StartPosition = FormStartPosition.CenterScreen;

			Scrolling scrolling = new Scrolling(new ScrollSettings
			{
				typePanelStart = 10
			});
			scrolling.Activate(panel2);
		}

		public void Open()
		{
			if (Instance is not null)
			{
				Instance.Activate();
				if (!Instance.Visible)
				{
					int x = (Screen.PrimaryScreen.Bounds.Width - Width) / 2;
					int y = (Screen.PrimaryScreen.Bounds.Height - Height) / 2;
					Instance.Location = new Point(x, y);
					Instance.Visible = true;
				}
				return;
			}
			Show();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			Instance = this;

			#region Commands
			List<TypeCommand> commands = new List<TypeCommand>();

			var heirs = Assembly.GetAssembly(typeof(TypeCommand)).GetExportedTypes().Where(x => x.BaseType == typeof(TypeCommand));
			foreach (var heir in heirs)
			{
				var comm = Activator.CreateInstance(heir, panel2);
				if (comm is TypeCommand someType)
				{
					someType.Click += (s, e) =>
					{
						if (currentCommand == someType || currentCommand is null) return;

						currentCommand.BackColor = someType.BackColor;

						currentCommand.fieldController.Hide();
						someType.fieldController.Activate();

						currentCommand = someType;
						currentCommand.BackColor = SystemColors.Info;
						requirements = someType.Use(panel1);
					};
					commands.Add(someType);
				}
			}
			#endregion

			int startPos = 10;

			if (commands[0] is not null)
			{
				requirements = commands[0].Use(panel1);
				currentCommand = commands[0];
				currentCommand.BackColor = SystemColors.Info;
			}

			foreach (TypeCommand type in commands)
			{
				type.Left = startPos;
				type.Top = panel2.Height / 2 - type.Height / 2;
				panel2.Controls.Add(type);
				startPos += type.Width + 5;
			}

			button1.Click += (s, e) =>
			{
				if (requirements is null) return;
				if (requirements.Invoke())
				{
					currentCommand.Result();
				}
			};
		}
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			e.Cancel = true;
			Instance.Visible = false;
		}
	}
}
