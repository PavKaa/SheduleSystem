using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
	internal class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasMany(u => u.Messages)
				   .WithOne(m => m.User)
				   .HasForeignKey(m => m.UserId)
				   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
