using BookIt.Domain.AppSettingModels;
using BookIt.Domain.Entities;
using BookIt.Domain.Enums;
using BookIt.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookIt.Persistence.DataInitializers;

public class DbContextInitializer
{
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public DbContextInitializer(AppDbContext appDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task InitDatabaseAsync()
    {
        await _appDbContext.Database.MigrateAsync();

        await _createRolesAsync();

        await _createAdminAsync();
    }

    private async Task _createRolesAsync()
    {
        var roles = Enum.GetNames(typeof(Role));

        foreach (var role in roles)
        {
            if (await _roleManager.FindByNameAsync(role) != null)
                continue;

            await _roleManager.CreateAsync(new IdentityRole { Name = role });
        }
    }
    private async Task _createAdminAsync()
    {
        var admin = _configuration.GetSection("Admin").Get<Admin>();

        if (admin == null)
            return;

        var existUser = await _userManager.FindByNameAsync(admin.Username);

        if (existUser != null)
            return;

        var newUser = new ApplicationUser
        {
            UserName = admin.Username,
            Email = admin.Email,
            FirstName = admin.Firstname,
            LastName = admin.Lastname,
            PhoneNumber = "502107941",
            Address = "Azerbaijan/Baku",
            IsActive = true,
            IsSubscribed = false
        };

        var result = await _userManager.CreateAsync(newUser, admin.Password);

        if (!result.Succeeded)
            throw new Exception("Admin is not created");

        result = await _userManager.AddToRoleAsync(newUser, Role.Admin.ToString());

        if (!result.Succeeded)
            throw new Exception("Admin is not assigned");
    }
}
