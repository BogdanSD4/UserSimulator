using Newtonsoft.Json;
using ProBotTelegramClient.CustomComands;
using ProBotTelegramClient.CustomComands.CommandsSettings;
using ProBotTelegramClient.CustomComands.CommandVarians;
using ProBotTelegramClient.FormControler;
using ProBotTelegramClient.FormControler.Forms.AddCommand;
using ProBotTelegramClient.FormControler.Forms.FormQuestion;
using ProBotTelegramClient.FormControler.Main.Scripts.ImpExp;
using ProBotTelegramWinForm;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace ProBotTelegramClient.FormControler.Main.Scripts
{
    public partial class KeyAssignmentForm
    {
        private int startIntervalX = 5;
        private int startIntervalY = 5;
        private int endIntervalX = 25;
        private int endIntervalY = 5;

        private int intervalX = 5;
        private int intervalY = 5;

        private int maxInRow = 3;
        private int maxColumn = 4;

        private int cellWidth = 180;
        private int cellHeight = 100;
        private int currentCellX;
        private int currentCellY;

        public Panel parent { get; set; }

        public ICollectionObject currentCollection { get; private set; }
        private Control last;

        public KeyAssignmentForm(Panel panel, CommandCollection command)
        {
            currentCollection = command;
            parent = panel;

            Initiate();

            MainForm.Instance.FormClosing += (s, e) =>
            {
				command.Save();
            };
        }

        private void Initiate()
        {
			if (currentCollection.Commands is not null)
			{
                MainForm.Instance.nameField.Text = currentCollection.Path();
				parent.Controls.Clear();
                last = null;

				int counter = 1;
				foreach (var item in currentCollection.Commands)
				{
					CreateCommand(item, counter);
					counter++;
				}
			}
		}
        public void OpenCollection(ICollectionObject collection)
        {
            collection.previous = currentCollection; 
            currentCollection = collection;
            Initiate();
        }
        public Control CreateCommand(BaseCommand command, int index = 0)
        {
            Point point = index == 0 ? Position() : Position(index);

            var panel = command.Display(cellWidth, cellHeight, point.X, point.Y);
            last = panel;

			new MethodInvoker(() => { parent.Controls.Add(panel); }).Invoke();
			return panel;
        }
        public void RemoveCommand(BaseCommand command)
        {
            currentCollection.Remove(command);
            Reordered();
        }

        public void Reordered()
        {
            last = null;
            int counter = 1;
            foreach (Control item in parent.Controls)
            {
                Point point = Position(counter);
                item.Left = point.X;
                item.Top = point.Y;

                last = item;
                counter++;
            }
        }
        public void Previous()
        {
            if(currentCollection.previous is not null)
            {
				currentCollection = currentCollection.previous;
				Initiate();
			}
        }
        public void OpenRoot() 
        {
            if (currentCollection is null) return;

            currentCollection = currentCollection.Root;
            Initiate();
        }

        private Point Position(int index = 0)
        {
            if (last is null)
            {
                currentCellX = startIntervalX;
                currentCellY = startIntervalY;
            }
            else
            {
                int controlIndex = index == 0 ? currentCollection.Commands.Count : index;
                int inCurrentRow = controlIndex % maxInRow;

                if (inCurrentRow == 1 && controlIndex > maxInRow)
                {
                    currentCellX = startIntervalX;
                    currentCellY = last.Location.Y + cellHeight + intervalY;
                }
                else
                {
                    currentCellX = last.Location.X + cellWidth + intervalX;
                    currentCellY = last.Location.Y;
                }
            }

            return new Point(currentCellX, currentCellY);
        }

		#region InterFace Methods
		public void OpenCommandMenu()
        {
            new AddCommandForm(parent).Open();
        }
        public void AddCommand(BaseCommand command)
        {
            currentCollection.Add(command);
            CreateCommand(command);
        }
		#endregion

     //   public string ExportCollection(string name)
     //   {
     //       return currentCollection.GetExportFile(name);
     //   }
     //   public void ImportCollection(string file)
     //   {
     //       var quest = new QuestionForm("How to import this collection?");

     //       Answer[] answers = new Answer[]
     //       {
     //           new Answer("As current", () => 
     //           {
					//var asCurrentQuest = new QuestionForm("Are you sure?\nAll current commands will be deleted");

     //               Answer[] asCurrentAnswer = new Answer[]
     //               {
     //                   new Answer("Yes", () => 
     //                   {
     //                       var asMainYesQuest = new QuestionForm("Save current command collection?");

     //                       Action callBack = () => 
     //                       {
     //                           foreach (var item in currentCollection.Commands)
     //                           {
     //                               item.RemoveSavedDatas();
     //                           }

					//			var collection = ImportController.ConvertImportFileToCollection(file);

					//			currentCollection.Commands = collection.Commands;

     //                           asMainYesQuest.Close();
     //                           asCurrentQuest.Close();
					//			quest.Close();

     //                           Initiate();
					//		};

     //                       Answer[] asMainYesAnswer = new Answer[]
     //                       {
     //                           new Answer("Yes", () =>
     //                           {
     //                               ExportControler export = new ExportControler((s) => 
     //                               {
     //                                   string name = Path.GetFileNameWithoutExtension(s.FileName);
					//					return ExportCollection(name);
					//				});
     //                               export.Export(file);

     //                               callBack();
     //                           }),
     //                           new Answer("No", callBack),
     //                       };

     //                       asMainYesQuest.Answers = asMainYesAnswer;
     //                       asMainYesQuest.Open();
     //                   }),
     //                   new Answer("No", () => { asCurrentQuest.Close(); })
     //               };

     //               asCurrentQuest.Answers = asCurrentAnswer;
     //               asCurrentQuest.Open();
     //           }),
     //           new Answer("As new collection", () => 
     //           {
					//var collection = ImportController.ConvertImportFileToCollection(file);

					//currentCollection.Add(collection);

     //               Initiate();
     //           }),
     //       };

     //       quest.Answers = answers;
     //       quest.Open();
     //   }
	}
}
