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

        private IEmployeeRepository _employees;
        private IAuthRepository _auth;

        public IEmployeeRepository Employees => _employees??=new EmployeeRepository(_context, _empLogger);
        public IAuthRepository Auth => _auth??=new AuthRepository(_context);

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
