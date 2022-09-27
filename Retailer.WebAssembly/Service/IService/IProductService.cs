using Retailer.Interface.Dtos;

namespace Retailer.WebAssembly.Service.IService
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProducts();

        Task<ProductDto> GetProduct(int id);
    }
}
