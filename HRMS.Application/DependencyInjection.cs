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

            #endregion Register Services

            return services;
        }
    }
}