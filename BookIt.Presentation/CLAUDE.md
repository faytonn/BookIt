# BookIt.Presentation — controller and view conventions

Loaded when working in this project. Repo-wide rules are in `../CLAUDE.md`.

## Controller shape (admin)

```csharp
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class XController : Controller
{
    public IActionResult Index() => View(_xService.GetAll());

    public IActionResult Create() { _populateDropdowns(); return View(); }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateXDTO dto)
    {
        if (!ModelState.IsValid) { _populateDropdowns(); return View(dto); }
        if (!await _xService.CreateAsync(dto, ModelState)) { _populateDropdowns(); return View(dto); }
        return RedirectToAction(nameof(Index));
    }
}
```

- Every state-changing action (`Create`, `Update`, `Delete`, `Restore`, `HardDelete`, `Confirm`, `Cancel`) is `[HttpPost]` + `[ValidateAntiForgeryToken]`. Views use `<form method="post">` for them, never `<a>` links.
- Errors reach the view through `ModelState` + `<div asp-validation-summary="All">` / `asp-validation-for`. Do not loop errors into `TempData["Error"]`.
- Dropdown data goes through `ViewBag.{Name} = List<SelectListItem>` built in a private `_populate...()` helper. Not `ViewData[...] = SelectList`, not lists inside DTOs.
- Cascading dropdowns use small JSON actions (`GetHallsByLocation(int generalLocationId)`) returning `Json(list.Select(h => new { id = h.Id, name = h.Name }))`.
- Admin controllers require `[Authorize(Roles = "Admin")]` (Moderator may be added where it makes sense). Public controllers that need a user use `[Authorize]`.

## Views

- Admin: KaiAdmin template. Page = `<div class="card"><div class="card-body">` + `<table class="table table-striped">`, action buttons `btn btn-warning` (Update) / `btn btn-danger` (Delete) / `btn btn-success` (Create).
- Public: Boleto template under `wwwroot/user`. Layout injects `ILayoutService`, `SignInManager`, `UserManager`.
- `@model List<GetXDTO>` for lists, `@model CreateXDTO` for forms. Use tag helpers (`asp-for`, `asp-action`, `asp-items`).
- Validation scripts via `@section Scripts { @{await Html.RenderPartialAsync("_ValidationScriptsPartial");} }`.
