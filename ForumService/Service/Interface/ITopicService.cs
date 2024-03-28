using Domain.Entity;
using Domain.Response;
using Service.DTO;

namespace Service.Interface
{
	public interface ITopicService
	{
		public Task<BaseResponse<Topic>> CreateAsync(TopicDTO model);

		public Task<BaseResponse<Topic>> UpdateAsync(TopicDTO model);

		public Task<BaseResponse<Topic>> DeleteAsync(long id);

		public Task<BaseResponse<Topic>> GetAsync(long id);

		public Task<BaseResponse<IEnumerable<Topic>>> GetAllAsync();
	}
}
