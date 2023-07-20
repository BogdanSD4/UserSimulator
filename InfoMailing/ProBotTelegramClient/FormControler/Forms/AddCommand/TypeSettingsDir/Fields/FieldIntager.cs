using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields
{
	public class FieldIntager : FieldText
	{
		public FieldIntager(string fieldName, string fieldText) : base(fieldName, fieldText) { }
		public FieldIntager(string fieldName, string fieldText, Func<FieldText, bool> func) : base(fieldName, fieldText, func) { }

		public override void InitializeComponets()
		{
			label = new Label();
			textBox = new TextBox();
			textBox.KeyPress += (s, e) => 
			{
				if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				{
					e.Handled = true;
				}
			};
		}
	}
}
