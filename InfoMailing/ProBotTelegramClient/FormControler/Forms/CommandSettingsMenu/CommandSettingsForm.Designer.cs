using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using SizeF = System.Drawing.SizeF;

namespace ProBotTelegramClient.FormControler.Forms.CommandSettingsMenu
{
	partial class CommandSettingsForm
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
			cancelButton = new Button();
			saveButton = new Button();
			panel1.SuspendLayout();
			SuspendLayout();
			// 
			// panel1
			// 
			panel1.BackColor = SystemColors.ControlLight;
			panel1.BorderStyle = BorderStyle.FixedSingle;
			panel1.Controls.Add(label1);
			panel1.Location = new Point(12, 12);
			panel1.Name = "panel1";
			panel1.Size = new Size(419, 42);
			panel1.TabIndex = 0;
			// 
			// label1
			// 
			label1.Dock = DockStyle.Fill;
			label1.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
			label1.Location = new Point(0, 0);
			label1.Name = "label1";
			label1.Size = new Size(417, 40);
			label1.TabIndex = 0;
			label1.Text = "Settings";
			label1.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// panel2
			// 
			panel2.AutoScroll = true;
			panel2.AutoScrollMargin = new Size(0, 10);
			panel2.BorderStyle = BorderStyle.Fixed3D;
			panel2.Location = new Point(12, 60);
			panel2.Name = "panel2";
			panel2.Size = new Size(419, 226);
			panel2.TabIndex = 1;
			// 
			// cancelButton
			// 
			cancelButton.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
			cancelButton.Location = new Point(351, 292);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new Size(79, 29);
			cancelButton.TabIndex = 2;
			cancelButton.Text = "Cancel";
			cancelButton.UseVisualStyleBackColor = true;
			cancelButton.Click += button1_Click;
			// 
			// saveButton
			// 
			saveButton.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
			saveButton.Location = new Point(266, 292);
			saveButton.Name = "saveButton";
			saveButton.Size = new Size(79, 29);
			saveButton.TabIndex = 2;
			saveButton.Text = "Save";
			saveButton.UseVisualStyleBackColor = true;
			// 
			// CommandSettingsForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(443, 330);
			Controls.Add(saveButton);
			Controls.Add(cancelButton);
			Controls.Add(panel2);
			Controls.Add(panel1);
			FormBorderStyle = FormBorderStyle.SizableToolWindow;
			Name = "CommandSettingsForm";
			panel1.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private Panel panel1;
		private Label label1;
		private Panel panel2;
		private Button cancelButton;
		private Button saveButton;
	}
}