using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
	public class AttachmentToTopic
	{
        public long Id { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public long TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}
