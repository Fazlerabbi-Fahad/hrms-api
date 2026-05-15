using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.RoleName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(r => r.RoleDisplayName)
                .IsRequired()
                .HasMaxLength(150);
            builder.HasIndex(r => r.RoleName)
                   .IsUnique()
                   .HasFilter("[RoleName] IS NOT NULL");
        }
    }
}
