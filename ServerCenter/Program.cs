using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServerCenter.Data;

namespace ServerCenter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();

            if (Global.Config.IdentityMode == Global.IdentityMode.OriginalIdentity)
            {
                webHost.MigrateDbContext<ApplicationDbContext>((context, service) =>
                {
                    new ApplicationDbContextSeed().SeedAsync(context, service).Wait();
                });
            }

            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
    }
}
