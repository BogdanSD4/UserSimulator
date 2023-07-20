using FileSystemTree;
using Newtonsoft.Json;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.FormControler.Forms.FormQuestion;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using ProBotTelegramClient.FormControler.Main.Scripts;
using ProBotTelegramWinForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.CustomComands.CommandVarians
{
    public class CommandCollection : BaseCommand, ICollectionObject
    {
        [JsonConstructor]
        public CommandCollection(string preferanceSave) : base(preferanceSave) { }
        public CommandCollection(BasePreferance preferance) : base(preferance) { }

        public static readonly string MainCollectionName = "Main"; 
        [JsonIgnore]
        public string CollectionName { get; set; }
        [JsonIgnore]
        public List<BaseCommand> Commands { get; set; }
        [JsonIgnore]
        public ICollectionObject previous { get; set; }
        public List<KeyValuePair<Type, string>> jsonSave { get; set; }

        private static event Action _afterLoadEvent;
        public static event Action AfterLoadEvent
        {
            add
            {
                _afterLoadEvent += value;
            }
            remove
            {
                _afterLoadEvent -= value;
            }
        }

        protected override void BaseSettings()
        {
            CollectionName = Preferance.Name;

            Commands = new List<BaseCommand>();
            jsonSave = new List<KeyValuePair<Type, string>>();

			base.BaseSettings();
		}

        public static CommandCollection Get(BasePreferance preferance)
        {
            using (var stream = FileManager.Read(preferance.FullName))
            {
                string data = stream.ReadToEnd();
                if (data is "")
                {
                    var command = new CommandCollection(preferance);
                    return command;
                }
                else
                {
                    var command = JsonConvert.DeserializeObject<CommandCollection>(data);
                    command.Load();

                    _afterLoadEvent?.Invoke();
                    return command;
                }
            }
        }

        public override string Save()
        {
            foreach (var item in Commands)
            {
                jsonSave.Add(new KeyValuePair<Type, string>(item.GetType(), item.Save()));
            }

            base.Save();

            return Preferance.FullName;
        }
        public override void Load()
        {
            foreach (var item in jsonSave)
            {
                if (FileManager.Exist(item.Value))
                {
                    using (var stream = FileManager.Read(item.Value))
                    {
                        var json = stream.ReadToEnd();
                        var data = (BaseCommand)JsonConvert.DeserializeObject(json, item.Key);
                        data.Load();
                        Commands.Add(data);
                    }
				}
            }

            base.Load();
            jsonSave.Clear();
        }

        public void Add(BaseCommand command)
        {
            Commands.Add(command);
        }
        public void Remove(BaseCommand command)
        {
            command.RemoveSavedDatas();
            Commands.Remove(command);
        }

        public override void RemoveSavedDatas()
        {
            foreach (BaseCommand item in Commands)
            {
                item.RemoveSavedDatas();
            }
            FileManager.Delete(Preferance.FullName);
        }

        public string GetExportFile(string name)
        {
            jsonSave.Clear();

            foreach(var item in Commands)
            {
                item.SavePreferance();
                jsonSave.Add(new KeyValuePair<Type, string>(item.GetType(), JsonConvert.SerializeObject(item)));
            }

			(Type, string) prefJson = (typeof(CommandCollectionPreferance), JsonConvert.SerializeObject(new CommandCollectionPreferance(name, Preferance.Extension)));
			PreferanceSave = JsonConvert.SerializeObject(prefJson);

            string json = JsonConvert.SerializeObject(this);

            PreferanceSave = null;
            jsonSave.Clear();

            return json;
        }

        public override Control DrawDisplay(int width, int height, int x, int y)
        {
			ScreenSettings settings = ScreenSettings.CommandSettings;

			Panel panel = base.DrawDisplay(width, height, x, y) as Panel;
			panel.DoubleClick += (s, e) =>
            {
                MainForm.Instance.ChangeKeyAssignment(this);
            };

			foreach (Control item in panel.Controls)
			{
				switch (item.Name)
				{
					case "Delete":
						{
							item.Click += (s, e) =>
							{
								var set = new QuestionForm($"Delete this command: {Preferance.Name}");

								List<IAnswer> answer = new List<IAnswer>()
								{
									new Answer<Form>("Yes", (f) =>
									{
										panel.Dispose();
										MainForm.Instance.keyAssignmentForm.RemoveCommand(this);
										f.Close();
									}, set),
									new Answer<Form>("No", (f) => { f.Close(); }, set),
								};

								set.Answers = answer;
								set.Open();
							};
						}
						break;
					case "Settings":
						{

						}
						break;
					case "Services":
						{

						}
						break;
				}
			}

            return panel;
        }
    }
}
