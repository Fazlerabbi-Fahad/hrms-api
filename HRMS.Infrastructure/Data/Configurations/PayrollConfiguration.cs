using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Data.Configurations
{
    public class PayrollConfiguration:IEntityTypeConfiguration<Payroll>
    {
        public void Configure(EntityTypeBuilder<Payroll> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Month)
                .IsRequired();
            builder.Property(p => p.Year)
                .IsRequired();
            builder.Property(p => p.GrossSalary)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(p => p.TotalDeductions)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(p => p.NetSalary)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasIndex(p => new { p.EmployeeId, p.Month, p.Year })
                   .IsUnique()
                   .HasFilter("[EmployeeId] IS NOT NULL AND [Month] IS NOT NULL AND [Year] IS NOT NULL");

            builder.HasOne(e => e.Employee)
                   .WithOne(d => d.Payroll)
                   .HasForeignKey<Payroll>(p => p.EmployeeId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Salary)
                   .WithOne(d => d.Payroll)
                   .HasForeignKey<Payroll>(p => p.SalaryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PaymentStatus)
                   .WithMany(es => es.Payrolls)
                   .HasForeignKey(e => e.PaymentStatusId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
