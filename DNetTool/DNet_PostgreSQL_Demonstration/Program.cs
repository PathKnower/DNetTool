using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNet_Communication.Connection;
using DNet_PostgreSQL_Demonstration.Processing;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog;
using NLog.Web;

namespace DNet_PostgreSQL_Demonstration
{
    public class Program
    {
        static Logger _logger;

        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();

            //LogManager.Configuration = new NLog.Config.LoggingConfiguration();
            //LogManager.Configuration.AddTarget("console", new NLog.Targets.ColoredConsoleTarget());

            _logger = LogManager.GetCurrentClassLogger();

            try
            {
                IHost host = CreateHostBuilder(args).Build();

                host.Start();

                #region Services

                var connectionService = (IConnectionService)host.Services.GetService(typeof(IConnectionService));
                var taskHandlerService = (IPostgreSQLDemonstrationTaskHandlerService)host.Services.GetService(typeof(IPostgreSQLDemonstrationTaskHandlerService));

                connectionService.ScheduleConnectionInitialize(TimeSpan.FromSeconds(5), "DB_Postgres"); //initialize connection without user input
                taskHandlerService.Initialize();

                //TestConnect testConnect = new TestConnect();

                #endregion

                host.WaitForShutdown();
            }
            catch (Exception ex)
            {
                _logger.Error("Something goes wrong", ex);
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(options => 
                {
                    options.UseStartup<Startup>();
                    options.UseNLog();
                    options.UseUrls("http://localhost:46843");
                });

            return builder;
        }
    }
}
