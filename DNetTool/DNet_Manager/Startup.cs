using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using DNet_Manager.Hubs;

using DNet_Communication.Connection;

namespace DNet_Manager
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

            services.AddSingleton(provider => Configuration); //Add config to DI
            services.AddScoped<IConnect, HubConnect>(); //Register connection impl
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

            app.UseSignalR(routes =>
            {
                routes.MapHub<LobbyHub>("/lobby");
            });

            //app.UseHttpsRedirection();
            //app.UseMvc(); //TODO: add mvc support in case of websocket unavailability
        }
    }
}
