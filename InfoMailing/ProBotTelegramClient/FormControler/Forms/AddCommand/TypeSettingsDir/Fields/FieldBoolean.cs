using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields
{
	public class FieldBoolean : BaseField
	{
		public FieldBoolean(string fieldName, string fieldText) : base(fieldName)
		{
			text = fieldText;
		}

		private string text;

		public bool startValue { get; set; }

		public Label label;
		public CheckBox checkBox;

		public override void InitializeComponets()
		{
			label = new Label();
			checkBox = new CheckBox();
		}

		public override bool ReturnedAction()
		{
			result.Invoke(checkBox.Checked);
			return true;
		}
		public override void CreateUI(Panel screen)
		{
			int posY = Controller.LastDrawed is null ? settings.Padding.Top : Controller.LastDrawed.Bottom + settings.IntervalY;

			label.Text = text;
			label.Left = settings.Padding.Left;
			label.Top = posY;

			checkBox.Checked = startValue;
			checkBox.Left = label.Width + 20;
			checkBox.Width = screen.Width / 2 - 20;
			checkBox.Top = label.Top + label.Height / 2 - checkBox.Height / 2;

			screen.Controls.Add(label);
			screen.Controls.Add(checkBox);

			if(IsStatic) Controller.LastDrawed = label;
			LastDrawed = label;
		}

		protected override void Enable()
		{
			label.Visible = true;
			checkBox.Visible = true;
		}
		protected override void Disable()
		{
			label.Visible = false;
			checkBox.Visible = false;
		}
	}
}
