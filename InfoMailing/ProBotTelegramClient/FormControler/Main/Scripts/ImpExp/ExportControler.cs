using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Main.Scripts.ImpExp
{
    public class ExportControler
    {
        public ExportControler(Func<SaveFileDialog, string> getExportData)
        {
            GetExportData = getExportData;
        }

		private Func<SaveFileDialog, string> GetExportData;

        public void Export()
		{
			var saveDialog = new SaveFileDialog();
			saveDialog.DefaultExt = ".ccs";
			saveDialog.Filter = "Command Collection Save (*.ccs)|*.ccs";

			BaseExport(saveDialog);
		}
		public void Export(string name)
		{
			var saveDialog = new SaveFileDialog();
			saveDialog.DefaultExt = ".ccs";
			saveDialog.FileName = name;
			saveDialog.Filter = "Command Collection Save (*.ccs)|*.ccs";

			BaseExport(saveDialog);
		}
		private void BaseExport(SaveFileDialog dialog)
		{
			if (dialog.ShowDialog() is DialogResult.OK)
			{
				string path = dialog.FileName;

				string data = GetExportData.Invoke(dialog);

				File.WriteAllText(path, data);
			}
		}
	}
}
