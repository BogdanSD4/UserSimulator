using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields
{
	public class FieldButton : BaseField
	{
		public FieldButton(string fieldName, string fieldText) : base(fieldName)
		{
			text = fieldText;
			requirement = (t) => { return true; };
		}
		public FieldButton(string fieldName, string fieldText, Func<FieldButton, bool> func) : base(fieldName)
		{
			text = fieldText;
			requirement = func;
		}

		private string text;
		public string startText { get; set; }
		protected Func<FieldButton, bool> requirement { get; set; }

		public Label label;
		public Button button;

		public override void InitializeComponets()
		{
			label = new Label();
			button = new Button();
		}

		public override void CreateUI(Panel screen)
		{
			int posY = Controller.LastDrawed is null ? settings.Padding.Top : Controller.LastDrawed.Bottom + settings.IntervalY;

			label.Text = text;
			label.Left = settings.Padding.Left;
			label.Top = posY;

			button.Text = startText;
			button.Left = label.Width + 20;
			button.AutoSize = true;
			button.Top = posY;
			button.Click += (s, e) =>
			{
				label.ForeColor = SystemColors.WindowText;
			};

			screen.Controls.Add(label);
			screen.Controls.Add(button);

			if (IsStatic) Controller.LastDrawed = label;
			LastDrawed = label;
		}

		

		public override bool ReturnedAction()
		{
			if (requirement.Invoke(this))
			{
				return true;
			}

			return false;
		}

		protected override void Disable()
		{
			label.Visible = false;
			button.Visible = false;
		}

		protected override void Enable()
		{
			label.Visible = true;
			button.Visible = true;
		}
	}
}
