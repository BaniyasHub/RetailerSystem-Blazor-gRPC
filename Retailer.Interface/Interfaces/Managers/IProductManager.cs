using Retailer.Interface.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Interface.Interfaces.Managers
{
    public interface IProductManager
    {
        Task<List<ProductDto>> GetAllProducts(CancellationToken? cancellationToken = null);

        Task<ProductDto> GetProduct(int id);

        Task AddProduct(ProductDto productDto);

        Task DeleteProduct(int id);

        Task UpdateProduct(ProductDto productDto);

        Task<List<ProductDto>> GetProductsByIds(List<int> ids, CancellationToken? cancellationToken = null);

        Task DeleteProductPriceById(int id);

        Task EditProductPrice(ProductPriceDto productPrice);

        Task AddProductPrice(ProductPriceDto productPrice);

        Task<ProductPriceDto> GetProductPriceById(int id);
    }
}
