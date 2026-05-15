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
        public static string DepartmentList(int page, int size, string? search)
        {
            return $"departments_p{page}_s{size}_q{search ?? "all"}";
        }

        public static string DepartmentById(int id)
        {
            return $"department_{id}";
        }
        public static string DesignationList(int page, int size, string? search)
        {
            return $"designations_p{page}_s{size}_q{search ?? "all"}";
        }

        public static string DesignationById(int id)
        {
            return $"designation_{id}";
        }
        public static string EmploymentStatusList(int page, int size, string? search)
        {
            return $"employment_statuses_p{page}_s{size}_q{search ?? "all"}";
        }

        public static string EmploymentStatusById(int id)
        {
            return $"employment_status_{id}";
        }
        public static string PaymentStatusList(int page, int size, string? search)
        {
            return $"payment_statuses_p{page}_s{size}_q{search ?? "all"}";
        }

        public static string PaymentStatusById(int id)
        {
            return $"payment_status_{id}";
        }
        public static string RoleList(int page, int size, string? search)
        {
            return $"roles_p{page}_s{size}_q{search ?? "all"}";
        }

        public static string RoleById(int id)
        {
            return $"role_{id}";
        }
        public static string SalaryList(int page, int size, string? search)
        {
            return $"salaries_p{page}_s{size}_q{search ?? "all"}";
        }

        public static string SalaryById(int id)
        {
            return $"salary_{id}";
        }
    }
}
