using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HRMS.Infrastructure.Data.HRMSDbContext
{
    public class HRMSDbContext : DbContext
    {
        public HRMSDbContext(DbContextOptions<HRMSDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Designation> Designations => Set<Designation>();
        public DbSet<EmploymentStatus> EmploymentStatuses => Set<EmploymentStatus>();
        public DbSet<Salary> Salaries => Set<Salary>();
        public DbSet<Payroll> Payrolls => Set<Payroll>();
        public DbSet<PaymentStatus> PaymentStatuses => Set<PaymentStatus>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<Menu> Menu => Set<Menu>();
        public DbSet<UserWiseMenuInformation> UserWiseMenuInformations => Set<UserWiseMenuInformation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("HRMS");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
