using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
	public class UserData
	{
		[BsonId]
		public string Id { get; set; }

		public string Name { get; set; }

        public string LastName { get; set; }

		[BsonIgnoreIfDefault]
		public string Group { get; set; }


		public string UserId { get; set; }
	}
}
