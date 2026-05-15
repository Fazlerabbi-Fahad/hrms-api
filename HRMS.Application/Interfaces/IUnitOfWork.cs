using HRMS.Application.Interfaces.Repository;

namespace HRMS.Application.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IEmployeeRepository Employees { get; }
        IAuthRepository Auth { get; }
        IDepartmentRepository Departments { get; }
        IDesignationRepository Designations { get; }
        IPaymentStatusRepository PaymentStatuses { get; }
        IEmploymentStatusRepository EmploymentStatuses { get; }
        IRoleRepository Roles { get; }
        ISalaryRepository Salaries { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
