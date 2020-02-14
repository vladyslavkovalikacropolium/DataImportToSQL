using System;

namespace DataImport.Data.Entities
{
    public class FieldsImportedData : BaseEntity<Guid>
    {
        public Guid RecordImportedDataId { get; set; }
        public RecordsImportedData RecordsImportedData { get; set; }
        public string FieldName { get; set; }
    }
}