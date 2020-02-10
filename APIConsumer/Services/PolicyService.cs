using System;
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
        public PolicyService()
        {
            TimeOutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(1);
            HttpRetryPolicy =
               Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).Or<TimeoutRejectedException>()
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
