using APIConsumer;
using APIConsumer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RealState.ReadStack;

namespace RealState.Statistics.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<IAgentStatisticsQueries, AgentStatisticsQueries>();
            /*
             Problem with disposing HttpClient instances:
               ------Why should avoid disposing HttpClient instance(ex: inside ‘using’):
               When disposing httpClient the underlying httpClient handler is disposed, which closes the underlying connection, so when re-instantiate a new httpClient
               for the next request a new httpClientHandler is instantiated and connection has to be reopened again, so not only this slower but also it could lead to not
               having enough sockets available to open a new connection as it does take a bit of time to completely close the connection and free up the socket(socket exhaustion)
               -----Why should avoid using single static instance from HttpClient :
               Because when reusing our instance the connection is reused until the socket is closed for example due to a network disruption . but it means that domain name changes are not honored,
               and that’s a problem as using a DNS change to switch between different environments, like switching from staging to production so if the change isn’t honored,
               requests would still go to staging instead of production
               -------.net core 2.1 has a better approach to solve those two problems using HttpClientFactory for instance management
               
             */
            services.AddHttpClient<RealStateStatisticsClient>();
            /*
             Polly is a library that allows developers to express resilience and transient fault handling policies such as Retry, Circuit Breaker, Timeout, Bulkhead Isolation, and Fallback in a fluent and thread-safe manner.
               
               Bulkhead isolation:
               It limits the number of  requests to the remote service that can execute in parallel and also limits the number of requests that can sit in a queue awaiting an execution slot 
               Benefits:
               Isolation
               Prevent overloading :
               If one part of your application becomes overloaded it could bring down other parts but by using Bulkhead isolation policy you could prevent this from happening or delay it.
               Resource allocation :
               By allocate execution slots and queues as you see fit for your case.
               Scaling :
               You can determine the number of ongoing parallel requests. You can also determine the number of requests waiting in the queue. When they reach some limit, you could trigger horizontal scaling.
               Load shedding :
               As it’s better to fail fast than to fail unpredictably so when your application is being overwhelmed, it will at some unknown point being slow and fail.
               So you could handle this by setting when your application stops accepting requests and immediately return an error to the caller
               
             */

            /*
             For re-usability aspect this service has been created to be able to reuse our policy among different areas
             */
            services.AddSingleton<IPolicyService,PolicyService>();
            services.AddApplicationInsightsTelemetry();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseMvcWithDefaultRoute();
        }
    }
}