using AutoMapper;
using BookIt.Application.DTOs.UserPositionDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class UserPositionAutoMapper : Profile
{
    public UserPositionAutoMapper()
    {
        CreateMap<UserPosition, GetUserPositionDTO>().ReverseMap();
    }
}
