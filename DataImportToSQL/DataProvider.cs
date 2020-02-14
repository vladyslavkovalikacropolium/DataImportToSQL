using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DataImportToSQL
{
    public class DataProvider
    {
        private readonly string _specificationPath;

        public DataProvider(string specificationPath)
        {
            _specificationPath = specificationPath;
        }

        public FileSpecification GetFileSpecifications()
        {
            return JsonConvert.DeserializeObject<FileSpecification>(File.ReadAllText(_specificationPath));
        }
    }

    public class FileSpecification
    {
        public string TableName { get; set; }
        public FileType FileType { get; set; }
        public List<FieldSpecification> Specifications { get; set; }
    }
    
    public class FieldSpecification
    {
        public string FieldName { get; set; }
        public string Beginning { get; set; }
        public int FieldLength { get; set; }
    }

    public class FileType
    {
        public bool FixedLength { get; set; } = true;
        public bool QuotedText { get; set; }
        public string FieldsDelimiter { get; set; } = ",";
        public string RecordsDelimiter { get; set; } = "\r\n";
    }
}