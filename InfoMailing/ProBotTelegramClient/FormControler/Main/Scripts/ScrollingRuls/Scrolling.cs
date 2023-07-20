using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Main.Scripts.ScrollingRuls
{
	public class Scrolling
	{
        public Scrolling(ScrollSettings scrollSettings)
        {
            ScrollSettings = scrollSettings;
        }

		public ScrollSettings ScrollSettings { get; set; }

        public void Activate(Panel panel)
		{
			panel.MouseWheel += (s, e) => 
			{
				HorizontalScroll(panel, e); 
			};
		}

		private void HorizontalScroll(Panel sender, MouseEventArgs e)
		{
			int start = ScrollSettings.typePanelStart;
			int res = e.Delta / 4;

			int left = sender.Controls[0].Left;
			if (left + res >= start)
			{
				if (res > 0)
				{
					if (left != start)
					{
						int val = start - left;
						foreach (Control c in sender.Controls)
						{
							c.Left += val;
						}
					}
					return;
				}
			}

			int right = sender.Controls[sender.Controls.Count - 1].Right;
			int typePanelEnd = sender.Width - start;
			if (right + res <= typePanelEnd)
			{
				if (res < 0)
				{
					if (right != typePanelEnd)
					{
						int val = typePanelEnd - right;
						foreach (Control c in sender.Controls)
						{
							c.Left += val;
						}
					}
					return;
				}
			}

			foreach (Control c in sender.Controls)
			{
				c.Left += res;
			}
		}
	}
}
