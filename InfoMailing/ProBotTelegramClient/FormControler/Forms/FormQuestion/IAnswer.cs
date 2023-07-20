using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Forms.FormQuestion
{
	public interface IAnswer
	{
		public string Name { get; set; }
		public abstract void Invoke();
	}
}
