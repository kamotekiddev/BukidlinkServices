namespace Auth.Infrastructure.Seeders;

public class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await RoleSeeder.SeedAsync(db);
    }
}