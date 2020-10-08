using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RealState.Statistics.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>

           WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        //.ConfigureAppConfiguration((context, config) =>
        //{
        //    if (context.HostingEnvironment.IsProduction())
        //    {
        //        /*
        //         Environment - The DefaultAzureCredential will read account information specified via environment variables and use it to authenticate.
        //         Managed Identity - If the application is deployed to an Azure host with Managed Identity enabled, the DefaultAzureCredential will authenticate with that account.
        //         Visual Studio - If the developer has authenticated via Visual Studio, the DefaultAzureCredential will authenticate with that account.
        //         Interactive - If enabled the DefaultAzureCredential will interactively authenticate the developer via the current system's default browser.
        //         */
        //        var client = new SecretClient(vaultUri: new Uri("https://keyvault204demooo.vault.azure.net/"), credential: new DefaultAzureCredential(true));

        //        // Create a new secret using the secret client.
        //        KeyVaultSecret secret = client.SetSecret("secret-name", "secret-value");

        //        // Retrieve a secret using the secret client.
        //        secret = client.GetSecret("secret-name");
        //    }
        //})
    
    }
}
