using BookIt.Application.DTOs.Common;

namespace BookIt.Application.DTOs.CancellationRefundDTO;

public class CreateCancellationRefundDTO : IDTO
{
    public int ReservationId { get; set; }
    public DateTime CancellationDate { get; set; }
    public decimal RefundAmount { get; set; }
    public bool IsRefunded { get; set; }
}
