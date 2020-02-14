using DataImport.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataImport.Storage.Mapping
{
    internal sealed class RecordsImportedDataMap : IEntityTypeConfiguration<RecordsImportedData>
    {
        public void Configure(EntityTypeBuilder<RecordsImportedData> builder)
        {
            builder.ToTable("RecordsImportedData");
            builder.HasKey(item => item.Id);
        }
    }
}