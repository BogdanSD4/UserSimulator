using ProBotTelegramClient.FormControler.Main.ErrorLable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = System.Drawing.Point;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields
{
	public class FieldDrop : FieldText
	{
		public FieldDrop(string fieldName, string fieldText) : base(fieldName, fieldText) { }
		public FieldDrop(string fieldName, string fieldText, Func<FieldText, bool> func) : base(fieldName, fieldText, func) { }

		public override void InitializeComponets()
		{
			base.InitializeComponets();

			textBox.PlaceholderText = "Drop file";
			textBox.AllowDrop = true;
			textBox.ReadOnly = true;
			textBox.MouseMove += (s, e) =>
			{
				Point objectMousePosition = textBox.PointToClient(Cursor.Position);

				int objectCenterX = textBox.Width / 2;

				if (objectMousePosition.X < objectCenterX)
				{
					if (textBox.SelectionStart > 0) textBox.SelectionStart--;
				}
				else
				{
					textBox.SelectionStart++;
				}
			};
			textBox.DragEnter += (sender, e) =>
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
				{
					e.Effect = DragDropEffects.Copy;
				}
			};
			textBox.DragDrop += (s, e) =>
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
				{
					string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
					if (files.Length == 0) return;

					if (files.Length > 1)
					{
						ErrorBox.Message("Drop only one file");
						return;
					}

					textBox.Text = files[0];
				}
			};
		}
	}
}
