using HRMS.Application.DTOs.Common;
using HRMS.Domain.Entities;

namespace HRMS.Application.Interfaces.Repository
{
    public interface IPaymentStatusRepository
    {
        Task<(List<PaymentStatus>, int totalCount)> GetAllPaymentStatusAsync(QueryParameters parameters);
        Task<PaymentStatus?> GetPaymentStatusByIdAsync(int id);
        Task<PaymentStatus> CreatePaymentStatusAsync(PaymentStatus PaymentStatus);
        Task<PaymentStatus> UpdatePaymentStatusAsync(int id, PaymentStatus PaymentStatus);
        Task<bool> DeletePaymentStatusAsync(int id, int userId);
    }
}
