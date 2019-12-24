using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cricetidae.UpdateData
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(configurationBuilder =>
               {
                   configurationBuilder.AddJsonFile("appsettings.json", false, true);
                   configurationBuilder.AddEnvironmentVariables();
                   configurationBuilder.AddUserSecrets<Program>();
               })
                .ConfigureServices(serviceCollection =>
                {
                    serviceCollection.AddLogging(configure =>
                      {
                          configure.AddConsole();
                      });
                    serviceCollection.AddTransient<IPipeLine, BasicPipeline>();
                    serviceCollection.AddTransient<BonusDataPersister>();
                    serviceCollection.AddHttpClient<ProductPriceDataReader>();
                    serviceCollection.AddHttpClient<BonusDataReader>();
                    serviceCollection.AddTransient<BeepTest>();
                    serviceCollection.AddTransient<BonusProductAmountCorrecter>();

                    serviceCollection.AddHostedService<BasicPipeline>();
                });
        }
    }
}
