using BookIt.Application.DTOs.EventDTO;
using BookIt.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookIt.Presentation.Areas.Admin.Controllers
{
    public class EventController: Controller
    {
        private readonly IEventService _eventService;
        private readonly IGeneralLocationService _locationService;
        private readonly ICategoryService _categoryService;
        private readonly IHallService _hallService;

       
        public EventController(IEventService eventService, IGeneralLocationService locationService, ICategoryService categoryService, IHallService hallService)
        {
            _eventService = eventService;
            _locationService = locationService;
            _categoryService = categoryService;
            _hallService = hallService;
        }

        public IActionResult Index()
        {
            var events =  _eventService.GetAll();
            return View(events);
        }

        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventDTO dto)
        {
            // Handle image upload if needed.
            if (dto.ImagePath == null && Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                dto.ImagePath = "/uploads/" + fileName;
            }

            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(dto);
            }

            var result = await _eventService.CreateAsync(dto, ModelState);
            if (!result)
            {
                PopulateDropdowns();
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var dto = await _eventService.GetUpdatedDtoAsync(id);
            PopulateDropdowns();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateEventDTO dto)
        {
            // Handle image upload if needed.
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                dto.ImagePath = "/uploads/" + fileName;
            }

            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(dto);
            }

            var result = await _eventService.UpdateAsync(dto, ModelState);
            if (!result)
            {
                PopulateDropdowns();
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _eventService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Archived()
        {
            var archived = _eventService.GetArchivedEvents();
            return View(archived);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            await _eventService.RestoreAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDelete(int id)
        {
            await _eventService.HardDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropdowns()
        {
            // Populate General Locations.
            var locations = _locationService.GetAll()
                .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name })
                .ToList();
            ViewBag.Locations = locations;

            // Populate Categories, ParentCategories, etc.
            // You would do similar calls to your ICategoryService.
        }
    }
}

