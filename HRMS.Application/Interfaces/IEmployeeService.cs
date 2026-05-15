using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Employee;

namespace HRMS.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<ApiResponse<PagedResult<EmployeeResponseDto>>> GetAllEmployeesAsync(EmployeeQueryParameters parameters);
        Task<ApiResponse<EmployeeResponseDto>> GetEmployeeByIdAsync(int id);
        Task<ApiResponse<EmployeeResponseDto>> CreateEmployeeAsync(EmployeeRequestDto dto);
        Task<ApiResponse<EmployeeResponseDto>> UpdateEmployeeAsync(int id,EmployeeUpdateRequestDto dto);
        Task<ApiResponse<bool>> DeleteEmployeeAsync(int id,int userId);
    }
}
