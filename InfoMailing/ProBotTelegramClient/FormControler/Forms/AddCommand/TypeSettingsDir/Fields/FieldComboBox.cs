using ProBotTelegramClient.CustomComands.CommandVarians.ClipboardArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields
{
	public class FieldComboBox : BaseField
	{
		public FieldComboBox(string fieldName, string fieldText, Type enumType, int index = 0) : base(fieldName)
		{
			text = fieldText;
			
			items = Enum.GetNames(enumType);

			comboBox.Items.Clear();
			comboBox.Items.AddRange(items);
			comboBox.SelectedIndex = index;
		}
        public FieldComboBox(string fieldName, string fieldText, IEnumerable<string> itemNames, int index = 0) : base(fieldName)
        {
			text = fieldText;

			items = itemNames.ToArray();

			comboBox.Items.Clear();
			comboBox.Items.AddRange(items);
			comboBox.SelectedIndex = index;
		}

        private string text;
		private object[] items;

		public Label label;
		public ComboBox comboBox;

		public override void InitializeComponets()
		{
			label = new Label();
			comboBox = new ComboBox();
			comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
		}

		public override bool ReturnedAction()
		{
			result.Invoke($"{comboBox.Items[comboBox.SelectedIndex]}");
			return true;
		}
		public override void CreateUI(Panel screen)
		{
			int posY = Controller.LastDrawed is null ? settings.Padding.Top : Controller.LastDrawed.Bottom + settings.IntervalY;

			label.Text = text;
			label.Left = settings.Padding.Left;
			label.Top = posY;

			comboBox.Left = label.Width + 20;
			comboBox.Width = screen.Width / 2 - 20;
			comboBox.Top = posY;

			screen.Controls.Add(label);
			screen.Controls.Add(comboBox);

			if (IsStatic) Controller.LastDrawed = label;
			LastDrawed = label;
		}

		protected override void Enable()
		{
			label.Visible = true;
			comboBox.Visible = true;
		}
		protected override void Disable()
		{
			label.Visible = false;
			comboBox.Visible = false;
		}
	}
}
