using InfoMailing.BotServices.Answers;
using InfoMailing.BotServices.MarkupService;
using InfoMailing.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TelegarmBot_Jmenka.Database;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotServices
{
    public class ClientQuery
	{
		private readonly Properties _properties;
        public ClientQuery(ITelegramBotClient telegram, Update update)
        {
            _properties = new Properties(telegram, update);
        }

        public void Query()
		{
            Console.WriteLine(_properties.update.Type);
            switch (_properties.update.Type)
			{
				case UpdateType.Unknown:
					break;
				case UpdateType.Message:
					{
						Console.WriteLine(_properties.update.Message.From.Id);
						MessageQuery();
					}
					break;
				case UpdateType.InlineQuery:
					break;
				case UpdateType.ChosenInlineResult:
					break;
				case UpdateType.CallbackQuery:
					{
						CallbackQuery();
					}
					break;
				case UpdateType.EditedMessage:
					break;
				case UpdateType.ChannelPost:
					break;
				case UpdateType.EditedChannelPost:
					break;
				case UpdateType.ShippingQuery:
					break;
				case UpdateType.PreCheckoutQuery:
					break;
				case UpdateType.PollAnswer:
					{
						PollAnswerQuery();
					}
					break;
				case UpdateType.MyChatMember:
					break;
				case UpdateType.ChatMember:
					break;
				case UpdateType.ChatJoinRequest:
					break;
				default:
					break;
			}	
		}

		private async void MessageQuery()
		{
			Message message = _properties.update.Message;
			UserInfo? userInfo = null;
			Console.WriteLine(message.Chat.Id);
			if (message.From.Id.ToString() == Startup.appConfig.AppSettings.Settings["Owner"].Value)
			{
                if (message.Chat.Type is ChatType.Private)
				{
					userInfo = UsersDataController.GetOrCreateUserInfo(message.From.Id);

					if (!userInfo.MenuEnabel)
					{
						await ClientAnswer.SendMessage(userInfo, "Enable", MarkupBuilder.CreateMarkup<ReplyKeyboardMarkup>(new string[][][]
						{
					new string[][]{new string[] {"_send"} }
						}));
						userInfo.MenuEnabel = true;
					}
				}
				else
				{
					
					long id = long.Parse(Startup.appConfig.AppSettings.Settings["Owner"].Value);
					UserInfo user = UsersDataController.GetOrCreateUserInfo(id);
					user.AddChat(message.Chat.Id);
					UsersDataController.SetUserInfo(user);
					return;
				}
			}

			switch (message.Type)
			{
				case MessageType.Unknown:
					break;
				case MessageType.Text:
					{
						if(message.Text == "_send")
						{
							if (userInfo is not null)
							{
								string text = userInfo.LastMessage;
								var chatList = userInfo.GetChats();
								if (chatList is null) return;
								foreach (var item in chatList)
								{
									await ClientAnswer.SendMessage(item, text);
								}
							}
						}
						else
						{
							if (userInfo is not null)
							{
								userInfo.LastMessage = message.Text;
							}
						}
					}
					break;
				case MessageType.Photo:
					break;
				case MessageType.Audio:
					break;
				case MessageType.Video:
					break;
				case MessageType.Voice:
					break;
				case MessageType.Document:
					break;
				case MessageType.Sticker:
					break;
				case MessageType.ChatMemberLeft:
					break;
				case MessageType.ChatTitleChanged:
					break;
				case MessageType.ChatPhotoChanged:

				case MessageType.Animation:
					break;
				default:
					break;
			}

			UsersDataController.SetUserInfo(userInfo);
		}

		private async void PollAnswerQuery() 
		{
			PollAnswer answer = _properties.update.PollAnswer;
		}

		private async void CallbackQuery()
		{
			CallbackQuery query = _properties.update.CallbackQuery;
		}
	}
}
