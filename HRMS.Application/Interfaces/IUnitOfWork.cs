using HRMS.Application.Interfaces.Repository;

namespace HRMS.Application.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IEmployeeRepository Employees { get; }
        IAuthRepository Auth { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
