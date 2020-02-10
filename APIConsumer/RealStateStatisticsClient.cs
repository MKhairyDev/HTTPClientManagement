using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using RealState.Models;
using Polly;
using Polly.Retry;
using Polly.Bulkhead;

namespace APIConsumer
{
    public class RealStateStatisticsClient
    {
        private readonly HttpClient _client;
        private readonly IPolicyService _policyService;
        public RealStateStatisticsClient(HttpClient client, IPolicyService policyService)
        {
            ;
            _client = client??throw new ArgumentNullException(nameof(client));
            _policyService = policyService ?? throw new ArgumentNullException(nameof(client));
            _client.BaseAddress = new Uri(ApiConstants.FundaUrlBase);
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();
        }

        public async Task<LocatieFeed> GetRealStateObjects(bool withGarden, CancellationToken cancellationToken)
        {
            string queryUrl = ApiConstants.FundaRequestUrl;
            if (withGarden)
                queryUrl = $"{ApiConstants.FundaRequestUrl}/tuin/";
            var request = new HttpRequestMessage(HttpMethod.Get, queryUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            /*
            Cancelling request could be beneficial to performance both at the level of bandwidth consumption and scalability
            Cancelling a task that is no longer needed will free up the thread that is used to run the task which mean that this thread will back to the thread pool
            which could be used for another work and this does improve the scalability of our application.
            "ResponseHeadersRead" is used blow instead the default option to be able to start the operation as soon as possible.
             */
            using (var response = await _policyService.BulkheadPolicy.ExecuteAsync(() =>
                _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)))
            {
                /*When doing http request the response will be as a stream in a wire which could be read from the content as a string(ReadAsStringAsync)
                 which will create In memory string which as large as the response body, after that we need to transform that string by deserializing to the required object ,
                So what we can do, instead of deserializing this from that in memory object we could then deserializing it directly from the stream,
                which mean we don’t need such in memory string.thus we Ensure memory use is kept low and Minimizing memory can also minimize garbage collection,
                which has a positive impact on performance*/

                if (!response.IsSuccessStatusCode)
                {
                    // inspect the status code
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new ResourceNotFoundException();
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        // trigger a login flow
                        throw new UnauthorizedApiAccessException();
                    }
                }
                var stream = await response.Content.ReadAsStreamAsync();
                //Extension method has been created for re-usability aspect
                return stream.ReadAndDeserializeFromJson<LocatieFeed>();
            }
        }
    }
}
