using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
	public class User
	{
        [BsonId]
        public string Id { get; set; }

        [BsonIgnoreIfDefault]
		public string Login { get; set; }

        public string HashPassword { get; set; }

        public string Email { get; set; }

        public string Salt { get; set; }
    }
}
