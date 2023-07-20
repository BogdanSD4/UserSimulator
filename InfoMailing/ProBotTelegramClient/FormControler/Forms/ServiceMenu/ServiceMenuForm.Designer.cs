using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using SizeF = System.Drawing.SizeF;

namespace ProBotTelegramClient.FormControler.Forms.ServiceMenu
{
	partial class ServiceMenuForm
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
			label1 = new Label();
			panel2 = new Panel();
			panel1.SuspendLayout();
			SuspendLayout();
			// 
			// panel1
			// 
			panel1.BorderStyle = BorderStyle.FixedSingle;
			panel1.Controls.Add(label1);
			panel1.Location = new Point(12, 12);
			panel1.Name = "panel1";
			panel1.Size = new Size(454, 44);
			panel1.TabIndex = 0;
			// 
			// label1
			// 
			label1.Dock = DockStyle.Fill;
			label1.Enabled = false;
			label1.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
			label1.Location = new Point(0, 0);
			label1.Name = "label1";
			label1.Size = new Size(452, 42);
			label1.TabIndex = 0;
			label1.Text = "Services";
			label1.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// panel2
			// 
			panel2.BorderStyle = BorderStyle.FixedSingle;
			panel2.Dock = DockStyle.Bottom;
			panel2.Location = new Point(20, 50);
			panel2.Name = "panel2";
			panel2.Size = new Size(438, 235);
			panel2.TabIndex = 1;
			// 
			// ServiceMenuForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(478, 305);
			Controls.Add(panel1);
			Controls.Add(panel2);
			Name = "ServiceMenuForm";
			Padding = new Padding(20);
			Text = "ServiceMenuForm";
			panel1.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private Panel panel1;
		private Label label1;
		private Panel panel2;
	}
}