using ProBotTelegramClient.CustomComands.CommandsSettings.ServiceSettings;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir;
using ProBotTelegramClient.FormControler.Main.Scripts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProBotTelegramClient.FormControler.Forms.CommandSettingsMenu
{
	public partial class CommandSettingsForm : Form
	{

		public CommandSettingsForm(Preferance preferanse, string? name = null)
		{
			InitializeComponent();
			BaseSettings();

			label1.Text = name is null ? $"Settings" : $"{name} Settings";

			var fieldController = preferanse.CreateFieldController();
			var requirements = fieldController.Show(panel2);

			saveButton.Click += (s, e) =>
			{
				if (requirements.Invoke())
				{
					preferanse.SavePreferance();
					Close();
				}
			};

			Show();
		}

		private void BaseSettings()
		{
			StartPosition = FormStartPosition.CenterScreen;

			LayersController.HigeImportance.Add(this);
		}

		public Form Previous { get; set; }

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
