using Newtonsoft.Json;
using ProBotTelegramClient.CustomComands.CommandVarians;
using ProBotTelegramClient.CustomComands;
using ProBotTelegramClient.FormControler.Forms;
using ProBotTelegramClient.FormControler.Main.ErrorLable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystemTree;

namespace ProBotTelegramClient.FormControler.Main.Scripts.ImpExp
{
	public class ImportController
	{
        public string Import()
		{
			var openDialog = new OpenFileDialog();
			if (openDialog.ShowDialog() is DialogResult.OK)
			{
				if (Path.GetExtension(openDialog.FileName) == ".ccs")
				{
					return File.ReadAllText(openDialog.FileName);
				}
				else
				{
					ErrorBox.Message("Format is not supported");
				}
			}
			return "";
		}

		public static CommandCollection ConvertImportFileToCollection(string file)
		{
			var collection = JsonConvert.DeserializeObject<CommandCollection>(file);

			if (collection.jsonSave is null || collection.jsonSave.Count == 0) return collection;

			foreach (var item in collection.jsonSave)
			{
				var command = (BaseCommand)JsonConvert.DeserializeObject(item.Value, item.Key);
				command.Load();

				if (FileManager.Exist(command.Preferance.FullName))
				{
					Application.Run(new ReplaceForm());
				}
				else
				{
					command.Save();
					collection.Commands.Add(command);
				}
			}

			return collection;
		}
	}
}
