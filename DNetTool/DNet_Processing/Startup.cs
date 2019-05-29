using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using DNet_Communication.Connection;
using DNet_Communication.Maintance;
using DNet_Processing.Hubs;

namespace DNet_Processing
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddSignalR();
            services.AddSingleton(provider => Configuration); //Add config to DI
            services.AddSingleton<IMachineInfoCollectorService, MachineInfoCollectorService>();
            services.AddSingleton<IConnect, HubConnect>();
            services.AddSingleton<ITaskHandlerService, BaseTaskHandlerService>();
            services.AddSingleton<IConnectionService, ConnectionService>();
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
            app.UseMvc();
        }
    }
}
