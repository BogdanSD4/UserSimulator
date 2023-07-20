using Image = System.Drawing.Image;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using SizeF = System.Drawing.SizeF;

namespace ProBotTelegramClient
{
	partial class MainForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			button1 = new Button();
			button2 = new Button();
			label1 = new Label();
			resetButton = new Button();
			button4 = new Button();
			panel1 = new Panel();
			mainScreen = new Panel();
			button5 = new Button();
			button6 = new Button();
			nameField = new TextBox();
			namePanel = new Panel();
			toMainPath = new Button();
			returnToMainScreen = new Button();
			status = new Label();
			panel2 = new Panel();
			screenName = new TextBox();
			panel3 = new Panel();
			error = new Label();
			timer1 = new System.Windows.Forms.Timer(components);
			mainSettings = new Button();
			toolTip1 = new ToolTip(components);
			impotButton = new Button();
			exportButton = new Button();
			namePanel.SuspendLayout();
			panel2.SuspendLayout();
			panel3.SuspendLayout();
			SuspendLayout();
			// 
			// button1
			// 
			button1.Enabled = false;
			button1.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
			button1.Location = new Point(12, 400);
			button1.Name = "button1";
			button1.Size = new Size(121, 38);
			button1.TabIndex = 1;
			button1.Text = "Stop";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// button2
			// 
			button2.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
			button2.Location = new Point(692, 333);
			button2.Name = "button2";
			button2.Size = new Size(91, 61);
			button2.TabIndex = 1;
			button2.Text = "Start";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
			label1.Location = new Point(12, 372);
			label1.Name = "label1";
			label1.Size = new Size(66, 25);
			label1.TabIndex = 2;
			label1.Text = "Status:";
			// 
			// resetButton
			// 
			resetButton.Location = new Point(58, 333);
			resetButton.Name = "resetButton";
			resetButton.Size = new Size(75, 33);
			resetButton.TabIndex = 3;
			resetButton.Text = "Clear";
			resetButton.UseVisualStyleBackColor = true;
			resetButton.Click += resetButton_Click;
			// 
			// button4
			// 
			button4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
			button4.Location = new Point(692, 400);
			button4.Name = "button4";
			button4.Size = new Size(91, 38);
			button4.TabIndex = 4;
			button4.Text = "Command";
			button4.UseVisualStyleBackColor = true;
			button4.Click += button4_Click;
			// 
			// panel1
			// 
			panel1.AutoScroll = true;
			panel1.BorderStyle = BorderStyle.FixedSingle;
			panel1.Location = new Point(807, 12);
			panel1.Name = "panel1";
			panel1.Size = new Size(600, 315);
			panel1.TabIndex = 5;
			// 
			// mainScreen
			// 
			mainScreen.AutoScroll = true;
			mainScreen.AutoScrollMargin = new Size(0, 8);
			mainScreen.BorderStyle = BorderStyle.FixedSingle;
			mainScreen.Cursor = Cursors.Hand;
			mainScreen.ForeColor = SystemColors.InfoText;
			mainScreen.Location = new Point(12, 12);
			mainScreen.Name = "mainScreen";
			mainScreen.Size = new Size(771, 315);
			mainScreen.TabIndex = 7;
			mainScreen.TabStop = true;
			// 
			// button5
			// 
			button5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
			button5.Location = new Point(1239, 333);
			button5.Name = "button5";
			button5.Size = new Size(168, 39);
			button5.TabIndex = 6;
			button5.Text = "Add";
			button5.UseVisualStyleBackColor = true;
			button5.Click += button5_Click;
			// 
			// button6
			// 
			button6.Location = new Point(1343, 378);
			button6.Name = "button6";
			button6.Size = new Size(64, 27);
			button6.TabIndex = 6;
			button6.Text = "Back";
			button6.UseVisualStyleBackColor = true;
			button6.Click += button6_Click;
			// 
			// nameField
			// 
			nameField.Enabled = false;
			nameField.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
			nameField.Location = new Point(45, 2);
			nameField.Multiline = true;
			nameField.Name = "nameField";
			nameField.Size = new Size(284, 33);
			nameField.TabIndex = 0;
			nameField.Text = "Main";
			nameField.TextAlign = HorizontalAlignment.Center;
			// 
			// namePanel
			// 
			namePanel.BorderStyle = BorderStyle.FixedSingle;
			namePanel.Controls.Add(toMainPath);
			namePanel.Controls.Add(nameField);
			namePanel.Location = new Point(807, 333);
			namePanel.Name = "namePanel";
			namePanel.Size = new Size(333, 39);
			namePanel.TabIndex = 8;
			// 
			// toMainPath
			// 
			toMainPath.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			toMainPath.Location = new Point(3, 2);
			toMainPath.Name = "toMainPath";
			toMainPath.Size = new Size(40, 33);
			toMainPath.TabIndex = 1;
			toMainPath.Text = ">>";
			toMainPath.UseVisualStyleBackColor = true;
			toMainPath.Click += toMainPath_Click;
			// 
			// returnToMainScreen
			// 
			returnToMainScreen.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
			returnToMainScreen.Location = new Point(12, 333);
			returnToMainScreen.Name = "returnToMainScreen";
			returnToMainScreen.Size = new Size(40, 33);
			returnToMainScreen.TabIndex = 1;
			returnToMainScreen.Text = "*/";
			returnToMainScreen.UseVisualStyleBackColor = true;
			returnToMainScreen.Click += returnToMainScreen_Click;
			// 
			// status
			// 
			status.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
			status.Location = new Point(72, 372);
			status.Name = "status";
			status.Size = new Size(61, 25);
			status.TabIndex = 9;
			status.Text = "None";
			status.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// panel2
			// 
			panel2.Controls.Add(screenName);
			panel2.Location = new Point(461, 333);
			panel2.Name = "panel2";
			panel2.Size = new Size(225, 39);
			panel2.TabIndex = 8;
			// 
			// screenName
			// 
			screenName.Enabled = false;
			screenName.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
			screenName.Location = new Point(4, 3);
			screenName.Multiline = true;
			screenName.Name = "screenName";
			screenName.Size = new Size(218, 33);
			screenName.TabIndex = 0;
			screenName.Text = "Root";
			screenName.TextAlign = HorizontalAlignment.Center;
			// 
			// panel3
			// 
			panel3.BorderStyle = BorderStyle.FixedSingle;
			panel3.Controls.Add(error);
			panel3.Location = new Point(807, 411);
			panel3.Name = "panel3";
			panel3.Size = new Size(507, 27);
			panel3.TabIndex = 10;
			// 
			// error
			// 
			error.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
			error.Location = new Point(-1, -1);
			error.Name = "error";
			error.Size = new Size(509, 26);
			error.TabIndex = 0;
			error.Text = "Some text";
			error.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// mainSettings
			// 
			mainSettings.BackgroundImage = (Image)resources.GetObject("mainSettings.BackgroundImage");
			mainSettings.BackgroundImageLayout = ImageLayout.Zoom;
			mainSettings.Location = new Point(1380, 411);
			mainSettings.Name = "mainSettings";
			mainSettings.Size = new Size(27, 27);
			mainSettings.TabIndex = 11;
			mainSettings.UseVisualStyleBackColor = true;
			mainSettings.Click += mainSettings_Click;
			// 
			// impotButton
			// 
			impotButton.BackgroundImage = (Image)resources.GetObject("impotButton.BackgroundImage");
			impotButton.BackgroundImageLayout = ImageLayout.Zoom;
			impotButton.Location = new Point(1146, 333);
			impotButton.Name = "impotButton";
			impotButton.Size = new Size(44, 39);
			impotButton.TabIndex = 12;
			toolTip1.SetToolTip(impotButton, "Import");
			impotButton.UseVisualStyleBackColor = true;
			impotButton.Click += impotButton_Click;
			// 
			// exportButton
			// 
			exportButton.BackgroundImage = (Image)resources.GetObject("exportButton.BackgroundImage");
			exportButton.BackgroundImageLayout = ImageLayout.Zoom;
			exportButton.Location = new Point(1193, 333);
			exportButton.Name = "exportButton";
			exportButton.Size = new Size(44, 39);
			exportButton.TabIndex = 12;
			toolTip1.SetToolTip(exportButton, "Export");
			exportButton.UseVisualStyleBackColor = true;
			exportButton.Click += exportButton_Click;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1416, 450);
			Controls.Add(exportButton);
			Controls.Add(impotButton);
			Controls.Add(mainSettings);
			Controls.Add(panel3);
			Controls.Add(status);
			Controls.Add(returnToMainScreen);
			Controls.Add(panel2);
			Controls.Add(namePanel);
			Controls.Add(mainScreen);
			Controls.Add(button6);
			Controls.Add(button5);
			Controls.Add(panel1);
			Controls.Add(button4);
			Controls.Add(resetButton);
			Controls.Add(label1);
			Controls.Add(button2);
			Controls.Add(button1);
			Name = "MainForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Form1";
			namePanel.ResumeLayout(false);
			namePanel.PerformLayout();
			panel2.ResumeLayout(false);
			panel2.PerformLayout();
			panel3.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private Button button2;
		private Label label1;
		private Button resetButton;
		private Button button4;
		public Button button1;
		private Panel panel1;
		public Panel mainScreen;
		private Button button5;
		private Button button6;
		private Panel namePanel;
		public TextBox nameField;
		private Button returnToMainScreen;
		private Button toMainPath;
		private Panel panel2;
		public TextBox screenName;
		public Label status;
		private Panel panel3;
		private Label error;
		private System.Windows.Forms.Timer timer1;
		private Button mainSettings;
		private ToolTip toolTip1;
		private Button impotButton;
		private Button exportButton;
	}
}