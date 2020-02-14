using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FieldsImportedDataMap());
            modelBuilder.ApplyConfiguration(new RecordsImportedDataMap());
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<FieldsImportedData>(new FieldsImportedDataMap().Configure);
            modelBuilder.Entity<RecordsImportedData>(new RecordsImportedDataMap().Configure);
        }


        public async Task BulkSaveAsync(string tableName, DataTable dataTable, CancellationToken token = default)
        {
            var dbConnection = Database.GetDbConnection();
            var sbCopy = new SqlBulkCopy(dbConnection.ConnectionString)
            {
                DestinationTableName = tableName
            };
            
            await sbCopy.WriteToServerAsync(dataTable, token);
        }

        public async Task TryToCreateTable(string tableName, DataTable table)
        {
            var create = @$" IF OBJECT_ID('dbo.{tableName}', 'U') IS NULL CREATE TABLE dbo.{tableName} (";
            create = table.Columns.Cast<DataColumn>().Aggregate(create, (current, column) => current + $@"[{column.ColumnName}] [nvarchar] ({column.MaxLength}),");

            create += ") ON [PRIMARY]";
            await Database.ExecuteSqlRawAsync(create);
        }
    }
}