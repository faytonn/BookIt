using BookIt.Application.DTOs.LanguageDTO;
using BookIt.Domain.Entities;
using System.Linq.Expressions;

namespace BookIt.Application.Interfaces.Services;

public interface ILanguageService
{
    Task<GetLanguageDTO> GetLanguageAsync(Expression<Func<Language, bool>> predicate);
    List<GetLanguageDTO> GetAll();
}
