using BookIt.Application.DTOs.EventAndDetailsCombinedDTO;
using BookIt.Application.DTOs.EventDetailDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookIt.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventCompositeController : Controller
    {
        private readonly IEventCompositeService _eventCompositeService;
        private readonly IGeneralLocationService _locationService;
        private readonly ICategoryService _categoryService;
        private readonly ILanguageService _languageService;
        private readonly IHallService _hallService;

        public EventCompositeController(
            IEventCompositeService eventCompositeService, IGeneralLocationService locationService, ICategoryService categoryService,
            ILanguageService languageService, IHallService hallService)
        {
            _eventCompositeService = eventCompositeService;
            _locationService = locationService;
            _categoryService = categoryService;
            _languageService = languageService;
            _hallService = hallService;
        }

        public IActionResult Index()
        {
            var compositeEvents =  _eventCompositeService.GetAll();
            return View(compositeEvents);
        }

        public async Task<IActionResult> Details(int id)
        {
            var compositeEvent = await _eventCompositeService.GetAsync(id, LanguageType.English);
            if (compositeEvent == null)
                return NotFound();
            return View(compositeEvent);
        }

        public IActionResult Create()
        {
            var model = new CreateEventCompositeDTO();
            PopulateDropdowns(model);

            var languages = _languageService.GetAll();
            foreach (var language in languages)
            {
                model.EventDetail.Add(new CreateEventDetailDTO
                {
                    LanguageId = language.Id
                });
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventCompositeDTO model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns(model);
                return View(model);
            }

            if (!await _eventCompositeService.CreateAsync(model, ModelState))
            {
                PopulateDropdowns(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var compositeDto = await _eventCompositeService.GetUpdatedDtoAsync(id);
            if (compositeDto == null)
                return NotFound();

            PopulateDropdowns(compositeDto);
            return View(compositeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateEventCompositeDTO model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns(model);
                return View(model);
            }

            if (!await _eventCompositeService.UpdateEventWithDetailsAsync(model, ModelState))
            {
                PopulateDropdowns(model);
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var compositeEvent = await _eventCompositeService.GetByIdAsync(id);
            if (compositeEvent == null)
                return NotFound();
            return View(compositeEvent);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _eventCompositeService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> HardDelete(int id)
        {
            var compositeEvent = await _eventCompositeService.GetByIdAsync(id);
            if (compositeEvent == null)
                return NotFound();
            return View(compositeEvent);
        }

        [HttpPost, ActionName("HardDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDeleteConfirmed(int id)
        {
            if (!await _eventCompositeService.HardDeleteAsync(id, ModelState))
            {
                return RedirectToAction(nameof(HardDelete), new { id });
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            await _eventCompositeService.RestoreAsync(id, ModelState);

            return RedirectToAction(nameof(Index));
        }

        
        private void PopulateDropdowns(dynamic model)
        {
            var locations = _locationService.GetAll();
            model.GeneralLocations = new SelectList(locations, "Id", "Name", model.Event.GeneralLocationId).ToList();

            var categories = _categoryService.GetAll(LanguageType.English);
            model.Categories = new SelectList(categories, "Id", "Name", model.Event.CategoryId).ToList();

            var halls = _hallService.GetAll();
            model.Halls = new SelectList(halls, "Id", "Name").ToList();

            var languages = _languageService.GetAll();
            model.Languages = new SelectList(languages, "Id", "Name").ToList();
        }
    }
}
