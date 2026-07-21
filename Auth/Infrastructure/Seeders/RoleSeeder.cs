using Auth.Constants;
using Auth.Domain;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Seeders;

public class RoleSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!await db.Roles.AnyAsync())
        {
            db.Roles.AddRange(
                new Role { Name = Roles.Customer },
                new Role { Name = Roles.Farmer },
                new Role { Name = Roles.Admin });

            await db.SaveChangesAsync();
        }
    }
}