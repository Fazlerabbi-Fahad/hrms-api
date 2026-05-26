using Microsoft.Extensions.DependencyInjection;
using HRMS.Application.Interfaces;
using HRMS.Application.Services;

namespace HRMS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            #region Register Services

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISalaryService, SalaryService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IDesignationService, DesignationService>();
            services.AddScoped<IEmploymentStatusService, EmploymentStatusService>();
            services.AddScoped<IPaymentStatusService, PaymentStatusService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPayrollService, PayrollService>();
            services.AddScoped<IMenuService, MenuService>();

            #endregion Register Services

            return services;
        }
    }
}