using Bari.Api.AMQP;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Bari.Api
{
    public class Program
    {
        public static void Main(string[] args)
        { var host = CreateHostBuilder(args).Build();
            var services = host.Services;
            
            ServiceLocator.SetLocatorProvider(services);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));
                });
    }
}