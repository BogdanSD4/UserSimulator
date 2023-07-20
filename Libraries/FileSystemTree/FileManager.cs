using System.Diagnostics;

namespace FileSystemTree
{
	public static class FileManager
	{
		private static bool _initialized;
		public static string MainPath = "C:\\Users\\dokto\\OneDrive\\Рабочий стол\\TelegramBotKPI\\Libraries\\FileSystemTree\\Datas\\";
		public static void Initialize()
		{
			_initialized = true;
			
			var directories = Directory.GetFiles(MainPath);
			dataContainer = new Dictionary<string, string>();

			foreach ( var directory in directories )
			{
				string name = Path.GetFileName(directory);
				Debug.WriteLine(name + " | " + directory);
				dataContainer.Add(name, directory);
			}
		}
		public static StreamReader Read(string fileNameWithExt)
		{
			return BaseStreamWork(fileNameWithExt, (boolean) => 
			{
				if (boolean)
				{
					return new StreamReader(dataContainer[fileNameWithExt]);
				}
				else
				{
					string path = $"{MainPath}{fileNameWithExt}";
					dataContainer.Add(fileNameWithExt, path);
					File.Create(path).Close();
					return new StreamReader(path);
				}
			});
		}
		public static StreamWriter Write(string fileNameWithExt)
		{
			return BaseStreamWork(fileNameWithExt, (boolean) =>
			{
				if (boolean)
				{
					return new StreamWriter(dataContainer[fileNameWithExt]);
				}
				else
				{
					string path = $"{MainPath}{fileNameWithExt}";
					dataContainer.Add(fileNameWithExt, path);
					File.Create(path).Close();
					return new StreamWriter(path);
				}
			});
		}
		public static Stream Open(string fileNameWithExt)
		{
			return BaseStreamWork(fileNameWithExt, (boolean) =>
			{
				if (boolean)
				{
					return File.Open(dataContainer[fileNameWithExt], FileMode.Open, FileAccess.ReadWrite);
				}
				else
				{
					string path = $"{MainPath}{fileNameWithExt}";
					dataContainer.Add(fileNameWithExt, path);
					File.Create(path).Close();
					return File.Open(path, FileMode.Open, FileAccess.ReadWrite);
				}
			});
		}

		private static TResult BaseStreamWork<TResult>(string fileName, Func<bool, TResult> func)
		{
			if (!_initialized) Initialize();
			if (dataContainer.ContainsKey(fileName))
			{
				return func(true);
			}
			else
			{
				return func(false);
			}
		}

		public static void Delete(string fileNameWithExt)
		{
			if (fileNameWithExt is null || fileNameWithExt == "") return;

			string path = $"{MainPath}{fileNameWithExt}";

			if (dataContainer is not null && dataContainer.ContainsKey(fileNameWithExt))
			{
				dataContainer.Remove(fileNameWithExt);
			}

			File.Delete(path);
		}

		public static bool Exist(string fileNameWithExt)
		{
			string path = $"{MainPath}{fileNameWithExt}";
			return File.Exists(path);
		}

		private static IDictionary<string, string> dataContainer { get; set; }
	}
}