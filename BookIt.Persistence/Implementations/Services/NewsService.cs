using AutoMapper;
using BookIt.Application.DTOs.NewsDTO;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.Implementations.Services;

public class NewsService : INewsService
{
    private readonly INewsRepository _newsRepository;
    private readonly IMapper _mapper;

    public NewsService(INewsRepository newsRepository, IMapper mapper)
    {
        _newsRepository = newsRepository;
        _mapper = mapper;
    }

    public async Task<List<GetNewsDTO>> GetLatestAsync(LanguageType language = LanguageType.English, int count = 3)
    {
        var news = await _newsRepository
            .GetAll(include: q => q.Include(n => n.NewsDetails.Where(d => d.LanguageId == (int)language)))
            .OrderByDescending(n => n.CreatedAt)
            .ThenByDescending(n => n.Id)
            .Take(count)
            .ToListAsync();

        return _mapper.Map<List<GetNewsDTO>>(news);
    }
}
