using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.PaymentStatus;

namespace HRMS.Application.Interfaces
{
    public interface IPaymentStatusService
    {
        Task<ApiResponse<PagedResult<PaymentStatusResponseDto>>> GetAllPaymentStatussAsync(QueryParameters parameters);
        Task<ApiResponse<PaymentStatusResponseDto>> GetPaymentStatusByIdAsync(int id);
        Task<ApiResponse<PaymentStatusResponseDto>> CreatePaymentStatusAsync(PaymentStatusRequestDto dto);
        Task<ApiResponse<PaymentStatusResponseDto>> UpdatePaymentStatusAsync(int id, PaymentStatusUpdateRequestDto dto);
        Task<ApiResponse<bool>> DeletePaymentStatusAsync(int id, int userId);
    }
}
