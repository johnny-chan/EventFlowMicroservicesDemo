using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common;
using Common.Settings;
using EventFlow;
using EventFlow.AspNetCore.Extensions;
using EventFlow.AspNetCore.Middlewares;
using EventFlow.Autofac.Extensions;
using EventFlow.Extensions;
using EventFlow.MsSql;
using EventFlow.MsSql.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var containerBuilder = new ContainerBuilder();

            var connectionString = Configuration["EventStoreConnectionString"];

            var container = EventFlowOptions.New
                .UseAutofacContainerBuilder(containerBuilder)
                .AddAspNetCoreMetadataProviders()
                .AddEvents(typeof(ExampleEvent))
                .AddEvents(typeof(ExampleMultiplerEvent))
                .AddCommands(typeof(ExampleCommand))
                .AddCommandHandlers(typeof(ExampleCommandHandler))
                .UseConsoleLog()
                .UseMssqlEventStore()
                .ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString(connectionString))
                .UseMssqlReadModel<ExampleReadModel>()
                .UseMssqlReadModel<ExampleDuplicateReadModel>();



            containerBuilder.Populate(services);

            return new AutofacServiceProvider(containerBuilder.Build());
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

            app.UseHttpsRedirection();
            app.UseMiddleware<CommandPublishMiddleware>();
            app.UseMvc();
        }
    }
}
