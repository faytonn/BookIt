using BookIt.Application.Interfaces.Services;
using BookIt.Presentation.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace BookIt.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventService _eventService;
        private readonly IGeneralLocationService _locationService;
        private readonly ICategoryService _categoryService;
        private readonly RequestLocalizationOptions _localizationOptions;

        public HomeController(
            ILogger<HomeController> logger,
            IEventService eventService,
            IGeneralLocationService locationService,
            ICategoryService categoryService,
            IOptions<RequestLocalizationOptions> localizationOptions)
        {
            _logger = logger;
            _eventService = eventService;
            _locationService = locationService;
            _categoryService = categoryService;
            _localizationOptions = localizationOptions.Value;
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

        [HttpGet]
        public IActionResult SetLanguage(string culture, string? returnUrl = null)
        {
            var supported = _localizationOptions.SupportedUICultures?.Select(c => c.Name) ?? Enumerable.Empty<string>();
            if (!supported.Contains(culture))
            {
                culture = _localizationOptions.DefaultRequestCulture.UICulture.Name;
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
