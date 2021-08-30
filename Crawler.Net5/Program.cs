using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Crawler.Net5
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddMyCrwaler()
                .AddMyRepository()
                .AddMyAppService()
                .AddLogging(builder => { builder.AddConsole(); 
                                         builder.SetMinimumLevel(LogLevel.Trace); });

            using var sp = services.BuildServiceProvider();
            await sp.GetCrawlerDotNet5().ExecuteAsync();
        }
    }
}
