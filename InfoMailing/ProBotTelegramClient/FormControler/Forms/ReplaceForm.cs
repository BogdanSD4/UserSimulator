using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Forms
{
	public class ReplaceForm : Form
	{
		private CheckBox applyToAllCheckbox;

		public ReplaceForm()
		{
			InitializeComponents();
		}

		private void InitializeComponents()
		{
			Text = "Заменить или продолжить?";
			Width = 300;
			Height = 150;

			Label label = new Label
			{
				Text = "Файл уже существует. Заменить или продолжить?",
				Location = new System.Drawing.Point(10, 10),
				AutoSize = true
			};

			applyToAllCheckbox = new CheckBox
			{
				Text = "Применить ко всем",
				Location = new System.Drawing.Point(10, 40)
			};

			Button replaceButton = new Button
			{
				Text = "Заменить",
				Location = new System.Drawing.Point(10, 70)
			};
			replaceButton.Click += ReplaceButton_Click;

			Button continueButton = new Button
			{
				Text = "Продолжить",
				Location = new System.Drawing.Point(90, 70)
			};
			continueButton.Click += ContinueButton_Click;

			Controls.Add(label);
			Controls.Add(applyToAllCheckbox);
			Controls.Add(replaceButton);
			Controls.Add(continueButton);
		}

		private void ReplaceButton_Click(object sender, EventArgs e)
		{
			// Код для замены файла
			bool applyToAll = applyToAllCheckbox.Checked;

			// Дополнительная логика при необходимости

			Close();
		}

		private void ContinueButton_Click(object sender, EventArgs e)
		{
			// Код для продолжения без замены файла
			bool applyToAll = applyToAllCheckbox.Checked;

			// Дополнительная логика при необходимости

			Close();
		}
	}
}
