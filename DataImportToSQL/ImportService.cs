using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DataImport.Storage;
using DataImportToSQL.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            
            var specification = _provider.GetSpecifications();
            var files = Directory.GetFiles(_providerOptions.FilesPath);

            foreach (var file in files)
            {
                using (var stream = new StreamReader(file))
                {
                    if (specification.FileType.IsFixedLength)
                    {
                        var line = stream.ReadLine();
                    }

                  

                }

                var sourceFilePath = new SqlParameter("@SourceFilePath", file);
                await _importContext.Database.ExecuteSqlRawAsync($"[dbo].[Bulk_Import]({file})", cancellationToken);
            }
        }
    }
}