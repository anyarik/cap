using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CAP.Bot.Telegram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run(); 
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                                 .ConfigureAppConfiguration((hostingContext, config) =>
                                 {
                                     var env = hostingContext.HostingEnvironment;
                                     var sharedFolder = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

                                     config
#if DEBUG
                            .AddJsonFile(Path.Combine(sharedFolder, "Solution Items", "SharedSettings.json"), optional: true) // When running using dotnet run
                            .AddJsonFile(Path.Combine(sharedFolder, "Solution Items", "SharedSettings.Development.json"), optional: true) // When running using dotnet run
#endif
                            .AddJsonFile("SharedSettings.json", optional: true) // When app is published
                            .AddJsonFile($"SharedSettings.{env.EnvironmentName}.json", optional: true) // When app is published
                                         .AddJsonFile("SharedSettings.json", optional: true) // When app is published
                                         .AddJsonFile("appsettings.json", optional: true)
                                         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

                                     config.AddEnvironmentVariables();
                                 }).UseStartup<Startup>();
    }
}
