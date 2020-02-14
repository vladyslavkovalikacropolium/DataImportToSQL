using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataImport.Storage;
using DataImportToSQL.Exceptions;
using Microsoft.Extensions.Options;

namespace DataImportToSQL
{
    public class ImportService
    {
        private readonly DataProviderOptions _providerOptions;
        private readonly DataProvider _provider;
        private readonly ImportContext _importContext;
        

        public ImportService(IOptions<DataProviderOptions> options, ImportContext importContext)
        {
            _importContext = importContext;
            _providerOptions = options.Value;

            _provider = new DataProvider(_providerOptions.SpecificationPath);
        }

        public async Task ImportAsync(CancellationToken cancellationToken = default)
        {
            if (!Directory.Exists(_providerOptions.FilesPath))
            {
                throw new ImportException($"Files does not exist in directory: {_providerOptions.FilesPath}");
            }
            
            var specifications = _provider.GetFileSpecifications();
            var dataTable = specifications.ToDataTable();
            await _importContext.TryToCreateTable(specifications.TableName, dataTable);
            
            var files = Directory.GetFiles(_providerOptions.FilesPath);

            var specCount = specifications.Specifications.Count;
            foreach (var file in files)
            {
                var stringToJoin = "";
                using var stream = new StreamReader(file);
                
                var buffer = new char[1500000];
                while(stream.ReadBlock(buffer,0,buffer.Length) > 0)
                {
                    var str = new string(buffer);
                    str = stringToJoin + str;
                    
                    foreach (var record in str.Split(specifications.FileType.RecordsDelimiter))
                    {
                        var fields = record.Split(specifications.FileType.FieldsDelimiter);
                        if (fields.Any(item => item == "750458753"))
                        {
                            
                        }
                        
                        if (specCount == fields.Length)
                        {
                            dataTable.Rows.Add(fields);
                            stringToJoin = string.Empty;
                        }
                        else if (specCount > fields.Length)
                        {
                            stringToJoin = record;
                        }
                        else if (specCount < fields.Length)
                        {
                           
                        }
                    }
                }

                await _importContext.BulkSaveAsync(specifications.TableName, dataTable, cancellationToken);
            }
        }
    }
}