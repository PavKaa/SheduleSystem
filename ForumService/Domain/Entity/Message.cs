using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
	public class Message
	{
        public long Id { get; set; }

        public string Content { get; set; }

		public DateTime CreatedAt { get; set; }

        public long ParentMessageId { get; set; }
        public Message ParentMessage { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public long TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}
