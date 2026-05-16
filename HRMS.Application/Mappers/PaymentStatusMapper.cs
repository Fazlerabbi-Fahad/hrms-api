using HRMS.Application.DTOs.PaymentStatus;
using HRMS.Domain.Entities;

namespace HRMS.Application.Mappers
{
    public class PaymentStatusMapper
    {
        public static PaymentStatusResponseDto ToResponseDto(PaymentStatus PaymentStatus)
        {
            return new PaymentStatusResponseDto
            {
                Id = PaymentStatus.Id,
                StatusName = PaymentStatus.StatusName,
            };
        }
        public static List<PaymentStatusResponseDto> ToResponseDtoList(List<PaymentStatus> PaymentStatuss)
        {
            return PaymentStatuss.Select(ToResponseDto).ToList();
        }
    }
}
