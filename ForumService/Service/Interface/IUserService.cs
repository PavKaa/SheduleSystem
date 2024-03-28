using Domain.Entity;
using Domain.Response;
using Service.DTO;

namespace Service.Interface
{
	public interface IUserService
	{
		public Task<BaseResponse<User>> CreateAsync(UserDTO model);

		public Task<BaseResponse<User>> UpdateAsync(UserDTO model);

		public Task<BaseResponse<User>> DeleteAsync(long id);

		public Task<BaseResponse<User>> GetAsync(long id);

		public Task<BaseResponse<IEnumerable<User>>> GetAllAsync();
	}
}
