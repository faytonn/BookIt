using AutoMapper;
using BookIt.Application.DTOs.LanguageDTO;
using BookIt.Application.Interfaces.Repositories;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Entities;
using Org.BouncyCastle.Crypto;
using System.Linq.Expressions;

namespace BookIt.Infrastracture.Implementations.Services;

public class LanguageService : ILanguageService
{
    private readonly ILanguageRepository _repository;
    private readonly IMapper _mapper;

    public LanguageService(ILanguageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public List<GetLanguageDTO> GetAll()
    {
        var languages = _repository.GetAll();

        var dtos = _mapper.Map<List<GetLanguageDTO>>(languages);

        return dtos;
    }

    public async Task<GetLanguageDTO> GetLanguageAsync(Expression<Func<Language, bool>> predicate)
    {
        var language = await _repository.GetAsync(predicate);

        var dto = _mapper.Map<GetLanguageDTO>(language);

        return dto;
    }
}
