using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entity
{
	public class FileModel
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public string Path { get; set; }

		public long MessageId { get; set; }

		[JsonIgnore]
		public Message Message { get; set; }
    }
}
