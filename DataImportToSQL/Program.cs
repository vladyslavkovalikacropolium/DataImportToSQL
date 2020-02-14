using System;
using System.IO;
using System.Threading.Tasks;
using DataImport.Storage;
using DataImportToSQL.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataImportToSQL
{
    public class Program
    {
        private static IServiceProvider _serviceProvider;

        public static async Task Main(string[] args)
        {
            RegisterContexts();
            await _serviceProvider.GetService<ImportContext>().Database.MigrateAsync();
            await _serviceProvider.GetService<ImportService>().ImportAsync();
        }

        private static void RegisterContexts()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var collection = new ServiceCollection();

            collection
                .AddImportContext(configuration)
                .AddScoped<ImportService>()
                .AddDataProvider(configuration);

            _serviceProvider = collection.BuildServiceProvider();
        }
    }
}
