using System;

using DNet_Communication.Connection;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using NLog;

namespace DNet_UI_Demonstration
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
                connectionService.ScheduleConnectionInitialize(TimeSpan.FromSeconds(5), DNet_DataContracts.ModuleTypes.UI); //initialize connection without user input

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

        public static IWebHost CreateWebHost(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args);
            //builder.UseConfiguration(new ConfigurationBuilder()
            //    .AddCommandLine(args)
            //    .Build());

            builder.UseStartup<Startup>();
            //builder.UseUrls("http://*:39753;http://*:39853");
            //builder.UseNLog();

            return builder.Build();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}
