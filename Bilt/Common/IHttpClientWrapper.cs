using System.Net.Http;
using System.Threading.Tasks;

namespace Bilt.Common
{
	public interface IHttpClientWrapper
	{
		Task<HttpResponseMessage> RequestAsync(HttpRequestMessage httpRequestMessage);
	}
}
