using BookIt.Application.DTOs.EventAndDetailsCombinedDTO;
using BookIt.Application.Interfaces.Services.Generic;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookIt.Application.Interfaces.Services;

public interface IEventCompositeService : IGetService<GetEventCompositeDTO>, IModifyService<CreateEventCompositeDTO, UpdateEventCompositeDTO>
{
    //Task<bool> CreateEventWithDetailsAsync(CreateEventCompositeDTO dto, ModelStateDictionary modelState);

    //Task<GetEventCompositeDTO?> GetAsync(int id, LanguageType language = LanguageType.English);

    //Task<UpdateEventCompositeDTO?> GetByIdForUpdateAsync(int id, LanguageType language = LanguageType.English);

    //Task<bool> UpdateEventWithDetailsAsync(UpdateEventCompositeDTO dto, ModelStateDictionary modelState);

    Task HardDeleteAsync(int id, ModelStateDictionary modelState);

    Task RestoreAsync(int id, ModelStateDictionary modelState);
    public List<GetEventCompositeDTO> GetArchivedEvents(LanguageType languageType = LanguageType.English);




}
