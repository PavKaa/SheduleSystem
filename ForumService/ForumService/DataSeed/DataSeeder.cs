using DAL;
using Microsoft.EntityFrameworkCore;

namespace ForumService.DataSeed
{
	public class DataSeeder
	{
		public DataSeeder(ApplicationDbContext context)
		{
			var migration = context.Database.GenerateCreateScript();
			context.Database.ExecuteSqlRaw(migration);
		}
	}
}
