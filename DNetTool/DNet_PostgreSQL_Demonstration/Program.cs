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

            _logger = LogManager.GetCurrentClassLogger();

            try
            {
                IWebHost host = CreateWebHost(args);

                host.Start();

                #region Services

                var connectionService = (IConnectionService)host.Services.GetService(typeof(IConnectionService));
                connectionService.ScheduleConnectionInitialize(TimeSpan.FromSeconds(5), DNet_DataContracts.ModuleTypes.Processing); //initialize connection without user input

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

        public static IWebHost CreateWebHost(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args);
            //builder.UseConfiguration(new ConfigurationBuilder()
            //    .AddCommandLine(args)
            //    .Build());

            builder.UseStartup<Startup>();
            //builder.UseUrls("http://*:39753;http://*:39853");
            builder.UseNLog();

            return builder.Build();
        }
    }
}
