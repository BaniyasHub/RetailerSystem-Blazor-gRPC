using Retailer.Interface.Dtos;

namespace Retailer.WASM.ViewModels
{
    public class ShoppingCart
    {
        public int ProductId { get; set; }

        public ProductDto Product { get; set; }

        public int ProductPriceId { get; set; }

        public ProductPriceDto ProductPrice { get; set; }

        public int Count { get; set; }
    }
}
