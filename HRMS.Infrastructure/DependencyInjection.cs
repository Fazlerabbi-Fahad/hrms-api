using HRMS.Application.DTOs.Common;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;
using HRMS.Infrastructure.Data.HRMSDbContext;
using HRMS.Infrastructure.Repositories;
using HRMS.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HRMSDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            #region Register Repositories

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ISalaryRepository, SalaryRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDesignationRepository, DesignationRepository>();
            services.AddScoped<IEmploymentStatusRepository, EmploymentStatusRepository>();
            services.AddScoped<IPaymentStatusRepository, PaymentStatusRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPayrollRepository, PayrollRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IAuthRepository, AuthRepository>();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddScoped<ICacheService, CacheService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion Register Repositories

            return services;
        }
    }
}