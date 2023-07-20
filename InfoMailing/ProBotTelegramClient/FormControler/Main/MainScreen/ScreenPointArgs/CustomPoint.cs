using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Main.MainScreen.ScreenPointArgs
{
	public partial class CustomPoint : BasePoint
	{
		public CustomPoint(Panel panel)
		{
			Body = panel;
		}

		public override void Delete()
		{
			Remove?.Invoke();
			Body?.Dispose();
		}
	}

	public partial class CustomPoint<T> : BasePoint
	{
		public CustomPoint(Panel panel, T value)
		{
			Body = panel;
			Value = value;
		}

		public T Value { get; set; }
		public new Action<T> Remove { get; set; }

		public override void Delete()
		{
			Remove?.Invoke(Value);
			Body?.Dispose();
		}
	}
}
