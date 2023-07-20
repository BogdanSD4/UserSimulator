using ProBotTelegramClient.CustomComands;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Main.Scripts
{
	public interface ICollectionObject
	{
		public string CollectionName { get; }
		public ICollectionObject previous { get; set; }
		public List<BaseCommand> Commands { get; set; }
		public ICollectionObject Root { get
			{
				if (previous is null) return this;
				return previous.Root;
			} 
		}

		public abstract void Add(BaseCommand command);
		public abstract void Remove(BaseCommand command);

		public string Path()
		{
			if(previous is null)
			{
				return CollectionName;
			}
			else
			{
				return $"{previous.Path()}/{CollectionName}";
			}
		}

		public abstract string GetExportFile(string fileName);
	}
}
