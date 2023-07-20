using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace InfoMailing.BotServices.Answers
{
    public class ClientAnswerBuilder
    {
        public static async Task<Message> AllowMarkup(long chatId, ReplyKeyboardMarkup markup)
        {
            Message message = await ClientAnswer.SendMessage(chatId, "Enable menu", markup, true);

            return message;
        }
        public static async Task DisableMarkup(long chatId)
        {
            Message message = await ClientAnswer.SendMessage(chatId, "Disable menu", new ReplyKeyboardRemove(), false);
            await ClientAnswer.DeleteMessage(chatId, message.MessageId);
        }

        public static async Task DeleteMessages(long chatId, IEnumerable<Message> messages)
        {
            #region Preconditions
            if (messages == null) return;
            #endregion
            var delllist = messages.Select(x => x.MessageId).ToArray();

            for (int i = 0; i < delllist.Length; i++)
            {
                await ClientAnswer.DeleteMessage(chatId, delllist[i]);
            }
        }
        public static async Task DeleteMessages(long chatId, IEnumerable<int> messagesId)
        {
            foreach (var message in messagesId)
            {
                await ClientAnswer.DeleteMessage(chatId, message);
            }
        }
    }
}
