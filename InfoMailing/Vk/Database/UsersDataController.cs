using InfoMailing.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegarmBot_Jmenka.Database
{
	public class UsersDataController
	{
		const string DATABASE_PATH = "../../../MockDatabase/";
		
		public static UserInfo GetOrCreateUserInfo(long userId)
		{
			return BaseDBRequest<UserInfo>(
			() =>
			{
				string path = $"{DATABASE_PATH}{userId}.json";
				if (System.IO.File.Exists(path))
				{
					string json = System.IO.File.ReadAllText(path);
					UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(json);
					userInfo.DownloadData();
					return userInfo;
				}
				else
				{
					return new UserInfo(userId, userId);
				}
			},
			(database) =>
			{
				UserInfo? userInfo = database.Users.SingleOrDefault(x => x.UserId == userId);
				if (userInfo is not null) 
				{
					userInfo.DownloadData();
					return userInfo;
				}
				else
				{
					return new UserInfo(userId, userId);
				}
			});
		}
		public static UserInfo GetUserInfo(long userId)
		{
			return BaseDBRequest<UserInfo>(
			() =>
			{
				string path = $"{DATABASE_PATH}{userId}.json";
				if (System.IO.File.Exists(path))
				{
					string json = System.IO.File.ReadAllText(path);
					UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(json);
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
				UserInfo? userInfo = database.Users.SingleOrDefault(x => x.UserId == userId);
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

		public static void SetUserInfo(UserInfo userInfo)
		{
			if (userInfo is null) return;
			userInfo.UploadData();

			BaseDBRequest(
			() =>
			{
				string path = $"{DATABASE_PATH}{userInfo.UserId}.json";

				string json = JsonConvert.SerializeObject(userInfo);

				System.IO.File.WriteAllText(path, json);
			},
			(database) => 
			{
				database.Users.AddOrUpdate(userInfo);
				database.SaveChanges();
			});
		}

		public static IEnumerable<TResult> GetAllUsers<TResult>(Func<UserInfo, TResult> func)
		{
			return BaseDBRequest<IEnumerable<TResult>>(
			() =>
			{
				var users = Directory.GetFiles(DATABASE_PATH)
				.Select(x => JsonConvert.DeserializeObject<UserInfo>(System.IO.File.ReadAllText(x)));

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
