using BookIt.Application.DTOs.Common;
using BookIt.Domain.Enums;

namespace BookIt.Application.DTOs.PaymentTransactionDTO;

public class CreatePaymentTransactionDTO : IDTO
{
    public int ReservationId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public PaymentStatus PaymentStatus { get; set; } 
    public string TransactionReference { get; set; } = null!;
}
