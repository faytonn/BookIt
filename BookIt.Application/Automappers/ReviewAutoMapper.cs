using AutoMapper;
using BookIt.Application.DTOs.ReviewDTO;
using BookIt.Domain.Entities;

namespace BookIt.Application.Automappers;

public class ReviewAutoMapper : Profile
{
    public ReviewAutoMapper()
    {
        CreateMap<Review, GetReviewDTO>().ReverseMap();
        CreateMap<Review, UpdateReviewDTO>().ReverseMap();
        CreateMap<Review, CreateReviewDTO>().ReverseMap();
    }
}
