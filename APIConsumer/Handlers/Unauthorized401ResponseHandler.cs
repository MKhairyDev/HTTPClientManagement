using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace APIConsumer.Handlers
{
    public class Unauthorized401ResponseHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            return Task.FromResult(response);
        }
    }
}
