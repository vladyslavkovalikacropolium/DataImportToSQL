using DataImport.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataImportToSQL.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddImportContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddScoped<ImportContext>()
                .AddScoped<DbContext, ImportContext>()
                .AddSqlConnection(configuration);

            return services;
        }


        private static IServiceCollection AddSqlConnection(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection");

            services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<ImportContext>(options => options.UseSqlServer(connection,
                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(ImportContext).Assembly.FullName)));

            return services;
        }
        
        
        public static IServiceCollection AddDataProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<DataProviderOptions>(configuration.GetSection("DataProvider"));

            return services;
        }
    }
}