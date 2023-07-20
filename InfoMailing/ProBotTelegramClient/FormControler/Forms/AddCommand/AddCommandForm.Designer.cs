using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using SizeF = System.Drawing.SizeF;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand
{
	partial class AddCommandForm
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
			panel1 = new Panel();
			button1 = new Button();
			panel2 = new Panel();
			SuspendLayout();
			// 
			// panel1
			// 
			panel1.AllowDrop = true;
			panel1.AutoScroll = true;
			panel1.BorderStyle = BorderStyle.Fixed3D;
			panel1.Location = new Point(12, 64);
			panel1.Name = "panel1";
			panel1.Size = new Size(395, 170);
			panel1.TabIndex = 1;
			// 
			// button1
			// 
			button1.Location = new Point(155, 240);
			button1.Name = "button1";
			button1.Size = new Size(111, 32);
			button1.TabIndex = 2;
			button1.Text = "Add";
			button1.UseVisualStyleBackColor = true;
			// 
			// panel2
			// 
			panel2.BorderStyle = BorderStyle.FixedSingle;
			panel2.Location = new Point(12, 12);
			panel2.Name = "panel2";
			panel2.Size = new Size(395, 46);
			panel2.TabIndex = 3;
			// 
			// AddCommandForm
			// 
			AllowDrop = true;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(419, 284);
			Controls.Add(panel2);
			Controls.Add(button1);
			Controls.Add(panel1);
			Name = "AddCommandForm";
			Text = "AddCommandForm";
			ResumeLayout(false);
		}

		#endregion
		private Panel panel1;
		private Button button1;
		private Panel panel2;
	}
}