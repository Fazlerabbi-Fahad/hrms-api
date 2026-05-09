using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Data.Configurations
{
    public class SalaryConfiguration:IEntityTyperConfiguration<Salary>
    {
        public void Configure(EntityTypeBuilder<Salary> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.BasicSalary)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(s => s.HouseAllowance)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(s => s.MedicalAllowance)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(s => s.TransportAllowance)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(e => e.Employee)
                    .WithOne(d => d.Salary)
                    .HasForeignKey<Salary>(s => s.EmployeeId)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
