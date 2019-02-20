using EventFlow;
using EventFlow.AspNetCore.Extensions;
using EventFlow.MsSql;
using EventFlow.MsSql.EventStores;
using EventFlow.MsSql.Extensions;

namespace MsSQLInitialisation
{
    class Program
    {
        /// <summary>
        /// Initalises the eventflow database tables
        /// </summary>
        /// <see cref="https://eventflow.readthedocs.io/EventStore.html#mssql-event-store"/>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var rootResolver = EventFlowOptions.New
                .AddAspNetCoreMetadataProviders()
                .UseMssqlEventStore()
                .ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString(
                    "Data Source=JC_MSI;Initial Catalog=EventFlowDemo;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True"))
                .CreateResolver();

            var msSqlDatabaseMigrator = rootResolver.Resolve<IMsSqlDatabaseMigrator>();
            EventFlowEventStoresMsSql.MigrateDatabase(msSqlDatabaseMigrator);

        }
    }
}
