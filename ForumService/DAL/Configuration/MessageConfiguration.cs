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

			builder.HasMany(m => m.Files)
				   .WithOne(f => f.Message)
				   .HasForeignKey(f => f.MessageId)
				   .OnDelete(DeleteBehavior.Cascade); //TODO: make the correct deletion of files from the wwwroot folder
		}
	}
}
