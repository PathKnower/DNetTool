using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DNet_Communication.Connection;
using DNet_Processing.Processing;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace DNet_Processing
{
    public class Program
    {
        static Logger _logger;

        static Random rnd = new Random();

        public static void Main(string[] args)
        {
            //LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("", true);

            _logger = LogManager.GetCurrentClassLogger();

            try
            {
                IWebHost host = CreateWebHost(args);

                host.Start();

                #region Services

                var connectionService = (IConnectionService)host.Services.GetService(typeof(IConnectionService));
                connectionService.ScheduleConnectionInitialize(TimeSpan.FromSeconds(5), "Processing"); //initialize connection without user input

                var taskHandlingService = (IProcessingModuleDemontrationTaskHandlerService)host.Services.GetService(typeof(IProcessingModuleDemontrationTaskHandlerService));
                taskHandlingService.Initialize();

                #endregion

                host.WaitForShutdown();
            }
            catch(Exception ex)
            {
                _logger.Error("Something goes wrong", ex);
            }
            finally
            {
                LogManager.Shutdown();
            }

            //CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHost CreateWebHost(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args);

            string httpUri = "http://*:";

            httpUri += rnd.Next(10000, 65000);

            builder.UseStartup<Startup>();
            builder.UseUrls(httpUri);
            builder.UseNLog();

            return builder.Build();
        }
        
    }
}
