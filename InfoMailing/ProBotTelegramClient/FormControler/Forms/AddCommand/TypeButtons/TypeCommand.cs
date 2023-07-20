using FileSystemTree;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand.TypeButtons
{
	public abstract class TypeCommand : Button
	{
		public TypeCommand(Control parent)
        {
			Width = 50;
			AutoSize = true;
			Top = parent.Height / 2 - Height / 2;

			Preferance();
		}

		public FieldController fieldController { get; set; }
		public abstract void Result();
		public abstract void Preferance();
		public Func<bool> Use(Panel panel)
		{
			return fieldController.Show(panel);
		}
	}
}
