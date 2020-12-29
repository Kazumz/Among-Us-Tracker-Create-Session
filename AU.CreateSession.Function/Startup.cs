using System.Linq;
using AU.CreateSession.Domain.CreateSession;
using AU.CreateSession.Domain.Services.Repositories;
using AU.CreateSession.External.Cosmos;
using AU.CreateSession.Function;
using AU.CreateSession.Function.Profiles;
using AU.CreateSession.Services.External.Cosmos;
using AU.CreateSession.Services.Repositories.Player;
using AutoMapper;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: FunctionsStartup(typeof(Startup))]
namespace AU.CreateSession.Function
{
    public class Startup : FunctionsStartup
    {
        private const string ConfigCosmosConnectionString = "CosmosDBConnection";
        private const string ConfigTableName = "Table";

        private IConfigurationRoot configRoot;

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            configRoot = builder.ConfigurationBuilder
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true) // Add our Local Settings.
                .AddEnvironmentVariables() // Add our Azure Settings (if hosted).
                .Build();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Replace our default IConfiguration for our application with our custom configuration root.
            builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), configRoot));
            var config = (IConfiguration)builder.Services.First(d => d.ServiceType == typeof(IConfiguration)).ImplementationInstance;

            builder.Services.AddAutoMapper(typeof(RequestProfile), typeof(ModelProfile));

            // Domain
            builder.Services.AddTransient<ICreateSessionHandler, CreateSessionHandler>();

            // Services
            builder.Services.AddTransient<IPlayerRepository, PlayerRepository>();

            // External
            builder.Services.AddTransient<ITableContext, TableContext>();
            builder.Services.AddSingleton<CloudTable>((s) =>
            {
                var storageAccount = CloudStorageAccount.Parse(config[ConfigCosmosConnectionString]);
                var cloudTableClinet = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

                return cloudTableClinet.GetTableReference(config[ConfigTableName]);
            });
        }
    }
}
