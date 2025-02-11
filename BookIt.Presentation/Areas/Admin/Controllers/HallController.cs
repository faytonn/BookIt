using BookIt.Application.DTOs.HallDTO;
using BookIt.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace BookIt.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HallController : Controller
    {
        private readonly IHallService _hallService;
        private readonly IGeneralLocationService _locationService;

        public HallController(IHallService hallService, IGeneralLocationService locationService)
        {
            _hallService = hallService;
            _locationService = locationService;
        }

        public IActionResult Index()
        {
            var halls = _hallService.GetAll();
            return View(halls);
        }

        public IActionResult Create()
        {
            PopulateLocations();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateHallDTO dto)
        {
            if (!ModelState.IsValid)
            {
                PopulateLocations();
                return View(dto);
            }

            var result = await _hallService.CreateAsync(dto, ModelState);
            if (!result)
            {
                PopulateLocations();
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var dto = await _hallService.GetUpdatedDtoAsync(id);
            if (dto == null)
                return NotFound();

            PopulateLocations();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateHallDTO dto)
        {
            if (!ModelState.IsValid)
            {
                PopulateLocations();
                return View(dto);
            }

            var result = await _hallService.UpdateAsync(dto, ModelState);
            if (!result)
            {
                PopulateLocations();
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _hallService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Archived()
        {
            var archivedHalls = _hallService.GetArchivedHalls();

            return View(archivedHalls);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            await _hallService.RestoreAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDelete(int id)
        {
            await _hallService.HardDeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private void PopulateLocations()
        {
            var locations = _locationService.GetAll()
                               .Select(loc => new SelectListItem
                               {
                                   Value = loc.Id.ToString(),
                                   Text = loc.Name
                               })
                               .ToList();

            ViewBag.Locations = locations;
        }
    }
}
