using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNet_Hub.Hubs;
using DNet_Hub.Processing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DNet_Hub
{
    public class Startup
    {
        IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;

            _env = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddScoped<ITaskHandlerService, TaskHandlerService>();

            services.AddSignalR().AddHubOptions<MainHub>(options =>
            { 
               
            }); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSignalR(config =>
            {
                config.MapHub<MainHub>("/mainhub"); //registering this as main hub
            });

            //app.UseHttpsRedirection();
            //app.UseMvc();
        }
    }
}
