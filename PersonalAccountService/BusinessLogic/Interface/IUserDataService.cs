using BusinessLogic.DTO;
using DAL.Entity;
using DAL.Response;

namespace BusinessLogic.Interface
{
    public interface IUserDataService
    {
		Task<BaseResponse<UserData>> Create(UserDataDTO model);

		Task<BaseResponse<UserData>> Find(string id);

		Task<BaseResponse<UserData>> Update(UserDataDTO model);

		Task<BaseResponse<bool>> Remove(string id);
	}
}
