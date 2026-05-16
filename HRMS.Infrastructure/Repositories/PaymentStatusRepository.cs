using HRMS.Application.DTOs.Common;
using HRMS.Application.Interfaces.Repository;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Data.HRMSDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure.Repositories
{
    public class PaymentStatusRepository: IPaymentStatusRepository
    {
        private readonly HRMSDbContext _hrmsDbContext;
        private readonly ILogger<PaymentStatusRepository> _logger;

        public PaymentStatusRepository(HRMSDbContext hrmsDbContext, ILogger<PaymentStatusRepository> logger)
        {
            _hrmsDbContext = hrmsDbContext;
            _logger = logger;
        }

        public async Task<(List<PaymentStatus>, int totalCount)> GetAllPaymentStatusAsync(QueryParameters parameters)
        {
            var query = _hrmsDbContext.PaymentStatuses.Where(e => e.IsActive);

            if (!string.IsNullOrEmpty(parameters.Search))
                query = query.Where(e =>
                    e.StatusName.Contains(parameters.Search));

            query = parameters.SortBy?.ToLower() switch
            {
                "PaymentStatusRepository" => parameters.SortDirection == "desc"
                        ? query.OrderByDescending(e => e.StatusName)
                        : query.OrderBy(e => e.StatusName),
                _ => query.OrderBy(e => e.Id),
            };

            var totalCount = await query.CountAsync();

            var PaymentStatus = await query
                                .Skip((parameters.PageNumber - 1) * totalCount)
                                .Take(parameters.PageSize)
                                .ToListAsync();
            return (PaymentStatus, totalCount);

        }

        public async Task<PaymentStatus?> GetPaymentStatusByIdAsync(int id)
        {
            return await _hrmsDbContext.PaymentStatuses.Where(e => e.Id == id && e.IsActive).FirstOrDefaultAsync();
        }

        public async Task<PaymentStatus> CreatePaymentStatusAsync(PaymentStatus PaymentStatus)
        {

            if (PaymentStatus == null)
            {
                throw new InvalidOperationException("PaymentStatus data is null");
            }
            var existingPaymentStatus = await _hrmsDbContext.PaymentStatuses.Where(x => (x.StatusName == PaymentStatus.StatusName) && x.IsActive).FirstOrDefaultAsync();
            if (existingPaymentStatus != null)
            {
                _logger.LogWarning("PaymentStatus already found!", existingPaymentStatus.StatusName);
                throw new InvalidOperationException("PaymentStatus with this email or phone already exists");
            }
            var previousPaymentStatus = await _hrmsDbContext.PaymentStatuses.OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            PaymentStatus.IsActive = true;
            PaymentStatus.CreatedAt = DateTime.UtcNow;

            await _hrmsDbContext.PaymentStatuses.AddAsync(PaymentStatus);
            return PaymentStatus;
        }

        public async Task<PaymentStatus> UpdatePaymentStatusAsync(int id, PaymentStatus PaymentStatus)
        {
            var existingPaymentStatus = await _hrmsDbContext.PaymentStatuses.Where(x => x.Id == id && x.IsActive).FirstOrDefaultAsync();
            if (existingPaymentStatus == null)
            {
                _logger.LogWarning("PaymentStatus not found!", existingPaymentStatus.StatusName);
                throw new Exception("PaymentStatus not found");
            }

            var duplicate = await _hrmsDbContext.PaymentStatuses
                                    .AnyAsync(x => x.StatusName == PaymentStatus.StatusName
                                                && x.Id != id
                                                && x.IsActive);

            if (duplicate)
                throw new InvalidOperationException("A payment status with this name already exists.");

            existingPaymentStatus.StatusName = PaymentStatus.StatusName;
            existingPaymentStatus.StatusDisplayName = PaymentStatus.StatusDisplayName;
            existingPaymentStatus.UpdatedBy = PaymentStatus.UpdatedBy;
            existingPaymentStatus.UpdatedAt = DateTime.UtcNow;

            return existingPaymentStatus;
        }

        public async Task<bool> DeletePaymentStatusAsync(int id, int userId)
        {
            var PaymentStatus = await _hrmsDbContext.PaymentStatuses.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (PaymentStatus == null)
            {
                return false;
            }
            PaymentStatus.IsActive = false;
            PaymentStatus.UpdatedAt = DateTime.UtcNow;
            PaymentStatus.UpdatedBy = userId;

            return true;
        }
    }
}
