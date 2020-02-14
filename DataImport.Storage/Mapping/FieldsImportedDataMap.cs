using DataImport.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataImport.Storage.Mapping
{
    internal sealed class FieldsImportedDataMap : IEntityTypeConfiguration<FieldsImportedData>
    {
        
        public void Configure(EntityTypeBuilder<FieldsImportedData> builder)
        {
            builder.ToTable("FieldsImportedData");
            builder.HasKey(item => item.Id);
            
            builder
                .HasOne(item => item.RecordsImportedData)
                .WithMany(item => item.Fields)
                .HasForeignKey(item => item.RecordImportedDataId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}