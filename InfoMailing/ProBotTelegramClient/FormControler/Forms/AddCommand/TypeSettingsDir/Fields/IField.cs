using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields
{
	public interface IField
	{
		public Control LastDrawed { get; set; }
		protected IEnumerable<BaseField> BaseFields { get; set; }
		public abstract Func<bool> Show(Panel screen);
	}
}
