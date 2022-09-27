using Retailer.BlazorServer.GrpcClient.Protos;
using Retailer.Interface.Dtos;

namespace Retailer.BlazorServer.GrpcClient.Clients
{
    public interface IGrpcClient
    {
        Task<string> GetGreeting();

        Task<CustomerDto> GetCustomerById(int id);

        Task<List<CustomerDto>> GetAllCustomers();

        Task<bool> AddCustomer(CustomerDto customer);

        Task DeleteCustomer(int id);

        Task<List<ProductDto>> GetAllProducts();

        Task<List<ProductDto>> GetProductsByIds(List<int> ids = null);

        Task<List<ProductDto>> GetStreamProductByIds(List<int> ids = null);

        Task FileUpload();

        Task FileDownload();

        Task<WisdomDto> GetWisdomModel();

        Task BiDirectionalExample();

    }
}