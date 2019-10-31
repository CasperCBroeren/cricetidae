using Cricetidae.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
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
                    serviceCollection.AddTransient<IBonusDataPersister, BonusDataPersister>();
                    serviceCollection.AddHttpClient<IProductPriceDataReader, ProductPriceDataReader>();
                    serviceCollection.AddHttpClient<IBonusDataReader, BonusDataReader>();
                    serviceCollection.AddTransient<IBonusProductAmountCorrecter, BonusProductAmountCorrecter>();

                    serviceCollection.AddHostedService<BasicPipeline>();
                });
        }
    }
}
