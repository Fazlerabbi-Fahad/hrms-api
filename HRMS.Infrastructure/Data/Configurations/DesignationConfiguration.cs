using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Data.Configurations
{
    internal class DesignationConfiguration:IEntityTypeConfiguration<Designation>
    {
        public void Configure(EntityTypeBuilder<Designation> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.DesignationName)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(d => d.DesignationName)
                   .IsUnique()
                   .HasFilter("[DesignationName] IS NOT NULL");
        }
    }
}
