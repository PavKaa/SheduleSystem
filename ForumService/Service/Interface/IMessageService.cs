using Domain.Entity;
using Domain.Response;
using Service.DTO;

namespace Service.Interface
{
	public interface IMessageService
	{
		public Task<BaseResponse<Message>> CreateAsync(MessageDTO model);

		public Task<BaseResponse<Message>> UpdateAsync(MessageDTO model);

		public Task<BaseResponse<Message>> DeleteAsync(long id);

		public Task<BaseResponse<IEnumerable<Message>>> GetAsyncByTopic(long topicId);

		public Task<BaseResponse<IEnumerable<Message>>> GetAsyncByUserAndTopic(long userId, long topicId);
	}
}
