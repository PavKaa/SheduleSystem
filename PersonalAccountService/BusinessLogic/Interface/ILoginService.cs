using BusinessLogic.DTO;
using DAL.Entity;
using DAL.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
	public interface ILoginService
	{
		Task<User?> Authenticate(UserLoginDTO model);

		Task<AuthenticateResponse> GenerateTokens(User model);

		Task<AuthenticateResponse> RefreshTokens(string refreshToken);
	}
}
