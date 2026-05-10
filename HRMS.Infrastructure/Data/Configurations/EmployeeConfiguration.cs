using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Data.Configurations
{
    public class EmployeeConfiguration: IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Email)
                  .HasMaxLength(150);

            builder.Property(e => e.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(e => e.EmpCode)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.HasIndex(e => e.Email)
                   .IsUnique()
                   .HasFilter("[Email] IS NOT NULL"); 

            builder.HasIndex(e => e.PhoneNumber)
                   .IsUnique()
                   .HasFilter("[PhoneNumber] IS NOT NULL");

            builder.HasIndex(e => e.EmpCode)
                   .IsUnique()
                   .HasFilter("[EmpCode] IS NOT NULL");


            builder.HasOne(e => e.Department)
                   .WithMany(d => d.Employees)
                   .HasForeignKey(e => e.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Designation)
                   .WithMany(d => d.Employees)
                   .HasForeignKey(e => e.DesignationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.EmploymentStatus)
                   .WithMany(es => es.Employees)
                   .HasForeignKey(e => e.EmploymentStatusId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
