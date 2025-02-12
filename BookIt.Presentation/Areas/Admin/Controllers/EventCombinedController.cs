using AutoMapper;
using BookIt.Application.DTOs.EventAndDetailsCombinedDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventCombinedController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IEventDetailService _eventDetailService;
        private readonly IGeneralLocationService _locationService;
        private readonly ICategoryService _categoryService;
        private readonly ILanguageService _languageService;
        private readonly IHallService _hallService;
        private readonly IMapper _mapper;

        public EventCombinedController(IEventService eventService, IEventDetailService eventDetailService, IGeneralLocationService locationService,
                                      ICategoryService categoryService, ILanguageService languageService, IHallService hallService, IMapper mapper)
        {
            _eventService = eventService;
            _eventDetailService = eventDetailService;
            _locationService = locationService;
            _categoryService = categoryService;
            _languageService = languageService;
            _hallService = hallService;
            _mapper = mapper;
        }

        public IActionResult Create()
        {
            var model = new EventCombinedDTO();

            PopulateDropdowns(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCombinedDTO model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns(model);
                return View(model);
            }

            var eventCreated = await _eventService.CreateAsync(model.Event, ModelState);
            if (!eventCreated)
            {
                PopulateDropdowns(model);
                return View(model);
            }

            var createdEvent = await _eventService.GetAsyncByTitle(model.Event.Title);
            if (createdEvent == null)
            {
                ModelState.AddModelError("", "Event creation failed: could not retrieve created event.");
                PopulateDropdowns(model);
                return View(model);
            }

            model.EventDetail.EventId = createdEvent.Id;
            model.EventDetail.EventDate = model.Event.EventDate;

            var detailCreated = await _eventDetailService.CreateAsync(model.EventDetail, ModelState);
            if (!detailCreated)
            {
                ModelState.AddModelError("", "Event detail creation failed.");
                PopulateDropdowns(model);
                return View(model);
            }

            return RedirectToAction("Index", "Event", new { area = "Admin" });
        }

        private void PopulateDropdowns(EventCombinedDTO model)
        {
            var locations = _locationService.GetAll();
            model.GeneralLocations = new SelectList(locations, "Id", "Name", model.Event.GeneralLocationId);

            var categories = _categoryService.GetAll(LanguageType.English);
            model.Categories = new SelectList(categories, "Id", "Name", model.Event.CategoryId);

            var languages = _languageService.GetAll();
            model.Languages = new SelectList(languages, "Id", "Name", model.EventDetail.LanguageId);

            var halls = _hallService.GetAll();
            model.Halls = new SelectList(halls, "Id", "Name", model.EventDetail.HallId);
        }
    }


}
