using AutoMapper;
using BookIt.Application.DTOs.PaymentTransactionDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class PaymentTransactionDTO : Profile
{
    public PaymentTransactionDTO()
    {
        CreateMap<PaymentTransaction, GetPaymentTransactionDTO>().ReverseMap();
        CreateMap<PaymentTransaction, UpdatePaymentTransactionDTO>().ReverseMap();
        CreateMap<PaymentTransaction, CreatePaymentTransactionDTO>().ReverseMap();
    }
}
