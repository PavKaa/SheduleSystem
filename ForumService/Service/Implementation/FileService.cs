using DAL;
using Domain.Entity;
using Domain.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Service.Interface;
using System.Net;

namespace Service.Implementation
{
	public class FileService : IFileService
	{
		private readonly ApplicationDbContext _context;

		public FileService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<BaseResponse<FileModel>> CreateAsync(IFormFile uploadedFile, string webRootPath)
		{
			var response = new BaseResponse<FileModel>();

			try
			{
				string path = "/Files/" + uploadedFile.FileName;

				using (var fileStream = new FileStream(webRootPath + path, FileMode.Create))
				{
					await uploadedFile.CopyToAsync(fileStream);
				}

				var fileModel = new FileModel
				{
					Name = uploadedFile.FileName,
					Path = path
				};

				await _context.FileModels.AddAsync(fileModel);
				await _context.SaveChangesAsync();

				response.Data = fileModel;
				response.StatusCode = HttpStatusCode.OK;

				return response;
			}
			catch (Exception ex)
			{
				response.StatusCode = HttpStatusCode.InternalServerError;
				response.Message = $"Error while adding a new file: {ex.Message}";

				return response;
			}
		}
	}
}
