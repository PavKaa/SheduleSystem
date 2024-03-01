using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Response
{
	public class AuthenticateResponse
	{
		[JsonIgnore]
		public HttpStatusCode StatusCode { get; set; }
		[JsonIgnore]
		public string Message { get; set; }
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string JwtToken { get; set; }

		[JsonIgnore]
		public string RefreshToken { get; set; }

		public AuthenticateResponse(UserData userData, string jwtToken, string refreshToken, HttpStatusCode statusCode)
		{
			Id = userData.UserId;
			FirstName = userData.Name;
			LastName = userData.LastName;
			JwtToken = jwtToken;
			RefreshToken = refreshToken;
			StatusCode = statusCode;
		}

        public AuthenticateResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
