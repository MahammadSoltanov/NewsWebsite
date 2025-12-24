using Domain.Constants;
using Domain.Entities;
using Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.Persistence;
public class DataSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly SeedAdminOptions _seedAdmin;

    private readonly string _baseDirectory = AppContext.BaseDirectory;
    private readonly string _seedPath = Path.Combine("Persistence", "Seed");

    public DataSeeder(
       ApplicationDbContext context,
       RoleManager<Role> roleManager,
       UserManager<User> userManager,
       IOptions<SeedAdminOptions> seedAdminOptions)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _seedAdmin = seedAdminOptions.Value;
    }

    public async Task SeedAsync()
    {
        await SeedRoles();
        await SeedRootUser();
        await SeedLanguages();
    }

    private async Task SeedLanguages()
    {
        if (!await _context.Languages.AnyAsync())
        {
            Console.WriteLine(" ---> Seeding Languages... ");

            var languages = DeserializeToListFromSource<Language>("languages.json");

            foreach (var language in languages)
            {
                await _context.Languages.AddAsync(language);
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedRootUser()
    {
        var adminRole = await _roleManager.FindByNameAsync(_seedAdmin.Role);
        if (adminRole is null)
        {
            throw new Exception($"Role '{_seedAdmin.Role}' not found.");
        }

        var adminUser = await _userManager.FindByEmailAsync(_seedAdmin.Email);
        if (adminUser is not null)
        {
            return;
        }

        Console.WriteLine(" ---> Seeding Root User... ");
        var user = new User
        {
            Name = "System",
            Surname = "Admin",
            Email = _seedAdmin.Email,
            UserName = _seedAdmin.UserName,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user, _seedAdmin.Password);

        if (!result.Succeeded)
        {
            throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        await _userManager.AddToRoleAsync(user, adminRole.Name);
    }


    private async Task SeedRoles()
    {
        if (!await _context.Roles.AnyAsync())
        {
            await _roleManager.CreateAsync(new Role { Name = UserRole.Admin });
            await _roleManager.CreateAsync(new Role { Name = UserRole.Moderator });
            await _roleManager.CreateAsync(new Role { Name = UserRole.Journalist });

            await _context.SaveChangesAsync();
        }
    }

    #region Helper methods

    private List<T> DeserializeToListFromSource<T>(string sourceFileName)
    {
        var jsonPath = Path.Combine(_baseDirectory, _seedPath, sourceFileName);
        var json = File.ReadAllText(jsonPath);
        return JsonConvert.DeserializeObject<List<T>>(json);
    }

    private T DeserializeFromSource<T>(string sourceFileName)
    {
        var jsonPath = Path.Combine(_baseDirectory, _seedPath, sourceFileName);
        var json = File.ReadAllText(jsonPath);

        return JsonConvert.DeserializeObject<T>(json);
    }

    #endregion

}
