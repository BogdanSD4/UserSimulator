using FileSystemTree;
using ProBotTelegramClient.Inputs.InputHook;
using ProBotTelegramClient.Inputs.InputHook.LowLevelHook;
using ProBotTelegramClient.Inputs.InputsCompilerDir;
using ProBotTelegramClient.Instruments.GoogleServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuration = System.Configuration.Configuration;

namespace ProBotTelegramClient
{
	public class Startup
	{
		private static Configuration _config;
		public static Configuration Config { get 
			{
				if (_config is null) throw new Exception("Program not initialized");

				return _config;
			}
		}

		public static void Initial(Form form)
		{
			using (var hook = new InputHook())
			{
				hook.HookActivate();

				ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
				configFileMap.ExeConfigFilename = "C:\\Users\\dokto\\OneDrive\\Рабочий стол\\TelegramBotKPI\\InfoMailing\\ProBotTelegramClient\\app.config";
				_config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

				if (GoogleService.HaveUserInfoCache)
				{
					string creditialsPath = _config.AppSettings.Settings["GoogleUserSettingsPath"].Value;
					GoogleService.Connect(creditialsPath);
				}

				Application.Run(form);
			}
		}
	}
}
