using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using CHE.Web;

Program.CreateHostBuilder(args).Build().Run();

namespace CHE.Web
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}