using System.Data;
using System.Linq;

namespace DataImportToSQL.Exceptions
{
    public static class FileSpecificationExtension
    {
        public static DataTable ToDataTable(this FileSpecification fileSpecification)
        {
            var dataTable = new DataTable();
            foreach (var fieldSpecification in fileSpecification.Specifications.OrderBy(item => item.Beginning))
            {
                var dc = new DataColumn(fieldSpecification.FieldName) {MaxLength = fieldSpecification.FieldLength};
                dataTable.Columns.Add(dc);
            }

            return dataTable;
        }
    }
}