namespace HRMS.Application.DTOs.Common
{
    public class CacheKeys
    {
        //public const string EmployeeList = "employee_list";

        public static string EmployeeList(int page, int size, string? search)
        {
            return $"employees_p{page}_s{size}_q{search ?? "all"}";
        }

        public static string EmployeeById(int id)
        {
            return $"employee_{id}";
        }
    }
}
