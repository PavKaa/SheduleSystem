using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
	public class RefreshToken
	{
		public string Id { get; set; }

		public string Token { get; set; }

		public bool isActive { get; set; }

		public DateTime Created { get; set; }

		public DateTime Expires { get; set; }

		public string UserId { get; set; }

		public bool isExpired => DateTime.UtcNow >= Expires;
	}
}
