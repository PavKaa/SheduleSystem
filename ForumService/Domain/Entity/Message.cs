using Newtonsoft.Json;
using System.ComponentModel;

namespace Domain.Entity
{
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class Message
	{
		public long Id { get; set; }

		public string Content { get; set; }

		public DateTime CreatedAt { get; set; }

		[DefaultValue(null)]
		[JsonIgnore]
		public long? ParentMessageId { get; set; }
		[JsonIgnore]
		public Message ParentMessage { get; set; }

		public long UserId { get; set; }
		[JsonIgnore]
		public User User { get; set; }

		public long TopicId { get; set; }
		[JsonIgnore]
		public Topic Topic { get; set; }

		[DefaultValue(null)]
		[JsonIgnore]
		public virtual ICollection<FileModel> Files { get; set; }

		[DefaultValue(null)]
		[JsonIgnore]
		public ICollection<Message> Replies { get; set; }
	}
}
