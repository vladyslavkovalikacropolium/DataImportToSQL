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

        public Specification GetSpecifications()
        {
            return JsonConvert.DeserializeObject<Specification>(File.ReadAllText(_specificationPath));
        }
    }

    public class Specification
    {
        public string FieldName { get; set; }
        public string Beginning { get; set; }
        public string FieldLength { get; set; }
        public FileType FileType { get; set; }
    }

    public class FileType
    {
        public bool IsFixedLength { get; set; }
        public bool IsQuotedText { get; set; }
        public Delimiter[] Delimiters { get; set; }
    }

    public enum Delimiter
    {
        Fields,
        Records
    }
}