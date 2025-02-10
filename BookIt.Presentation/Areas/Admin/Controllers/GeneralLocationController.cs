using BookIt.Application.DTOs.GeneralLocationDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using BookIt.Persistence.Implementations.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookIt.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GeneralLocationController : Controller
    {
        private readonly IGeneralLocationService _locationService;

        public GeneralLocationController(IGeneralLocationService locationService)
        {
            _locationService = locationService;
        }

        public IActionResult Index()
        {
            var locations =  _locationService.GetAll(language: LanguageType.English);
            return View(locations);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGeneralLocationDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _locationService.CreateAsync(dto, ModelState);
            if (!result)
                return View(dto);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var dto = await _locationService.GetUpdatedDtoAsync(id);
            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateGeneralLocationDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _locationService.UpdateAsync(dto, ModelState);
            if (!result)
                return View(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _locationService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Archived()
        {
            var archivedLocs = _locationService.GetArchivedLocations(language: LanguageType.English);

            var allLocs = _locationService.GetAll(LanguageType.English);

            ViewBag.allLocs = allLocs;

            return View(archivedLocs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            await _locationService.RestoreAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDelete(int id)
        {
            await _locationService.HardDeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
