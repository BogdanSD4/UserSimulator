using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Forms.FormQuestion
{
	public partial class Answer : IAnswer
	{
        public Answer(string text, Action action)
        {
            Name = text;
			Action = action;
        }

        public string Name { get; set; }
        public Action Action { get; set; }

		public void Invoke()
		{
			Action();
		}
	}

	public partial class Answer<TValue> : IAnswer
	{
		public Answer(string text, Action<TValue> action, TValue value)
		{
			Name = text;
			Action = action;
			Value = value;
		}

		public string Name { get; set; }
		public TValue Value { get; set; }
		public Action<TValue> Action { get; set; }

		public void Invoke()
		{
			Action(Value);
		}
	}
}
