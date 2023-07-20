using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotServices
{
	public class Properties
	{
        public Properties(ITelegramBotClient telegram, Update updateBase)
        {
            client = telegram;
            update = updateBase;
        }
        public ITelegramBotClient client { get; private set; }
        public Update update { get; private set; }
	}
}
