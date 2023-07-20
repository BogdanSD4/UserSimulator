using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotSettings.Database;

namespace InfoMailing.Data
{

    public class ChatInfo
    {
		static readonly string path = "../../../owners.json";
		private static ChatInfo instance;
        public static ChatInfo Instanse { get 
            {
                if(instance is null)
                {
                    string text = File.ReadAllText(path);
					var json = JsonConvert.DeserializeObject<IDictionary<long, User>>(text);
					instance = ChatsDataController.GetOrCreateUserInfo() ?? new ChatInfo();

                    foreach (var item in json)
                    {
                        if (!instance.Owners.ContainsKey(item.Key))
                        {
                            instance.Owners.Add(item);
                        }
					}
				}

                return instance;
            } 
        }
        private ChatInfo()
        {
            Owners = new Dictionary<long, User>();
        }

        public int Id { get; set; }
        
        public string BotChats { get; set; }
       
        public IDictionary<long, User> Owners { get; set; }

        public void AddChat(long id)
        {
            if (!ChatExist(id))
            {
				BotChats += $"{id};";
			}
        }
        public IEnumerable<long>? GetChats()
        {
            var list = BotChats;

			if (list is null) return null;

            return list.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x));
        }
        public void RemoveChat(long id)
        {
            var list = BotChats.Split(";").ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == id.ToString())
                {
                    list.RemoveAt(i);
                    break;
                }
            }
            BotChats = string.Join(";", list);
        }
        public bool ChatExist(long chatId)
        {
            if(BotChats is null) return false;
            return BotChats.Split(';').Contains(chatId.ToString());
        }

        public void DownloadOwnerList()
        {
			string text = File.ReadAllText(path);
			var json = JsonConvert.DeserializeObject<IDictionary<long, User>>(text);

			var dictionary = new Dictionary<long, User>();

			foreach (var item in json)
			{
                if (instance.Owners.ContainsKey(item.Key))
                {
                    dictionary.Add(item.Key, instance.Owners[item.Key]);
                    continue;
                }

				dictionary.Add(item.Key, item.Value);
			}

            instance.Owners = dictionary;
		}
		public void UploadOwnerList()
		{
			string text = File.ReadAllText(path);
			var json = JsonConvert.DeserializeObject<IDictionary<long, User>>(text);

			var dictionary = instance.Owners;

            foreach (var item in dictionary)
            {
                if (!json.ContainsKey(item.Key))
                {
                    json.Add(item.Key, item.Value);
                }
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(json));
		}

		public void DownloadData()
        {
            
        }
        public void UploadData()
        {

        }

        #region DB_Data_Converter
        public TResult GetData<TResult>(string file)
        {
            return JsonConvert.DeserializeObject<TResult>(file);
        }
        public string? SetData(object value)
        {
            if (value is null) return null;
            return JsonConvert.SerializeObject(value);
        }
        #endregion
    }
}
