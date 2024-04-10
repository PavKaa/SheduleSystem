using DAL;
using Microsoft.EntityFrameworkCore;

namespace ForumService.Extensions
{
	public static class MigrationServiceExtension
	{
		public static IHost MigrateDatabase(this IHost host)
		{
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var dbContext = services.GetRequiredService<ApplicationDbContext>();
					dbContext.Database.Migrate();
				}
				catch (Exception ex)
				{
					Console.WriteLine("An error occurred while migrating the database.");
					Console.WriteLine(ex);
				}
			}
			return host;
		}
	}
}
