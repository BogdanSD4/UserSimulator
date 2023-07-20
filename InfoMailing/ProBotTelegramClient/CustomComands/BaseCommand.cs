using FileSystemTree;
using Newtonsoft.Json;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.CustomComands.CommandsSettings.ServiceSettings;
using ProBotTelegramClient.FormControler.Forms.CommandSettingsMenu;
using ProBotTelegramClient.FormControler.Forms.FormQuestion;
using ProBotTelegramClient.FormControler.Forms.ServiceMenu;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramWinForm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;

namespace ProBotTelegramClient.CustomComands
{
    public abstract class BaseCommand
    {
        public BaseCommand(string json)
        {
			var pref = JsonConvert.DeserializeObject<(Type, string)>(json);
			Preferance = JsonConvert.DeserializeObject(pref.Item2, pref.Item1) as BasePreferance;
			BaseSettings();
		}
        public BaseCommand(BasePreferance preferance) 
		{
			Preferance = preferance;
			BaseSettings();
		}

		public ServiceController ServiceController { get; set; }
		public string PreferanceSave { get; set; }

		[JsonIgnore]
		public BasePreferance Preferance { get; set; }
		[JsonIgnore]
		public Func<Func<Task<bool>>?, Task<bool>> OnExecute;

		public event  Action OnDelete;

		private Panel? mainUIPanel;
		[JsonIgnore]
		public Panel? MainUIPanel { get { return mainUIPanel ?? null; } set { mainUIPanel = value; } }

		protected virtual void BaseSettings()
		{
			Preferance.Command = this;

			OnDelete += () =>
			{
				if (this is IScreenViewer viewer)
				{
					MainScreenController.Delete(viewer);
				}
			};
		}

		public Control Display(int width, int height, int x, int y)
		{
			if (MainUIPanel is not null) return MainUIPanel;

			return DrawDisplay(width, height, x, y);
		}
		public virtual Control DrawDisplay(int width, int height, int x, int y)
		{
			ScreenSettings settings = ScreenSettings.CommandSettings;

			Panel panel = new Panel();
			panel.SetBounds(x, y, width, height);
			panel.BorderStyle = BorderStyle.Fixed3D;
			panel.BackColor = Color.WhiteSmoke;
			panel.MouseEnter += (s, e) => { panel.BackColor = Color.LightGray; };
			panel.MouseLeave += (s, e) => { panel.BackColor = Color.WhiteSmoke; };

			Button buttonSettings = new Button();
			buttonSettings.Name = "Settings";
			buttonSettings.Width = 20;
			buttonSettings.Height = 20;
			buttonSettings.Text = null;
			buttonSettings.BackgroundImageLayout = ImageLayout.Zoom;
			buttonSettings.BackgroundImage = Image.FromFile("C:\\Users\\dokto\\OneDrive\\Рабочий стол\\TelegramBotKPI\\InfoMailing\\ProBotTelegramClient\\Images\\Icon\\free-icon-cogwheel-44427.png");
			buttonSettings.Left = panel.Width - settings.Padding.Right - buttonSettings.Width;
			buttonSettings.Top = settings.Padding.Top;
			buttonSettings.FlatStyle = FlatStyle.Standard;
			buttonSettings.Click += (s, e) => 
			{
				new CommandSettingsForm(Preferance, Preferance.Name);
			};

			Button buttonDelete = new Button();
			buttonDelete.Name = "Delete";
			buttonDelete.Width = 20;
			buttonDelete.Height = 20;
			buttonDelete.Text = null;
			buttonDelete.BackgroundImage = Image.FromFile("C:\\Users\\dokto\\OneDrive\\Рабочий стол\\TelegramBotKPI\\InfoMailing\\ProBotTelegramClient\\Images\\Icon\\free-icon-cross-mark-17047.png");
			buttonDelete.BackgroundImageLayout = ImageLayout.Zoom;
			buttonDelete.Left = buttonSettings.Left;
			buttonDelete.Top = buttonSettings.Bottom + settings.IntervalY;
			buttonDelete.FlatStyle = FlatStyle.Standard;
			buttonDelete.Click += (s, e) =>
			{
				var set = new QuestionForm($"Delete this command: {Preferance.Name}");

				List<IAnswer> answer = new List<IAnswer>()
				{
					new Answer<Form>("Yes", (f) =>
					{
						panel.Dispose();
						MainForm.Instance.keyAssignmentForm.RemoveCommand(this);
						f.Close();
					}, set),
					new Answer<Form>("No", (f) => { f.Close(); }, set),
				};

				set.Answers = answer;
				set.Open();
			};

			Button buttonServices = new Button();
			buttonServices.Name = "Services";
			buttonServices.Width = 20;
			buttonServices.Height = 20;
			buttonServices.Text = null;
			buttonServices.BackgroundImage = Image.FromFile("C:\\Users\\dokto\\OneDrive\\Рабочий стол\\TelegramBotKPI\\InfoMailing\\ProBotTelegramClient\\Images\\Icon\\free-icon-internet-149229.png");
			buttonServices.BackgroundImageLayout = ImageLayout.Zoom;
			buttonServices.Left = buttonDelete.Left - settings.IntervalX - buttonServices.Width;
			buttonServices.Top = buttonDelete.Top;
			buttonServices.FlatStyle = FlatStyle.Standard;
			if(ServiceController is null) buttonServices.Visible = false;
			else
			{
				buttonServices.Click += (s, e) => 
				{
					new ServiceMenuForm(this, Preferance.Name);
				};
			}

			Label label = new Label() { Enabled = false };
			label.Name = "Name";
			label.Text = Preferance.Name;
			label.Left = settings.Padding.Left;
			label.Top = buttonDelete.Top;
			label.ForeColor = SystemColors.ControlText;

			TextBox textBox = new TextBox();
			textBox.Enabled = false;
			textBox.Text = Preferance.Type;
			textBox.TextAlign = HorizontalAlignment.Center;
			textBox.Multiline = true;
			textBox.Dock = DockStyle.Bottom;
			textBox.Height = 20;

			panel.Controls.Add(textBox);
			panel.Controls.Add(buttonServices);
			panel.Controls.Add(buttonSettings);
			panel.Controls.Add(buttonDelete);
			panel.Controls.Add(label);

			Preferance.OnPreferanceSaved += (p) => 
			{
				label.Text = p.Name;
				OnPreferanceSaved();
			};

			MainUIPanel = panel;

			return panel;
		}
		public virtual void RemoveSavedDatas()
		{
			OnDelete?.Invoke();
			ICommandNet.CommandNet.Remove(Preferance.FullName);
			FileManager.Delete(Preferance.FullName);
		}

		public virtual void PreSaveActions() { }
		public virtual string Save()
		{
			PreSaveActions();

			SavePreferance();

			using (var stream = FileManager.Write(Preferance.FullName))
			{
				string json = JsonConvert.SerializeObject(this);
				stream.Write(json);
			}

			return Preferance.FullName;
		}
		public void SavePreferance()
		{
			ServiceController?.Save();

			(Type, string) prefJson = (Preferance.GetType(), JsonConvert.SerializeObject(Preferance));
			PreferanceSave = JsonConvert.SerializeObject(prefJson);
		}
		public virtual void Load()
		{
			ServiceController?.LoadCommandData(this);
		}

		public async virtual Task<bool> MainExecute(Func<Task<bool>>? execute = null)
		{
			if (OnExecute is null)
			{
				if(execute is null)
				{
					return true;
				}
				return await execute.Invoke();
			}

			return await OnExecute.Invoke(execute);
		}

		public virtual void OnPreferanceSaved()
		{

		}
	}
}
