using BusinessLogic.DTO;
using BusinessLogic.Interface;
using BusinessLogic.Tools;
using DAL.DbContext;
using DAL.Entity;
using DAL.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogic.Implementation
{
	public class LoginService : ILoginService
	{
		private readonly ApplicationDbContext context;
		private readonly IConfiguration configuration;

		public LoginService(ApplicationDbContext context, IConfiguration configuration)
		{
			this.context = context;
			this.configuration = configuration;
		}

		public async Task<User?> Authenticate(UserLoginDTO model)
		{
			try
			{
				var filter = Builders<User>.Filter.Or(
					Builders<User>.Filter.Eq(u => u.Login, model.Login),
					Builders<User>.Filter.Eq(u => u.Email, model.Email));

				var user = await context.Users.Find(filter).FirstAsync();

				if(user != null && PasswordsComparer.ComparePasswords(user.HashPassword, model.Password, user.Salt))
				{
					return user;
				}

				return null;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<AuthenticateResponse> GenerateTokens(User model)
		{
			try
			{
				var userDataFilter = Builders<UserData>.Filter.Eq(ud => ud.UserId, model.Id);
				var userData = await context.UsersData.Find(userDataFilter).FirstAsync();

				var securityKey = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
				var tokenHandler = new JwtSecurityTokenHandler();
				var tokenDescriptor = new SecurityTokenDescriptor()
				{
					Audience = configuration["Jwt:Audience"],
					Issuer = configuration["Jwt:Issuer"],
					Claims = new Dictionary<string, object>()
					{
						{ "Login", model.Login }
					},
					IssuedAt = DateTime.UtcNow,
					Expires = DateTime.UtcNow.AddMinutes(15),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256)
				};

				var accessToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
				var refreshToken = new RefreshToken()
				{
					Id = ObjectId.GenerateNewId().ToString(),
					Token = await GetUniqueToken(),
					isActive = true,
					Created = DateTime.UtcNow,
					Expires = DateTime.UtcNow.AddDays(7),
					UserId = model.Id
				};

				var refreshTokenFilter = Builders<RefreshToken>.Filter.And(
					Builders<RefreshToken>.Filter.Eq(rt => rt.isActive, true),
					Builders<RefreshToken>.Filter.Eq(rt => rt.UserId, model.Id));
				var update = Builders<RefreshToken>.Update.Set(rt => rt.isActive, false);

				await context.RefreshTokens.UpdateManyAsync(refreshTokenFilter, update);
				await context.RefreshTokens.InsertOneAsync(refreshToken);

				return new AuthenticateResponse(userData, tokenHandler.WriteToken(accessToken),
														refreshToken.Token, HttpStatusCode.OK);
			}
			catch (Exception)
			{
				return new AuthenticateResponse(HttpStatusCode.InternalServerError);
			}
		}

		public async Task<AuthenticateResponse> RefreshTokens(string refreshToken)
		{
			try
			{
				var refreshTokenFilter = Builders<RefreshToken>.Filter.Eq(rt => rt.Token, refreshToken);
				var refreshTokenUpdate = Builders<RefreshToken>.Update.Set(rt => rt.isActive, false);
				var oldRefreshToken = await context.RefreshTokens.FindOneAndUpdateAsync(refreshTokenFilter, refreshTokenUpdate);

				if (oldRefreshToken == null || oldRefreshToken.isActive || oldRefreshToken.isExpired) 
				{
					return new AuthenticateResponse(HttpStatusCode.BadRequest);
				}

				var userFilter = Builders<User>.Filter.Eq(u => u.Id, oldRefreshToken.UserId);
				var user = await context.Users.Find(userFilter).FirstAsync();

				var response = await GenerateTokens(user);

				return response;
			}
			catch (Exception)
			{
				return new AuthenticateResponse(HttpStatusCode.InternalServerError);
			}
		}

		private async Task<string> GetUniqueToken()
		{
			var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
			var filter = Builders<RefreshToken>.Filter.Eq(rt => rt.Token, token);

			var result = await context.RefreshTokens.Find(filter).FirstOrDefaultAsync();

			if(result != null)
			{
				return await GetUniqueToken();
			}

			return token;
		}
	}
}
