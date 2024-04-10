using BusinessLogic.DTO;
using BusinessLogic.Interface;
using BusinessLogic.Tools;
using DAL.DbContext;
using DAL.Entity;
using DAL.Response;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;

namespace BusinessLogic.Implementation
{
	public class UserService : IUserService
	{
		private readonly ApplicationDbContext context;

		public UserService(ApplicationDbContext context)
		{
			this.context = context;
		}

		public async Task<BaseResponse<User>> Create(CreateUserDTO model)
		{
			var response = new BaseResponse<User>();

			try
			{
				var filter = Builders<User>.Filter.Or(
					Builders<User>.Filter.Eq(u => u.Email, model.Email),
					Builders<User>.Filter.Eq(u => u.Login, model.Login));
				var result = await context.Users.Find(filter).ToListAsync();

				if(result.Count > 0)
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					response.Message = "User is already exists!";
					return response;
				}

				Dictionary<string, string> dictionary = HashingTool.GenerateHash(model.Password);

				var user = new User()
				{
					Id = ObjectId.GenerateNewId().ToString(),
					Email = model.Email,
					Login = model.Login,
					HashPassword = dictionary["hash"],	
					Salt = dictionary["salt"]
				};

				await context.Users.InsertOneAsync(user);

				var userData = new UserData()
				{
					Id = ObjectId.GenerateNewId().ToString(),
					Group = model.Group,
					Name = model.Name,
					LastName = model.LastName,
					UserId = user.Id
				};

				await context.UsersData.InsertOneAsync(userData);

				response.Data = user;
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception)
			{
				response.Message = "Error while inserting new user";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<User>> Find(string id)
		{
			var response = new BaseResponse<User>();

			try
			{
				var filter = Builders<User>.Filter.Eq(u => u.Id, id);
				var entity = await context.Users.Find(filter).FirstAsync();

				if(entity == null) 
				{
					response.StatusCode = HttpStatusCode.NotFound;
					response.Message = "User with the specified id is not exists!";
					return response;
				}

				response.Data = entity;
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception)
			{
				response.Message = "Error while finding the user";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<bool>> Remove(string id)
		{
			var response = new BaseResponse<bool>();

			try
			{
				var filter = Builders<User>.Filter.Eq(u => u.Id, id);
				var result = await context.Users.FindOneAndDeleteAsync(filter);

				if(result == null)
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					response.Message = "User with the specified id is not exists!";
					return response;
				}

				response.StatusCode = HttpStatusCode.OK;
				response.Data = true;

				return response;
			}
			catch (Exception)
			{
				response.Message = "Error while deleting the user";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<User>> Update(UserDTO model)
		{
			var response = new BaseResponse<User>();

			try
			{
				var filter = Builders<User>.Filter.Eq(u => u.Id, model.Id);
				var oldEntity = await context.Users.Find(filter).FirstAsync();

				if (oldEntity == null)
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					response.Message = "User with the specified id is not exists!";
					return response;
				}

				var newEntity = new User()
				{
					Id = model.Id,
					Login = model.Login ?? oldEntity.Login,
					Email = model.Email ?? oldEntity.Login,
					Salt = oldEntity.Salt,
					HashPassword = oldEntity.HashPassword
				};

				if(!PasswordsComparer.ComparePasswords(oldEntity.HashPassword, model.Password, oldEntity.Salt))
				{
					var dictionary = HashingTool.GenerateHash(model.Password);

					newEntity.HashPassword = dictionary["hash"];
					newEntity.Salt = dictionary["salt"];
				}

				var update = Builders<User>.Update
					.Set(u => u.Login, newEntity.Login)
					.Set(u => u.Email, newEntity.Email)
					.Set(u => u.Salt, newEntity.Salt)
					.Set(u => u.HashPassword, newEntity.HashPassword);

				var result = await context.Users.UpdateOneAsync(filter, update);

				if(result.ModifiedCount > 0) 
				{
					response.StatusCode = HttpStatusCode.OK;
					response.Data = newEntity;
					
					return response;
				}
				else
				{
					response.StatusCode = HttpStatusCode.InternalServerError;
					response.Message = "Error while updating the user";
					return response;
				}
			}
			catch (Exception)
			{
				response.Message = "Error while updating the user";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}
	}
}
