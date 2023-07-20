
using Newtonsoft.Json;
using System.Configuration;
using System.Reflection;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using BotServices;
using Instruments;


Configuration configuration = FileManager.GetAppSettings();
string botToken = configuration.AppSettings.Settings["TelegramApi"].Value;

TelegramBotClient botClient = new TelegramBotClient(botToken);
Startup.EnableServices(botClient, configuration);

botClient.StartReceiving(Update, Error);
Console.WriteLine("Receiving");
Console.ReadLine();


async Task Update(ITelegramBotClient client, Update update, CancellationToken arg3)
{
    //return;
    new ClientQuery(client, update).Query();
}

async Task Error(ITelegramBotClient client, Exception error, CancellationToken arg3)
{
	throw error;
}

