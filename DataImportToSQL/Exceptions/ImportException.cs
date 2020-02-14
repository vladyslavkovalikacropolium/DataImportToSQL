using System;

namespace DataImportToSQL.Exceptions
{
    public class ImportException : Exception
    {
        public ImportException(string message) : base($"Importing of data has been stopped. {message}")
        {
        }
        
        public ImportException() : base($"Importing of data has been stopped.")
        {
        }
    }
}