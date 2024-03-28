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
	public class MessageService : IMessageService
	{
		private readonly ApplicationDbContext _context;

		public MessageService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<BaseResponse<Message>> CreateAsync(MessageDTO model)
		{
			var response = new BaseResponse<Message>();

			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.UserId);
				var topic = await _context.Topics.FirstOrDefaultAsync(t => t.Id == model.TopicId);

				if (user == null || topic == null)
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					response.Message = "Incorrect user or topic id";

					return response;
				}

				var message = new Message
				{
					Content = model.Content,
					CreatedAt = DateTime.Now,
					ParentMessageId = model.ParentMessageId ?? 0,
					UserId = model.UserId,
					TopicId = model.TopicId
				};

				await _context.Messages.AddAsync(message);
				await _context.SaveChangesAsync();

				response.Data = message;
				response.StatusCode = HttpStatusCode.OK;

				return response;

			}
			catch (Exception ex)
			{
				response.Message = $"Error while adding message: {ex.Message}";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<Message>> DeleteAsync(long id)
		{
			var response = new BaseResponse<Message>();

			try
			{
				var messageToDelete = await _context.Messages.FirstOrDefaultAsync(t => t.Id == id);

				if (messageToDelete == null)
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					response.Message = "Invalid message id to delete";

					return response;
				}

				_context.Messages.Remove(messageToDelete);
				await _context.SaveChangesAsync();

				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception ex)
			{
				response.Message = $"Error while deleting message: {ex.Message}";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<IEnumerable<Message>>> GetAsyncByTopic(long topicId)
		{
			var response = new BaseResponse<IEnumerable<Message>>();

			try
			{
				var topic = await _context.Topics.FirstOrDefaultAsync(t => t.Id == topicId);

				if(topic == null)
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					response.Message = "Invalid topic id";

					return response;
				}

				response.Data = topic.Messages;
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception ex)
			{
				response.Message = $"Error while getting messages in topic: {ex.Message}";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<IEnumerable<Message>>> GetAsyncByUserAndTopic(long userId, long topicId)
		{
			var response = new BaseResponse<IEnumerable<Message>>();

			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
				var topic = await _context.Topics.FirstOrDefaultAsync(t => t.Id == topicId);

				if (user == null || topic == null)
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					response.Message = "Incorrect user or topic id";

					return response;
				}

				response.Data = user.Messages.Where(m => m.TopicId == topicId);
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception ex)
			{
				response.Message = $"Error while getting messages by topic and user: {ex.Message}";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}

		public async Task<BaseResponse<Message>> UpdateAsync(MessageDTO model)
		{
			var response = new BaseResponse<Message>();

			try
			{
				var messageToUpdate = await _context.Messages.FirstOrDefaultAsync(m => m.Id == model.Id);

				if (messageToUpdate == null)
				{
					response.StatusCode = HttpStatusCode.BadRequest;
					response.Message = "Invalid message id";

					return response;
				}

				messageToUpdate.Content = model.Content ?? messageToUpdate.Content;

				_context.Messages.Update(messageToUpdate);
				await _context.SaveChangesAsync();

				response.Data = messageToUpdate;
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception ex)
			{
				response.Message = $"Error while updating message: {ex.Message}";
				response.StatusCode = HttpStatusCode.InternalServerError;

				return response;
			}
		}
	}
}
