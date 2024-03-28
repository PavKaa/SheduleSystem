using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
	internal class MessageConfiguration : IEntityTypeConfiguration<Message>
	{
		public void Configure(EntityTypeBuilder<Message> builder)
		{
			builder.HasOne(m => m.ParentMessage)
				   .WithOne()
				   .HasForeignKey<Message>(m => m.ParentMessageId)
				   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
