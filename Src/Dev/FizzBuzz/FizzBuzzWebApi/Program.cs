using Autofac.Extensions.DependencyInjection;
using GrainInterfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using System.Threading.Tasks;

namespace FizzBuzzWebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IFizzBuzzService fizzBuzzService = await new FizzBuzzService("dev", "OrleansBasics", addConsole: true)
                .Connect();

            CreateHostBuilder(args)
                .ConfigureServices(services => services.AddSingleton(fizzBuzzService))
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
