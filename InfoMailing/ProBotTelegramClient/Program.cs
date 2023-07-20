using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;

namespace ProBotTelegramClient
{
	internal static class Program
	{
		[STAThread]
		static void Main()
		{
			ApplicationConfiguration.Initialize();

			Form form = new MainForm();
			Startup.Initial(form);
		}
	}
}