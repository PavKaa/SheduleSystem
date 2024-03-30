using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
	public class Topic
	{
		public long Id { get; set; }

        public string Title { get; set; }

        public long CreatorId { get; set; }

        public DateTime CreatedAt { get; set; }

		public virtual ICollection<Message> Messages { get; set; }

		public virtual ICollection<AttachmentToTopic> Attachmnets { get; set; }
	}
}
