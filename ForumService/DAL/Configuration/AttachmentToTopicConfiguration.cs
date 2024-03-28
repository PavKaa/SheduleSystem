using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configuration
{
	internal class AttachmentToTopicConfiguration : IEntityTypeConfiguration<AttachmentToTopic>
	{
		public void Configure(EntityTypeBuilder<AttachmentToTopic> builder)
		{
			builder.HasKey(att => new { att.UserId, att.TopicId });

			builder.HasOne(att => att.User)
				   .WithMany(u => u.Attachmnets)
				   .HasForeignKey(att => att.UserId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(att => att.Topic)
				   .WithMany(t => t.Attachmnets)
				   .HasForeignKey(att => att.TopicId)
				   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
