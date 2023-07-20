using Newtonsoft.Json;
using ProBotTelegramClient.CustomComands;
using ProBotTelegramClient.CustomComands.CommandVarians;
using ProBotTelegramClient.FormControler.Main.ErrorLable;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramClient.FormControler.Main.Scripts;
using ProBotTelegramClient.FormControler.Main.Scripts.ImpExp;
using ProBotTelegramClient.FormControler.Main.TimerDir;
using ProBotTelegramClient.FormControler.MainSettings;
using ProBotTelegramClient.Inputs.BaseInputs;
using ProBotTelegramClient.Inputs.InputHook;
using ProBotTelegramClient.Inputs.InputsCompilerDir;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using UserSimulator.Inputs.InputHook;

namespace ProBotTelegramClient
{
	public partial class MainForm : Form
	{
		public static MainForm Instance { get; set; }

		public KeyAssignmentForm keyAssignmentForm;

		public static ImmitateCancelingToken CancelingToken { get; set; }
		public bool isCommandOpen { get; set; } = true;
		public event Action OnImitateStart;
		public event Action OnImitateEnd;

		private bool isImmitate { get; set; }

		public MainForm()
		{
			Instance = this;
			InitializeComponent();

			MainScreenController.Initial(mainScreen);

			BaseSettings();
		}

		#region OnEvents
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
		}
		#endregion

		#region CustomMethods
		public void ChangeKeyAssignment(ICollectionObject collection)
		{
			if (keyAssignmentForm is null) return;

			keyAssignmentForm.OpenCollection(collection);
		}

		private void BaseSettings()
		{
			BasePreferance preferance = new CommandCollectionPreferance("Main", ".txt");

			CommandCollection collection = CommandCollection.Get(preferance);
			keyAssignmentForm = new KeyAssignmentForm(panel1, collection);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			StartPosition = FormStartPosition.CenterScreen;
			CancelingToken = ImmitateCancelingToken.Standart;

			ErrorBox.Initial(error);
			TimerController.Initial(timer1);
			LayersController.Initial(this);

			InputHook.OnHookStart += () =>
			{
				status.Text = "Record";
				button1.Enabled = true;
			};
			InputHook.OnHookStop += () =>
			{
				status.Text = "None";
				button1.Enabled = false;
			};
			InputHook.OnHookPause += () => { status.Text = "Pause"; };
			InputHook.OnHookContinue += () => { status.Text = "Record"; };
		}
		#endregion

		private void button1_Click(object sender, EventArgs e)
		{
			if (InputHook.isRecord)
			{
				button1.Text = "Start";
				InputHook.Stop();
			}
		}

		private async void button2_Click(object sender, EventArgs e)
		{
			if (!isImmitate)
			{
				isImmitate = true;
				HookSettings hookSettings = new HookSettings()
				{
					OnKeyDown = (e) =>
					{
						if (e.KeyValue == 96)
						{
							CancelingToken.Value = false;
						}
					},
					OnHookStop = () => { CancelingToken.Value = true; },
				};
				InputHook.StartImmitate(hookSettings);

				OnImitateStart?.Invoke();
				button2.Text = "Work";

				await Task.Delay(1000);

				MainScreenController.Play(() =>
				{
					isImmitate = false;
					button2.Text = "Start";
					OnImitateEnd?.Invoke();
					InputHook.Stop();
				});
			}
		}

		private void resetButton_Click(object sender, EventArgs e)
		{
			InputsCompiler.Inputs = null;
		}

		private void button4_Click(object sender, EventArgs e)
		{
			if (isCommandOpen)
			{
				Width -= 620;
			}
			else
			{
				Width += 620;
			}
			isCommandOpen = !isCommandOpen;
		}

		private void button5_Click(object sender, EventArgs e)
		{
			keyAssignmentForm.OpenCommandMenu();
		}

		private void button6_Click(object sender, EventArgs e)
		{
			keyAssignmentForm.Previous();
		}

		private void toMainPath_Click(object sender, EventArgs e)
		{
			keyAssignmentForm.OpenRoot();
		}

		private void returnToMainScreen_Click(object sender, EventArgs e)
		{
			MainScreenController.ToMain();
		}

		private void mainSettings_Click(object sender, EventArgs e)
		{
			MainSettingsController.Open();
		}

		private void impotButton_Click(object sender, EventArgs e)
		{
			//ImportController import = new ImportController();
			//string file = import.Import();

			//keyAssignmentForm.ImportCollection(file);
		}

		private void exportButton_Click(object sender, EventArgs e)
		{
			//ExportControler export = new ExportControler((s) => 
			//{
			//	string name = Path.GetFileNameWithoutExtension(s.FileName);
			//	return keyAssignmentForm.ExportCollection(name);
			//});

			//export.Export(keyAssignmentForm.currentCollection.CollectionName);
		}
	}
}