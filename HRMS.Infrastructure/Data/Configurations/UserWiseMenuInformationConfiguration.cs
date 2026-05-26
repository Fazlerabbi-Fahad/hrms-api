using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Data.Configurations
{
    public class UserWiseMenuInformationConfiguration : IEntityTypeConfiguration<UserWiseMenuInformation>
    {
        public void Configure(EntityTypeBuilder<UserWiseMenuInformation> builder)
        {
            builder.HasKey(ps => ps.Id);
            builder.Property(ps => ps.UserId)
                .IsRequired();
            builder.Property(ps => ps.MenuId)
                .IsRequired();
        }
    }
}
