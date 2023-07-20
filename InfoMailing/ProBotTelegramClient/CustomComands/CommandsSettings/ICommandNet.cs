using ProBotTelegramClient.FormControler.Main.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.CustomComands.CommandsSettings
{
	public interface ICommandNet
	{
		/// <summary>
		/// Key = filename | Value = command
		/// </summary>
		public static IDictionary<string, BaseCommand> CommandNet { get; } = new Dictionary<string, BaseCommand>();
		public abstract void AddToNet();
		public abstract Task<bool> Execute();
	}
}
