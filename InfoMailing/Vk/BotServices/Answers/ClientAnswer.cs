using InfoMailing.User;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace InfoMailing.BotServices.Answers
{
    public class ClientAnswer
    {
        /// <summary>
        /// if method exist, return <color=green>Message</color>, else return Exception message
        /// </summary>
        /// <param name="text"></param>
        /// <param name="replymarkup"></param>
        /// <returns></returns>
        public static async Task<Message> SendMessage(UserInfo userInfo, string text, IReplyMarkup? replymarkup = null, bool addToManager = true)
        {
            #region Precondition
            if (text == null)
            {
                throw new Exception("text can not be null");
            }
            #endregion
            Message message = await Startup.telegramBot
                .SendTextMessageAsync(userInfo.ChatId, text, replyMarkup: replymarkup);

            return message;
        }
		public static async Task<Message> SendMessage(long chatId, string text, IReplyMarkup? replymarkup = null, bool addToManager = true)
		{
			#region Precondition
			if (text == null)
			{
				throw new Exception("text can not be null");
			}
			#endregion
			Message message = await Startup.telegramBot
				.SendTextMessageAsync(chatId, text, replyMarkup: replymarkup);

			return message;
		}

		public static async Task<Message> SendPhoto(UserInfo userInfo, InputFile inputFile)
        {
            Message message = await Startup.telegramBot.SendPhotoAsync(userInfo.ChatId, inputFile);

            return message;
        }
        public static async Task<Message[]> SendDataSet(UserInfo userInfo, IEnumerable<InputFile> inputFile)
        {
            List<IAlbumInputMedia> albumInputs = new List<IAlbumInputMedia>();

            foreach (var item in inputFile)
            {
                InputMediaPhoto media = new InputMediaPhoto(item);
                albumInputs.Add(media);
            }

            Message[] messages = await Startup.telegramBot.SendMediaGroupAsync(userInfo.ChatId, albumInputs);

            return messages;
        }
        public static async Task<Message> SendVideo(UserInfo userInfo, InputFile inputFile)
        {
            Message message = await Startup.telegramBot.SendVideoAsync(userInfo.ChatId, inputFile);

            return message;
        }

        public static async Task DeleteMessage(UserInfo userInfo, int messageId)
        {
            try
            {
                await Startup.telegramBot
                .DeleteMessageAsync(userInfo.ChatId, messageId);
            }
            catch (Exception) { }

        }
        public static async Task DeleteMessage(UserInfo userInfo, int messageId, bool dellInManager)
        {
            await Startup.telegramBot
                .DeleteMessageAsync(userInfo.ChatId, messageId);
        }
    }
}
