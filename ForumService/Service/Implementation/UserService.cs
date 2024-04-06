using DAL;
using Domain.Entity;
using Domain.Response;
using Microsoft.EntityFrameworkCore;
using Service.DTO;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
	public class UserService : IUserService
	{
		private readonly ApplicationDbContext _context;

		public UserService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<BaseResponse<User>> CreateAsync(UserDTO model)
		{
			var response = new BaseResponse<User>();

			try
			{
				var user = new User
				{
					Nickname = model.Nickname,
					CreatedAt = DateTime.UtcNow,
				};

				await _context.Users.AddAsync(user);
				await _context.SaveChangesAsync();

				response.Data = user;
				response.StatusCode = HttpStatusCode.OK;

				return response;

			}
			catch (Exception ex)
			{
				response.Message = $"Error while adding user: {ex.Message}";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<User>> DeleteAsync(long id)
		{
			var response = new BaseResponse<User>();

			try
			{
				var userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

				if(userToDelete == null) 
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					response.Message = "Invalid user id to delete";

					return response;
				}

				_context.Users.Remove(userToDelete);
				await _context.SaveChangesAsync();

				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception ex)
			{
				response.Message = $"Error while deleting user: {ex.Message}";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<IEnumerable<User>>> GetAllAsync()
		{
			var response = new BaseResponse<IEnumerable<User>>();

			try
			{
				response.Data = await _context.Users.ToListAsync();
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception ex)
			{
				response.Message = $"Error while getting all users: {ex.Message}";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<User>> GetAsync(long id)
		{
			var response = new BaseResponse<User>();

			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

				if (user == null)
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					response.Message = "Invalid user id";

					return response;
				}

				response.Data = user;
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception ex)
			{
				response.Message = $"Error while getting user by id: {ex.Message}";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<User>> UpdateAsync(UserDTO model)
		{
			var response = new BaseResponse<User>();

			try
			{
				var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.Id);

				if (userToUpdate == null)
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					response.Message = "Invalid user id";

					return response;
				}

				userToUpdate.Nickname = model.Nickname ?? userToUpdate.Nickname;

				//await _context.Users
				//		.Where(u => u.Id == model.Id)
				//		.ExecuteUpdateAsync(setters => setters
				//			.SetProperty(u => u.Nickname, model.Nickname));

				_context.Users.Update(userToUpdate);
				await _context.SaveChangesAsync();

				response.Data = userToUpdate;
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception ex)
			{
				response.Message = $"Error while updating user: {ex.Message}";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}
	}
}
