using Google.Apis.Drive.v3;
using ProBotTelegramClient.Instruments.GoogleServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleServices.GoogleServices
{
	public class GoogleDisk
	{
		internal static DriveService _driveService;

		public static DriveService DriveService
		{
			get
			{
				if (!GoogleService.IsInitialized) throw new Exception("Google service not initialized");

				return _driveService;
			}
		}

		public static void UpLoad(FileBody body)
		{
			using (MemoryStream stream2 = new MemoryStream())
			{
				FilesResource.CreateMediaUpload request = DriveService.Files.Create(body, stream2, body.MimeType);
			}
		}
	}
}
