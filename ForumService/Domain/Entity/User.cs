using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
	public class User
	{
        public long Id { get; set; }

        public string Nickname { get; set; }

        public DateTime CreatedAt { get; set; }

		public virtual IEnumerable<Message> Messages { get; set; }

		public virtual IEnumerable<AttachmentToTopic> Attachmnets{ get; set; }
	}
}
