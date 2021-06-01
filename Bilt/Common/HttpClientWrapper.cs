using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bilt.Common
{
	public class HttpClientWrapper : IHttpClientWrapper
	{
		private readonly HttpClient httpClient = new HttpClient();

		public async Task<HttpResponseMessage> RequestAsync(HttpRequestMessage httpRequestMessage)
		{
			//return await httpClient.SendAsync(httpRequestMessage);
			return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
		}
	}
}
