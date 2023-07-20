using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfoMailing.User;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace InfoMailing.BotServices.Answers
{
    public class ClientAnswerBuilder
    {
        public static async Task<Message> AllowMarkup(UserInfo userInfo, ReplyKeyboardMarkup markup)
        {
            Message message = await ClientAnswer.SendMessage(userInfo, "Enable menu", markup, true);

            return message;
        }
        public static async Task DisableMarkup(UserInfo userInfo)
        {
            Message message = await ClientAnswer.SendMessage(userInfo, "Disable menu", new ReplyKeyboardRemove(), false);
            await ClientAnswer.DeleteMessage(userInfo, message.MessageId);
        }

        public static async Task DeleteMessages(UserInfo userInfo, IEnumerable<Message> messages)
        {
            #region Preconditions
            if (messages == null) return;
            #endregion
            var delllist = messages.Select(x => x.MessageId).ToArray();

            for (int i = 0; i < delllist.Length; i++)
            {
                await ClientAnswer.DeleteMessage(userInfo, delllist[i]);
            }
        }
        public static async Task DeleteMessages(UserInfo userInfo, IEnumerable<int> messagesId)
        {
            foreach (var message in messagesId)
            {
                await ClientAnswer.DeleteMessage(userInfo, message);
            }
        }
    }
}
