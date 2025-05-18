using BookIt.Application.Interfaces.Services;
using BookIt.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BookIt.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventService _eventService;
        private readonly IGeneralLocationService _locationService;
        private readonly ICategoryService _categoryService;

        public HomeController(
            ILogger<HomeController> logger,
            IEventService eventService,
            IGeneralLocationService locationService,
            ICategoryService categoryService)
        {
            _logger = logger;
            _eventService = eventService;
            _locationService = locationService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Locations =  _locationService.GetAll();
            ViewBag.Categories =  _categoryService.GetAll();
            ViewBag.UpcomingEvents = await _eventService.GetUpcomingEventsAsync();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
