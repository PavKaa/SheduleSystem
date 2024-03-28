using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
	public class TopicDTO
	{
		public long? Id { get; set; }

		public string Title { get; set; }

		public long CreatorId { get; set; }
	}
}
