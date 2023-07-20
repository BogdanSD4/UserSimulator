using InfoMailing.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BotSettings.Database;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = InfoMailing.Data.User;

public class Startup
{
	public static ITelegramBotClient telegramBot;
	public static Configuration appConfig;
	public static ChatInfo chatInfo;

	public static void EnableServices(ITelegramBotClient client, Configuration configuration)
	{
		#region ReadConfigFile
		string creditialPath = configuration.AppSettings.Settings["GoogleCreditials"].Value;
		string apiKey_OpenAi = configuration.AppSettings.Settings["OpenAIAPI"].Value;
		#endregion

		telegramBot = client;
		appConfig = configuration;

		#region CreateBaseUser
		string path = "../../../owners.json";
		long userId = 393379820;
		if (!System.IO.File.Exists(path))
		{
			string json = JsonConvert.SerializeObject(new Dictionary<long, User>(
				new KeyValuePair<long, User>[] { new KeyValuePair<long, User>(userId, new User("")) }));

			System.IO.File.WriteAllText(path, json);
		}
		#endregion

		ChatInfo chatInfo = ChatInfo.Instanse;
		chatInfo.DownloadOwnerList();

		CreateBotCommands(client);

		#region Services
		#endregion

		//DatabaseConnector.Connection();
	}

	public static void CreateBotCommands(ITelegramBotClient client)
	{
		var commands = new List<BotCommand>()
		{
				
		};
		client.SetMyCommandsAsync(commands);
	}
	public static void CreateBotCommands(ITelegramBotClient client, List<BotCommand> botCommands)
	{
		client.SetMyCommandsAsync(botCommands);
	}
}
