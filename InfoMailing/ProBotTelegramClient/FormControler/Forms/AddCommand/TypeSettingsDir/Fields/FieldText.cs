using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields
{
    public class FieldText : BaseField
    {
        public FieldText(string fieldName, string fieldText) : base (fieldName)
        {
            text = fieldText;
			requirement = (t) => { return true; };
        }
		public FieldText(string fieldName, string fieldText, Func<FieldText, bool> func) : base (fieldName)
        {
			text = fieldText;
			requirement = func;
        }

		private string text;
		public string startText { get; set; }
		protected Func<FieldText, bool> requirement { get; set; }

		public Label label;
		public TextBox textBox;

		public override void InitializeComponets()
		{
			label = new Label();
			textBox = new TextBox();
		}

		public override bool ReturnedAction()
		{
			if (requirement.Invoke(this))
			{
				result.Invoke(textBox.Text);
				textBox.Text = "";
				textBox.PlaceholderText = "";
				return true;
			}

			return false;
		}
		public override void CreateUI(Panel screen)
		{
			int posY = Controller.LastDrawed is null ? settings.Padding.Top : Controller.LastDrawed.Bottom + settings.IntervalY;

			label.Text = text;
			label.Left = settings.Padding.Left;
			label.Top = posY;

			textBox.Text = startText;
			textBox.Left = label.Width + 20;
			textBox.Width = screen.Width / 2 - 20;
			textBox.Top = posY;
			textBox.Click += (s, e) =>
			{
				label.ForeColor = SystemColors.WindowText;
			};

			screen.Controls.Add(label);
			screen.Controls.Add(textBox);

			if(IsStatic) Controller.LastDrawed = label;
			LastDrawed = label;
		}

		protected override void Enable()
		{
			label.Visible = true;
			textBox.Visible = true;
		}
		protected override void Disable()
		{
			label.Visible = false;
			textBox.Visible = false;
		}
	}
}
