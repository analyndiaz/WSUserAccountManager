using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WSUserAccountManager.Handlers;
using WSUserAccountManager.Extensions;
using WSUserAccountManager.Database;
using Microsoft.EntityFrameworkCore;
using WSUserAccountManager.Middlewares;

namespace WSUserAccountManager
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
            services.AddServices();
            services.AddDbContext<UMSEntities>(opt =>
                                               opt.UseInMemoryDatabase("WSUserAccountManager"));
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;

            app.UseWebSockets();
            app.Map("/user", (_app) => _app.UseMiddleware<WebSocketMiddleware>(serviceProvider.GetService<UserAccountHandler>()));

            app.UseHttpsRedirection();
        }
    }
}
