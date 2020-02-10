using System.Net.Http;
using System.Threading;
using APIConsumer.Exceptions;
using APIConsumer.Handlers;
using APIConsumer.Services;
using Moq;
using NUnit.Framework;
using Polly;
namespace APIConsumer.Test
{
    public class RealStateStatisticsClientTests
    {
        private readonly CancellationTokenSource _cancellationTokenSource =
            new CancellationTokenSource();

        private HttpClient _httpClient;
        private Mock<IPolicyService> _mockPolicyService;
        private RealStateStatisticsClient _statisticsClient;

        [SetUpAttribute]
        public void TestInitialization()
        {
            _mockPolicyService = new Mock<IPolicyService>();
            _mockPolicyService.SetupAllProperties();
            _mockPolicyService.Object.BulkheadPolicy = Policy.NoOpAsync<HttpResponseMessage>();
            _mockPolicyService.Object.HttpRetryPolicy = Policy.NoOpAsync<HttpResponseMessage>();
            _mockPolicyService.Object.TimeOutPolicy = Policy.NoOpAsync<HttpResponseMessage>();
            _mockPolicyService.Object.FallBackPolicy = Policy.NoOpAsync<HttpResponseMessage>();
            _mockPolicyService.Setup(x => x.PolicyWrap).Returns
            (Policy.WrapAsync(_mockPolicyService.Object.FallBackPolicy, _mockPolicyService.Object.HttpRetryPolicy,
                _mockPolicyService.Object.TimeOutPolicy, _mockPolicyService.Object.BulkheadPolicy));
        }

        [Test]
        public void GetRealStateObjects_UnauthorizedResponse_UnauthorizedAccessExceptionISThrown()
        {
            _httpClient = new HttpClient(new Unauthorized401ResponseHandler());
            _statisticsClient = new RealStateStatisticsClient(_httpClient, _mockPolicyService.Object);
            Assert.ThrowsAsync<UnauthorizedApiAccessException>(async () =>
                await _statisticsClient.GetRealStateObjects(true, _cancellationTokenSource.Token));
        }

        [Test]
        public void GetRealStateObjects_OnNotFoundResponse_ResourceNotFoundExceptionISThrown()
        {
            _httpClient = new HttpClient(new ResourceNotFoundResponseHandler());
            _statisticsClient = new RealStateStatisticsClient(_httpClient, _mockPolicyService.Object);
            Assert.ThrowsAsync<ResourceNotFoundException>(async () =>
                await _statisticsClient.GetRealStateObjects(true, _cancellationTokenSource.Token));
        }
    }
}