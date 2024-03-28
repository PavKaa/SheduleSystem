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
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
	public class TopicService : ITopicService
	{
		private readonly ApplicationDbContext _context;

		public TopicService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<BaseResponse<Topic>> CreateAsync(TopicDTO model)
		{
			var response = new BaseResponse<Topic>();

			try
			{
				var creator = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.CreatorId);

				if(creator == null) 
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					response.Message = "Incorrect creator's id";

					return response;
				}

				var topic = new Topic
				{
					Title = model.Title,
					CreatorId = model.CreatorId,
					CreatedAt = DateTime.Now,
				};

				await _context.Topics.AddAsync(topic);
				await _context.SaveChangesAsync();

				response.Data = topic;
				response.StatusCode = HttpStatusCode.OK;

				return response;

			}
			catch (Exception ex)
			{
				response.Message = $"Error while adding topic: {ex.Message}";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<Topic>> DeleteAsync(long id)
		{
			var response = new BaseResponse<Topic>();

			try
			{
				var topicToDelete = await _context.Topics.FirstOrDefaultAsync(t => t.Id == id);

				if (topicToDelete == null)
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					response.Message = "Invalid topic id to delete";

					return response;
				}

				_context.Topics.Remove(topicToDelete);
				await _context.SaveChangesAsync();

				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception ex)
			{
				response.Message = $"Error while deleting topic: {ex.Message}";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<IEnumerable<Topic>>> GetAllAsync()
		{
			var response = new BaseResponse<IEnumerable<Topic>>();

			try
			{
				response.Data = await _context.Topics.ToListAsync();
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception ex)
			{
				response.Message = $"Error while getting all topics: {ex.Message}";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<Topic>> GetAsync(long id)
		{
			var response = new BaseResponse<Topic>();

			try
			{
				var topic = await _context.Topics.FirstOrDefaultAsync(t => t.Id == id);

				if (topic == null)
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					response.Message = "Invalid topic id";

					return response;
				}

				response.Data = topic;
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception ex)
			{
				response.Message = $"Error while getting topic by id: {ex.Message}";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<Topic>> UpdateAsync(TopicDTO model)
		{
			var response = new BaseResponse<Topic>();

			try
			{
				var topicToUpdate = await _context.Topics.FirstOrDefaultAsync(t => t.Id == model.Id);

				if (topicToUpdate == null)
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					response.Message = "Invalid topic id";

					return response;
				}

				topicToUpdate.Title = model.Title ?? topicToUpdate.Title;

				_context.Topics.Update(topicToUpdate);
				await _context.SaveChangesAsync();

				response.Data = topicToUpdate;
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception ex)
			{
				response.Message = $"Error while updating topic: {ex.Message}";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}
	}
}
