using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.Timeout;
using Polly.Wrap;

namespace APIConsumer.Services
{
    public class PolicyService: IPolicyService
    {
        public IAsyncPolicy<HttpResponseMessage> TimeOutPolicy { get; set; }
        public IAsyncPolicy<HttpResponseMessage> HttpRetryPolicy { get;  set; }
        public IAsyncPolicy<HttpResponseMessage> FallBackPolicy { get; set; }
        public IAsyncPolicy<HttpResponseMessage> BulkheadPolicy { get;  set; }
        public AsyncPolicyWrap<HttpResponseMessage> PolicyWrap { get; set; }
        // Handle both exceptions and return values in one policy
        private readonly List<HttpStatusCode> _httpStatusCodesWorthRetrying = new List<HttpStatusCode>()
        {
            HttpStatusCode.RequestTimeout, // 408
            HttpStatusCode.InternalServerError, // 500
            HttpStatusCode.BadGateway, // 502
            HttpStatusCode.ServiceUnavailable, // 503
            HttpStatusCode.GatewayTimeout // 504
        };
        public PolicyService()
        {
            TimeOutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(1);
            HttpRetryPolicy =
                Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).Or<TimeoutRejectedException>()
                    .OrResult(r => _httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt), (response, timespan) =>
                    {
                        var result = response.Result;
                        // log the result
                    });
            FallBackPolicy =
                Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).Or<TimeoutRejectedException>()
                    .FallbackAsync(new HttpResponseMessage(HttpStatusCode.OK) 
                    {
                        //Do something here
                        
                    });
            BulkheadPolicy =
                Policy.BulkheadAsync<HttpResponseMessage>(2, 4,  OnBulkheadRejectedAsync );
            PolicyWrap = Policy.WrapAsync(FallBackPolicy, HttpRetryPolicy, TimeOutPolicy, BulkheadPolicy);
        }

        private Task OnBulkheadRejectedAsync(Context arg)
        {
            //do something here.
            return Task.CompletedTask;
        }
    }
}
