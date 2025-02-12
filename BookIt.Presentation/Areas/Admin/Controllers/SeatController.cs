using BookIt.Application.DTOs.SeatDTO;
using BookIt.Application.Interfaces.Services;
using BookIt.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.Web.Controllers
{
    public class SeatController : Controller
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        public async Task<IActionResult> Index(int hallId)
        {
            var seats = await _seatService.GetSeatsByHallAsync(hallId);
            ViewBag.HallId = hallId;
            return View(seats);
        }

        public IActionResult Create(int hallId)
        {
            var dto = new CreateSeatDTO { HallId = hallId };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSeatDTO dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _seatService.CreateAsync(dto, ModelState);
                if (result)
                    return RedirectToAction("Index", new { hallId = dto.HallId });
            }
            return View(dto);
        }
        public IActionResult BulkCreate(int hallId)
        {
            var dto = new CreateBulkSeatDTO { HallId = hallId };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> BulkCreate(CreateBulkSeatDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _seatService.BulkCreateSeatsAsync(dto, ModelState);
                return RedirectToAction("Index", new { hallId = dto.HallId });
            }
            return View(dto);
        }

        public async Task<IActionResult> Update(int id)
        {
            var dto = await _seatService.GetUpdatedDtoAsync(id);
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateSeatDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _seatService.UpdateAsync(dto, ModelState);
                return RedirectToAction("Index", new { hallId = dto.HallId });
            }
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int hallId)
        {
            await _seatService.DeleteAsync(id);
            return RedirectToAction("Index", new { hallId });
        }

        public IActionResult Archived(LanguageType language = LanguageType.English)
        {
            var archivedSeats = _seatService.GetArchivedSeats(language);
            return View(archivedSeats);
        }
    }
}
