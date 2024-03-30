using Domain.Entity;
using Domain.Response;
using Service.DTO;

namespace Service.Interface
{
	public interface IMessageService
	{
		public Task<BaseResponse<Message>> CreateAsync(MessageDTO model);

		public Task<BaseResponse<Message>> CreateAsync(byte[] byteArray);

		public Task<BaseResponse<Message>> UpdateAsync(MessageDTO model);

		public Task<BaseResponse<Message>> DeleteAsync(long id);

		public Task<BaseResponse<IEnumerable<Message>>> GetByTopicAsync(long topicId);

		public Task<BaseResponse<IEnumerable<Message>>> GetByUserAndTopicAsync(long userId, long topicId);
	}
}
