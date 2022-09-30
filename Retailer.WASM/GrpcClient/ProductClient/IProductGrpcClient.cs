using Retailer.Interface.Dtos;

namespace Retailer.WASM.GrpcClient.ProductClient
{
    public interface IProductGrpcClient
    {
        Task<ProductDto> GetProductById(int id);

        Task<List<ProductDto>> GetAllProducts();

        //Task<ProductPriceDto> GetProductPriceById(int id);

        Task<List<ProductDto>> GetProductsByIds(List<int> productIds);
    }
}
