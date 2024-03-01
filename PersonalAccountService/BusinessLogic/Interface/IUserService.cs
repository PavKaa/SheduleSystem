using BusinessLogic.DTO;
using DAL.DbContext;
using DAL.Entity;
using DAL.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IUserService
    {
        Task<BaseResponse<User>> Create(UserDTO model);

        Task<BaseResponse<User>> Find(string id);

        Task<BaseResponse<User>> Update(UserDTO model);

        Task<BaseResponse<bool>> Remove(string id);
    }
}
