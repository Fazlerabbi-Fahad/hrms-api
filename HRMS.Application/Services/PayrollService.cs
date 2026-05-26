using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Payroll;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;
using HRMS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISalaryRepository _salaryRepository;
        private readonly ILogger<PayrollService> _logger;

        private const decimal TaxRate = 0.10m;
        private const decimal ProvidentFundRate = 0.05m;

        public PayrollService(
            IPayrollRepository payrollRepository,
            IEmployeeRepository employeeRepository,
            ISalaryRepository salaryRepository,
            ILogger<PayrollService> logger)
        {
            _payrollRepository = payrollRepository;
            _employeeRepository = employeeRepository;
            _salaryRepository = salaryRepository;
            _logger = logger;
        }

        public async Task<ApiResponse<PagedResult<PayrollResponseDto>>> GetAllAsync(
            PayrollQueryParameters parameters)
        {
            var (payrolls, totalCount) = await _payrollRepository
                .GetAllAsync(parameters);

            var pagedResult = new PagedResult<PayrollResponseDto>
            {
                Items = payrolls,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

            return ApiResponse<PagedResult<PayrollResponseDto>>.Success(
                pagedResult,
                $"Retrieved {payrolls.Count} of {totalCount} payroll records");
        }

        public async Task<ApiResponse<PayrollResponseDto>> GetByIdAsync(int id)
        {
            var payroll = await _payrollRepository.GetByIdAsync(id);

            if (payroll == null)
                return ApiResponse<PayrollResponseDto>.Failure(null,
                    "Payroll record not found", 404);

            return ApiResponse<PayrollResponseDto>.Success(payroll);
        }

        public async Task<ApiResponse<PayrollResponseDto>> ProcessPayrollAsync(
            PayrollRequestDto dto)
        {
            var alreadyExists = await _payrollRepository.ExistsAsync(
                dto.EmployeeId, dto.Month, dto.Year);

            if (alreadyExists)
                return ApiResponse<PayrollResponseDto>.Failure(null,
                    $"Payroll already processed for this employee " +
                    $"for {dto.Month}/{dto.Year}", 409);

            var employee = await _employeeRepository
                .GetEmployeeByIdAsync(dto.EmployeeId);

            if (employee == null)
                return ApiResponse<PayrollResponseDto>.Failure(null,
                    "Employee not found", 404);

            var salary = await _salaryRepository
                .GetActiveSalaryAsync(dto.EmployeeId);

            if (salary == null)
                return ApiResponse<PayrollResponseDto>.Failure(null,
                    $"No active salary found for {employee.EmpCode}. " +
                    "Please configure salary first.", 404);

            var grossSalary = CalculateGrossSalary(salary);
            var totalDeductions = CalculateDeductions(grossSalary);
            var netSalary = grossSalary - totalDeductions;

            _logger.LogInformation(
                "Processing payroll for {EmpCode} {Month}/{Year} " +
                "Gross:{Gross} Deductions:{Deductions} Net:{Net}",
                employee.EmpCode, dto.Month, dto.Year,
                grossSalary, totalDeductions, netSalary);

            var payroll = new Payroll
            {
                EmployeeId = dto.EmployeeId,
                SalaryId = salary.Id,
                Month = dto.Month,
                Year = dto.Year,
                GrossSalary = grossSalary,
                TotalDeductions = totalDeductions,
                NetSalary = netSalary,
                PaymentStatusId = 3, // Pending
                PaymentDate = null,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = dto.UserId,
                IsActive = true
            };

            await _payrollRepository.CreateAsync(payroll);

            var result = await _payrollRepository.GetByIdAsync(payroll.Id);

            _logger.LogInformation(
                "Payroll processed successfully for {EmpCode}", employee.EmpCode);

            return ApiResponse<PayrollResponseDto>.Success(
                result!, "Payroll processed successfully", 201);
        }

        public async Task<ApiResponse<PayrollResponseDto>> MarkAsPaidAsync(
            int id, int userId)
        {
            var payroll = await _payrollRepository.GetEntityByIdAsync(id);

            if (payroll == null)
                return ApiResponse<PayrollResponseDto>.Failure(null,
                    "Payroll record not found", 404);

            if (payroll.PaymentStatusId == 4)
                return ApiResponse<PayrollResponseDto>.Failure(null,
                    "Payroll is already marked as paid", 409);

            if (payroll.PaymentStatusId == 5)
                return ApiResponse<PayrollResponseDto>.Failure(null,
                    "Cannot mark a cancelled payroll as paid", 400);

            payroll.PaymentStatusId = 4;
            payroll.PaymentDate = DateTime.UtcNow;
            payroll.UpdatedBy = userId;
            payroll.UpdatedAt = DateTime.UtcNow;

            await _payrollRepository.UpdateAsync(payroll);

            _logger.LogInformation(
                "Payroll {PayrollId} marked as paid by user {UserId}", id, userId);

            var result = await _payrollRepository.GetByIdAsync(id);

            return ApiResponse<PayrollResponseDto>.Success(
                result!, "Payroll marked as paid successfully");
        }

        public async Task<ApiResponse<PayrollReportDto>> GetMonthlyReportAsync(
            int month, int year)
        {
            if (month < 1 || month > 12)
                return ApiResponse<PayrollReportDto>.Failure(null,
                    "Invalid month", 400);

            var report = await _payrollRepository
                .GetMonthlyReportAsync(month, year);

            if (report == null || report.TotalEmployees == 0)
                return ApiResponse<PayrollReportDto>.Failure(null,
                    $"No payroll records found for {month}/{year}", 404);

            return ApiResponse<PayrollReportDto>.Success(
                report, $"Payroll report for {report.MonthName} {year}");
        }

        private static decimal CalculateGrossSalary(Salary salary)
        {
            return salary.BasicSalary
                 + salary.HouseAllowance
                 + salary.MedicalAllowance
                 + (salary.TransportAllowance ?? 0)
                 + (salary.Bonus ?? 0);
        }

        private static decimal CalculateDeductions(decimal grossSalary)
        {
            var tax = grossSalary * TaxRate;
            var providentFund = grossSalary * ProvidentFundRate;
            return Math.Round(tax + providentFund, 2);
        }
    }
}
