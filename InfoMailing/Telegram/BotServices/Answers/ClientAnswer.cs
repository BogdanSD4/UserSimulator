using InfoMailing.Data;
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

		public static async Task<Message> SendPhoto(long chatId, InputFile inputFile)
        {
            Message message = await Startup.telegramBot.SendPhotoAsync(chatId, inputFile);

            return message;
        }
        public static async Task<Message[]> SendDataSet(long chatId, IEnumerable<InputFile> inputFile)
        {
            List<IAlbumInputMedia> albumInputs = new List<IAlbumInputMedia>();

            foreach (var item in inputFile)
            {
                InputMediaPhoto media = new InputMediaPhoto(item);
                albumInputs.Add(media);
            }

            Message[] messages = await Startup.telegramBot.SendMediaGroupAsync(chatId, albumInputs);

            return messages;
        }
        public static async Task<Message> SendVideo(long chatId, InputFile inputFile)
        {
            Message message = await Startup.telegramBot.SendVideoAsync(chatId, inputFile);

            return message;
        }
		public static async Task<Message> SendDocument(long chatId, InputFile inputFile)
		{
			Message message = await Startup.telegramBot.SendDocumentAsync(chatId, inputFile);

			return message;
		}

		public static async Task DeleteMessage(long chatId, int messageId)
        {
            try
            {
                await Startup.telegramBot
                .DeleteMessageAsync(chatId, messageId);
            }
            catch (Exception) { }

        }
        public static async Task DeleteMessage(long chatId, int messageId, bool dellInManager)
        {
            await Startup.telegramBot
                .DeleteMessageAsync(chatId, messageId);
        }
    }
}
