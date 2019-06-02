using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNet_Communication.Connection;
using DNet_Communication.Maintance;

using DNet_PostgreSQL_Demonstration.Processing;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DNet_PostgreSQL_Demonstration
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(Configuration.GetSection("Database")["ConnectionString"]); //Connection string 
            }, ServiceLifetime.Singleton);


            services.AddSingleton(provider => Configuration); //Add config to DI
            services.AddSingleton<IMachineInfoCollectorService, MachineInfoCollectorService>();
            services.AddSingleton<IConnect, HubConnect>();
            services.AddSingleton<IPostgreSQLDemonstrationTaskHandlerService, PostgreSQLDemonstrationTaskHandlerService>();
            services.AddSingleton<IConnectionService, ConnectionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }


        }
    }
}
