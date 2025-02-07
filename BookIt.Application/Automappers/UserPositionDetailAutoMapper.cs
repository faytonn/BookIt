using AutoMapper;
using BookIt.Application.DTOs.UserPositionDetailDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class UserPositionDetailAutoMapper : Profile
{
    public UserPositionDetailAutoMapper()
    {
        CreateMap<UserPositionDetail, GetUserPositionDetailDTO>().ReverseMap();
    }
}
