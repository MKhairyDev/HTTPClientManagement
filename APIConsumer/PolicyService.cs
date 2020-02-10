using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.Bulkhead;
using Polly.Retry;

namespace APIConsumer
{
    public class PolicyService: IPolicyService
    {
        public IAsyncPolicy<HttpResponseMessage> HttpRetryPolicy { get;  set; }
        public IAsyncPolicy<HttpResponseMessage> BulkheadPolicy { get;  set; }
        public PolicyService()
        {
            HttpRetryPolicy =
               Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
               .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt), (response, timespan) =>
               {
                   var result = response.Result;
                   // log the result
               });
            BulkheadPolicy =
                Policy.BulkheadAsync<HttpResponseMessage>(2, 4,  OnBulkheadRejectedAsync );
        }

        private Task OnBulkheadRejectedAsync(Context arg)
        {
            //do something here.
            return Task.CompletedTask;
        }
    }
}
