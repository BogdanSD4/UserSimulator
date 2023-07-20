using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TelegarmBot_Jmenka.Database;
using Telegram.Bot;
using Telegram.Bot.Types;


public class Startup
{
	public static ITelegramBotClient telegramBot;
	public static Configuration appConfig;

	public static void EnableServices(ITelegramBotClient client, Configuration configuration)
	{
		#region ReadConfigFile
		string creditialPath = configuration.AppSettings.Settings["GoogleCreditials"].Value;
		string apiKey_OpenAi = configuration.AppSettings.Settings["OpenAIAPI"].Value;
		#endregion

		telegramBot = client;
		appConfig = configuration;

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
