using Instruments;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InfoMailing.BotServices.Answers
{
	public class ClientAnswerRuntime
	{
		private static ITelegramBotClient client { get 
			{
				//Configuration configuration = FileManager.GetAppSettings();
				//string botToken = configuration.AppSettings.Settings["TelegramApi"].Value;
				return new TelegramBotClient("6013670733:AAE6_Am2-dfoJ5PDN1LSnc1S2hvDbgkSL7w");
			}
		}
		public static async Task SendMessage(string text, long chatId)
		{
			await client.SendTextMessageAsync(chatId, text);
		}
		public static async Task SendPhoto(string filePath, long chatId)
		{
			using (var stream = System.IO.File.OpenRead(filePath))
			{
				await client.SendPhotoAsync(chatId, new InputFileStream(stream));
			}
			System.IO.File.Delete(filePath);
		}

	}
}
