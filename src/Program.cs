using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

// Namespace for the ContosoCrafts Website
namespace ContosoCrafts.WebSite
{
    // This class creates and executes the program. This is also the program entry point
    public class Program
    {
        
        // Web application entry point
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// A class which creates the web host
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
