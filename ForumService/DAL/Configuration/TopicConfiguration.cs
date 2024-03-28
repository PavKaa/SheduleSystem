using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
	internal class TopicConfiguration : IEntityTypeConfiguration<Topic>
	{
		public void Configure(EntityTypeBuilder<Topic> builder)
		{
			builder.HasMany(t => t.Messages)
				   .WithOne(m => m.Topic)
				   .HasForeignKey(m => m.TopicId)
				   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
