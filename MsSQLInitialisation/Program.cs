using System;
using System.IO;
using Common.Settings;
using EventFlow;
using EventFlow.AspNetCore.Extensions;
using EventFlow.MsSql;
using EventFlow.MsSql.EventStores;
using EventFlow.MsSql.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace MsSQLInitialisation
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        /// <summary>
        /// Initalises the eventflow database tables
        /// </summary>
        /// <see cref="https://eventflow.readthedocs.io/EventStore.html#mssql-event-store"/>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) || devEnvironmentVariable.ToLower() == "development";
            //Determines the working environment as IHostingEnvironment is unavailable in a console app

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());
               

            if (isDevelopment) //only add secrets in development
            {
                builder.AddUserSecrets("babaea2e-4e26-4c24-8243-7f166cdf3230");
            }

            Configuration = builder.Build();

            var connectionString = Configuration["EventStoreConnectionString"];

            //var services = new ServiceCollection()
            //    .Configure<Secrets>(Configuration.GetSection(nameof(Secrets)))
            //    .AddOptions()
            //    .BuildServiceProvider();

            //services.GetService<Secrets>();

            var rootResolver = EventFlowOptions.New
                .AddAspNetCoreMetadataProviders()
                .UseMssqlEventStore()
                .ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString(connectionString))
                .CreateResolver();

            var msSqlDatabaseMigrator = rootResolver.Resolve<IMsSqlDatabaseMigrator>();
            EventFlowEventStoresMsSql.MigrateDatabase(msSqlDatabaseMigrator);

        }
    }
}
