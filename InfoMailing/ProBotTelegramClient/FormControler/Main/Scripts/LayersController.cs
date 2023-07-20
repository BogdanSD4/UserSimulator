using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Main.Scripts
{
	public class LayersController
	{
		public static void Initial(Form mainForm)
		{
			HigeImportance = new List<Form>();
			AddTrackingForm(mainForm);
		} 

		public static void AddTrackingForm(Form form)
		{
			for (int i = 0; i < HigeImportance.Count; i++)
			{
				if (HigeImportance[i].IsDisposed)
				{
					HigeImportance.RemoveAt(i);
				}
			}

			form.Activated += (s, e) =>
			{
				foreach (var item in HigeImportance)
				{
					item.BringToFront();
				}
			};
		}

		public static List<Form> HigeImportance { get; private set; }
	}
}
