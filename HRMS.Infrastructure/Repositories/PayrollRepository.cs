using HRMS.Application.DTOs.Payroll;
using HRMS.Application.Interfaces.Repository;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Data.HRMSDbContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories
{
    public class PayrollRepository : IPayrollRepository
    {
        private readonly HRMSDbContext _hrmsDbContext;

        public PayrollRepository(HRMSDbContext context)
        {
            _hrmsDbContext = context;
        }

        public async Task<(List<PayrollResponseDto> payrolls, int totalCount)> GetAllAsync(
            PayrollQueryParameters parameters)
        {
            var employeeIdParam = new SqlParameter("@EmployeeId",
                (object?)parameters.EmployeeId ?? DBNull.Value);
            var monthParam = new SqlParameter("@Month",
                (object?)parameters.Month ?? DBNull.Value);
            var yearParam = new SqlParameter("@Year",
                (object?)parameters.Year ?? DBNull.Value);
            var statusParam = new SqlParameter("@PaymentStatusId",
                (object?)parameters.PaymentStatusId ?? DBNull.Value);
            var pageParam = new SqlParameter("@PageNumber", parameters.PageNumber);
            var sizeParam = new SqlParameter("@PageSize", parameters.PageSize);

            var connection = _hrmsDbContext.Database.GetDbConnection();
            await connection.OpenAsync();

            var payrolls = new List<PayrollResponseDto>();
            int totalCount = 0;

            using var command = connection.CreateCommand();
            command.CommandText = "EXEC HRMS.sp_GetAllPayrolls " +
                "@EmployeeId, @Month, @Year, @PaymentStatusId, @PageNumber, @PageSize";

            command.Parameters.Add(employeeIdParam);
            command.Parameters.Add(monthParam);
            command.Parameters.Add(yearParam);
            command.Parameters.Add(statusParam);
            command.Parameters.Add(pageParam);
            command.Parameters.Add(sizeParam);

            using var reader = await command.ExecuteReaderAsync();


            while (await reader.ReadAsync())
            {
                payrolls.Add(new PayrollResponseDto
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                    EmpCode = reader.GetString(reader.GetOrdinal("EmpCode")),
                    EmployeeName = reader.GetString(reader.GetOrdinal("EmployeeName")),
                    Department = reader.GetString(reader.GetOrdinal("DepartmentName")),
                    Month = reader.GetInt32(reader.GetOrdinal("Month")),
                    Year = reader.GetInt32(reader.GetOrdinal("Year")),
                    BasicSalary = reader.GetDecimal(reader.GetOrdinal("BasicSalary")),
                    HouseAllowance = reader.GetDecimal(reader.GetOrdinal("HouseAllowance")),
                    MedicalAllowance = reader.GetDecimal(reader.GetOrdinal("MedicalAllowance")),
                    TransportAllowance = reader.IsDBNull(reader.GetOrdinal("TransportAllowance"))
                        ? 0 : reader.GetDecimal(reader.GetOrdinal("TransportAllowance")),
                    Bonus = reader.IsDBNull(reader.GetOrdinal("Bonus"))
                        ? 0 : reader.GetDecimal(reader.GetOrdinal("Bonus")),
                    GrossSalary = reader.GetDecimal(reader.GetOrdinal("GrossSalary")),
                    TotalDeductions = reader.GetDecimal(reader.GetOrdinal("TotalDeductions")),
                    NetSalary = reader.GetDecimal(reader.GetOrdinal("NetSalary")),
                    PaymentStatus = reader.GetString(reader.GetOrdinal("PaymentStatusName")),
                    PaymentDate = reader.IsDBNull(reader.GetOrdinal("PaymentDate"))
                        ? null : reader.GetDateTime(reader.GetOrdinal("PaymentDate")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                });
            }

            if (await reader.NextResultAsync() && await reader.ReadAsync())
            {
                totalCount = reader.GetInt32(0);
            }

            await connection.CloseAsync();
            return (payrolls, totalCount);
        }

        public async Task<PayrollResponseDto?> GetByIdAsync(int id)
        {
            var idParam = new SqlParameter("@Id", id);
            var connection = _hrmsDbContext.Database.GetDbConnection();
            await connection.OpenAsync();

            PayrollResponseDto? result = null;

            using var command = connection.CreateCommand();
            command.CommandText = "EXEC HRMS.sp_GetPayrollById @Id";
            command.Parameters.Add(idParam);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                result = new PayrollResponseDto
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                    EmpCode = reader.GetString(reader.GetOrdinal("EmpCode")),
                    EmployeeName = reader.GetString(reader.GetOrdinal("EmployeeName")),
                    Department = reader.GetString(reader.GetOrdinal("DepartmentName")),
                    Month = reader.GetInt32(reader.GetOrdinal("Month")),
                    Year = reader.GetInt32(reader.GetOrdinal("Year")),
                    BasicSalary = reader.GetDecimal(reader.GetOrdinal("BasicSalary")),
                    HouseAllowance = reader.GetDecimal(reader.GetOrdinal("HouseAllowance")),
                    MedicalAllowance = reader.GetDecimal(reader.GetOrdinal("MedicalAllowance")),
                    TransportAllowance = reader.IsDBNull(
                        reader.GetOrdinal("TransportAllowance"))
                        ? 0 : reader.GetDecimal(reader.GetOrdinal("TransportAllowance")),
                    Bonus = reader.IsDBNull(reader.GetOrdinal("Bonus"))
                        ? 0 : reader.GetDecimal(reader.GetOrdinal("Bonus")),
                    GrossSalary = reader.GetDecimal(reader.GetOrdinal("GrossSalary")),
                    TotalDeductions = reader.GetDecimal(reader.GetOrdinal("TotalDeductions")),
                    NetSalary = reader.GetDecimal(reader.GetOrdinal("NetSalary")),
                    PaymentStatus = reader.GetString(reader.GetOrdinal("PaymentStatusName")),
                    PaymentDate = reader.IsDBNull(reader.GetOrdinal("PaymentDate"))
                        ? null : reader.GetDateTime(reader.GetOrdinal("PaymentDate")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                };
            }

            await connection.CloseAsync();
            return result;
        }

        public async Task<PayrollReportDto?> GetMonthlyReportAsync(int month, int year)
        {
            var monthParam = new SqlParameter("@Month", month);
            var yearParam = new SqlParameter("@Year", year);

            var connection = _hrmsDbContext.Database.GetDbConnection();
            await connection.OpenAsync();

            PayrollReportDto? report = null;

            using var command = connection.CreateCommand();
            command.CommandText = "EXEC HRMS.sp_GetPayrollMonthlyReport @Month, @Year";
            command.Parameters.Add(monthParam);
            command.Parameters.Add(yearParam);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                report = new PayrollReportDto
                {
                    Month = reader.GetInt32(reader.GetOrdinal("Month")),
                    Year = reader.GetInt32(reader.GetOrdinal("Year")),
                    TotalEmployees = reader.GetInt32(reader.GetOrdinal("TotalEmployees")),
                    TotalGrossSalary = reader.GetDecimal(
                        reader.GetOrdinal("TotalGrossSalary")),
                    TotalDeductions = reader.GetDecimal(
                        reader.GetOrdinal("TotalDeductions")),
                    TotalNetSalary = reader.GetDecimal(reader.GetOrdinal("TotalNetSalary")),
                    PendingCount = reader.GetInt32(reader.GetOrdinal("PendingCount")),
                    PaidCount = reader.GetInt32(reader.GetOrdinal("PaidCount"))
                };
            }

            await connection.CloseAsync();
            return report;
        }

        public async Task<bool> ExistsAsync(int employeeId, int month, int year)
        {
            return await _hrmsDbContext.Payrolls.AnyAsync(p =>
                p.EmployeeId == employeeId &&
                p.Month == month &&
                p.Year == year);
        }

        public async Task<Payroll> CreateAsync(Payroll payroll)
        {
            await _hrmsDbContext.Payrolls.AddAsync(payroll);
            await _hrmsDbContext.SaveChangesAsync();
            return payroll;
        }

        public async Task<Payroll?> GetEntityByIdAsync(int id)
        {
            return await _hrmsDbContext.Payrolls
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payroll> UpdateAsync(Payroll payroll)
        {
            payroll.UpdatedAt = DateTime.UtcNow;
            _hrmsDbContext.Payrolls.Update(payroll);
            await _hrmsDbContext.SaveChangesAsync();
            return payroll;
        }
    }
}
