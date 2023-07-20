using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Http;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using GoogleServices.GoogleServices;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace ProBotTelegramClient.Instruments.GoogleServices
{
	public class GoogleService
	{
		private GoogleService()
		{

		}

		public static readonly string CredentialSavePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Personal)}\\.credentials/user";

		private static UserCredential? _userCredential;
		private static Oauth2Service? _auth2Service;
		private static Userinfo? _currentUserInfo;

		public static bool IsInitialized
		{
			get => _userCredential is not null;
		}
		public static bool HaveUserInfoCache
		{
			get
			{
				if (Directory.Exists(CredentialSavePath))
				{
					if (Directory.GetFiles(CredentialSavePath).Length > 0) return true;
				}

				return false;
			}
		}


		public static UserCredential Credential
		{
			get
			{
				if (!IsInitialized) throw new Exception("Google service not initialized");

				return _userCredential;
			}
		}
		public static Oauth2Service Oauth2Service
		{
			get
			{
				if (!IsInitialized) throw new Exception("Google service not initialized");

				return _auth2Service;
			}
		}
		public static Userinfo Userinfo
		{
			get
			{
				if (!IsInitialized) throw new Exception("Google service not initialized");

				return _currentUserInfo;
			}
		}

		public static Userinfo Connect(string clientSecretPath, Action<bool>? responce = null)
		{
			string[] Scopes =
			{
				DriveService.Scope.Drive,
				Oauth2Service.Scope.UserinfoEmail,
			};

			var cancellationTokenSource = new CancellationTokenSource();
			var cancellationToken = cancellationTokenSource.Token;

			_userCredential = AuthorizeAsync(clientSecretPath, Scopes, cancellationToken).Result;

			bool result = _userCredential is not null;

			if (!result) return null;
			else
			{
				responce?.Invoke(result);
			}

			var initializer = new BaseClientService.Initializer
			{
				HttpClientInitializer = _userCredential,
			};

			_auth2Service = new Oauth2Service(initializer);
			GoogleDisk._driveService = new DriveService(initializer);

			_currentUserInfo = _auth2Service.Userinfo.Get().Execute();

			return _currentUserInfo;
		}
		public static void Disconnect()
		{
			Directory.Delete(CredentialSavePath, true);
			_userCredential = null;
		}

		public static async Task<UserCredential> AuthorizeAsync(string clientSecretPath, string[] scopes, CancellationToken cancellationToken)
		{
			using (FileStream stream = new FileStream(clientSecretPath, FileMode.Open, FileAccess.Read))
			{
				var secrets = GoogleClientSecrets.FromStream(stream).Secrets;
				var data = new FileDataStore(CredentialSavePath, true);

				return await GoogleWebAuthorizationBroker.AuthorizeAsync(
					secrets,
					scopes,
					secrets.ClientId,
					cancellationToken,
					data).WaitAsync(cancellationToken);
			}
		}
	}
}
