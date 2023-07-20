using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using SizeF = System.Drawing.SizeF;

namespace ProBotTelegramClient.FormControler.Forms.FormQuestion
{
	partial class QuestionForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			label1 = new Label();
			SuspendLayout();
			// 
			// label1
			// 
			label1.Anchor = AnchorStyles.None;
			label1.BackColor = SystemColors.ButtonFace;
			label1.BorderStyle = BorderStyle.FixedSingle;
			label1.FlatStyle = FlatStyle.Flat;
			label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
			label1.Location = new Point(12, 9);
			label1.Name = "label1";
			label1.Size = new Size(331, 24);
			label1.TabIndex = 0;
			label1.Text = "eifuhseiufhsi\r\nekfeuhf\r\n";
			label1.TextAlign = ContentAlignment.TopCenter;
			label1.Click += label1_Click;
			// 
			// QuestionForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(355, 105);
			Controls.Add(label1);
			Name = "QuestionForm";
			Text = "QuestionForm";
			ResumeLayout(false);
		}

		#endregion

		private Label label1;
	}
}