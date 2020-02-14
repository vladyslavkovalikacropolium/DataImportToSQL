using System;
using System.Collections.Generic;

namespace DataImport.Data.Entities
{
    public class RecordsImportedData : BaseEntity<Guid>
    {
        public string Record { get; set; }
        public IList<FieldsImportedData> Fields { get; } = new List<FieldsImportedData>();
    }
}