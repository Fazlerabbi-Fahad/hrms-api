using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Data.Configurations
{
    public class EmploymentStatusConfiguration:IEntityTypeConfiguration<EmploymentStatus>
    {
        public void Configure(EntityTypeBuilder<EmploymentStatus> builder)
        {
            builder.HasKey(es => es.Id);
            builder.Property(es => es.StatusName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(es => es.StatusDisplayName)
                .IsRequired()
                .HasMaxLength(150);
            builder.HasIndex(es => es.StatusName)
                   .IsUnique()
                   .HasFilter("[StatusName] IS NOT NULL");
        }
    }
}
