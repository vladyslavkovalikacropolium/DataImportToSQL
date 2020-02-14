using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DataImport.Data.Entities;
using DataImport.Storage.Extensions;
using DataImport.Storage.Mapping;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataImport.Storage
{
    public class ImportContext : DbContext
    {
        public DbSet<FieldsImportedData> FieldsImportedData { get; set; }
        public DbSet<RecordsImportedData> RecordsImportedData { get; set; }
        
        public ImportContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Data Source=KOVALYK-V\\SQLEXPRESS;Initial Catalog=ImportedDataDB;Integrated Security=True;Persist Security Info=False;MultipleActiveResultSets=True
            optionsBuilder.UseSqlServer(@"Server=KOVALYK-V\SQLEXPRESS;Database=ImportedDataDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FieldsImportedDataMap());
            modelBuilder.ApplyConfiguration(new RecordsImportedDataMap());
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<FieldsImportedData>(new FieldsImportedDataMap().Configure);
            modelBuilder.Entity<RecordsImportedData>(new RecordsImportedDataMap().Configure);
        }


        public async Task BulkSaveAsync<T>(IEnumerable<T> entities, CancellationToken token = default) where T : BaseEntity
        {
            var dbConnection = Database.GetDbConnection();
            var sbCopy = new SqlBulkCopy(dbConnection.ConnectionString)
            {
                DestinationTableName = typeof(T).GetTypeInfo().Name
            };

            await sbCopy.WriteToServerAsync(entities.ToDataTable(), token);
        }
    }
}