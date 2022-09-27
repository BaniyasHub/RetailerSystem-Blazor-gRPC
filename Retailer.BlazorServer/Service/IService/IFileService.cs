using Microsoft.AspNetCore.Components.Forms;

namespace Retailer.BlazorServer.Service.IService
{
    public interface IFileService
    {
        Task<string> UploadFile(IBrowserFile file);

        bool DeleteFile(string filePath);
    }
}
