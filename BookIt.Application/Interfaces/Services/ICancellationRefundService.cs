using BookIt.Application.DTOs.CancellationRefundDTO;
using BookIt.Application.Interfaces.Services.Generic;
using BookIt.Domain.Enums;

namespace BookIt.Application.Interfaces.Services;

public interface ICancellationRefundService : IGetService<GetCancellationRefundDTO>, IModifyService<CreateCancellationRefundDTO, UpdateCancellationRefundDTO>
{
    Task<bool> ApproveRefundAsync(int refundId, string approvedBy);
    Task<bool> RejectRefundAsync(int refundId, string rejectedBy);
    Task<List<GetCancellationRefundDTO>> GetRefundsByStatusAsync(PaymentStatus paymentStatus);
}
