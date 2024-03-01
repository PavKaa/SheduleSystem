using BusinessLogic.DTO;
using BusinessLogic.Interface;
using DAL.DbContext;
using DAL.Entity;
using DAL.Response;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;

namespace BusinessLogic.Implementation
{
    public class UserDataService : IUserDataService
    {
        private readonly ApplicationDbContext context;

        public UserDataService(ApplicationDbContext context)
        {
            this.context = context;
        }

		public async Task<BaseResponse<UserData>> Create(UserDataDTO model)
		{
			var response = new BaseResponse<UserData>();

			try
			{
				var userData = new UserData()
				{
					Id = ObjectId.GenerateNewId().ToString(),
					Group = model.Group,
					Name = model.Name,
					LastName = model.LastName,
					UserId = model.UserId
				};

				await context.UsersData.InsertOneAsync(userData);

				response.Data = userData;
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception)
			{
				response.Message = "Error while inserting new user data";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<UserData>> Find(string id)
		{
			var response = new BaseResponse<UserData>();

			try
			{
				var filter = Builders<UserData>.Filter.Eq(ud => ud.Id, id);
				var entity = await context.UsersData.Find(filter).FirstAsync();

				if (entity == null)
				{
					response.StatusCode = HttpStatusCode.NotFound;
					return response;
				}

				response.Data = entity;
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception)
			{
				response.Message = "Error while finding the user data";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<bool>> Remove(string id)
		{
			var response = new BaseResponse<bool>();

			try
			{
				var filter = Builders<UserData>.Filter.Eq(ud => ud.Id, id);
				var result = await context.UsersData.FindOneAndDeleteAsync(filter);

				if (result == null)
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					return response;
				}

				response.StatusCode = HttpStatusCode.OK;
				response.Data = true;

				return response;
			}
			catch (Exception)
			{
				response.Message = "Error while deleting the user data";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<UserData>> Update(UserDataDTO model)
		{
			var response = new BaseResponse<UserData>();

			try
			{
				var filter = Builders<UserData>.Filter.Eq(u => u.Id, model.Id);
				var oldEntity = await context.UsersData.Find(filter).FirstAsync();

				if (oldEntity == null)
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					return response;
				}

				var newEntity = new UserData()
				{
					Name = model.Name ?? oldEntity.Name,
					LastName = model.LastName ?? oldEntity.LastName,
					Group = model.Group ?? oldEntity.Group
				};

				var update = Builders<UserData>.Update
					.Set(ud => ud.Name, newEntity.Name)
					.Set(ud => ud.LastName, newEntity.LastName)
					.Set(ud => ud.Group, newEntity.Group);

				var result = await context.UsersData.UpdateOneAsync(filter, update);

				if (result.ModifiedCount > 0)
				{
					response.StatusCode = HttpStatusCode.OK;
					response.Data = newEntity;

					return response;
				}
				else
				{
					response.StatusCode = HttpStatusCode.InternalServerError;
					return response;
				}
			}
			catch (Exception)
			{
				response.Message = "Error while updating the user data";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}
	}
}
