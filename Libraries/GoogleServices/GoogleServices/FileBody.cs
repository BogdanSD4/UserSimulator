using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleServices.GoogleServices
{
	public class FileBody : Google.Apis.Drive.v3.Data.File
	{
		public override string Name
		{
			get => base.Name;
			set
			{
				base.Name = value;

				var extension = Path.GetExtension(value);
				base.MimeType = GetMimeType(extension);
			}
		}

		private string GetMimeType(string extansion)
		{
			switch (extansion)
			{
				case ".avi":
					return "video/x-msvideo";
				case ".css":
					return "text/css";
				case ".doc":
					return "application/msword";
				case ".html":
					return "text/html";
				case ".bmp":
					return "image/bmp";
				case ".gif":
					return "image/gif";
				case ".jpeg":
					return "image/jpeg";
				case ".jpg":
					return "image/jpeg";
				case ".docx":
					return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
				case ".pdf":
					return "application/pdf";
				case ".ppt":
					return "application/vnd.ms-powerpoint";
				case ".pptx":
					return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
				case ".xls":
					return "application/vnd.ms-excel";
				case ".xlsx":
					return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				case ".txt":
					return "text/plain";
				case ".zip":
					return "application/zip";
				case ".rar":
					return "application/x-rar-compressed";
				case ".mp3":
					return "audio/mpeg";
				case ".mp4":
					return "video/mp4";
				case ".png":
					return "image/png";
				default:
					return "application/octet-stream";
			}
		}
	}
}
