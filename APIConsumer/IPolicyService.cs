using Polly.Bulkhead;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Polly;

namespace APIConsumer
{
    public interface IPolicyService
    {
        IAsyncPolicy<HttpResponseMessage> HttpRetryPolicy { get; set; }
        IAsyncPolicy<HttpResponseMessage> BulkheadPolicy { get; set; }
    }
}
