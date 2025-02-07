using AutoMapper;
using BookIt.Application.DTOs.CancellationRefundDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class CancellationRefundAutoMapper : Profile
{
    public CancellationRefundAutoMapper()
    {
        CreateMap<CancellationRefund, GetCancellationRefundDTO>().ReverseMap();
        CreateMap<CancellationRefund, UpdateCancellationRefundDTO>().ReverseMap();
        CreateMap<CancellationRefund, CreateCancellationRefundDTO>().ReverseMap();
    }
}
