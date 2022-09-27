using Retailer.Interface.Dtos;

namespace Retailer.WASM.Service.IService
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProducts();

        Task<ProductDto> GetProduct(int id);

        Task<ProductPriceDto> GetProductPriceById(int id);
    }
}
