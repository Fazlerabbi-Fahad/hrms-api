using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Data.Configurations
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasKey(ps => ps.Id);
            builder.Property(ps => ps.MenuName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(ps => ps.MenuDisplayName)
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(ps => ps.Route)
                .IsRequired()
                .HasMaxLength(150);
        }
    }
}
