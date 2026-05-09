using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Data.Configurations
{
    public class PaymentStatusConfiguration:IEntityTypeConfiguration<PaymentStatus>
    {
        public void Configure(EntityTypeBuilder<PaymentStatus> builder)
        {
            builder.HasKey(ps => ps.Id);
            builder.Property(ps => ps.StatusName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(ps => ps.StatusDisplayName)
                .IsRequired()
                .HasMaxLength(150);
            builder.HasIndex(ps => ps.StatusName)
                   .IsUnique()
                   .HasFilter("[StatusName] IS NOT NULL");
        }
    }
}