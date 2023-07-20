using Google.Apis.PeopleService.v1.Data;
using GoogleServices.GoogleServices;
using ProBotTelegramClient.CustomComands;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields;
using ProBotTelegramClient.FormControler.Forms.CommandSettingsMenu;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramClient.Instruments.GoogleServices;
using ProBotTelegramWinForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace ProBotTelegramClient.FormControler.MainSettings
{
	public class MainSettingsController : Preferance 
	{
		private static MainSettingsController _instance;
		public static MainSettingsController Instance { get 
			{
				if(_instance is null) _instance = new MainSettingsController();
				return _instance;
			}
		}

		private static bool isGoogleConnection;
		private static CancellationTokenSource connectionToken;

		public static void Open()
		{
			new CommandSettingsForm(Instance, "Main settings");
		}

		public override FieldController CreateFieldController()
		{
			isGoogleConnection = GoogleService.IsInitialized;

			var connectionStatus = new FieldLable("Connection", "Account") { startText = isGoogleConnection ? GoogleService.Userinfo.Email : "None" };

			var googleConnection = new FieldButton("GoogleAkk", "Google account") { startText = isGoogleConnection ? "Connect" : "Disconnect" };
			googleConnection.button.BackColor = isGoogleConnection ? Color.Green : Color.Red;
			googleConnection.button.Click += (s, e) => 
			{
				if (isGoogleConnection)
				{
					isGoogleConnection = false;
					GoogleService.Disconnect();
					googleConnection.button.BackColor = Color.Red;
					googleConnection.button.Text = "Disconnect";
					connectionStatus.resultLable.Text = "None";
				}
				else
				{
					connectionToken?.Cancel	();

					connectionToken = new CancellationTokenSource();

					Task.Run(() =>
					{
						string creditialsPath = Startup.Config.AppSettings.Settings["GoogleUserSettingsPath"].Value;
						GoogleService.Connect(creditialsPath);

						isGoogleConnection = true;
						
						googleConnection.button.Invoke(() => 
						{
							googleConnection.button.BackColor = Color.Green;
							googleConnection.button.Text = "Connect";
							connectionStatus.resultLable.Text = GoogleService.Userinfo.Email;
						}); 
						
					}, connectionToken.Token);
				}
			};

			fieldController = new FieldController(ScreenSettings.FieldSettings, new BaseField[]
			{
				googleConnection,
				connectionStatus,
			});

			return fieldController;
		}

		public override void SavePreferance()
		{
			
		}
	}
}
