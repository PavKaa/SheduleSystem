using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
	public class MessageDTO
	{
		public long Id { get; set; }

		public string Content { get; set; }

		public long? ParentMessageId { get; set; }

		public long UserId { get; set; }

		public long TopicId { get; set; }
	}
}
