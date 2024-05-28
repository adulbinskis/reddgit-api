using Microsoft.AspNetCore.Hosting;
using ReddgitAPI.ORM.Entities;

namespace ReddgitAPI
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var host = Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })             
                .Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            await host.RunAsync();
        }
    }
}