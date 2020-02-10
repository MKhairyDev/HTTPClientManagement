using System.Net.Http;
using Polly;
using Polly.Wrap;

namespace APIConsumer.Services
{
    public interface IPolicyService
    {
        IAsyncPolicy<HttpResponseMessage> TimeOutPolicy { get; set; }
        IAsyncPolicy<HttpResponseMessage> HttpRetryPolicy { get; set; }
        IAsyncPolicy<HttpResponseMessage> FallBackPolicy { get; set; }
        IAsyncPolicy<HttpResponseMessage> BulkheadPolicy { get; set; }
        AsyncPolicyWrap<HttpResponseMessage> PolicyWrap { get; set; }
    }
}