using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Instruments
{
	public static class FileManager
	{
		private static readonly string appConfigPath = "../../../appSettings.config";
		public static Configuration GetAppSettings()
		{
			ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
			fileMap.ExeConfigFilename = appConfigPath;
			Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			return configuration;
		}

		public static byte[] ConvertToXlsxReturnFilePath(IEnumerable<string> array) 
		{
			string xlsxFilePath = "../../../file.xlsx";
			var list = array.ToArray();

			using (XLWorkbook workbook = new XLWorkbook())
			{
				IXLWorksheet worksheet = workbook.Worksheets.Add("Sheet1");

				IXLRange range = worksheet.Range("A1:E1").Merge();
				range.Value = "Links";
				range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

				int startRow = 3;
				for (int i = 0; i < list.Length; i++)
				{
					IXLRange newCells = worksheet.Range($"A{i+startRow}:E{i+startRow}").Merge();
					newCells.Value = list[i];
				}

				workbook.SaveAs(xlsxFilePath);
			}

			var result = System.IO.File.ReadAllBytes(xlsxFilePath);
            System.IO.File.Delete(xlsxFilePath);
			return result;
		}
	}
}
