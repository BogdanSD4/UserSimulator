using InfoMailing.BotServices.Answers;
using InfoMailing.BotServices.MarkupService;
using InfoMailing.Data;
using Instruments;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BotSettings.Database;
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

        public async void Query()
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

					var member = _properties.update.MyChatMember;

					if(member.NewChatMember.Status == ChatMemberStatus.Member)
					{
						
					}
					else if(member.NewChatMember.Status == ChatMemberStatus.Left)
					{

					}

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
			ChatInfo chatInfo = ChatInfo.Instanse;
			long chatId = message.Chat.Id;

			Console.WriteLine(chatId);
			bool isOwner = chatInfo.Owners.ContainsKey(message.From.Id);
			InfoMailing.Data.User? currentUser = null;

			if (isOwner)
			{
                currentUser = chatInfo.Owners[message.From.Id];

				if (message.Chat.Type is ChatType.Private)
				{
					if (!currentUser.MenuEnabel)
					{
						await ClientAnswer.SendMessage(chatId, "Enable", MarkupBuilder.CreateMarkup<ReplyKeyboardMarkup>(new string[][][]
						{
							new string[][]{new string[] {"_send"} },
							new string[][]{new string[] {"_addUser"} },
						}));
						currentUser.MenuEnabel = true;
					}
				}
				else
				{
					if (message.LeftChatMember is not null)
					{
						chatInfo.RemoveChat(message.Chat.Id);
						ChatsDataController.SetUserInfo(chatInfo);
					}
					else if (message.NewChatMembers is not null)
					{
						var id = (await _properties.client.GetMeAsync()).Id;

						if (message.NewChatMembers.Any(x => x.Id == id))
						{
							chatInfo.AddChat(message.Chat.Id);
							ChatsDataController.SetUserInfo(chatInfo);
						}
					}
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
							if (isOwner)
							{
								if (string.IsNullOrEmpty(currentUser.LastMessage)) return;

								List<Message> messages = new List<Message>();
								string text = currentUser.LastMessage;
								var chatList = chatInfo.GetChats();
								if (chatList is null)
								{
									await ClientAnswer.SendMessage(chatId, "No chats", new ReplyKeyboardRemove());
									return;
								}

								await ClientAnswer.SendMessage(chatId, "Start mailing", new ReplyKeyboardRemove());
								currentUser.MenuEnabel = false;

								foreach (var item in chatList)
								{
									 messages.Add(await ClientAnswer.SendMessage(item, text));
								}

								var list = messages.Select(x => 
								{
									if (x.Chat.Type == ChatType.Group || x.Chat.Type == ChatType.Channel || x.Chat.Type == ChatType.Supergroup)
									{
										string resultId = (x.Chat.Id < 0? 
											x.Chat.Id.ToString().Substring(4) : 
											x.Chat.Id.ToString().Substring(3));
										return $"https://t.me/c/{resultId}/{message.MessageId}";
									}
									return null;
								}).Where(x => x is not null);

								if (list is null) return;

								using (MemoryStream memory = new MemoryStream(FileManager.ConvertToXlsxReturnFilePath(list)))
								{
									await ClientAnswer.SendDocument(chatId, new InputFileStream(memory, "Links.xlsx"));
								}

								foreach (var item in list)
								{
									await ClientAnswer.SendMessage(chatId, item);
								}
							}
						}
						else if(message.Text == "_addUser")
						{
							if (isOwner)
							{
								if (string.IsNullOrEmpty(currentUser.LastMessage)) return;

								long id = 0;
								if (long.TryParse(currentUser.LastMessage, out id))
								{
									chatInfo.Owners.Add(id, new InfoMailing.Data.User($"{message.From.FirstName}{message.From.LastName}"));
									chatInfo.UploadOwnerList();
								}
								currentUser.MenuEnabel = false;
							}
						}
						else
						{
							if (isOwner)
							{
								currentUser.LastMessage = message.Text;
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

			ChatsDataController.SetUserInfo(chatInfo);
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
