using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataImport.Storage
{
    public class ImportContext : DbContext
    {
        public ImportContext(DbContextOptions options) : base(options)
        {
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