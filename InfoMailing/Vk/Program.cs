
using Newtonsoft.Json;
using System.Configuration;
using System.Reflection;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using BotServices;
using Instruments;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

Configuration configuration = FileManager.GetAppSettings();
string vkToken = configuration.AppSettings.Settings["VkApi"].Value;

VkApi vkClient = new VkApi();
vkClient.Authorize(new ApiAuthParams { AccessToken = vkToken });

var accessToken = vkToken; // Замените на ваш ключ доступа сообщества

var server = vkClient.Groups.GetLongPollServer();

Console.WriteLine("Receiving");
Console.ReadLine();


async Task Update(ITelegramBotClient client, Update update, CancellationToken arg3)
{
    //return;
}

async Task Error(ITelegramBotClient client, Exception error, CancellationToken arg3)
{
	throw error;
}

