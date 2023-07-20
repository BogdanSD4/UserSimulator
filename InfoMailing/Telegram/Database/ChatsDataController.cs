using InfoMailing.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BotSettings.Database
{
	public class ChatsDataController
	{
		const string DATABASE_PATH = "../../../MockDatabase/";
		const string name = "ChatSettings";
		public static ChatInfo? GetOrCreateUserInfo()
		{
			return BaseDBRequest<ChatInfo>(
			() =>
			{
				string path = $"{DATABASE_PATH}{name}.json";
				if (System.IO.File.Exists(path))
				{
					string json = System.IO.File.ReadAllText(path);
					ChatInfo userInfo = JsonConvert.DeserializeObject<ChatInfo>(json);
					userInfo.DownloadData();
					return userInfo;
				}
				else
				{
					return null;
				}
			},
			(database) =>
			{
				ChatInfo? userInfo = null;
				if (userInfo is not null) 
				{
					userInfo.DownloadData();
					return userInfo;
				}
				else
				{
					return null;
				}
			});
		}
		public static ChatInfo GetUserInfo(long userId)
		{
			return BaseDBRequest<ChatInfo>(
			() =>
			{
				string path = $"{DATABASE_PATH}{userId}.json";
				if (System.IO.File.Exists(path))
				{
					string json = System.IO.File.ReadAllText(path);
					ChatInfo userInfo = JsonConvert.DeserializeObject<ChatInfo>(json);
					userInfo.DownloadData();
					return userInfo;
				}
				else
				{
					throw new Exception("Don't use \"GetUserInfo\" there, use \"GetOrCreateUserInfo\"");
				}
			},
			(database) =>
			{
				ChatInfo? userInfo = null;
				if (userInfo is not null)
				{
					userInfo.DownloadData();
					return userInfo;
				}
				else
				{
					throw new Exception("Don't use \"GetUserInfo\" there, use \"GetOrCreateUserInfo\"");
				}
			});
		}

		public static void SetUserInfo(ChatInfo userInfo)
		{
			if (userInfo is null) return;
			userInfo.UploadData();

			BaseDBRequest(
			() =>
			{
				string path = $"{DATABASE_PATH}{name}.json";

				string json = JsonConvert.SerializeObject(userInfo);

				System.IO.File.WriteAllText(path, json);
			},
			(database) => 
			{
				database.Users.AddOrUpdate(userInfo);
				database.SaveChanges();
			});
		}

		public static IEnumerable<TResult> GetAllUsers<TResult>(Func<ChatInfo, TResult> func)
		{
			return BaseDBRequest<IEnumerable<TResult>>(
			() =>
			{
				var users = Directory.GetFiles(DATABASE_PATH)
				.Select(x => JsonConvert.DeserializeObject<ChatInfo>(System.IO.File.ReadAllText(x)));

				return users.Select(x => func(x));
			},
			(database) =>
			{
				var users = database.Users.ToArray();
				IEnumerable<TResult> result = users.Select(x => func(x));

				return result;
			});
		}

		private static void BaseDBRequest(Action dbNotExist, Action<DBcontroller> dbExist)
		{
			string? connectionString = DatabaseConnector.ConnectionString;
			if (connectionString is null)
			{
				dbNotExist();
			}
			else
			{
				using (DBcontroller database = new DBcontroller(connectionString))
				{
					dbExist(database);
				}
			}
		}
		private static TResult BaseDBRequest<TResult>(Func<TResult> dbNotExist, Func<DBcontroller, TResult> dbExist)
		{
			string? connectionString = DatabaseConnector.ConnectionString;
			if (connectionString is null)
			{
				return dbNotExist();
			}
			else
			{
				using (DBcontroller database = new DBcontroller(connectionString))
				{
					return dbExist(database);
				}
			}
		}
	}
}
