using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProBotTelegramClient.FormControler.Forms.FormQuestion
{
	public partial class QuestionForm : Form
	{
		private static Form Instance;
		int startPosY = 50;
		int intervalY = 50;
		public IEnumerable<IAnswer> Answers { get; set; }

		public QuestionForm(string question)
		{
			InitializeComponent();
			BaseSettings(question);
		}
		public QuestionForm(string question, IEnumerable<IAnswer> answers)
		{
			InitializeComponent();
			Answers = answers;
			BaseSettings(question);
		}

		private void BaseSettings(string question)
		{
			int baseHeigth = label1.Height;
			label1.AutoSize = true;
			label1.Text = question;
			int differance = label1.Height - baseHeigth;
			label1.AutoSize = false;

			label1.Height += differance;
			this.Height += differance;
			startPosY += differance;

			StartPosition = FormStartPosition.CenterScreen;
		}
		public void Open()
		{
			if (Answers is null) return;
			if (Instance is not null) Instance.Close();
			Show();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			Instance = this;

			int counter = 0;
			bool first = false;
			Control last = null;
			foreach (IAnswer answer in Answers)
			{
				Button button = new Button();
				button.Width = Width / 2 - 40;
				button.Top = startPosY;
				button.Text = answer.Name;
				button.Click += (s, e) => { answer.Invoke(); };

				int quoter = Width / 4;
				if (counter % 2 == 0)
				{
					button.Left = quoter - button.Width / 2;
					last = button;
					if (first)
					{
						Height += intervalY;
					}
					else first = true;
				}
				else
				{
					button.Left = Width - quoter - button.Width / 2 - 20;
					startPosY += intervalY;
				}

				Controls.Add(button);
				counter++;
			}
			if (counter % 2 != 0)
			{
				last.Left = Width / 2 - last.Width / 2 - 10;
			}
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			Instance = null;
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}
	}
}
