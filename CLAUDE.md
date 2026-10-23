# BookIt — project conventions

Event/ticket booking site. ASP.NET Core 8 MVC + EF Core 8 (SQL Server) + ASP.NET Identity + AutoMapper + Cloudinary + MailKit.
Admin panel under `BookIt.Presentation/Areas/Admin`, public site under `BookIt.Presentation/Controllers` + `Views`.

## Git

- Never add `Co-Authored-By` or any Claude attribution to commits.
- One task = one commit. Commit messages are plain English, imperative-ish, no prefixes.
- `bin/`, `obj/`, `.vs/` must stay out of commits.

## Build / run

```
dotnet build BookIt.sln
cd BookIt.Presentation && dotnet run --launch-profile http   # http://localhost:5078
```

Startup (`Program.cs`) calls `DbContextInitializer.InitDatabaseAsync()`: applies migrations, seeds roles from `Role` enum and the admin user from `appsettings.json:Admin`.
Migrations live in `BookIt.Persistence/Migrations`; add with `dotnet ef migrations add <Name> -p BookIt.Persistence -s BookIt.Presentation`.
There are no tests yet. If a test project is added it goes at repo root as `BookIt.Tests` (xUnit).

## Solution layout

| Project | Holds |
|---|---|
| `BookIt.Domain` | Entities (`Entities/`), enums (`Enums/`), appsettings POCOs (`AppSettingModels/`) |
| `BookIt.Application` | DTOs (`DTOs/{X}DTO/`), interfaces (`Interfaces/Repositories`, `Interfaces/Services`), AutoMapper profiles (`Automappers/`), `Exceptions/` |
| `BookIt.Persistence` | `AppDbContext`, `Configurations/`, `Migrations/`, `Implementations/Repositories`, `Implementations/Services` (business services live here), `DataInitializers/`, `Interceptors/` |
| `BookIt.Infrastracture` | External integrations: `UserAuthorizationService`, `CloudinaryService`, `EmailService`, localizers |
| `BookIt.Presentation` | MVC app, views, `wwwroot` (admin template under `wwwroot/admin`, public template under `wwwroot/user`) |
| `BookIt.Core` | Empty. Do not add to it. |
| `BookIt/` (root folder) | Dead template project, not in the sln. Ignore. |

Keep this split. Business services stay in Persistence; only external-system adapters go in Infrastracture.
**Namespace must match folder** (`BookIt.Persistence.Implementations.Services`, `BookIt.Presentation.Areas.Admin.Controllers`, …). Some old files drift; fix when touching them.

## Layering rules

Controller → `I{X}Service` → `I{X}Repository` → `AppDbContext`.

- Controllers never use repositories or `AppDbContext`.
- Every entity gets: `I{X}Repository : IRepository<X>` + `{X}Repository : Repository<X>, I{X}Repository` (usually empty), and `I{X}Service` + `{X}Service`.
- Register both in `PersistenceServiceRegistration._addRepositories` / `._addServices` (all `AddScoped`). A service that is not registered crashes at first request — check this first when adding one.
- Services implement the generic contracts from `Application/Interfaces/Services/Generic`:
  - `IGetService<GetDTO>` → `GetAsync(id, language)`, `GetAll(language)`, `GetPagesAsync(language, page, limit)`, `IsExistAsync(id)`
  - `IModifyService<CreateDTO, UpdateDTO>` → `CreateAsync(dto, ModelState)`, `UpdateAsync(dto, ModelState)`, `GetUpdatedDtoAsync(id)`, `DeleteAsync(id)`
  - plus per-entity `RestoreAsync(id)`, `HardDeleteAsync(id)`, `GetArchived{X}s(language)`.

## Repository contract (`IRepository<T>`)

- `GetAsync(id)`, `GetAsync(predicate, include, ignoreFilter)`, `GetAll(predicate, include, ignoreFilter)` returns `IQueryable`, `IsExistAsync`, `Paginate(query, limit, page)`.
- `CreateAsync`, `Update`, `SoftDelete`, `HardDelete`, `Repair` **do not save**. The service calls `await _repo.SaveChangesAsync()` once at the end of the operation.
- Includes are passed as lambdas: `include: q => q.Include(e => e.Category).Include(e => e.GeneralLocation)`.
- Soft delete = `IsDeleted = true`; global `HasQueryFilter(x => !x.IsDeleted)` in `AppDbContext.OnModelCreating`. Archived queries use `GetAll(ignoreFilter: true).Where(x => x.IsDeleted)`.
- `BaseEntityInterceptor` fills `CreatedAt/UpdatedAt/CreatedBy/UpdatedBy` and turns `Remove()` into a soft delete. Do not set audit fields by hand.

## Entities

