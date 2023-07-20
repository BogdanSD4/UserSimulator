using EyesSimulator.PhotoEditor;
using FileSystemTree;
using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json;
using ProBotTelegramClient.CustomComands.CommandVarians;
using ProBotTelegramClient.FormControler.Main.ErrorLable;
using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenPointArgs;
using ProBotTelegramClient.FormControler.Main.MainScreen.ScreenSettingsArgs;
using ProBotTelegramClient.FormControler.Main.Scripts.SelectorRuls;
using ProBotTelegramWinForm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProBotTelegramClient.FormControler.Main.MainScreen
{
    public class MainScreenController
    {
        private MainScreenController() { }

		private const string NAME = "MainScreen.txt";

		public static Panel ScreenPanel;
        public static ScreenSettings Settings;
        private static IScreenViewer? Selected;

		[JsonIgnore]
		public static MainList MainPointList { get; set; }

		private static int startScreenPoint;
		private static Control lastPoint;

		public static void Initial(Panel screen, ScreenSettings screenSettings = null)
        {
			ScreenPanel = screen;
            Settings = screenSettings?? ScreenSettings.MainScreenSettings;

			BaseSettings();
        }
		private static void BaseSettings()
		{
			MainPointList = new MainList();

			CommandCollection.AfterLoadEvent += () => 
			{
				using (var stream = FileManager.Read(NAME))
				{
					string data = stream.ReadToEnd();
					if (data != "" && data is not null)
					{
						MainPointList = LoadData(data);
					}
				}
				if (MainPointList?.Count > 0) ToMain();
			};

			MainForm.Instance.FormClosing += (s, e) =>
			{
				using (var stream = FileManager.Write(NAME))
				{
					string json = SaveData();
					stream.Write(json);
				}
			};
		}

        public static void Show(IScreenViewer viewer, ScreenSettings settings) 
        {
            if (ScreenPanel is null) throw new Exception("Controller was not initialized");

            Settings = settings;
            ScreenPanel.Padding = Settings.Padding;
			MainForm.Instance.screenName.Text = viewer.ViewerName;

			Selected = viewer;
			lastPoint = null;
			ScreenPanel.Controls.Clear();
			viewer.View(ScreenPanel);  
        }
		public static void Delete(IScreenViewer viewer)
		{
			if(Selected == viewer)
			{
				ToMain();
			}
		}
		public static void ToMain()
        {
			Selected = null;
			lastPoint = null;
			ScreenPanel.Controls.Clear();
			Settings = ScreenSettings.MainScreenSettings;
			MainForm.Instance.screenName.Text = "Root";

			if (MainPointList is null) return;
			DrawUI(MainPointList.Items);
		}
		
		public static void DrawUI(IEnumerable<MainPoint> points)
		{
			if (points is null) return;
			foreach (var item in points)
			{
				DrawMainPoint(item);
			}
		}
		public static void DrawMainPoint(MainPoint point)
		{
			lastPoint = point.DrawPoint(ScreenPanel, Settings, lastPoint);
		}
		public static void Reordered()
		{
			int start = Settings.Padding.Top;
			foreach (var item in MainPointList.Items)
			{
				item.Body.Top = start;
				start += item.Body.Height + Settings.IntervalY;

				if (item.IsOpen)
				{
					var control = item.Open(start);
					if (control is not null) lastPoint = control;
					start = lastPoint.Bottom + Settings.IntervalY;
				}
				else
				{
					lastPoint = item.Body;
				}
			}
		}
		public static void Remove(List<BasePoint> points)
		{
			foreach (var item in points)
			{
				item.Delete();
			}

			Reordered();
		}
		public static async void Play(Action? callback = null)
		{
			foreach (var item in MainPointList.Items)
			{
				await item.Execute();
			}

			callback?.Invoke();
		}

		public static void AddMainPoint(IScreenClient point)
		{
			MainPoint mainPoint = point.GetMainPoint();

			if (Selected is null)
			{
				mainPoint.Enable = true;
				mainPoint.Visible = true;

				MainPointList.Add(mainPoint);
			}
			else
			{
				if (Selected is IScreenField field)
				{
					field.AddCommand(mainPoint);
				}
				else ErrorBox.Message("Invalid field to write command");
			}

			DrawMainPoint(mainPoint);
		}

		private static string SaveData()
		{
			List<string> jsonlist = new List<string>();
			foreach (var item in MainPointList.Items) 
			{
				jsonlist.Add(item.Save());
			}
			return JsonConvert.SerializeObject(jsonlist);
		}
		private static MainList LoadData(string json)
		{
			List<string> res = JsonConvert.DeserializeObject<List<string>>(json);

			MainPoint[] points = new MainPoint[res.Count];

			for (int i = 0; i < res.Count; i++)
			{
				MainPointData data = JsonConvert.DeserializeObject<MainPointData>(res[i]);
				MainPoint point = new MainPoint(data.Name);
				point.Load(data.Datas);
				points[i] = point;
			}

			return new MainList(points);
		}
    }
}
