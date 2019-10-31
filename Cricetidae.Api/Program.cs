using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Cricetinea.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Starting api application v0.3");
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
