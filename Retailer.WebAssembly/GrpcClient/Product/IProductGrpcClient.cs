using Retailer.Interface.Dtos;

namespace Retailer.WebAssembly.GrpcClient.Product
{
    public interface IProductGrpcClient
    {
        Task<ProductDto> GetProductById(int id);

        Task<List<ProductDto>> GetAllProducts();
    }
}
