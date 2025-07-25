using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infrastructure.Persistence;
public class DataSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    private readonly string _baseDirectory = AppContext.BaseDirectory;
    private readonly string _seedPath = Path.Combine("Persistence", "Seed");

    public DataSeeder(ApplicationDbContext context, RoleManager<Role> roleManager, UserManager<User> userManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
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
        const string adminEmail = "admin@example.com";
        const string adminPassword = "P@assw0rd!123";

        var adminUser = await _userManager.FindByEmailAsync(adminEmail);
        var adminRole = await _roleManager.FindByNameAsync(UserRole.Admin);

        if (adminUser is null)
        {
            var user = new User()
            {
                Name = "System",
                Surname = "Admin",
                UserName = adminEmail,
                Password = adminPassword,
                RoleId = adminRole.Id,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var createResult = await _userManager.CreateAsync(user, adminPassword);

            if (createResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, adminRole.Name);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception(
                  "Failed to create admin user: " +
                  string.Join(", ", createResult.Errors)
                );
            }
        }
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