- `BaseEntity { int Id }`; `BaseAuditableEntity` adds audit + `IsDeleted`. Use auditable for anything a user manages.
- Required nav/string: `= null!`. Optional: `?`. Collections: `= []`.
- Multi-language content = `{X}` + `{X}Detail { LanguageId, Language }` pair (Category/CategoryDetail, Event/EventDetail, Setting/SettingDetail). `LanguageType` enum: 1 = English, 2 = Azerbaijani, 3 = Czech, matching seeded `Language` rows.
- Every entity gets an `IEntityTypeConfiguration<X>` in `Persistence/Configurations/{X}Configuration.cs`: `ToTable("Xs")`, `HasKey`, `Property().IsRequired().HasMaxLength()`, `decimal` columns get `HasColumnType("decimal(18,2)")`, every relationship declares `OnDelete`.
- Add a `DbSet<X>` in `AppDbContext` and a `HasQueryFilter` line for auditable entities.

## DTOs & mapping

- `DTOs/{X}DTO/Create{X}DTO.cs`, `Update{X}DTO.cs`, `Get{X}DTO.cs`; all `: IDTO`. Suffix is uppercase `DTO`.
- **Validation: DataAnnotations on Create/Update DTOs** (`[Required]`, `[MaxLength]`, `[Range]`, `[EmailAddress]`). Business rules (uniqueness, existence, availability) stay in the service via `modelState.AddModelError(...)`.
- AutoMapper profile per entity: `Application/Automappers/{X}AutoMapper.cs`, `class {X}AutoMapper : Profile`. Default `CreateMap<X, GetXDTO>().ReverseMap()`; use `ForMember` only for computed fields (e.g. `LocationName`). Update maps onto the tracked entity: `_mapper.Map(dto, entity)`.
- DTOs must not reference MVC types (`SelectListItem`, `IFormFile` is tolerated for uploads).

## Service method shape

```csharp
public async Task<bool> CreateAsync(CreateXDTO dto, ModelStateDictionary modelState)
{
    if (!modelState.IsValid)
        return false;

    var existing = await _xRepository.GetAsync(x => x.Name.ToLower() == dto.Name.ToLower());
    if (existing != null)
    {
        modelState.AddModelError("", "An X with this name already exists.");
        return false;
    }

    var entity = _mapper.Map<X>(dto);
    await _xRepository.CreateAsync(entity);
    await _xRepository.SaveChangesAsync();
    return true;
}
```

- GET-by-id that finds nothing **throws `NotFoundException`** (`Application/Exceptions`). Never return `null`, never `Console.WriteLine`.
- Create/Update return `bool` and report problems through `ModelState`.
- Pagination fills **all five** `PaginateDTO` fields: `Items`, `TotalCount`, `CurrentPage`, `PageSize`, `PageCount`. Clamp `page` to `[1, PageCount]`.
- Async all the way: `ToListAsync`, `CountAsync`, `AnyAsync`. No `.Result`, no `.Wait()`, no `Thread.Sleep`.
- No `Console.WriteLine`. Use `ILogger<T>` if logging is needed.
- Anything that writes more than one aggregate (reservation + seats, event + details) must save in one `SaveChangesAsync` or inside an explicit transaction.

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

## Naming

- Private fields `_camelCase`, `readonly`, constructor-injected.
- **Private methods `_camelCase`** (`_populateDropdowns`, `_sendConfirmEmailTokenAsync`). Public methods PascalCase, async ones end in `Async` (except the sync `GetAll`).
- File-scoped namespaces (`namespace X;`). One type per file, file name = type name.
- Views: `Areas/Admin/Views/{Controller}/{Index,Create,Update,Archived}.cshtml`, shared partials in `Views/Shared/Partials/_{Name}Partial.cshtml`.

## Views

- Admin: KaiAdmin template. Page = `<div class="card"><div class="card-body">` + `<table class="table table-striped">`, action buttons `btn btn-warning` (Update) / `btn btn-danger` (Delete) / `btn btn-success` (Create).
- Public: Boleto template under `wwwroot/user`. Layout injects `ILayoutService`, `SignInManager`, `UserManager`.
- `@model List<GetXDTO>` for lists, `@model CreateXDTO` for forms. Use tag helpers (`asp-for`, `asp-action`, `asp-items`).
- Validation scripts via `@section Scripts { @{await Html.RenderPartialAsync("_ValidationScriptsPartial");} }`.

## Config & auth

- Settings POCOs in `Domain/AppSettingModels`, bound with `_configuration.GetSection("X").Get<T>()` in the consuming class ctor.
- Identity options in `PersistenceServiceRegistration`: password min 6, upper+lower required, email confirmation required, unique email.
- `ApplicationUser : IdentityUser` with `FirstName/LastName/IsActive/IsDeleted/SelectedLanguage`. Roles from `Role` enum.
- Localization: resx under `Presentation/Resources/Layout.{en,az,cs}.resx`; culture via cookie (`CookieService`).
