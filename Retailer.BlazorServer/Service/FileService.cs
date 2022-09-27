using Microsoft.AspNetCore.Components.Forms;
using Retailer.BlazorServer.Service.IService;

namespace Retailer.BlazorServer.Service
{
    public class FileService : IFileService
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadFile(IBrowserFile file)
        {
            FileInfo fileInfo = new(file.Name);

            var fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
            var folderDirectory = $"{_webHostEnvironment.WebRootPath}\\images\\product";

            if (!Directory.Exists(folderDirectory))
            {
                Directory.CreateDirectory(folderDirectory);
            }
            var filePath = Path.Combine(folderDirectory, fileName);

            await using FileStream fileStream = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(fileStream);

            var fullPath = $"/images/product/{fileName}";

            return fullPath;
        }

        public bool DeleteFile(string filePath)
        {
            if (File.Exists(_webHostEnvironment.WebRootPath + filePath))
            {
                File.Delete(_webHostEnvironment.WebRootPath + filePath);
                return true;
            }

            return false;
        }


    }
}
