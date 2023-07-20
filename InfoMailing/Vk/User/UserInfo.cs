using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoMailing.User
{

    public class UserInfo
    {
        public UserInfo() { }
        public UserInfo(long userId, long chatId)
        {
            UserId = userId;
            ChatId = chatId;
        }
        public int Id { get; set; }
        public long UserId { get; set; }
        public long ChatId { get; set; }
        public bool MenuEnabel { get; set; }
        public string BotChats { get; set; }
        public string LastMessage { get; set; }

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
