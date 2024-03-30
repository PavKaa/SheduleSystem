using Domain.Entity;
using Domain.Response;
using Microsoft.AspNetCore.Http;

namespace Service.Interface
{
	public interface IFileService
	{
		public Task<BaseResponse<FileModel>> CreateAsync(IFormFile uploadedFile, string webRootPath);
	}
}
