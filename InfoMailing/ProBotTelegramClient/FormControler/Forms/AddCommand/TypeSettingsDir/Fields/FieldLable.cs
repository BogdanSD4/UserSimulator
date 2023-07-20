using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields
{
	public class FieldLable : BaseField
	{
		public FieldLable(string fieldName, string fieldText) : base(fieldName)
		{
			text = fieldText;
			requirement = (t) => { return true; };
		}
		public FieldLable(string fieldName, string fieldText, Func<FieldLable, bool> func) : base(fieldName)
		{
			text = fieldText;
			requirement = func;
		}

		private string text;
		public string startText { get; set; }
		protected Func<FieldLable, bool> requirement { get; set; }

		public Label label;
		public Label resultLable;

		public override void CreateUI(Panel screen)
		{
			int posY = Controller.LastDrawed is null ? settings.Padding.Top : Controller.LastDrawed.Bottom + settings.IntervalY;

			label.Text = text;
			label.Left = settings.Padding.Left;
			label.Top = posY;

			resultLable.Text = startText;
			resultLable.Left = label.Width + 20;
			resultLable.Width = screen.Width / 2 - 20;
			resultLable.Top = posY;
			resultLable.Click += (s, e) =>
			{
				label.ForeColor = SystemColors.WindowText;
			};

			screen.Controls.Add(label);
			screen.Controls.Add(resultLable);

			if (IsStatic) Controller.LastDrawed = label;
			LastDrawed = label;
		}

		public override void InitializeComponets()
		{
			label = new Label();
			resultLable = new Label();
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
			resultLable.Visible = false;
		}

		protected override void Enable()
		{
			label.Visible = true;
			resultLable.Visible = true;
		}
	}
}
