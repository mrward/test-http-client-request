using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace TestHttpClientRequest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			try {
				if (args.Length == 0) {
					Console.WriteLine ("TestHttpClientRequest url");
					return;
				}

				string url = args[0];
				Run (url).Wait ();
			} catch (Exception ex) {
				var baseException = ex.GetBaseException ();
				Console.WriteLine (baseException);
			}
		}

		static async Task Run (string url)
		{
			ServicePointManager.ServerCertificateValidationCallback = ServerCertificateValidationCallback;
			var httpClient = new HttpClient ();
			using (var response = await httpClient.GetAsync (url)) {
				if (response.StatusCode == HttpStatusCode.BadRequest) {
					Console.WriteLine ("Unexpected Bad request status code: {0} {1}", response.StatusCode, (int)(response.StatusCode));
				} else if (response.StatusCode == HttpStatusCode.Unauthorized) {
					Console.WriteLine ("Got expected unauthorized status code: {0} {1}", response.StatusCode, (int)(response.StatusCode));
				} else {
					Console.WriteLine (response.StatusCode);
				}
			}
		}

		static bool ServerCertificateValidationCallback (object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}
	}
}
