using DAL.Configuration;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
	public class ApplicationDbContext : DbContext
	{
        public DbSet<User> Users { get; set; }

        public DbSet<Topic> Topics { get; set; }

		public DbSet<Message> Messages { get; set; }

        public DbSet<AttachmentToTopic> Attachments { get; set; }

        public DbSet<FileModel> FileModels { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> optionsBuilder) : base(optionsBuilder)
        {
            Database.EnsureCreated();
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new AttachmentToTopicConfiguration());
			modelBuilder.ApplyConfiguration(new MessageConfiguration());
			modelBuilder.ApplyConfiguration(new TopicConfiguration());
			modelBuilder.ApplyConfiguration(new UserConfiguration());

			//SaveChanges();
			base.OnModelCreating(modelBuilder);
		}
	}
}
