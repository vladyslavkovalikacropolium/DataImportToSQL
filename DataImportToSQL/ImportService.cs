using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataImport.Storage;
using DataImportToSQL.Exceptions;
using DataImportToSQL.Extensions;
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
            
            var fileSpecifications = _provider.GetFileSpecifications();
            var dataTable = fileSpecifications.ToDataTable();
            await _importContext.TryToCreateTable(fileSpecifications.TableName, dataTable);
            
            var files = Directory.GetFiles(_providerOptions.FilesPath);

            var specCount = fileSpecifications.Specifications.Count;
            foreach (var file in files)
            {
                var recordToJoin = "";
                using var stream = new StreamReader(file);

                var buffer = new char[8000];

                while (!stream.EndOfStream)
                {
                    var charsRead = stream.ReadBlock(buffer, 0, buffer.Length);
                    var isLastReading = charsRead < buffer.Length;

                    if (isLastReading)
                    {
                        Array.Resize(ref buffer, charsRead);
                    }

                    var block = new string(buffer);
                    block = recordToJoin + block;

                    var lastIndex = block.LastIndexOf(fileSpecifications.FileType.RecordsDelimiter,
                        StringComparison.Ordinal);
                    var lastUncompletedRecord = block.Substring(lastIndex);

                    var records = block.Split(fileSpecifications.FileType.RecordsDelimiter,
                        StringSplitOptions.RemoveEmptyEntries);
                    if (!string.IsNullOrEmpty(lastUncompletedRecord) && !isLastReading)
                    {
                        recordToJoin = lastUncompletedRecord;
                        records = records.SkipLast(1).ToArray();
                    }

                    foreach (var record in records)
                    {
                        var fields = record.Split(fileSpecifications.FileType.FieldsDelimiter);
                        if (specCount == fields.Length)
                        {
                            dataTable.Rows.Add(fields);
                        }
                    }
                }

                await _importContext.BulkSaveAsync(fileSpecifications.TableName, dataTable, cancellationToken);
            }
        }
    }
}