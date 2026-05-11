using HRMS.Application.Interfaces.Repository;
using HRMS.Infrastructure.Data.HRMSDbContext;
using HRMS.Infrastructure.Repositories;
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
            #endregion Register Repositories

            return services;
        }
    }
}
