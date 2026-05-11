using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(u => u.PhoneNumber) 
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);
            builder.HasIndex(u => u.Email)
                   .IsUnique()
                   .HasFilter("[Email] IS NOT NULL");
            builder.HasIndex(u => u.PhoneNumber)
                   .IsUnique()
                   .HasFilter("[ContactNumber] IS NOT NULL");
    }
}
}
