using Newtonsoft.Json;

namespace Domain.Entity
{
	[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
	public class Message
	{
        public long Id { get; set; }

        public string Content { get; set; }

		public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public long ParentMessageId { get; set; }
        public Message ParentMessage { get; set; }

		[JsonIgnore]
		public long UserId { get; set; }
        public User User { get; set; }

		[JsonIgnore]
		public long TopicId { get; set; }
		[JsonIgnore]
		public Topic Topic { get; set; }

        public virtual ICollection<FileModel> Files { get; set; }
    }
}
