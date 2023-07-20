using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.CustomComands.CommandsSettings.ServiceSettings
{
	public class ServiceController
	{
        [JsonConstructor]
        public ServiceController(string json)
        {
            Json = json;
            Load(Json);
        }
        public ServiceController(BaseCommand command, IEnumerable<ServicePreferance> servicePreferanses) 
        {
            ServicePreferanse = servicePreferanses;
            LoadCommandData(command);
        }

        public string Json { get; set; }
        
        [JsonIgnore]
        public IEnumerable<ServicePreferance> ServicePreferanse { get; private set; }

        public void Save()
        {
            List<(Type ,string)> sb = new List<(Type, string)>();
            foreach (var item in ServicePreferanse)
            {
                sb.Add((item.GetType(), item.GetJson()));
            }
            Json = JsonConvert.SerializeObject(sb);
        }
        public void Load(string json)
        {
            if (string.IsNullOrEmpty(json)) return;

            List<ServicePreferance> services = new List<ServicePreferance>();
            var list = JsonConvert.DeserializeObject<List<(Type, string)>>(json);
            foreach (var item in list)
            {
                services.Add((ServicePreferance)JsonConvert.DeserializeObject(item.Item2, item.Item1));
            }

            if(services.Count == 0) ServicePreferanse = new ServicePreferance[] {};
            else ServicePreferanse = services;
        }
        public void LoadCommandData(BaseCommand command)
        {
            foreach (var item in ServicePreferanse)
            {
                item.LoadCommandData(command);
            }
        }
    }
}
