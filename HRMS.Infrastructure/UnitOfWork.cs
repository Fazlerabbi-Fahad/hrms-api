using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;
using HRMS.Infrastructure.Data.HRMSDbContext;
using HRMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly HRMSDbContext _context;
        private IDbContextTransaction _transaction;
        private readonly ILogger<EmployeeRepository> _empLogger;
        private readonly ILogger<DepartmentRepository> _deptLogger;
        private readonly ILogger<DesignationRepository> _desigLogger;
        private readonly ILogger<EmploymentStatusRepository> _empStatusLogger;
        private readonly ILogger<PaymentStatusRepository> _payStatusLogger;
        private readonly ILogger<RoleRepository> _roleLogger;
        private readonly ILogger<SalaryRepository> _salaryLogger;

        private IEmployeeRepository _employees;
        private IAuthRepository _auth;
        private IDepartmentRepository _departments;
        private IDesignationRepository _designations;
        private IEmploymentStatusRepository _employmentStatuses;
        private IPaymentStatusRepository _employmentStatus;
        private IRoleRepository _roleRepository;
        private ISalaryRepository _salaryRepository;

        public IEmployeeRepository Employees => _employees??=new EmployeeRepository(_context, _empLogger);
        public IAuthRepository Auth => _auth??=new AuthRepository(_context);
        public IDepartmentRepository Departments => _departments ??= new DepartmentRepository(_context,_deptLogger);
        public IDesignationRepository Designations => _designations??=new DesignationRepository(_context, _desigLogger);
        public IEmploymentStatusRepository EmploymentStatuses => _employmentStatuses??=new EmploymentStatusRepository(_context, _empStatusLogger);
        public IPaymentStatusRepository PaymentStatuses => _employmentStatus??=new PaymentStatusRepository(_context, _payStatusLogger);
        public IRoleRepository Roles => _roleRepository??=new RoleRepository(_context, _roleLogger);
        public ISalaryRepository Salaries => _salaryRepository??=new SalaryRepository(_context, _salaryLogger);

        public UnitOfWork(HRMSDbContext context, ILogger<EmployeeRepository> empLogger)
        {
            _context = context;
            _empLogger = empLogger;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }

        public void Dispose() 
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

    }
}
